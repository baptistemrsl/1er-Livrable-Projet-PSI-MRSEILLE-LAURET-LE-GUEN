using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIVRABLE_2_PSI
{
        public class Graphe<T>
        {
            /// <summary>
            /// Classe générique représentant un graphe orienté pondéré.
            /// Contient une liste d’adjacence et une possibilité de générer une matrice d’adjacence.
            /// </summary>
            public List<Noeud<T>> Noeuds { get; private set; } = new();

            /// <summary>
            /// Dictionnaire des voisins pour chaque nœud (liste d’adjacence).
            /// </summary>
            public Dictionary<Noeud<T>, List<(Noeud<T> voisin, double poids)>> Adjacence { get; private set; } = new();

            /// <summary>
            /// Matrice d’adjacence, générée à la demande (ex. Floyd-Warshall).
            /// </summary>
            public double[,]? MatriceAdjacence { get; private set; } = null;

            public Noeud<T> AjouterNoeud(T valeur)
            {
                var noeud = new Noeud<T>(valeur);
                Noeuds.Add(noeud);
                Adjacence[noeud] = new List<(Noeud<T>, double)>();
                return noeud;
            }

            public void AjouterArc(Noeud<T> source, Noeud<T> destination, double poids)
            {
                if (!Adjacence.ContainsKey(source))
                    Adjacence[source] = new List<(Noeud<T>, double)>();
                Adjacence[source].Add((destination, poids));
            }

            public Noeud<T>? TrouverNoeud(Func<Noeud<T>, bool> predicate)
            {
                return Noeuds.FirstOrDefault(predicate);
            }

            /// <summary>
            /// Génère la matrice d’adjacence utile pour certains algorithmes comme Floyd-Warshall.
            /// </summary>
            public void GenererMatriceAdjacence()
            {
                int n = Noeuds.Count;
                MatriceAdjacence = new double[n, n];

                // Initialisation avec +∞ sauf diagonale à 0
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        MatriceAdjacence[i, j] = (i == j) ? 0 : double.PositiveInfinity;

                for (int i = 0; i < n; i++)
                {
                    var noeud = Noeuds[i];
                    foreach (var (voisin, poids) in Adjacence[noeud])
                    {
                        int j = Noeuds.IndexOf(voisin);
                        MatriceAdjacence[i, j] = poids;
                    }
                }
            }

            /// <summary>
            /// Vérifie si le graphe est connexe en utilisant un parcours en profondeur.
            /// </summary>
            public bool EstConnexe()
            {
                if (Noeuds.Count == 0) return true;
                var visités = new HashSet<Noeud<T>>();
                DFS(Noeuds[0], visités);
                return visités.Count == Noeuds.Count;
            }

            private void DFS(Noeud<T> noeud, HashSet<Noeud<T>> visités)
            {
                if (!visités.Add(noeud)) return;
                foreach (var (voisin, _) in Adjacence[noeud])
                    DFS(voisin, visités);
            }

            /// <summary>
            /// Parcours en largeur à partir d’un nœud.
            /// </summary>
            public void ParcoursLargeur(Noeud<T> depart)
            {
                var file = new Queue<Noeud<T>>();
                var visités = new HashSet<Noeud<T>>();

                file.Enqueue(depart);
                visités.Add(depart);

                while (file.Count > 0)
                {
                    var courant = file.Dequeue();
                    Console.WriteLine(courant);

                    foreach (var (voisin, _) in Adjacence[courant])
                    {
                        if (!visités.Contains(voisin))
                        {
                            visités.Add(voisin);
                            file.Enqueue(voisin);
                        }
                    }
                }
            }

            /// <summary>
            /// Parcours en profondeur à partir d’un nœud.
            /// </summary>
            public void ParcoursProfondeur(Noeud<T> depart)
            {
                var visités = new HashSet<Noeud<T>>();
                DFS(depart, visités);
            }
        }
}
