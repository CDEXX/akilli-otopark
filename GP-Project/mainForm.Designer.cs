namespace GP_Project
{
    partial class mainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.mainPanel = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pbHeaderIcon = new System.Windows.Forms.PictureBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panelLine2 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panelLine3 = new System.Windows.Forms.Panel();
            this.panelLine1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnOverview = new System.Windows.Forms.Button();
            this.btnManagement = new System.Windows.Forms.Button();
            this.btnParkingArea = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeaderIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.Black;
            this.mainPanel.Controls.Add(this.buttonClose);
            this.mainPanel.Controls.Add(this.label5);
            this.mainPanel.Controls.Add(this.pictureBox1);
            this.mainPanel.Controls.Add(this.label6);
            this.mainPanel.Controls.Add(this.label4);
            this.mainPanel.Controls.Add(this.pbHeaderIcon);
            this.mainPanel.Controls.Add(this.lblHeader);
            this.mainPanel.Controls.Add(this.panelContainer);
            this.mainPanel.Controls.Add(this.panel13);
            this.mainPanel.Controls.Add(this.panelLine2);
            this.mainPanel.ForeColor = System.Drawing.Color.Gray;
            this.mainPanel.Location = new System.Drawing.Point(226, -1);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(875, 637);
            this.mainPanel.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.Black;
            this.buttonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.buttonClose.ForeColor = System.Drawing.Color.White;
            this.buttonClose.Location = new System.Drawing.Point(838, 9);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(34, 36);
            this.buttonClose.TabIndex = 100;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.ForeColor = System.Drawing.Color.Silver;
            this.label5.Location = new System.Drawing.Point(673, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 15);
            this.label5.TabIndex = 99;
            this.label5.Text = "Parking Attendant";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(787, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 36);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 101;
            this.pictureBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(652, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 18);
            this.label6.TabIndex = 102;
            this.label6.Text = "Welcome,";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(733, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 18);
            this.label4.TabIndex = 103;
            this.label4.Text = "Admin";
            // 
            // pbHeaderIcon
            // 
            this.pbHeaderIcon.Image = global::GP_Project.Properties.Resources.dashboardslateblue;
            this.pbHeaderIcon.Location = new System.Drawing.Point(22, 15);
            this.pbHeaderIcon.Name = "pbHeaderIcon";
            this.pbHeaderIcon.Size = new System.Drawing.Size(24, 24);
            this.pbHeaderIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbHeaderIcon.TabIndex = 98;
            this.pbHeaderIcon.TabStop = false;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblHeader.ForeColor = System.Drawing.Color.SlateBlue;
            this.lblHeader.Location = new System.Drawing.Point(49, 19);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(68, 15);
            this.lblHeader.TabIndex = 97;
            this.lblHeader.Text = "Dashboard";
            // 
            // panelContainer
            // 
            this.panelContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(21)))), ((int)(((byte)(24)))));
            this.panelContainer.Location = new System.Drawing.Point(0, 55);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(875, 582);
            this.panelContainer.TabIndex = 92;
            this.panelContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.panelContainer_Paint);
            // 
            // panel13
            // 
            this.panel13.Location = new System.Drawing.Point(0, 0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(881, 20);
            this.panel13.TabIndex = 89;
            this.panel13.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel12_MouseDown);
            this.panel13.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Panel12_MouseMove);
            this.panel13.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Panel12_MouseUp);
            // 
            // panelLine2
            // 
            this.panelLine2.BackColor = System.Drawing.Color.Gray;
            this.panelLine2.Location = new System.Drawing.Point(1, 53);
            this.panelLine2.Name = "panelLine2";
            this.panelLine2.Size = new System.Drawing.Size(872, 1);
            this.panelLine2.TabIndex = 96;
            // 
            // panel12
            // 
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(225, 20);
            this.panel12.TabIndex = 0;
            // 
            // panelLine3
            // 
            this.panelLine3.BackColor = System.Drawing.Color.Gray;
            this.panelLine3.ForeColor = System.Drawing.Color.Gray;
            this.panelLine3.Location = new System.Drawing.Point(0, 518);
            this.panelLine3.Name = "panelLine3";
            this.panelLine3.Size = new System.Drawing.Size(226, 1);
            this.panelLine3.TabIndex = 88;
            // 
            // panelLine1
            // 
            this.panelLine1.BackColor = System.Drawing.Color.Gray;
            this.panelLine1.ForeColor = System.Drawing.Color.Gray;
            this.panelLine1.Location = new System.Drawing.Point(0, 88);
            this.panelLine1.Name = "panelLine1";
            this.panelLine1.Size = new System.Drawing.Size(226, 1);
            this.panelLine1.TabIndex = 87;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Location = new System.Drawing.Point(31, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 15);
            this.label1.TabIndex = 86;
            this.label1.Text = "Park smart. Parkwise.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(93, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 25);
            this.label3.TabIndex = 80;
            this.label3.Text = "Parkwise";
            // 
            // settingsPanel
            // 
            this.settingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.settingsPanel.Location = new System.Drawing.Point(4, 388);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(215, 185);
            this.settingsPanel.TabIndex = 91;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.ForeColor = System.Drawing.Color.Gray;
            this.panel1.Location = new System.Drawing.Point(225, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 630);
            this.panel1.TabIndex = 86;
            // 
            // btnLogOut
            // 
            this.btnLogOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogOut.FlatAppearance.BorderSize = 0;
            this.btnLogOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnLogOut.ForeColor = System.Drawing.Color.Silver;
            this.btnLogOut.Image = ((System.Drawing.Image)(resources.GetObject("btnLogOut.Image")));
            this.btnLogOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogOut.Location = new System.Drawing.Point(6, 576);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(220, 48);
            this.btnLogOut.TabIndex = 90;
            this.btnLogOut.Text = "           Log Out";
            this.btnLogOut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogOut.UseVisualStyleBackColor = true;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            this.btnLogOut.Enter += new System.EventHandler(this.btnLogOut_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSettings.ForeColor = System.Drawing.Color.Silver;
            this.btnSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnSettings.Image")));
            this.btnSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettings.Location = new System.Drawing.Point(6, 527);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(220, 48);
            this.btnSettings.TabIndex = 89;
            this.btnSettings.Text = "           Settings";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnOverview
            // 
            this.btnOverview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOverview.FlatAppearance.BorderSize = 0;
            this.btnOverview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOverview.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnOverview.ForeColor = System.Drawing.Color.Silver;
            this.btnOverview.Image = ((System.Drawing.Image)(resources.GetObject("btnOverview.Image")));
            this.btnOverview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOverview.Location = new System.Drawing.Point(6, 269);
            this.btnOverview.Name = "btnOverview";
            this.btnOverview.Size = new System.Drawing.Size(213, 48);
            this.btnOverview.TabIndex = 84;
            this.btnOverview.Text = "           Overview";
            this.btnOverview.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOverview.UseVisualStyleBackColor = true;
            this.btnOverview.Click += new System.EventHandler(this.btnOverview_Click);
            // 
            // btnManagement
            // 
            this.btnManagement.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManagement.FlatAppearance.BorderSize = 0;
            this.btnManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManagement.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnManagement.ForeColor = System.Drawing.Color.Silver;
            this.btnManagement.Image = ((System.Drawing.Image)(resources.GetObject("btnManagement.Image")));
            this.btnManagement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManagement.Location = new System.Drawing.Point(6, 214);
            this.btnManagement.Name = "btnManagement";
            this.btnManagement.Size = new System.Drawing.Size(213, 48);
            this.btnManagement.TabIndex = 83;
            this.btnManagement.Text = "           Management";
            this.btnManagement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManagement.UseVisualStyleBackColor = true;
            this.btnManagement.Click += new System.EventHandler(this.btnManagement_Click);
            // 
            // btnParkingArea
            // 
            this.btnParkingArea.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnParkingArea.FlatAppearance.BorderSize = 0;
            this.btnParkingArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParkingArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnParkingArea.ForeColor = System.Drawing.Color.Silver;
            this.btnParkingArea.Image = ((System.Drawing.Image)(resources.GetObject("btnParkingArea.Image")));
            this.btnParkingArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnParkingArea.Location = new System.Drawing.Point(6, 159);
            this.btnParkingArea.Name = "btnParkingArea";
            this.btnParkingArea.Size = new System.Drawing.Size(213, 48);
            this.btnParkingArea.TabIndex = 82;
            this.btnParkingArea.Text = "           Parking Area";
            this.btnParkingArea.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnParkingArea.UseVisualStyleBackColor = true;
            this.btnParkingArea.Click += new System.EventHandler(this.btnParkingArea_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.Black;
            this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDashboard.ForeColor = System.Drawing.Color.Silver;
            this.btnDashboard.Image = ((System.Drawing.Image)(resources.GetObject("btnDashboard.Image")));
            this.btnDashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.Location = new System.Drawing.Point(6, 104);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(213, 48);
            this.btnDashboard.TabIndex = 81;
            this.btnDashboard.Text = "           Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(45, 13);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(60, 45);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 79;
            this.pictureBox5.TabStop = false;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1101, 635);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.btnLogOut);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.panelLine3);
            this.Controls.Add(this.panelLine1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOverview);
            this.Controls.Add(this.btnManagement);
            this.Controls.Add(this.btnParkingArea);
            this.Controls.Add(this.btnDashboard);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.mainPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "mainForm";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeaderIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel panelLine2;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Panel panelLine3;
        private System.Windows.Forms.Panel panelLine1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOverview;
        private System.Windows.Forms.Button btnManagement;
        private System.Windows.Forms.Button btnParkingArea;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.PictureBox pbHeaderIcon;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
    }
}
