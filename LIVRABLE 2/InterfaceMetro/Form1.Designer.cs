namespace LIVRABLE_2_PSI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ComboBox comboDepart;
        private System.Windows.Forms.ComboBox comboArrivee;
        private System.Windows.Forms.Button btnTracer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.comboDepart = new System.Windows.Forms.ComboBox();
            this.comboArrivee = new System.Windows.Forms.ComboBox();
            this.btnTracer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();

            /// pictureBox
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(800, 600);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;

            /// comboDepart
            this.comboDepart.FormattingEnabled = true;
            this.comboDepart.Location = new System.Drawing.Point(830, 50);
            this.comboDepart.Name = "comboDepart";
            this.comboDepart.Size = new System.Drawing.Size(200, 24);
            this.comboDepart.TabIndex = 1;

            /// comboArrivee
            this.comboArrivee.FormattingEnabled = true;
            this.comboArrivee.Location = new System.Drawing.Point(830, 100);
            this.comboArrivee.Name = "comboArrivee";
            this.comboArrivee.Size = new System.Drawing.Size(200, 24);
            this.comboArrivee.TabIndex = 2;

            /// btnTracer
            this.btnTracer.Location = new System.Drawing.Point(830, 150);
            this.btnTracer.Name = "btnTracer";
            this.btnTracer.Size = new System.Drawing.Size(200, 30);
            this.btnTracer.TabIndex = 3;
            this.btnTracer.Text = "Afficher le chemin";
            this.btnTracer.UseVisualStyleBackColor = true;
            this.btnTracer.Click += new System.EventHandler(this.btnTracer_Click);

            /// Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 640);
            this.Controls.Add(this.btnTracer);
            this.Controls.Add(this.comboArrivee);
            this.Controls.Add(this.comboDepart);
            this.Controls.Add(this.pictureBox);
            this.Name = "Form1";
            this.Text = "Visualisation du métro";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
