using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIVRABLE_2_PSI
{
    public class Chemins<T>
    {
        private Graphe<T> _graphe;

        public Chemins(Graphe<T> graphe)
        {
            _graphe = graphe;
        }

        public (double distance, List<Noeud<T>> chemin) Dijkstra(Noeud<T> source, Noeud<T> cible)
        {
            var distances = new Dictionary<Noeud<T>, double>();
            var precedent = new Dictionary<Noeud<T>, Noeud<T>?>();
            var file = new PriorityQueue<Noeud<T>, double>();

            foreach (var n in _graphe.Noeuds)
            {
                distances[n] = double.PositiveInfinity;
                precedent[n] = null;
            }

            distances[source] = 0;
            file.Enqueue(source, 0);

            while (file.Count > 0)
            {
                var courant = file.Dequeue();

                if (courant.Equals(cible))
                    break;

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

            var chemin = ReconstruireChemin(cible, precedent);
            return (distances[cible], chemin);
        }

        public (double distance, List<Noeud<T>> chemin) BellmanFord(Noeud<T> source, Noeud<T> cible)
        {
            var distances = new Dictionary<Noeud<T>, double>();
            var precedent = new Dictionary<Noeud<T>, Noeud<T>?>();
            foreach (var n in _graphe.Noeuds)
            {
                distances[n] = double.PositiveInfinity;
                precedent[n] = null;
            }
            distances[source] = 0;

            var edges = _graphe.Adjacence.SelectMany(pair =>
                pair.Value.Select(edge => (from: pair.Key, to: edge.Item1, poids: edge.Item2))).ToList();

            for (int i = 0; i < _graphe.Noeuds.Count - 1; i++)
            {
                foreach (var (from, to, poids) in edges)
                {
                    if (distances[from] + poids < distances[to])
                    {
                        distances[to] = distances[from] + poids;
                        precedent[to] = from;
                    }
                }
            }

            var chemin = ReconstruireChemin(cible, precedent);
            return (distances[cible], chemin);
        }

        private Dictionary<(Noeud<T>, Noeud<T>), double> _distancesFW;
        private Dictionary<(Noeud<T>, Noeud<T>), Noeud<T>?> _suivantFW;

        public void CalculerFloydWarshall()
        {
            _distancesFW = new();
            _suivantFW = new();

            foreach (var u in _graphe.Noeuds)
            {
                foreach (var v in _graphe.Noeuds)
                {
                    if (u.Equals(v))
                        _distancesFW[(u, v)] = 0;
                    else
                        _distancesFW[(u, v)] = double.PositiveInfinity;
                    _suivantFW[(u, v)] = null;
                }
            }

            foreach (var u in _graphe.Noeuds)
            {
                foreach (var (v, poids) in _graphe.Adjacence[u])
                {
                    _distancesFW[(u, v)] = poids;
                    _suivantFW[(u, v)] = v;
                }
            }

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

        public (double distance, List<Noeud<T>> chemin) FloydWarshall(Noeud<T> source, Noeud<T> cible)
        {
            if (_distancesFW == null || _suivantFW == null)
                throw new Exception("Floyd-Warshall non encore calculé.");

            if (_distancesFW[(source, cible)] == double.PositiveInfinity)
                return (double.PositiveInfinity, new());

            var chemin = new List<Noeud<T>> { source };
            var courant = source;
            while (!courant.Equals(cible))
            {
                courant = _suivantFW[(courant, cible)]!;
                chemin.Add(courant);
            }

            return (_distancesFW[(source, cible)], chemin);
        }

        private List<Noeud<T>> ReconstruireChemin(Noeud<T> cible, Dictionary<Noeud<T>, Noeud<T>?> precedent)
        {
            var chemin = new List<Noeud<T>>();
            var courant = cible;
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

