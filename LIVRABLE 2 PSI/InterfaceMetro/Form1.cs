using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace LIVRABLE_2_PSI
{
    public partial class Form1 : Form
    {
        private Graphe<Station> graphe;
        private Dictionary<int, Noeud<Station>> noeudsParId;
        private Chemins<Station> chemins;
        private Bitmap fondCarte;
        private MaProjection projection;

        public Form1()
        {
            InitializeComponent();
            InitialiserGraphEtCarte();
        }

        private void InitialiserGraphEtCarte()
        {
            var builder = new CsvGraphBuilder("noeuds.csv", "arcs.csv");
            graphe = builder.ConstruireGraphe();
            noeudsParId = builder.NoeudParId;
            chemins = new Chemins<Station>(graphe);

            var stations = graphe.Noeuds.Select(n => n.Valeur).OrderBy(s => s.Nom).ToList();
            comboDepart.DataSource = new List<Station>(stations);
            comboArrivee.DataSource = new List<Station>(stations);

            // Charger la carte de fond
            fondCarte = new Bitmap("fondMetro.png"); // Mets le fichier image dans le dossier bin/Debug/netX
            projection = new MaProjection(48.82, 48.90, 2.27, 2.42, fondCarte.Width, fondCarte.Height);

            pictureBox.Image = new Bitmap(fondCarte);
        }

        private void btnTracer_Click(object sender, EventArgs e)
        {
            var depart = (Station)comboDepart.SelectedItem;
            var arrivee = (Station)comboArrivee.SelectedItem;
            if (depart == null || arrivee == null)
            {
                MessageBox.Show("Veuillez choisir deux stations.");
                return;
            }

            if (!noeudsParId.TryGetValue(depart.Id, out var nd1) || !noeudsParId.TryGetValue(arrivee.Id, out var nd2))
            {
                MessageBox.Show("Stations non trouvées dans le graphe.");
                return;
            }

            var (temps, chemin) = chemins.Dijkstra(nd1, nd2);

            if (chemin == null || chemin.Count == 0)
            {
                MessageBox.Show("Aucun chemin trouvé.");
                return;
            }

            // Dessiner
            var img = new Bitmap(fondCarte);
            using Graphics g = Graphics.FromImage(img);
            Pen ligne = new Pen(Color.Blue, 2);
            Brush rond = Brushes.Red;

            for (int i = 0; i < chemin.Count - 1; i++)
            {
                var p1 = projection.Projeter(chemin[i].Valeur);
                var p2 = projection.Projeter(chemin[i + 1].Valeur);
                g.DrawLine(ligne, p1.x, p1.y, p2.x, p2.y);
                g.FillEllipse(rond, p1.x - 3, p1.y - 3, 6, 6);
            }

            var last = projection.Projeter(chemin.Last().Valeur);
            g.FillEllipse(rond, last.x - 3, last.y - 3, 6, 6);

            pictureBox.Image?.Dispose();
            pictureBox.Image = img;
        }
    }
}

