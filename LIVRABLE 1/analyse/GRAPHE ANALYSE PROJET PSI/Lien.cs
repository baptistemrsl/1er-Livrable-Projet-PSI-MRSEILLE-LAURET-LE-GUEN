using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace karat
{
    internal class Lien
    {
        public Noeud n1;
        public Noeud n2;

        
        public Lien(Noeud N1,Noeud N2) {
            n1 = N1;
            n2 = N2;
        }

        public string toStringL()
        {
            return $"Ce lien relie les noeuds {n1} et {n2}.";
        }
    }
}
