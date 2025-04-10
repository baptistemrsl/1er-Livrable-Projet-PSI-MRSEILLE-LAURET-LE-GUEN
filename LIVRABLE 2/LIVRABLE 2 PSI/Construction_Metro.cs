﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Globalization;

using static LIVRABLE_2_PSI.Graphe;

namespace LIVRABLE_2_PSI
{
    internal class Construction_Metro
    {
        public class Station
        {
            public int Id { get; set; }
            public string Nom { get; set; } = "";
            public string Ligne { get; set; } = "";
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Commune { get; set; } = "";

            public override string ToString() => $"{Nom} (Ligne {Ligne})";
        }

        public class MetroGraphBuilder
        {
            private readonly string _fichierExcel;
            private readonly Graphes<Station> _graphe = new();
            private readonly Dictionary<int, Noeud<Station>> _noeudsParId = new();

            public MetroGraphBuilder(string fichierExcel)
            {
                _fichierExcel = "MetroParis.xlsx";
                using var package = new ExcelPackage(new FileInfo(_fichierExcel));

            }

            public Graphes<Station> ConstruireGraphe()
            {
                using var package = new ExcelPackage(new FileInfo("MetroParis.xlsx"));
                Console.WriteLine("Feuilles disponibles dans le fichier Excel :");

                foreach (var feuille in package.Workbook.Worksheets)
                {
                    Console.WriteLine($"'{feuille.Name}'");
                }
                var feuilleStations = package.Workbook.Worksheets["Noeuds"];
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
                        Longitude = double.Parse(feuilleStations.Cells[i, 4].Text, System.Globalization.CultureInfo.InvariantCulture),
                        Latitude = double.Parse(feuilleStations.Cells[i, 5].Text, System.Globalization.CultureInfo.InvariantCulture),
                        Commune = feuilleStations.Cells[i, 6].Text
                    };


                    var noeud = _graphe.AjouterNoeud(station);
                    _noeudsParId[station.Id] = noeud;
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
                        _graphe.AjouterArc(_noeudsParId[idPrecedent], _noeudsParId[idStation], temps);
                    }

                    if (int.TryParse(suivant, out int idSuivant))
                    {
                        _graphe.AjouterArc(_noeudsParId[idStation], _noeudsParId[idSuivant], temps);
                    }

                }

                // Création des correspondances (temps de changement entre stations de même nom)
                var groupesParNom = _noeudsParId.Values
                    .GroupBy(n => n.Valeur.Nom)
                    .Where(g => g.Count() > 1);

                foreach (var groupe in groupesParNom)
                {
                    var stations = groupe.ToList();
                    for (int i = 0; i < stations.Count; i++)
                    {
                        for (int j = i + 1; j < stations.Count; j++)
                        {
                            _graphe.AjouterArc(stations[i], stations[j], 2); // correspondance bidirectionnelle par défaut 2 min
                            _graphe.AjouterArc(stations[j], stations[i], 2);
                        }
                    }
                }

                return _graphe;
            }
        }
    }
}
