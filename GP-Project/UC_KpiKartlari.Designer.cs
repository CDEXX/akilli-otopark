namespace GP_Project
{
    partial class UC_KpiKartlari
    {
        /// <summary> 
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Bileşen Tasarımcısı üretimi kod

        /// <summary> 
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblAracSayisi = new System.Windows.Forms.Label();
            this.lblDoluluk = new System.Windows.Forms.Label();
            this.lblCiro = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblAracSayisi
            // 
            this.lblAracSayisi.AutoSize = true;
            this.lblAracSayisi.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAracSayisi.ForeColor = System.Drawing.Color.White;
            this.lblAracSayisi.Location = new System.Drawing.Point(23, 20);
            this.lblAracSayisi.Name = "lblAracSayisi";
            this.lblAracSayisi.Size = new System.Drawing.Size(69, 23);
            this.lblAracSayisi.TabIndex = 0;
            this.lblAracSayisi.Text = "label1";
            // 
            // lblDoluluk
            // 
            this.lblDoluluk.AutoSize = true;
            this.lblDoluluk.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblDoluluk.ForeColor = System.Drawing.Color.White;
            this.lblDoluluk.Location = new System.Drawing.Point(127, 20);
            this.lblDoluluk.Name = "lblDoluluk";
            this.lblDoluluk.Size = new System.Drawing.Size(69, 23);
            this.lblDoluluk.TabIndex = 1;
            this.lblDoluluk.Text = "label2";
            // 
            // lblCiro
            // 
            this.lblCiro.AutoSize = true;
            this.lblCiro.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblCiro.ForeColor = System.Drawing.Color.White;
            this.lblCiro.Location = new System.Drawing.Point(230, 20);
            this.lblCiro.Name = "lblCiro";
            this.lblCiro.Size = new System.Drawing.Size(69, 23);
            this.lblCiro.TabIndex = 2;
            this.lblCiro.Text = "label3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.SlateGray;
            this.label1.Location = new System.Drawing.Point(19, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "OCCUPANCY";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.SlateGray;
            this.label2.Location = new System.Drawing.Point(140, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "RATE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.Color.SlateGray;
            this.label3.Location = new System.Drawing.Point(229, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "REVENUE";
            // 
            // UC_KpiKartlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(4)))), ((int)(((byte)(9)))));
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCiro);
            this.Controls.Add(this.lblDoluluk);
            this.Controls.Add(this.lblAracSayisi);
            this.Name = "UC_KpiKartlari";
            this.Size = new System.Drawing.Size(415, 101);
            this.Load += new System.EventHandler(this.UC_KpiKartlari_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAracSayisi;
        private System.Windows.Forms.Label lblDoluluk;
        private System.Windows.Forms.Label lblCiro;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
