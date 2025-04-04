using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace LIVRABLE_2_PSI
{
    public class CsvGraphBuilder
    {
        private string _nodesFile;
        private string _arcsFile;
        public Dictionary<int, Noeud<Station>> NoeudParId { get; private set; }

        public CsvGraphBuilder(string nodesCsvPath, string arcsCsvPath)
        {
            _nodesFile = "noeuds.csv";
            _arcsFile = "arcs.csv";
            NoeudParId = new Dictionary<int, Noeud<Station>>();
        }

        public Graphe<Station> ConstruireGraphe()
        {
            var graphe = new Graphe<Station>();

            /// Lire toutes les lignes du fichier noeuds.csv (en ignorant la première ligne d'en-tête)
            string[] lignesNoeuds = File.ReadAllLines(_nodesFile);
            foreach (string ligne in lignesNoeuds.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(ligne)) continue;
                string[] cols = ligne.Split(',');
                
                int idStation = int.Parse(cols[0]);
                string ligneNum = cols[1];
                string nomStation = cols[2];
                double longitude = double.Parse(cols[3], CultureInfo.InvariantCulture);
                double latitude = double.Parse(cols[4], CultureInfo.InvariantCulture);
                string commune = cols[5];

                /// Créer l'objet Station
                Station station = new Station
                {
                    Id = idStation,
                    Nom = nomStation,
                    Ligne = ligneNum,
                    Longitude = longitude,
                    Latitude = latitude,
                    Commune = commune
                };

                /// Ajouter au graphe et stocker le Noeud<Station> créé
                Noeud<Station> noeud = graphe.AjouterNoeud(station);
                NoeudParId[idStation] = noeud;
            }

            /// Lire le fichier arcs.csv pour ajouter les arêtes
            string[] lignesArcs = File.ReadAllLines(_arcsFile);
            /// Pour gérer les correspondances, on stocke temporairement les stations par nom
            var stationsParNom = new Dictionary<string, List<int>>();

            foreach (string ligne in lignesArcs.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(ligne)) continue;
                string[] cols = ligne.Split(',');
                
                int id = int.Parse(cols[0]);
                string nom = cols[1];
                string precedentStr = cols[2];
                string suivantStr = cols[3];
                string tempsTrajetStr = cols[4];
                string tempsChangementStr = cols[5];

                /// Ajouter l'entrée pour correspondance
                if (!stationsParNom.ContainsKey(nom))
                    stationsParNom[nom] = new List<int>();
                stationsParNom[nom].Add(id);

                /// Temps de trajet entre station courante et son précédent/suivant
                if (double.TryParse(tempsTrajetStr, out double tempsTrajet))
                {
                    /// Ajouter arête vers la station précédente
                    if (!string.IsNullOrEmpty(precedentStr))
                    {
                        int idPrec = int.Parse(precedentStr);
                        if (NoeudParId.ContainsKey(id) && NoeudParId.ContainsKey(idPrec))
                        {
                            graphe.AjouterArc(NoeudParId[id], NoeudParId[idPrec], tempsTrajet);
                        }
                    }
                    /// Ajouter arête vers la station suivante
                    if (!string.IsNullOrEmpty(suivantStr))
                    {
                        int idSuiv = int.Parse(suivantStr);
                        if (NoeudParId.ContainsKey(id) && NoeudParId.ContainsKey(idSuiv))
                        {
                            graphe.AjouterArc(NoeudParId[id], NoeudParId[idSuiv], tempsTrajet);
                        }
                    }
                }
            }

            /// Ajouter les arêtes de correspondance entre stations portant le même nom
            foreach (var pair in stationsParNom)
            {
                string nomStation = pair.Key;
                List<int> ids = pair.Value;
                if (ids.Count < 2) continue; 

                
                double tempsCorrespondance = 0;
                
                for (int i = 0; i < ids.Count; i++)
                {
                    for (int j = i + 1; j < ids.Count; j++)
                    {
                        int idA = ids[i];
                        int idB = ids[j];
                        if (NoeudParId.ContainsKey(idA) && NoeudParId.ContainsKey(idB))
                        {
                            
                            graphe.AjouterArc(NoeudParId[idA], NoeudParId[idB], tempsCorrespondance);
                            graphe.AjouterArc(NoeudParId[idB], NoeudParId[idA], tempsCorrespondance);
                        }
                    }
                }
            }

            return graphe;
        }
    }
}

