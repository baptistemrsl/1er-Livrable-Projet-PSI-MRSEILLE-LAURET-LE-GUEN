using System.Drawing;
using System.Windows.Forms;
using System;

namespace InterfaceMetro
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private ComboBox comboDepart;
        private ComboBox comboArrivee;
        private TextBox txtResultat;
        private Button btnCalculer;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

            this.comboDepart = new ComboBox();
            this.comboArrivee = new ComboBox();
            this.txtResultat = new TextBox();
            this.btnCalculer = new Button();

            // Positionnement (tu peux adapter)
            this.comboDepart.Location = new Point(20, 20);
            this.comboArrivee.Location = new Point(20, 60);
            this.txtResultat.Location = new Point(20, 100);
            this.txtResultat.Multiline = true;
            this.txtResultat.Size = new Size(400, 200);
            this.btnCalculer.Location = new Point(20, 320);
            this.btnCalculer.Text = "Calculer";

            // Ajouter l’événement
            this.btnCalculer.Click += new EventHandler(this.btnCalculer_Click);

            // Ajout au formulaire
            this.Controls.Add(this.comboDepart);
            this.Controls.Add(this.comboArrivee);
            this.Controls.Add(this.txtResultat);
            this.Controls.Add(this.btnCalculer);

        }

        #endregion
    }
}

