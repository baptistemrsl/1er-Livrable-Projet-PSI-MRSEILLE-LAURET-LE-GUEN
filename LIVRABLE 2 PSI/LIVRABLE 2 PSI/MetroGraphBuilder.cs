using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIVRABLE_2_PSI
{
    public class MetroGraphBuilder
    {
        private readonly string _fichierExcel;
        private readonly Graphe<Station> graphe = new();
        private readonly Dictionary<int, Noeud<Station>> noeudsParId = new();
        public Dictionary<int, Noeud<Station>> NoeudParId => noeudsParId;

        public MetroGraphBuilder(string _fichierExcel)
        {
            _fichierExcel = "MetroParis.xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(new FileInfo(_fichierExcel));

        }

        public Graphe<Station> ConstruireGraphe()
        {
            using var package = new ExcelPackage(new FileInfo("MetroParis.xlsx"));
            Console.WriteLine("Feuilles disponibles dans le fichier Excel :");

            foreach (var feuille in package.Workbook.Worksheets)
            {
                Console.WriteLine($"'{feuille.Name}'");
            }
            var feuilleStations = package.Workbook.Worksheets
    .FirstOrDefault(ws => ws.Name.Trim().Equals("Noeuds", StringComparison.OrdinalIgnoreCase));

            var feuilleArcs = package.Workbook.Worksheets["Arcs"];

            // Création des nœuds (stations)
            int lignesStations = feuilleStations.Dimension.Rows;
            for (int i = 2; i <= lignesStations; i++)
            {
                var station = new Station
                {
                    Id = int.Parse(feuilleStations.Cells[i, 1].Text),
                    Nom = feuilleStations.Cells[i, 2].Text,
                    Ligne = feuilleStations.Cells[i, 3].Text,
                    Longitude = double.Parse(feuilleStations.Cells[i, 4].Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                    Latitude = double.Parse(feuilleStations.Cells[i, 5].Text.Replace(',', '.'), CultureInfo.InvariantCulture),

                    Commune = feuilleStations.Cells[i, 6].Text
                };


                var noeud = graphe.AjouterNoeud(station);
                noeudsParId[station.Id] = noeud;
            }

            // Création des arêtes (liaisons directes entre stations)
            int lignesArcs = feuilleArcs.Dimension.Rows;
            for (int i = 2; i <= lignesArcs; i++)
            {
                int idStation = int.Parse(feuilleArcs.Cells[i, 1].Text); // Colonne A

                var precedent = feuilleArcs.Cells[i, 3].Text; // Colonne C
                var suivant = feuilleArcs.Cells[i, 4].Text;   // Colonne D
                var tempsTxt = feuilleArcs.Cells[i, 5].Text;  // Colonne E
                var changementTxt = feuilleArcs.Cells[i, 6].Text; // Colonne F

                double temps = double.TryParse(tempsTxt.Replace(',', '.'), CultureInfo.InvariantCulture, out var t) ? t : 0;
                double changement = double.TryParse(changementTxt.Replace(',', '.'), CultureInfo.InvariantCulture, out var c) ? c : 0;

                // Ajout des arcs
                if (int.TryParse(precedent, out int idPrecedent))
                {
                    graphe.AjouterArc(noeudsParId[idPrecedent], noeudsParId[idStation], temps);
                }

                if (int.TryParse(suivant, out int idSuivant))
                {
                    graphe.AjouterArc(noeudsParId[idStation], noeudsParId[idSuivant], temps);
                }

            }

            // Création des correspondances (temps de changement entre stations de même nom)
            var groupesParNom = noeudsParId.Values
                .GroupBy(n => n.Valeur.Nom)
                .Where(g => g.Count() > 1);

            foreach (var groupe in groupesParNom)
            {
                var stations = groupe.ToList();
                for (int i = 0; i < stations.Count; i++)
                {
                    for (int j = i + 1; j < stations.Count; j++)
                    {
                        graphe.AjouterArc(stations[i], stations[j], 2); // correspondance bidirectionnelle par défaut 2 min
                        graphe.AjouterArc(stations[j], stations[i], 2);
                    }
                }
            }

            return graphe;
        }
    }
}
