using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GP_Project
{
    public partial class RegisterForm : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        // Property to send this username to Login form if registration is successful
        public string KayitlanKullaniciAdi { get; private set; }

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            UIHelper.SetRoundedRegion(this, 20);

            // Setting Placeholder texts
            if (txtAdSoyad != null) UIHelper.SetPlaceholder(txtAdSoyad, "Full Name");
            if (txtKullaniciAdi != null) UIHelper.SetPlaceholder(txtKullaniciAdi, "Username");
            if (txtEmail != null) UIHelper.SetPlaceholder(txtEmail, "Email Address");
            if (txtSifre != null) UIHelper.SetPlaceholder(txtSifre, "Password");
        }

        // --- REGISTER BUTTON ---
        private void btnKayitOl_Click_2(object sender, EventArgs e)
        {
            // 1. EMPTY FIELD CHECK
            if (string.IsNullOrWhiteSpace(txtAdSoyad.Text) ||
                txtAdSoyad.Text == "Full Name" || // Placeholder check (Must match the Load string)
                string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) ||
                string.IsNullOrWhiteSpace(txtSifre.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                ShowCustomWarning("Warning", "Please fill in all required fields.");
                return;
            }

            // 2. EMAIL FORMAT CHECK
            if (!EmailGecerliMi(txtEmail.Text))
            {
                ShowCustomWarning("Warning", "Please enter a valid email address.\n(Ex: name@example.com)");
                txtEmail.Focus();
                return;
            }

            // 3. PHONE CHECK (MaskedTextBox)
            if (txtTelefon != null && !txtTelefon.MaskCompleted)
            {
                ShowCustomWarning("Warning", "Please enter the phone number completely.");
                txtTelefon.Focus();
                return;
            }

            // 4. PASSWORD STRENGTH CHECK (Optional)
            if (lblSifreGucu.Text == "Weak")
            {
                DialogResult cvp = MessageBox.Show("Your password is very weak. Do you still want to continue?", "Security Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (cvp == DialogResult.No) return;
            }

            // 5. DATABASE REGISTRATION
            bool basarili = DatabaseHelper.KullaniciKaydet(
                txtAdSoyad.Text.Trim(),        // Full Name
                txtKullaniciAdi.Text.Trim(),   // Username
                txtSifre.Text,                 // Password
                txtEmail.Text.Trim(),          // Email
                txtTelefon.Text                // Phone
            );

            if (basarili)
            {
                KayitlanKullaniciAdi = txtKullaniciAdi.Text;
                this.DialogResult = DialogResult.OK; // Returns OK to Login form

                ShowCustomWarning("Success", "Registration Successfully Created! You can now log in.");

                // Return to Login Form
                loginForm login = new loginForm();
                login.Show();
                this.Close();
            }
            else
            {
                ShowCustomWarning("Error", "This username is already taken! Please choose another one.");
            }
        }

        // =======================================================================
        //  HELPER: CUSTOM WARNING FORM
        // =======================================================================
        private void ShowCustomWarning(string type, string message)
        {
            customWarningForm warningForm = new customWarningForm();
            warningForm.SetPopupStyle(type, message);
            warningForm.ShowDialog();
        }

        // --- HELPER METHODS ---

        private bool EmailGecerliMi(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException) { return false; }
        }

        private void txtSifre_TextChanged_1(object sender, EventArgs e)
        {
            string sifre = txtSifre.Text;
            int skor = 0;

            // --- DÜZELTME BURADA ---
            // Eğer kutu boşsa VEYA içinde Placeholder ("Password") yazıyorsa,
            // hesaplama yapma ve yazıyı temizle.
            if (string.IsNullOrEmpty(sifre) || sifre == "Password")
            {
                lblSifreGucu.Text = "";
                return;
            }

            // Kriterler
            if (sifre.Length >= 6) skor++;
            if (sifre.Length >= 10) skor++;
            if (Regex.IsMatch(sifre, @"[0-9]")) skor++;
            if (Regex.IsMatch(sifre, @"[A-Z]")) skor++;
            if (Regex.IsMatch(sifre, @"[a-z]")) skor++;
            if (Regex.IsMatch(sifre, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]")) skor++;

            // Skora göre durum
            if (skor < 3) { lblSifreGucu.Text = "Weak"; lblSifreGucu.ForeColor = Color.Red; }
            else if (skor < 5) { lblSifreGucu.Text = "Medium"; lblSifreGucu.ForeColor = Color.Orange; }
            else { lblSifreGucu.Text = "Strong"; lblSifreGucu.ForeColor = Color.Green; }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            loginForm login = new loginForm();
            login.Show();
            this.Close();
        }

        // --- FORM DRAGGING ---
        private void PanelPanel_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void PanelPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            this.Location = Point.Add(dragFormPoint, new Size(diff));
        }

        private void PanelPanel_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        // Empty events (Can be deleted)
        private void btnKayitOl_Click(object sender, EventArgs e) { }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblSifreGucu_Click(object sender, EventArgs e)
        {

        }
    }
}