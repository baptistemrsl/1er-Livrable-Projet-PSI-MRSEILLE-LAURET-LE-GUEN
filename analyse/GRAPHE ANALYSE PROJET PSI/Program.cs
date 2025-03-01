using System;
using System.Collections.Generic;
using System.IO;



namespace karat
{
    class Program1
    {
        static void Main()
        {
            var lignes = File.ReadAllLines("soc-karate.mtx");  ///Lire le fichier
            var firstLigne = lignes[0].Split();
            int n = int.Parse(firstLigne[0]);       ///Nombres de sommets
            int l = int.Parse(firstLigne[2]);       ///Nombre d'arretes

            Noeud[] noeuds = new Noeud[n];
            for (int i = 0; i < n; i++)
            {
                noeuds[i] = new Noeud(i + 1);   ///Creation des sommets dans un tableau
            }

            Lien[] liens = new Lien[l];
            int[,] adj = new int[n, n];
            for (int i = 0; i < l; i++)
            {
                var parts = lignes[i + 1].Split();
                int a = int.Parse(parts[0]) - 1;
                int b = int.Parse(parts[1]) - 1;

                Noeud n1 = noeuds[a];
                Noeud n2 = noeuds[b];

                liens[i] = new Lien(n1, n2);        ///Creation des arretes dans un tableau

                adj[a, b] = 1;                      ///Matrice d'adjacence
                adj[b, a] = 1;
            }
            Graphe2 k = new Graphe2(n, noeuds, liens, adj);         ///Creation du graphe à partir des sommets, des arretes et de la matrice d'adjacence



            Console.WriteLine("Parcours en largeur:");
            k.Largeur(0);

            Console.WriteLine("\nParcours en profondeur:");
            k.Profondeur(0);

            Console.WriteLine("\nLe graphe est connexe ? " + k.Connexe());

            (bool, string) S = k.Cycle();
            Console.WriteLine("\nLe graphe contient au moins un cycle ? " + S.Item1);
            Console.WriteLine(S.Item2);

            //Visualisation v = new Visualisation(noeuds, liens);
            //v.DessinerGraphe("soc-karate.mtx");
        }
    }
}
