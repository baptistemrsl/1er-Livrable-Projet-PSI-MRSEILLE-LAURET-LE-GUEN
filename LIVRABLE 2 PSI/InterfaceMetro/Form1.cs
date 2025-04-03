using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LIVRABLE_2_PSI;

namespace InterfaceMetro
{
    public partial class Form1 : Form
    {
        private MetroGraphBuilder builder;
        private Graphe<Station> graphe;
        private Chemins<Station> chemins;
        private Bitmap fond;

        public Form1()
        {
            InitializeComponent();
            InitialiserGrapheEtCarte();
        }

        private void InitialiserGrapheEtCarte()
        {
            builder = new MetroGraphBuilder("MetroParis.xlsx");
            graphe = builder.ConstruireGraphe();
            chemins = new Chemins<Station>(graphe);
            chemins.CalculerFloydWarshall();

            comboDepart.DisplayMember = "Valeur";
            comboArrivee.DisplayMember = "Valeur";

            var items = builder.NoeudParId.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray();
            comboDepart.Items.AddRange(items);
            comboArrivee.Items.AddRange(items);

            // Chargement de l'image de fond
            if (System.IO.File.Exists("fondMetro.png"))
            {
                fond = new Bitmap("fondMetro.png");
                pictureBoxCarte.Image = new Bitmap(fond);
            }
            else
            {
                MessageBox.Show("L’image fondMetro.png est manquante !");
            }
        }

        private void btnCalculer_Click(object sender, EventArgs e)
        {
            if (comboDepart.SelectedItem is not Noeud<Station> depart ||
                comboArrivee.SelectedItem is not Noeud<Station> arrivee)
            {
                MessageBox.Show("Veuillez choisir une station de départ et une station d’arrivée.");
                return;
            }

            var (distance, chemin) = chemins.Dijkstra(depart, arrivee);
            if (chemin == null || chemin.Count == 0)
            {
                MessageBox.Show("Aucun chemin trouvé entre les stations sélectionnées.");
                return;
            }

            if (fond == null)
            {
                MessageBox.Show("L’image de fond n’est pas chargée.");
                return;
            }

            Bitmap bmp = new Bitmap(fond); // Copie du fond pour dessin
            using Graphics g = Graphics.FromImage(bmp);
            Pen ligneChemin = new Pen(Color.Blue, 2);
            Brush pointChemin = Brushes.Red;

            for (int i = 0; i < chemin.Count - 1; i++)
            {
                Point p1 = MapProjection.Projection(chemin[i].Valeur);
                Point p2 = MapProjection.Projection(chemin[i + 1].Valeur);
                g.DrawLine(ligneChemin, p1, p2);
            }

            foreach (var station in chemin)
            {
                Point pt = MapProjection.Projection(station.Valeur);
                g.FillEllipse(pointChemin, pt.X - 4, pt.Y - 4, 8, 8);
            }

            pictureBoxCarte.Image = bmp;
        }
    }
}

