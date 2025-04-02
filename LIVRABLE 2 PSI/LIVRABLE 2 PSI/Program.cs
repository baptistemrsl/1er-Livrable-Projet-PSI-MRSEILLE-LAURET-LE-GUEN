using LIVRABLE_2_PSI;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;



namespace LIVRABLE_2_PSI
{
    class Program1
    {
        static void Main()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Charger et construire le graphe du métro de Paris à partir du fichier Excel
            var builder = new MetroGraphBuilder("MetroParis.xlsx");
            var graphe = builder.ConstruireGraphe();

            Console.WriteLine($"Nombre total de stations : {graphe.Noeuds.Count}");

            // Vérifier la connectivité du graphe
            bool estConnexe = graphe.EstConnexe();
            Console.WriteLine($"\nLe graphe est connexe ? {estConnexe}");

            // Générer la matrice d’adjacence (utile pour Floyd-Warshall par exemple)
            graphe.GenererMatriceAdjacence();

            ///test algo tri

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var cheminAlgo = new Chemins<Station>(graphe);
            cheminAlgo.CalculerFloydWarshall();

            // 🔎 Affichage des 10 premières stations pour aide utilisateur
            Console.WriteLine("\nVoici quelques stations disponibles :");
            foreach (var pair in builder.NoeudParId.OrderBy(p => p.Key).Take(10))
            {
                Console.WriteLine($"ID {pair.Key} → {pair.Value.Valeur.Nom} (Ligne {pair.Value.Valeur.Ligne})");
            }


            Console.WriteLine("\n==== CALCUL D'ITINÉRAIRE ====");

            Console.Write("Entrez l'ID de la station de départ : ");
            int idDepart = int.Parse(Console.ReadLine()!);

            Console.Write("Entrez l'ID de la station d'arrivée : ");
            int idArrivee = int.Parse(Console.ReadLine()!);

            if (!builder.NoeudParId.TryGetValue(idDepart, out var depart) || !builder.NoeudParId.TryGetValue(idArrivee, out var arrivee))
            {
                Console.WriteLine("\nID invalide. Station non trouvée.");
                return;
            }

            Console.WriteLine("\nChoisissez l'algorithme :");
            Console.WriteLine("1 - Dijkstra");
            Console.WriteLine("2 - Bellman-Ford");
            Console.WriteLine("3 - Floyd-Warshall");
            Console.Write("Votre choix : ");
            int choix = int.Parse(Console.ReadLine()!);

            (double distance, System.Collections.Generic.List<Noeud<Station>> chemin) resultat = choix switch
            {
                1 => cheminAlgo.Dijkstra(depart, arrivee),
                2 => cheminAlgo.BellmanFord(depart, arrivee),
                3 => cheminAlgo.FloydWarshall(depart, arrivee),
                _ => throw new Exception("Choix invalide")
            };

            Console.WriteLine($"\nTemps total estimé : {resultat.distance} minutes");
            Console.WriteLine("\nItinéraire :");
            foreach (var noeud in resultat.chemin)
            {
                Console.WriteLine($"- {noeud.Valeur.Nom} (Ligne {noeud.Valeur.Ligne})");
            }
            ///methode comparaison algo tries
            ComparerAlgorithmes(graphe, cheminAlgo, depart, arrivee);

        }
        static void ComparerAlgorithmes(Graphe<Station> graphe, Chemins<Station> cheminAlgo, Noeud<Station> depart, Noeud<Station> arrivee)
        {
            Console.WriteLine("\n==== COMPARAISON DES ALGORITHMES ====");
            Console.WriteLine($"Départ : {depart.Valeur.Nom} (Ligne {depart.Valeur.Ligne})");
            Console.WriteLine($"Arrivée : {arrivee.Valeur.Nom} (Ligne {arrivee.Valeur.Ligne})\n");

            (double temps, long duree, string nom, System.Collections.Generic.List<Noeud<Station>> chemin) Run(Func<(double, System.Collections.Generic.List<Noeud<Station>>)> algo, string nom)
            {
                var sw = Stopwatch.StartNew();
                var (dist, chemin) = algo();
                sw.Stop();
                return (dist, sw.ElapsedMilliseconds, nom, chemin);
            }

            var r1 = Run(() => cheminAlgo.Dijkstra(depart, arrivee), "Dijkstra");
            var r2 = Run(() => cheminAlgo.BellmanFord(depart, arrivee), "Bellman-Ford");
            var r3 = Run(() => cheminAlgo.FloydWarshall(depart, arrivee), "Floyd-Warshall");

            foreach (var res in new[] { r1, r2, r3 })
            {
                Console.WriteLine($"[{res.nom}]\n Temps : {res.temps} min\n Durée d'exécution : {res.duree} ms\n");
            }

            bool memeDistance = Math.Abs(r1.temps - r2.temps) < 0.001 && Math.Abs(r1.temps - r3.temps) < 0.001;
            Console.WriteLine(memeDistance
                ? " Tous les algorithmes donnent le même résultat."
                : " Les résultats diffèrent entre les algorithmes.");
        }
    }
}