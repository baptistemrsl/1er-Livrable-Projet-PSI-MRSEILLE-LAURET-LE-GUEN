using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;  

namespace VISUALISATION_GRAPHE_PROJET_PSI
{
    public partial class Form1 : Form
    {
        private Graph graph;

        public Form1()
        {
            InitializeComponent();
            this.graph = new Graph(); /// Créer une instance du graphe
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Visualisation graphe";  
            this.ClientSize = new System.Drawing.Size(800, 600);  /// Définir la taille du formulaire

            /// Charger les fichiers de données dès que le formulaire est chargé
            graph.LoadFromMatrixMarketText(@"soc-karate.mtx");
            //graph.LoadFromHtmlText(@"readme.html");

            ///(redessiner le formulaire)Ca appelle l'événement Paint qui redessinera le graph
            this.Invalidate();  
        }

        /// Méthode pour dessiner le graphe
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; 

            /// Vérification si les arêtes et nœuds sont bien chargés
            if (graph.Edges.Count == 0)
            {
                g.DrawString("Aucune arête chargée.", new Font("Arial", 12), Brushes.Red, new PointF(100, 100));
                return;
            }

            /// Variables pour l'espacement des nœuds
            int offsetX = 100;
            int offsetY = 100;
            int nodeDistance = 200; 

            /// Dessiner les arêtes
            foreach (var edge in graph.Edges)
            {
                int node1 = edge.Item1;
                int node2 = edge.Item2;

                /// Positionnement des nœuds
                PointF point1 = new PointF(offsetX + (node1 % 10) * nodeDistance, offsetY + (node1 / 10) * nodeDistance);
                PointF point2 = new PointF(offsetX + (node2 % 10) * nodeDistance, offsetY + (node2 / 10) * nodeDistance);

                /// Dessiner une ligne (arête)
                g.DrawLine(Pens.Black, point1, point2);
            }

            /// Dessiner les nœuds
            foreach (var node in graph.Nodes)
            {
                /// Positionnement des nœuds
                PointF point = new PointF(offsetX + (node % 10) * nodeDistance, offsetY + (node / 10) * nodeDistance);

                /// Dessiner un cercle bleu pour le nœud
                g.FillEllipse(Brushes.Blue, point.X - 15, point.Y - 15, 30, 30);
                g.DrawEllipse(Pens.Black, point.X - 15, point.Y - 15, 30, 30);  /// Contour du cercle
                g.DrawString(node.ToString(), new Font("Arial", 10), Brushes.White, point.X - 10, point.Y - 10); /// Affichage du label du nœud
            }

            g.DrawString("Form1_Visualisation", new Font("Arial", 12), Brushes.Green, new PointF(300, 50));
        }
    }

    public class Graph
    {
        /// Liste des nœuds
        public List<int> Nodes { get; private set; }

        /// Liste des arêtes, chaque arête est représentée par un tuple (nœud1, nœud2)
        public List<Tuple<int, int>> Edges { get; private set; }

        /// Constructeur de la classe Graph
        public Graph()
        {
            Nodes = new List<int>();   /// liste des nœuds
            Edges = new List<Tuple<int, int>>(); /// liste des arêtes
        }

       
        ///Méthode pour charger les données depuis un fichier texte
        public void LoadFromMatrixMarketText(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show("Le fichier n'existe pas.");
                    return;
                }

                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    bool inMatrixData = false; /// Pour savoir quand commencer à traiter les données des arêtes

                    while ((line = sr.ReadLine()) != null)
                    {
                        /// Pour ignorer les lignes de commentaires ou de métadonnées
                        if (line.StartsWith("%"))
                            continue;

                        if (!inMatrixData)
                        {
                            var firstLine = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (firstLine.Length == 3)
                            {
                                /// Récupérer le nombre de nœuds et d'arêtes
                                int numNodes = int.Parse(firstLine[0]);
                                int numEdges = int.Parse(firstLine[2]);

                                inMatrixData = true;
                            }
                        }
                        else
                        {
                            var values = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (values.Length >= 2)
                            {
                                try
                                {
                                    int node1 = int.Parse(values[0]) - 1;  
                                    int node2 = int.Parse(values[1]) - 1;

                                    Edges.Add(new Tuple<int, int>(node1, node2));
                                    if (!Nodes.Contains(node1))
                                        Nodes.Add(node1);
                                    if (!Nodes.Contains(node2))
                                        Nodes.Add(node2);
                                }
                                catch (FormatException ex)
                                {
                                    MessageBox.Show($"Erreur sur la ligne '{line}': {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement du fichier texte : {ex.Message}");
            }
        }

        /// Charger le fichier html
        public void LoadFromHtmlText(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show("Le fichier texte n'existe pas.");
                    return;
                }

                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    string title = null;
                    string author = null;
                    string year = null;

                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();

                        if (line.StartsWith("title =", StringComparison.OrdinalIgnoreCase))
                        {
                            title = ExtractValue(line);
                        }
                        else if (line.StartsWith("author =", StringComparison.OrdinalIgnoreCase))
                        {
                            author = ExtractValue(line);
                        }
                        else if (line.StartsWith("year =", StringComparison.OrdinalIgnoreCase))
                        {
                            year = ExtractValue(line);
                        }
                    }

                    MessageBox.Show("Title: " + (title ?? "Not found"));
                    MessageBox.Show("Author: " + (author ?? "Not found"));
                    MessageBox.Show("Year: " + (year ?? "Not found"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement du fichier texte : {ex.Message}");
            }
        }
        private string ExtractValue(string line)
        {
            int start = line.IndexOf('{');
            int end = line.LastIndexOf('}');

            if (start != -1 && end != -1 && end > start)
            {
                return line.Substring(start + 1, end - start - 1).Trim();
            }

            return null;
        }


    }
}

