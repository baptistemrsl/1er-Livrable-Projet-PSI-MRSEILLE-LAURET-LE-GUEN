using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LIVRABLE_2_PSI
{
    public class Chemins<T>
    {
        private Graphe<T> _graphe;

        /// <summary>
        /// Structures de données pour Floyd-Warshall 
        /// </summary>
        private Dictionary<(Noeud<T>, Noeud<T>), double>? _distancesFW;
        private Dictionary<(Noeud<T>, Noeud<T>), Noeud<T>?>? _suivantFW;

        public Chemins(Graphe<T> graphe)
        {
            _graphe = graphe;
        }

        /// <summary>
        /// Algorithme de Dijkstra – renvoie la distance minimale et le chemin de source à cible.
        /// </summary>
        public (double distance, List<Noeud<T>> chemin) Dijkstra(Noeud<T> source, Noeud<T> cible)
        {
            var distances = new Dictionary<Noeud<T>, double>();
            var precedent = new Dictionary<Noeud<T>, Noeud<T>?>();
            var file = new PriorityQueue<Noeud<T>, double>();  

            /// Initialisation : distance infinie pour tous, sauf source=0
            foreach (var noeud in _graphe.Noeuds)
            {
                distances[noeud] = double.PositiveInfinity;
                precedent[noeud] = null;
            }
            distances[source] = 0;
            file.Enqueue(source, 0);

            while (file.Count > 0)
            {
                var courant = file.Dequeue();
                if (courant.Equals(cible))
                    break;  
                /// Relaxation des arêtes sortantes du noeud courant
                foreach (var (voisin, poids) in _graphe.Adjacence[courant])
                {
                    double tentative = distances[courant] + poids;
                    if (tentative < distances[voisin])
                    {
                        distances[voisin] = tentative;
                        precedent[voisin] = courant;
                        file.Enqueue(voisin, tentative);
                    }
                }
            }

            /// Reconstruire le chemin optimal trouvé de source à cible
            List<Noeud<T>> chemin = ReconstruireChemin(cible, precedent);
            return (distances[cible], chemin);
        }

        /// <summary>
        /// Algorithme de Bellman-Ford – renvoie la distance minimale et le chemin de source à cible.
        /// </summary>
        public (double distance, List<Noeud<T>> chemin) BellmanFord(Noeud<T> source, Noeud<T> cible)
        {
            var distances = new Dictionary<Noeud<T>, double>();
            var precedent = new Dictionary<Noeud<T>, Noeud<T>?>();

            /// Initialisation
            foreach (var noeud in _graphe.Noeuds)
            {
                distances[noeud] = double.PositiveInfinity;
                precedent[noeud] = null;
            }
            distances[source] = 0;

            /// Relaxation sur V-1 itérations
            var aretes = _graphe.Adjacence.SelectMany(
                pair => pair.Value.Select(edge => (from: pair.Key, to: edge.Item1, poids: edge.Item2))
            ).ToList();

            int n = _graphe.Noeuds.Count;
            for (int i = 0; i < n - 1; i++)
            {
                foreach (var (u, v, poids) in aretes)
                {
                    if (distances[u] + poids < distances[v])
                    {
                        distances[v] = distances[u] + poids;
                        precedent[v] = u;
                    }
                }
            }


            List<Noeud<T>> chemin = ReconstruireChemin(cible, precedent);
            return (distances[cible], chemin);
        }

        /// <summary>
        /// Pré-calcul de Floyd-Warshall sur tout le graphe.
        /// Remplit _distancesFW et _suivantFW pour interroger ensuite la distance et le chemin.
        /// </summary>
        public void CalculerFloydWarshall()
        {
            _distancesFW = new Dictionary<(Noeud<T>, Noeud<T>), double>();
            _suivantFW = new Dictionary<(Noeud<T>, Noeud<T>), Noeud<T>?>();

            /// Initialisation : distances directes depuis la matrice d'adjacence
            foreach (var i in _graphe.Noeuds)
            {
                foreach (var j in _graphe.Noeuds)
                {
                    if (i.Equals(j))
                        _distancesFW[(i, j)] = 0;
                    else
                        _distancesFW[(i, j)] = double.PositiveInfinity;
                    _suivantFW[(i, j)] = null;
                }
            }
            /// Initialisation avec les arêtes existantes du graphe
            foreach (var u in _graphe.Noeuds)
            {
                foreach (var (v, poids) in _graphe.Adjacence[u])
                {
                    _distancesFW[(u, v)] = poids;
                    _suivantFW[(u, v)] = v;
                }
            }
            /// Triple boucle de Floyd-Warshall
            foreach (var k in _graphe.Noeuds)
            {
                foreach (var i in _graphe.Noeuds)
                {
                    foreach (var j in _graphe.Noeuds)
                    {
                        if (_distancesFW[(i, k)] + _distancesFW[(k, j)] < _distancesFW[(i, j)])
                        {
                            _distancesFW[(i, j)] = _distancesFW[(i, k)] + _distancesFW[(k, j)];
                            _suivantFW[(i, j)] = _suivantFW[(i, k)];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Récupère le plus court chemin source->cible à partir des résultats Floyd-Warshall pré-calculés.
        /// </summary>
        public (double distance, List<Noeud<T>> chemin) FloydWarshall(Noeud<T> source, Noeud<T> cible)
        {
            if (_distancesFW == null || _suivantFW == null)
                throw new InvalidOperationException("Floyd-Warshall n'a pas été calculé. Appelez CalculerFloydWarshall().");

            if (_distancesFW[(source, cible)] == double.PositiveInfinity)
            {
                /// Pas de chemin
                return (double.PositiveInfinity, new List<Noeud<T>>());
            }

            /// Reconstruire le chemin à partir de _suivantFW
            var chemin = new List<Noeud<T>>();
            Noeud<T>? courant = source;
            chemin.Add(courant);
            while (courant != null && !courant.Equals(cible))
            {
                courant = _suivantFW[(courant, cible)];
                if (courant != null)
                    chemin.Add(courant);
            }
            return (_distancesFW[(source, cible)], chemin);
        }

        /// <summary>
        /// Méthode utilitaire pour reconstruire un chemin à partir du dictionnaire des prédécesseurs
        /// </summary>
        /// <param name="cible"></param>
        /// <param name="precedent"></param>
        /// <returns></returns>
        private List<Noeud<T>> ReconstruireChemin(Noeud<T> cible, Dictionary<Noeud<T>, Noeud<T>?> precedent)
        {
            var chemin = new List<Noeud<T>>();
            Noeud<T>? courant = cible;
            while (courant != null)
            {
                chemin.Add(courant);
                courant = precedent[courant];
            }
            chemin.Reverse();
            return chemin;
        }
    }
}

