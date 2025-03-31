using LIVRABLE_2_PSI;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using static LIVRABLE_2_PSI.Construction_Metro;
using static LIVRABLE_2_PSI.Graphe;



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

            // Afficher quelques stations et leurs connexions (debug)
            foreach (var noeud in graphe.Noeuds.Take(5))
            {
                Console.WriteLine($"\nStation : {noeud.Valeur}");
                foreach (var (voisin, poids) in graphe.Adjacence[noeud])
                {
                    Console.WriteLine($"=> Vers {voisin.Valeur} en {poids} minutes");
                }
            }

            // Vérifier la connectivité du graphe
            bool estConnexe = graphe.EstConnexe();
            Console.WriteLine($"\nLe graphe est connexe ? {estConnexe}");

            // Générer la matrice d’adjacence (utile pour Floyd-Warshall par exemple)
            graphe.GenererMatriceAdjacence();
        }
    }
}