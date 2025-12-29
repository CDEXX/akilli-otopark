namespace GP_Project
{
    partial class ParkSpotMenu
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
            this.btnDoluIsaretle = new System.Windows.Forms.Button();
            this.btnBosIsaretle = new System.Windows.Forms.Button();
            this.btnArizaliBildir = new System.Windows.Forms.Button();
            this.btnKapat = new System.Windows.Forms.Button();
            this.Engellibuton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDoluIsaretle
            // 
            this.btnDoluIsaretle.BackColor = System.Drawing.Color.Linen;
            this.btnDoluIsaretle.Location = new System.Drawing.Point(0, 0);
            this.btnDoluIsaretle.Name = "btnDoluIsaretle";
            this.btnDoluIsaretle.Size = new System.Drawing.Size(180, 41);
            this.btnDoluIsaretle.TabIndex = 0;
            this.btnDoluIsaretle.Text = "Mark as Filled";
            this.btnDoluIsaretle.UseVisualStyleBackColor = false;
            this.btnDoluIsaretle.Click += new System.EventHandler(this.btnDoluIsaretle_Click_1);
            // 
            // btnBosIsaretle
            // 
            this.btnBosIsaretle.BackColor = System.Drawing.Color.Linen;
            this.btnBosIsaretle.Location = new System.Drawing.Point(0, 47);
            this.btnBosIsaretle.Name = "btnBosIsaretle";
            this.btnBosIsaretle.Size = new System.Drawing.Size(180, 41);
            this.btnBosIsaretle.TabIndex = 1;
            this.btnBosIsaretle.Text = "Mark as Empty";
            this.btnBosIsaretle.UseVisualStyleBackColor = false;
            this.btnBosIsaretle.Click += new System.EventHandler(this.btnBosIsaretle_Click);
            // 
            // btnArizaliBildir
            // 
            this.btnArizaliBildir.BackColor = System.Drawing.Color.Linen;
            this.btnArizaliBildir.Location = new System.Drawing.Point(0, 94);
            this.btnArizaliBildir.Name = "btnArizaliBildir";
            this.btnArizaliBildir.Size = new System.Drawing.Size(180, 41);
            this.btnArizaliBildir.TabIndex = 2;
            this.btnArizaliBildir.Text = "Mask as Defective";
            this.btnArizaliBildir.UseVisualStyleBackColor = false;
            this.btnArizaliBildir.Click += new System.EventHandler(this.btnArizaliBildir_Click);
            // 
            // btnKapat
            // 
            this.btnKapat.BackColor = System.Drawing.Color.Linen;
            this.btnKapat.Location = new System.Drawing.Point(0, 188);
            this.btnKapat.Name = "btnKapat";
            this.btnKapat.Size = new System.Drawing.Size(180, 41);
            this.btnKapat.TabIndex = 3;
            this.btnKapat.Text = "Close";
            this.btnKapat.UseVisualStyleBackColor = false;
            this.btnKapat.Click += new System.EventHandler(this.btnKapat_Click);
            // 
            // Engellibuton
            // 
            this.Engellibuton.BackColor = System.Drawing.Color.Linen;
            this.Engellibuton.Location = new System.Drawing.Point(0, 141);
            this.Engellibuton.Name = "Engellibuton";
            this.Engellibuton.Size = new System.Drawing.Size(180, 41);
            this.Engellibuton.TabIndex = 4;
            this.Engellibuton.Text = "Mark As Disabled";
            this.Engellibuton.UseVisualStyleBackColor = false;
            this.Engellibuton.Click += new System.EventHandler(this.Engellibuton_Click);
            // 
            // ParkSpotMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumPurple;
            this.ClientSize = new System.Drawing.Size(181, 237);
            this.Controls.Add(this.Engellibuton);
            this.Controls.Add(this.btnKapat);
            this.Controls.Add(this.btnArizaliBildir);
            this.Controls.Add(this.btnBosIsaretle);
            this.Controls.Add(this.btnDoluIsaretle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ParkSpotMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ParkSpotMenu";
            this.Deactivate += new System.EventHandler(this.ParkSpotMenu_Deactivate_1);
            this.Load += new System.EventHandler(this.ParkSpotMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDoluIsaretle;
        private System.Windows.Forms.Button btnBosIsaretle;
        private System.Windows.Forms.Button btnArizaliBildir;
        private System.Windows.Forms.Button btnKapat;
        private System.Windows.Forms.Button Engellibuton;
    }
}