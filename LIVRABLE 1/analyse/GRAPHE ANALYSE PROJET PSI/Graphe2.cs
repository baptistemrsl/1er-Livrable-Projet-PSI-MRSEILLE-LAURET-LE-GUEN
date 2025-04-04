using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace karat
{
    internal class Graphe2
    {
        public int n;
        public Noeud[] Noeuds;

        private  Lien[] Liens;

        private int[,] Adjacence;

        public Graphe2(int N, Noeud[] noeuds, Lien[] liens, int[,]adj)
        {
            n = N;
            Noeuds = noeuds;                ///Un graphe est defini par ses noeuds et ses liens. Pour des raisons de praticité, on inclue aussi sa matrice.
            Liens = liens;
            Adjacence = adj;

        }

        public bool Connexe()
        {
            bool R = false;
            bool[] S = new bool[n];
            
            for (int i = 0; i < n; i++)
            {
                for (int j = i+1; j < n; j++)       ///Regarde dans le triangle superieur de la matrice d'ajacence
                {
                    if (Adjacence[i, j] == 1)       ///Si il existe un lien entre deux sommets
                    {
                        S[j] = true;                ///Marquer le sommet comme atteint
                    }

                }
            }
            foreach(bool b in S)
            {
                if (b) { R = true; }                ///Si tous les sommets sont atteints, renvoie vrai
            }
            return R;
        }
        public (bool,string) Cycle()
        {
            (bool,string) S = (false, "Nan");
            bool[]visite = new bool[n];

            string l = "\nUn exemple de cycle du graphe est : ";
            for (int i = 1; i <= n; i++)                             ///Pour  
            {
                for(int b = 0; b < n; b++)
                {
                    visite[b] = false;
                }
                return CycleRecursif(i,visite,i,l);
                
            }
            return (S);
        }
        private (bool, string) CycleRecursif(int noeud, bool[] visite, int parent, string chemin)
        {
            visite[noeud - 1] = true;
            chemin += " " + noeud;

            for (int voisin = 0; voisin < n; voisin++)
            {
                if (Adjacence[noeud - 1, voisin] == 1) 
                {
                    if (voisin + 1 != parent && visite[voisin])
                    {
                        return (true, chemin + " " + (voisin + 1));
                    }
                    else if (!visite[voisin])
                    {
                        var (cycleTrouve, cycleChemin) = CycleRecursif(voisin + 1, visite, noeud, chemin);
                        if (cycleTrouve)
                        {
                            return (true, cycleChemin);
                        }
                    }
                }
            }

            return (false, "Il n'y a pas de cycle");
        }


        public void Profondeur(int index)
        {

            bool[] visite = new bool[n];
            string l = "";
            Console.WriteLine(ProfondeurRecursive(index, visite, l));
        }

        private string ProfondeurRecursive(int index, bool[] visite, string l)
        {

            visite[index] = true;
            int Idnoeud = index + 1;
            l = l + " " + Idnoeud;


            for (int voisin = 0; voisin < n; voisin++)
            {

                if (Adjacence[index, voisin] == 1 && !visite[voisin])
                {
                    l = ProfondeurRecursive(voisin, visite, l) + "/";
                }
            }
            return l;
        }

        public void Largeur(int index)
        {
            bool[] visite = new bool[n];
            List<Noeud> file = new List<Noeud> { Noeuds[index] };

            string l = LargeurRecursive(file, visite, "");

            Console.WriteLine(l);
        }
        private string LargeurRecursive(List<Noeud> file, bool[] visite, string l)
        {
            if (file.Count == 0) return l;

            Noeud noeud = file[0];
            file.RemoveAt(0);
            l += " " + noeud.id;

            int index = noeud.id - 1;
            visite[index] = true;

            
            for (int voisin = 0; voisin < n; voisin++)
            {
                if (Adjacence[index, voisin] == 1 && !visite[voisin])
                {
                    file.Add(Noeuds[voisin]);
                    visite[voisin] = true;
                }
            }
            return LargeurRecursive(file, visite, l);
        }
    }
}
