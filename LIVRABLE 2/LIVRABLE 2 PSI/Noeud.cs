using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIVRABLE_2_PSI
{
    /// <summary>
    /// Représente un nœud générique dans un graphe, contenant une valeur de type T
    /// et une liste de voisins avec des poids associés.
    /// </summary>
    public class Noeud<T>
    {
        public T Valeur { get; set; }
        public List<(Noeud<T> Voisin, double Poids)> Voisins { get; set; } = new();

        public Noeud(T valeur)
        {
            Valeur = valeur;
        }

        /// <summary>
        /// Ajoute un voisin avec un poids.
        /// </summary>
        public void AjouterVoisin(Noeud<T> voisin, double poids)
        {
            Voisins.Add((voisin, poids));
        }

        public override string ToString()
        {
            return Valeur?.ToString() ?? "(null)";
        }
    }
}