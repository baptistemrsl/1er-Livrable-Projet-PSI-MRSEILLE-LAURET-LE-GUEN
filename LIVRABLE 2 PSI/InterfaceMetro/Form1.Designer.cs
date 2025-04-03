namespace InterfaceMetro
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboDepart;
        private System.Windows.Forms.ComboBox comboArrivee;
        private System.Windows.Forms.Button btnCalculer;
        private System.Windows.Forms.PictureBox pictureBoxCarte;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboDepart = new System.Windows.Forms.ComboBox();
            this.comboArrivee = new System.Windows.Forms.ComboBox();
            this.btnCalculer = new System.Windows.Forms.Button();
            this.pictureBoxCarte = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCarte)).BeginInit();
            this.SuspendLayout();
            // 
            // comboDepart
            // 
            this.comboDepart.FormattingEnabled = true;
            this.comboDepart.Location = new System.Drawing.Point(30, 30);
            this.comboDepart.Name = "comboDepart";
            this.comboDepart.Size = new System.Drawing.Size(200, 21);
            this.comboDepart.TabIndex = 0;
            // 
            // comboArrivee
            // 
            this.comboArrivee.FormattingEnabled = true;
            this.comboArrivee.Location = new System.Drawing.Point(30, 70);
            this.comboArrivee.Name = "comboArrivee";
            this.comboArrivee.Size = new System.Drawing.Size(200, 21);
            this.comboArrivee.TabIndex = 1;
            // 
            // btnCalculer
            // 
            this.btnCalculer.Location = new System.Drawing.Point(30, 110);
            this.btnCalculer.Name = "btnCalculer";
            this.btnCalculer.Size = new System.Drawing.Size(200, 30);
            this.btnCalculer.TabIndex = 2;
            this.btnCalculer.Text = "Afficher le chemin";
            this.btnCalculer.UseVisualStyleBackColor = true;
            this.btnCalculer.Click += new System.EventHandler(this.btnCalculer_Click);
            // 
            // pictureBoxCarte
            // 
            this.pictureBoxCarte.Location = new System.Drawing.Point(250, 10);
            this.pictureBoxCarte.Name = "pictureBoxCarte";
            this.pictureBoxCarte.Size = new System.Drawing.Size(800, 800);
            this.pictureBoxCarte.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCarte.TabIndex = 3;
            this.pictureBoxCarte.TabStop = false;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1080, 820);
            this.Controls.Add(this.pictureBoxCarte);
            this.Controls.Add(this.btnCalculer);
            this.Controls.Add(this.comboArrivee);
            this.Controls.Add(this.comboDepart);
            this.Name = "Form1";
            this.Text = "Trajet Métro Paris";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCarte)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
