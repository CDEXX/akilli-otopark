using System;
using System.Drawing;
using System.Windows.Forms;

namespace GP_Project
{
    public partial class loginForm : Form
    {
        public loginForm()
        {
            InitializeComponent();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            // Create database tables (If they don't exist)
            DatabaseHelper.TablolariOlustur();

            // UI Design Settings
            UIHelper.SetRoundedRegion(this, 20);
            UIHelper.SetPlaceholder(textBox1, "Enter your email or username.");
            UIHelper.SetPlaceholder(textBox2, "*************");
        }

        // =========================================================
        // 1. STANDARD LOGIN BUTTON (buttonLogin)
        // =========================================================
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            // Empty Check (Checking against Placeholders)
            if (username == "Enter your email or username." || string.IsNullOrEmpty(username))
            {
                ShowCustomWarning("Warning", "Please enter a username.");
                return;
            }
            if (password == "*************" || string.IsNullOrEmpty(password))
            {
                ShowCustomWarning("Warning", "Please enter a password.");
                return;
            }

            // Database Check
            bool success = DatabaseHelper.KullaniciGirisKontrol(username, password);

            if (success)
            {
                // If successful, pass the Username to the main form
                AnaSayfayaGec(username);
            }
            else
            {
                ShowCustomWarning("Error", "Invalid username or password!");
            }
        }

        // =========================================================
        // 2. GUEST LOGIN BUTTON (btnMisafir / kryptonButton1)
        // =========================================================
        private void btnMisafir_Click(object sender, EventArgs e)
        {
            // Guest login bypasses password check
            // Sending "Guest" so mainForm can apply restrictions
            AnaSayfayaGec("Misafir");
        }

        // If your design uses kryptonButton1, keep this logic synced:
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            // Validation logic...
            if (username == "Enter your email or username." || string.IsNullOrEmpty(username)) return;

            bool success = DatabaseHelper.KullaniciGirisKontrol(username, password);

            if (success)
            {
                // Pass the real username typed in the box
                AnaSayfayaGec(username);
            }
            else
            {
                ShowCustomWarning("Error", "Invalid username or password!");
            }
        }

        // =========================================================
        // 3. REGISTER REDIRECT
        // =========================================================
        private void btnKayitOlGit_Click(object sender, EventArgs e)
        {
            RegisterForm regForm = new RegisterForm();
            this.Hide();

            if (regForm.ShowDialog() == DialogResult.OK)
            {
                // If registration successful, autofill username
                textBox1.Text = regForm.KayitlanKullaniciAdi;
                textBox1.ForeColor = Color.White;

                // Clear password field
                textBox2.Text = "";
                UIHelper.SetPlaceholder(textBox2, "*************");
                textBox2.Focus();
            }
            this.Show();
        }

        // =========================================================
        // HELPER METHODS
        // =========================================================

        // Switch to Main Page (Critical Method carrying data)
        private void AnaSayfayaGec(string incomingUsername)
        {
            mainForm anaForm = new mainForm();

            // --- PASS DATA TO MAIN FORM ---
            // Send the username (or "Guest") to the main form label
            anaForm.KullaniciAdiGuncelle(incomingUsername);
            // ------------------------------

            anaForm.Show();
            this.Hide();
        }

        private void ShowCustomWarning(string type, string message)
        {
            customWarningForm warningForm = new customWarningForm();
            warningForm.SetPopupStyle(type, message);
            warningForm.ShowDialog();
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Empty events
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox4_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}