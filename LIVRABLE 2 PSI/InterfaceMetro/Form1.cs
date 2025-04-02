using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LIVRABLE_2_PSI;

namespace InterfaceMetro
{

    public partial class Form1 : Form
    {
        private LIVRABLE_2_PSI.MetroGraphBuilder builder;
        private LIVRABLE_2_PSI.Graphe<LIVRABLE_2_PSI.Station> graphe;
        private LIVRABLE_2_PSI.Chemins<LIVRABLE_2_PSI.Station> chemins;

        public Form1()
        {
            InitializeComponent();
            InitialiserGraphe();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Tu peux laisser vide ou initialiser des choses ici
        }


        private void InitialiserGraphe()
        {
            builder = new LIVRABLE_2_PSI.MetroGraphBuilder("MetroParis.xlsx");
            graphe = builder.ConstruireGraphe();
            chemins = new LIVRABLE_2_PSI.Chemins<LIVRABLE_2_PSI.Station>(graphe);
            chemins.CalculerFloydWarshall();

            comboDepart.DisplayMember = "Valeur";
            comboArrivee.DisplayMember = "Valeur";

            var items = builder.NoeudParId.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            comboDepart.Items.AddRange(items.ToArray());
            comboArrivee.Items.AddRange(items.ToArray());
        }

        private void btnCalculer_Click(object sender, EventArgs e)
        {
            if (comboDepart.SelectedItem is not LIVRABLE_2_PSI.Noeud<LIVRABLE_2_PSI.Station> depart ||
                comboArrivee.SelectedItem is not LIVRABLE_2_PSI.Noeud<LIVRABLE_2_PSI.Station> arrivee)
            {
                MessageBox.Show("Veuillez sélectionner une station de départ et une d'arrivée.");
                return;
            }

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Départ : {depart.Valeur.Nom} (Ligne {depart.Valeur.Ligne})");
            sb.AppendLine($"Arrivée : {arrivee.Valeur.Nom} (Ligne {arrivee.Valeur.Ligne})\n");

            (string nom, double temps, long ms, System.Collections.Generic.List<LIVRABLE_2_PSI.Noeud<LIVRABLE_2_PSI.Station>> chemin) meilleur = ("", double.PositiveInfinity, 0, null);

            var algos = new[] {
                ("Dijkstra", new Func<(double, System.Collections.Generic.List<LIVRABLE_2_PSI.Noeud<LIVRABLE_2_PSI.Station>>)>(() => chemins.Dijkstra(depart, arrivee))),
                ("Bellman-Ford", new Func<(double, System.Collections.Generic.List<LIVRABLE_2_PSI.Noeud<LIVRABLE_2_PSI.Station>>)>(() => chemins.BellmanFord(depart, arrivee))),
                ("Floyd-Warshall", new Func<(double, System.Collections.Generic.List<LIVRABLE_2_PSI.Noeud<LIVRABLE_2_PSI.Station>>)>(() => chemins.FloydWarshall(depart, arrivee)))
            };

            foreach (var (nom, methode) in algos)
            {
                var sw = Stopwatch.StartNew();
                var (temps, chemin) = methode();
                sw.Stop();

                sb.AppendLine($"[{nom}] → {temps} min en {sw.ElapsedMilliseconds} ms");

                if (temps < meilleur.temps)
                    meilleur = (nom, temps, sw.ElapsedMilliseconds, chemin);
            }

            sb.AppendLine("\n✔ Meilleur chemin trouvé par : " + meilleur.nom);
            if (meilleur.chemin == null)
            {
                sb.AppendLine("\n❌ Aucun chemin trouvé.");
            }
            else
            {
                sb.AppendLine("\n✔ Meilleur chemin trouvé par : " + meilleur.nom);
                foreach (var noeud in meilleur.chemin)
                {
                    sb.AppendLine($"- {noeud.Valeur.Nom} (Ligne {noeud.Valeur.Ligne})");
                }
            }
            foreach (var noeud in meilleur.chemin)
            {
                sb.AppendLine($"- {noeud.Valeur.Nom} (Ligne {noeud.Valeur.Ligne})");
            }

            txtResultat.Text = sb.ToString();
        }
    }

    public static class NoeudExtensions
    {
        public static string Valeur(this LIVRABLE_2_PSI.Noeud<LIVRABLE_2_PSI.Station> n)
        {
            return $"{n.Valeur.Nom} (Ligne {n.Valeur.Ligne})";
        }
    }
}
