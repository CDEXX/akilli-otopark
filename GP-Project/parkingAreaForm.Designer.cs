namespace GP_Project
{
    partial class parkingAreaForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(parkingAreaForm));
            this.chkDuzenlemeModu = new System.Windows.Forms.CheckBox();
            this.btnDuzenlemeyiKaydet = new System.Windows.Forms.Button();
            this.lblCurrentFloor = new System.Windows.Forms.Label();
            this.btnNextFloor = new System.Windows.Forms.Button();
            this.btnPrevFloor = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlNavigation = new System.Windows.Forms.Panel();
            this.panelLine2 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnYerEkle_Click = new System.Windows.Forms.Button();
            this.btnKatEkle = new System.Windows.Forms.Button();
            this.pnlWorld = new GP_Project.DoubleBufferedPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlNavigation.SuspendLayout();
            this.pnlWorld.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDuzenlemeModu
            // 
            this.chkDuzenlemeModu.AutoSize = true;
            this.chkDuzenlemeModu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkDuzenlemeModu.Location = new System.Drawing.Point(21, 12);
            this.chkDuzenlemeModu.Name = "chkDuzenlemeModu";
            this.chkDuzenlemeModu.Size = new System.Drawing.Size(117, 20);
            this.chkDuzenlemeModu.TabIndex = 99;
            this.chkDuzenlemeModu.Text = "Editing Mode";
            this.chkDuzenlemeModu.UseVisualStyleBackColor = true;
            this.chkDuzenlemeModu.CheckedChanged += new System.EventHandler(this.chkDuzenlemeModu_CheckedChanged);
            // 
            // btnDuzenlemeyiKaydet
            // 
            this.btnDuzenlemeyiKaydet.BackColor = System.Drawing.Color.White;
            this.btnDuzenlemeyiKaydet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDuzenlemeyiKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDuzenlemeyiKaydet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDuzenlemeyiKaydet.ForeColor = System.Drawing.Color.Black;
            this.btnDuzenlemeyiKaydet.Location = new System.Drawing.Point(179, 7);
            this.btnDuzenlemeyiKaydet.Name = "btnDuzenlemeyiKaydet";
            this.btnDuzenlemeyiKaydet.Size = new System.Drawing.Size(97, 29);
            this.btnDuzenlemeyiKaydet.TabIndex = 100;
            this.btnDuzenlemeyiKaydet.Text = "Save";
            this.btnDuzenlemeyiKaydet.UseVisualStyleBackColor = false;
            this.btnDuzenlemeyiKaydet.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblCurrentFloor
            // 
            this.lblCurrentFloor.AutoSize = true;
            this.lblCurrentFloor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblCurrentFloor.Location = new System.Drawing.Point(9, 9);
            this.lblCurrentFloor.Name = "lblCurrentFloor";
            this.lblCurrentFloor.Size = new System.Drawing.Size(14, 20);
            this.lblCurrentFloor.TabIndex = 2;
            this.lblCurrentFloor.Text = ".";
            // 
            // btnNextFloor
            // 
            this.btnNextFloor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNextFloor.FlatAppearance.BorderSize = 0;
            this.btnNextFloor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextFloor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnNextFloor.ForeColor = System.Drawing.Color.White;
            this.btnNextFloor.Location = new System.Drawing.Point(467, 534);
            this.btnNextFloor.Name = "btnNextFloor";
            this.btnNextFloor.Size = new System.Drawing.Size(50, 36);
            this.btnNextFloor.TabIndex = 1;
            this.btnNextFloor.Text = ">";
            this.btnNextFloor.UseVisualStyleBackColor = true;
            this.btnNextFloor.Click += new System.EventHandler(this.btnNextFloor_Click);
            // 
            // btnPrevFloor
            // 
            this.btnPrevFloor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrevFloor.FlatAppearance.BorderSize = 0;
            this.btnPrevFloor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevFloor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnPrevFloor.ForeColor = System.Drawing.Color.White;
            this.btnPrevFloor.Location = new System.Drawing.Point(363, 534);
            this.btnPrevFloor.Name = "btnPrevFloor";
            this.btnPrevFloor.Size = new System.Drawing.Size(50, 36);
            this.btnPrevFloor.TabIndex = 0;
            this.btnPrevFloor.Text = "<";
            this.btnPrevFloor.UseVisualStyleBackColor = true;
            this.btnPrevFloor.Click += new System.EventHandler(this.btnPrevFloor_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Black;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(788, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 106;
            this.label4.Text = "Defective";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Black;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(771, 12);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(18, 18);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 107;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Black;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(682, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(18, 18);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 105;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(595, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(18, 18);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 104;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(699, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 103;
            this.label2.Text = "Occupied";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(612, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 16);
            this.label1.TabIndex = 102;
            this.label1.Text = "Avaiable";
            // 
            // pnlNavigation
            // 
            this.pnlNavigation.BackColor = System.Drawing.Color.Black;
            this.pnlNavigation.Controls.Add(this.lblCurrentFloor);
            this.pnlNavigation.Location = new System.Drawing.Point(416, 532);
            this.pnlNavigation.Name = "pnlNavigation";
            this.pnlNavigation.Size = new System.Drawing.Size(45, 40);
            this.pnlNavigation.TabIndex = 101;
            // 
            // panelLine2
            // 
            this.panelLine2.BackColor = System.Drawing.Color.Gray;
            this.panelLine2.Location = new System.Drawing.Point(1, 44);
            this.panelLine2.Name = "panelLine2";
            this.panelLine2.Size = new System.Drawing.Size(872, 1);
            this.panelLine2.TabIndex = 108;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gray;
            this.panel2.Location = new System.Drawing.Point(1, 526);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(872, 1);
            this.panel2.TabIndex = 109;
            // 
            // btnYerEkle_Click
            // 
            this.btnYerEkle_Click.BackColor = System.Drawing.Color.White;
            this.btnYerEkle_Click.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnYerEkle_Click.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYerEkle_Click.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYerEkle_Click.ForeColor = System.Drawing.Color.Black;
            this.btnYerEkle_Click.Location = new System.Drawing.Point(282, 7);
            this.btnYerEkle_Click.Name = "btnYerEkle_Click";
            this.btnYerEkle_Click.Size = new System.Drawing.Size(97, 29);
            this.btnYerEkle_Click.TabIndex = 110;
            this.btnYerEkle_Click.Text = "Add Spot";
            this.btnYerEkle_Click.UseVisualStyleBackColor = false;
            this.btnYerEkle_Click.Click += new System.EventHandler(this.btnYerEkle_Click_Click);
            // 
            // btnKatEkle
            // 
            this.btnKatEkle.BackColor = System.Drawing.Color.White;
            this.btnKatEkle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKatEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKatEkle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKatEkle.ForeColor = System.Drawing.Color.Black;
            this.btnKatEkle.Location = new System.Drawing.Point(385, 7);
            this.btnKatEkle.Name = "btnKatEkle";
            this.btnKatEkle.Size = new System.Drawing.Size(97, 29);
            this.btnKatEkle.TabIndex = 111;
            this.btnKatEkle.Text = "Add Floor";
            this.btnKatEkle.UseVisualStyleBackColor = false;
            this.btnKatEkle.Click += new System.EventHandler(this.btnKatEkle_Click);
            // 
            // pnlWorld
            // 
            this.pnlWorld.BackColor = System.Drawing.Color.Black;
            this.pnlWorld.Controls.Add(this.panel1);
            this.pnlWorld.Location = new System.Drawing.Point(0, 44);
            this.pnlWorld.Name = "pnlWorld";
            this.pnlWorld.Size = new System.Drawing.Size(872, 482);
            this.pnlWorld.TabIndex = 1;
            this.pnlWorld.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlWorld_Paint_1);
            this.pnlWorld.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlWorld_MouseDown);
            this.pnlWorld.MouseEnter += new System.EventHandler(this.pnlWorld_MouseEnter);
            this.pnlWorld.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlWorld_MouseMove);
            this.pnlWorld.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlWorld_MouseUp);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(872, 483);
            this.panel1.TabIndex = 96;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint_1);
            // 
            // parkingAreaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(21)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(872, 575);
            this.Controls.Add(this.btnKatEkle);
            this.Controls.Add(this.btnYerEkle_Click);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelLine2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnNextFloor);
            this.Controls.Add(this.btnPrevFloor);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlWorld);
            this.Controls.Add(this.pnlNavigation);
            this.Controls.Add(this.btnDuzenlemeyiKaydet);
            this.Controls.Add(this.chkDuzenlemeModu);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "parkingAreaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.parkingAreaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlNavigation.ResumeLayout(false);
            this.pnlNavigation.PerformLayout();
            this.pnlWorld.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkDuzenlemeModu;
        private System.Windows.Forms.Button btnDuzenlemeyiKaydet;
        private System.Windows.Forms.Label lblCurrentFloor;
        private System.Windows.Forms.Button btnNextFloor;
        private System.Windows.Forms.Button btnPrevFloor;
        private DoubleBufferedPanel pnlWorld;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlNavigation;
        private System.Windows.Forms.Panel panelLine2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnYerEkle_Click;
        private System.Windows.Forms.Button btnKatEkle;
    }
}