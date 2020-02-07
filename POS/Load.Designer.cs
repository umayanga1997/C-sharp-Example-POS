namespace POS
{
    partial class Load
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Load));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.loading_pgrsbar = new System.Windows.Forms.ProgressBar();
            this.load_lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // loading_pgrsbar
            // 
            this.loading_pgrsbar.ForeColor = System.Drawing.Color.Black;
            this.loading_pgrsbar.Location = new System.Drawing.Point(12, 185);
            this.loading_pgrsbar.Name = "loading_pgrsbar";
            this.loading_pgrsbar.Size = new System.Drawing.Size(376, 23);
            this.loading_pgrsbar.TabIndex = 0;
            // 
            // load_lbl
            // 
            this.load_lbl.AutoSize = true;
            this.load_lbl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.load_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.load_lbl.ForeColor = System.Drawing.Color.Gold;
            this.load_lbl.Location = new System.Drawing.Point(11, 166);
            this.load_lbl.Name = "load_lbl";
            this.load_lbl.Size = new System.Drawing.Size(79, 15);
            this.load_lbl.TabIndex = 1;
            this.load_lbl.Text = "Please wait...";
            // 
            // Load
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(400, 250);
            this.Controls.Add(this.load_lbl);
            this.Controls.Add(this.loading_pgrsbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Load";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Load_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ProgressBar loading_pgrsbar;
        private System.Windows.Forms.Label load_lbl;
    }
}

