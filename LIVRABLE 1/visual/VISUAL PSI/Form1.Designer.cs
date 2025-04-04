namespace VISUALISATION_GRAPHE_PROJET_PSI
{
    partial class Form1
    {
        /// Nécessaire pour les composants du concepteur.
        private System.ComponentModel.IContainer components = null;

        /// Libère les ressources utilisées.
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le concepteur

        /// Nécessaire pour la prise en charge du designer.
        /// Ne modifiez pas le contenu de cette méthode avec l'éditeur de code.
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "Mon premier formulaire Windows Forms";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

