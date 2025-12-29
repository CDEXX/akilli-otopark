using System;
using System.Drawing;
using System.Windows.Forms;

namespace GP_Project
{
    public partial class mainForm : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private Button activeButton = null;
        private Form activeForm = null;

        private settingsMenuForm settingsForm;

        public mainForm()
        {
            InitializeComponent();

            // Bind dragging events to the panel
            panel12.MouseDown += Panel12_MouseDown;
            panel12.MouseMove += Panel12_MouseMove;
            panel12.MouseUp += Panel12_MouseUp;
        }

        // =================================================================
        // UPDATE USERNAME METHOD (Called from Login Form)
        // =================================================================
        public void KullaniciAdiGuncelle(string kullaniciAdi)
        {
            if (label4 != null)
            {
                label4.Text = kullaniciAdi;
            }
        }

        // =================================================================
        // HELPER: CUSTOM WARNING FORM
        // =================================================================
        private void ShowCustomWarning(string type, string message)
        {
            customWarningForm warningForm = new customWarningForm();
            warningForm.SetPopupStyle(type, message);
            warningForm.ShowDialog();
        }

        // =================================================================
        // FORM LOAD (GUEST CHECK IS HERE)
        // =================================================================
        private void mainForm_Load(object sender, EventArgs e)
        {
            settingsPanel.Visible = false;
            UIHelper.SetRoundedRegion(settingsPanel, 15);
            UIHelper.SetRoundedRegion(this, 20);

            // --- GUEST CONTROL ---
            // If the logged-in user is "Guest", apply restrictions
            if (label4.Text == "Guest" || label4.Text == "Misafir")
            {
                // 1. Warning Message using Custom Form
                ShowCustomWarning("Warning", "You have logged in with restricted Guest privileges.\n\nYou can only view the 'Parking Area'.");

                // 2. Hide other menu buttons
                btnDashboard.Visible = false;
                btnManagement.Visible = false;
                btnOverview.Visible = false;
                btnSettings.Visible = false;

                // 3. Open Parking Area by default
                UIHelper.ActivateMenuButton(ref activeButton, btnParkingArea);
                UIHelper.SetRoundedRegion(btnParkingArea, 10);
                OpenChildForm(new parkingAreaForm());
            }
            else
            {
                // --- NORMAL (ADMIN) LOGIN ---
                // All buttons visible, open Dashboard by default

                UIHelper.ActivateMenuButton(ref activeButton, btnDashboard);
                UIHelper.SetRoundedRegion(btnDashboard, 10);

                // Create fake data if empty
                if (parkingAreaForm._sahteVeritabani == null || parkingAreaForm._sahteVeritabani.Count == 0)
                    parkingAreaForm.OlusturSahteVeri();

                dashboardForm dash = new dashboardForm();
                dash.VerileriGuncelle(parkingAreaForm._sahteVeritabani);

                OpenChildForm(dash);
            }
        }

        // =================================================================
        // OTHER METHODS
        // =================================================================

        private void OpenChildForm(Form childForm)
        {
            if (childForm == null) return;

            if (activeForm != null)
            {
                activeForm.Close();
                activeForm.Dispose();
                activeForm = null;
            }

            activeForm = childForm;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panelContainer.SuspendLayout();
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(childForm);
            panelContainer.Tag = childForm;
            panelContainer.ResumeLayout();

            if (childForm is ITitledPage pageInfo)
            {
                lblHeader.Text = pageInfo.PageTitle;
                pbHeaderIcon.Image = pageInfo.PageIcon;
                pbHeaderIcon.Visible = pageInfo.PageIcon != null;
            }
            else
            {
                lblHeader.Text = "";
                pbHeaderIcon.Image = null;
                pbHeaderIcon.Visible = false;
            }

            childForm.BringToFront();
            childForm.Show();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // --- FORM DRAGGING ---
        private void Panel12_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Panel12_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            this.Location = Point.Add(dragFormPoint, new Size(diff));
        }

        private void Panel12_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            loginForm loginForm = new loginForm();
            loginForm.Show();
            this.Hide();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            settingsPanel.Visible = true;
            settingsPanel.BringToFront();

            if (settingsForm == null || settingsForm.IsDisposed)
            {
                settingsPanel.Controls.Clear();
                settingsForm = new settingsMenuForm(settingsPanel);
                settingsForm.TopLevel = false;
                settingsForm.FormBorderStyle = FormBorderStyle.None;
                settingsForm.Dock = DockStyle.Fill;
                settingsForm.Location = new Point(0, 0);
                settingsForm.Margin = Padding.Empty;
                settingsPanel.Padding = Padding.Empty;
                settingsPanel.Controls.Add(settingsForm);
                settingsForm.Show();
            }
            else
            {
                settingsForm.BringToFront();
            }
        }

        private void btnParkingArea_Click(object sender, EventArgs e)
        {
            UIHelper.ActivateMenuButton(ref activeButton, (Button)sender);
            UIHelper.SetRoundedRegion(btnParkingArea, 10);
            OpenChildForm(new parkingAreaForm());
        }

        private void btnManagement_Click(object sender, EventArgs e)
        {
            UIHelper.ActivateMenuButton(ref activeButton, (Button)sender);
            UIHelper.SetRoundedRegion(btnManagement, 10);
            OpenChildForm(new managementForm());
        }

        private void btnOverview_Click(object sender, EventArgs e)
        {
            UIHelper.ActivateMenuButton(ref activeButton, (Button)sender);
            UIHelper.SetRoundedRegion(btnOverview, 10);
            OpenChildForm(new overviewForm());
        }

        public void DisaridanOverviewAc(string parkYeriId)
        {
            UIHelper.ActivateMenuButton(ref activeButton, btnOverview);
            UIHelper.SetRoundedRegion(btnOverview, 10);
            overviewForm yeniSayfa = new overviewForm();
            OpenChildForm(yeniSayfa);
            yeniSayfa.DisaridanParkYeriSec(parkYeriId);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            UIHelper.ActivateMenuButton(ref activeButton, (Button)sender);
            UIHelper.SetRoundedRegion(btnDashboard, 10);

            if (parkingAreaForm._sahteVeritabani == null || parkingAreaForm._sahteVeritabani.Count == 0)
                parkingAreaForm.OlusturSahteVeri();

            dashboardForm dash = new dashboardForm();
            dash.VerileriGuncelle(parkingAreaForm._sahteVeritabani);
            OpenChildForm(dash);
        }

        // Empty events
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void panelContainer_Paint(object sender, PaintEventArgs e) { }
    }
}