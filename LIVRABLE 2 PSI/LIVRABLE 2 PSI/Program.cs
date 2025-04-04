using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LIVRABLE_2_PSI
{
    class Program
    {
        static void Main(string[] args)
        {
            /// Construire le graphe à partir des fichiers CSV
            var builder = new CsvGraphBuilder("noeuds.csv", "arcs.csv");
            Graphe<Station> graphe = builder.ConstruireGraphe();
            Console.WriteLine($"Nombre total de stations : {graphe.Noeuds.Count}");

            /// Vérifier si le graphe est connexe
            bool estConnexe = graphe.EstConnexe();
            Console.WriteLine($"Le graphe est connexe ? {estConnexe}");

            /// Préparation des algorithmes de chemin
            var chemins = new Chemins<Station>(graphe);
            /// Calcul préalable de Floyd-Warshall (pour ne pas le refaire à chaque requête)
            chemins.CalculerFloydWarshall();

            /// Afficher quelques stations en exemple pour guider l'utilisateur
            Console.WriteLine("\nExemples de stations (ID -> Nom (Ligne)) :");
            foreach (var pair in builder.NoeudParId.OrderBy(p => p.Key).Take(5))
            {
                Console.WriteLine($"ID {pair.Key} -> {pair.Value.Valeur.Nom} (Ligne {pair.Value.Valeur.Ligne})");
            }

            /// Demande des stations de départ et d'arrivée
            Console.Write("\nEntrez l'ID de la station de départ : ");
            if (!int.TryParse(Console.ReadLine(), out int idDepart) || !builder.NoeudParId.ContainsKey(idDepart))
            {
                Console.WriteLine("ID de départ invalide.");
                return;
            }
            Console.Write("Entrez l'ID de la station d'arrivée : ");
            if (!int.TryParse(Console.ReadLine(), out int idArrivee) || !builder.NoeudParId.ContainsKey(idArrivee))
            {
                Console.WriteLine("ID d'arrivée invalide.");
                return;
            }

            var noeudDepart = builder.NoeudParId[idDepart];
            var noeudArrivee = builder.NoeudParId[idArrivee];

            Console.WriteLine($"\nCalcul du plus court chemin entre \"{noeudDepart.Valeur.Nom}\" et \"{noeudArrivee.Valeur.Nom}\"...");

            /// Exécuter et chronométrer chaque algorithme
            var stopwatch = new Stopwatch();

            stopwatch.Restart();
            var (distDij, cheminDij) = chemins.Dijkstra(noeudDepart, noeudArrivee);
            stopwatch.Stop();
            long tempsDijkstra = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var (distBell, cheminBell) = chemins.BellmanFord(noeudDepart, noeudArrivee);
            stopwatch.Stop();
            long tempsBellman = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var (distFW, cheminFW) = chemins.FloydWarshall(noeudDepart, noeudArrivee);
            stopwatch.Stop();
            long tempsFloyd = stopwatch.ElapsedMilliseconds;

            /// Affichage des temps d'exécution
            Console.WriteLine("\n---- RÉSULTATS DES ALGORITHMES ----");
            Console.WriteLine($"Dijkstra    -> Temps calcul: {tempsDijkstra} ms, Distance: {distDij} minutes");
            Console.WriteLine($"Bellman-Ford-> Temps calcul: {tempsBellman} ms, Distance: {distBell} minutes");
            Console.WriteLine($"Floyd-Warshall -> Temps calcul: {tempsFloyd} ms, Distance: {distFW} minutes");

            /// Vérifier que tous les algos donnent la même distance (ce qui devrait être le cas)
            if (distDij == distBell && distDij == distFW)
            {
                Console.WriteLine("Tous les algorithmes trouvent la même distance minimale.");
            }
            else
            {
                Console.WriteLine("Les algorithmes n'ont pas trouvé la même distance  vérifiez les données ou implémentations ");
            }

            /// Identifier le plus rapide
            long tempsMin = Math.Min(tempsDijkstra, Math.Min(tempsBellman, tempsFloyd));
            string algoRapide;
            List<Noeud<Station>> cheminRapide;
            double distanceRapide;
            if (tempsMin == tempsDijkstra)
            {
                algoRapide = "Dijkstra";
                cheminRapide = cheminDij;
                distanceRapide = distDij;
            }
            else if (tempsMin == tempsBellman)
            {
                algoRapide = "Bellman-Ford";
                cheminRapide = cheminBell;
                distanceRapide = distBell;
            }
            else
            {
                algoRapide = "Floyd-Warshall";
                cheminRapide = cheminFW;
                distanceRapide = distFW;
            }

            Console.WriteLine($"\n=> L'algorithme le plus rapide est {algoRapide} (temps = {tempsMin} ms).");
            Console.WriteLine($"Distance du plus court chemin : {distanceRapide} minutes.");
            Console.WriteLine("Chemin le plus court :");
            foreach (var noeud in cheminRapide)
            {
                Console.WriteLine($" - {noeud.Valeur.Nom} (Ligne {noeud.Valeur.Ligne})");
            }
        }
    }
}
