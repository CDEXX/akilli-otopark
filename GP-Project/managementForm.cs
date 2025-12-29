using GP_Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;

namespace GP_Project
{
    public partial class managementForm : Form, ITitledPage
    {
        public string PageTitle => "Management";
        public Image PageIcon => Properties.Resources.managementslateblue;

        string dbPath = "Data Source=parkwise.db;Version=3;";

        public managementForm()
        {
            InitializeComponent();
        }

        private void managementForm_Load(object sender, EventArgs e)
        {
            UIHelper.SetPlaceholder(textBox1, "Search License Plate...");
            UIHelper.SetPlaceholder(textBox2, "Search License Plate...");

            // Rounding Panels
            if (panel1 != null) UIHelper.SetRoundedRegion(panel1, 20);
            if (panel2 != null) UIHelper.SetRoundedRegion(panel2, 20);
            if (panel5 != null) UIHelper.SetRoundedRegion(panel5, 20);
            if (panel7 != null) UIHelper.SetRoundedRegion(panel7, 20);
            if (panel9 != null) UIHelper.SetRoundedRegion(panel9, 20);
            if (panel10 != null) UIHelper.SetRoundedRegion(panel10, 20);

            // Generate fake data if empty (for testing)
            if (parkingAreaForm._sahteVeritabani.Count == 0)
            {
                parkingAreaForm.OlusturSahteVeri();
            }

            SetupGridSettings();
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

        private void SetupGridSettings()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;

            dataGridView1.ScrollBars = ScrollBars.Both;

            dataGridView1.DefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Font and Row Height
            dataGridView1.DefaultCellStyle.Font = new Font("Century Gothic", 10);
            dataGridView1.RowTemplate.Height = 40;

            // Color Theme
            dataGridView1.BackgroundColor = Color.Black;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.GridColor = Color.FromArgb(50, 50, 50);

            // Row Colors
            dataGridView1.DefaultCellStyle.BackColor = Color.Black;
            dataGridView1.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.SeaGreen;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            // Selection Mode
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
        }

        private void ClearLabels()
        {
            lblPlaka.Text = "---";        // Plate
            lblGuncelDurum.Text = "---";  // Status
            lblKonum.Text = "---";        // Location
            lblAracTuru.Text = "---";     // Type
            lblGirisSaati.Text = "---";   // Entry Time
            lblSure.Text = "---";         // Duration
            lblGuncelDurum.ForeColor = Color.White;
        }

        // Empty Events
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void panel9_Paint(object sender, PaintEventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        // =============================================================
        // SEARCH VEHICLE (LEFT PANEL)
        // =============================================================
        private void buttonSrc_Click_1(object sender, EventArgs e)
        {
            string rawInput = textBox1.Text;
            string searchPlate = rawInput.Replace(" ", "").Replace("-", "").ToUpper().Trim();

            // Placeholder or empty check
            if (string.IsNullOrEmpty(searchPlate) || searchPlate.Length < 3 || rawInput.Contains("Search"))
            {
                ShowCustomWarning("Warning", "Please enter a valid license plate.");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                conn.Open();
                // Using LIKE for flexible search
                string sql = "SELECT * FROM ParkYerleri WHERE Plaka LIKE @p AND MevcutDurum = 'DOLU' LIMIT 1";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p", "%" + searchPlate + "%");

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblPlaka.Text = dr["Plaka"].ToString();
                            lblGuncelDurum.Text = dr["MevcutDurum"].ToString(); // "DOLU"
                            lblKonum.Text = dr["ParkYeriId"].ToString();

                            // Translate Customer Type if needed
                            string type = dr["MusteriTipi"]?.ToString();
                            if (type == "Abone") lblAracTuru.Text = "Subscriber";
                            else if (type == "Misafir") lblAracTuru.Text = "Guest";
                            else lblAracTuru.Text = type ?? "Guest";

                            if (dr["GirisSaati"] != DBNull.Value)
                            {
                                DateTime entryTime;
                                if (DateTime.TryParse(dr["GirisSaati"].ToString(), out entryTime))
                                {
                                    lblGirisSaati.Text = entryTime.ToString("dd.MM.yyyy HH:mm");
                                    TimeSpan duration = DateTime.Now - entryTime;
                                    lblSure.Text = $"{(int)duration.TotalHours}h {duration.Minutes}m";
                                }
                            }
                            lblGuncelDurum.ForeColor = Color.Red;
                        }
                        else
                        {
                            ShowCustomWarning("Info", "Vehicle with this license plate not found inside.\n(Search: " + searchPlate + ")");
                            ClearLabels();
                        }
                    }
                }
            }
        }

        // =============================================================
        // LOAD HISTORY (MIDDLE PANEL)
        // =============================================================
        private void button1_Click_1(object sender, EventArgs e)
        {
            string startDate = dtpBaslangic.Value.ToString("yyyy-MM-dd 00:00:00");
            string endDate = dtpBitis.Value.ToString("yyyy-MM-dd 23:59:59");

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT 
                                   Plaka, 
                                   ParkYeriId, 
                                   IslemTipi, 
                                   MusteriTipi, 
                                   Zaman, 
                                   Ucret 
                                   FROM GecmisHareketler 
                                   WHERE Zaman BETWEEN @t1 AND @t2 
                                   ORDER BY Zaman DESC";

                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@t1", startDate);
                        da.SelectCommand.Parameters.AddWithValue("@t2", endDate);

                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dataGridView1.DataSource = dt;
                        SetupGridSettings();

                        if (dt.Rows.Count == 0)
                        {
                            ShowCustomWarning("Info", "No records found for the selected dates.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowCustomWarning("Error", "History Load Error: " + ex.Message);
                }
            }
        }

        private void label13_Click(object sender, EventArgs e) { }
        private void label34_Click(object sender, EventArgs e) { }

        // =============================================================
        // CHECK SUBSCRIBER (RIGHT PANEL)
        // =============================================================
        private void btnSorgula_Click(object sender, EventArgs e)
        {
            // Get and clean plate
            string searchPlate = textBox2.Text.Trim().ToUpper().Replace(" ", "");

            if (string.IsNullOrEmpty(searchPlate))
            {
                ShowCustomWarning("Warning", "Please enter a license plate.");
                return;
            }

            // --- SORGULAMA BAŞLANGICI: LABEL7 SIFIRLA ---
            if (label7 != null) label7.Text = "---";

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();

                    // --- 1. FETCH SUBSCRIBER INFO ---
                    string sqlAbone = "SELECT * FROM Aboneler WHERE Plaka = @p";
                    bool isSubscriberFound = false;

                    using (SQLiteCommand cmd = new SQLiteCommand(sqlAbone, conn))
                    {
                        cmd.Parameters.AddWithValue("@p", searchPlate);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                isSubscriberFound = true;

                                // --- ABONE BULUNDU: TİPİ "SUBSCRIBER" YAP ---
                                if (label7 != null)
                                {
                                    label7.Text = "Subscriber";
                                    label7.ForeColor = Color.LimeGreen;
                                }

                                // Plate
                                lblpltnumber.Text = dr["Plaka"].ToString();

                                // Start Date
                                string startStr = dr["BaslangicTarihi"].ToString();
                                lbldateabone.Text = startStr;

                                // End Date & Status Calculation
                                string endStr = dr["BitisTarihi"].ToString();
                                int isBlacklisted = Convert.ToInt32(dr["KaraListe"]);

                                if (DateTime.TryParse(endStr, out DateTime endDate))
                                {
                                    TimeSpan remaining = endDate - DateTime.Now;

                                    // Remaining Days
                                    if (remaining.TotalDays > 0)
                                    {
                                        lblnekdr.Text = $"{(int)remaining.TotalDays} Days Left";
                                    }
                                    else
                                    {
                                        lblnekdr.Text = "Expired";
                                    }

                                    // Status Label
                                    if (isBlacklisted == 1)
                                    {
                                        lblaktiflik.Text = "BLACKLISTED";
                                        lblaktiflik.ForeColor = Color.Red;
                                    }
                                    else if (remaining.TotalDays < 0)
                                    {
                                        lblaktiflik.Text = "PASSIVE (Expired)";
                                        lblaktiflik.ForeColor = Color.Orange;
                                    }
                                    else
                                    {
                                        lblaktiflik.Text = "ACTIVE";
                                        lblaktiflik.ForeColor = Color.LimeGreen;
                                    }
                                }
                                else
                                {
                                    lblnekdr.Text = "Error";
                                    lblaktiflik.Text = "Unknown";
                                }
                            }
                        }
                    }

                    if (!isSubscriberFound)
                    {
                        // --- ABONE DEĞİLSE: TİPİ "GUEST" YAP ---
                        if (label7 != null)
                        {
                            label7.Text = "Guest";
                            label7.ForeColor = Color.White;
                        }

                        ShowCustomWarning("Info", "Subscriber record not found for this license plate.");

                        // Clear Labels
                        lblpltnumber.Text = "-";
                        lbldateabone.Text = "-";
                        lblnekdr.Text = "-";
                        lblaktiflik.Text = "-";
                        lbllocation.Text = "-";
                        return;
                    }

                    // --- 2. FIND CURRENT LOCATION (If parked) ---
                    string sqlLocation = "SELECT ParkYeriId FROM ParkYerleri WHERE Plaka = @p AND MevcutDurum = 'DOLU'";
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlLocation, conn))
                    {
                        cmd.Parameters.AddWithValue("@p", searchPlate);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            // Vehicle is inside
                            lbllocation.Text = result.ToString(); // e.g., K1-A05
                            lbllocation.ForeColor = Color.Yellow;
                        }
                        else
                        {
                            lbllocation.Text = "Outside";
                            lbllocation.ForeColor = Color.White;
                        }
                    }

                }
                catch (Exception ex)
                {
                    ShowCustomWarning("Error", "Query error: " + ex.Message);
                }
            }
        }

        private void panel10_Paint(object sender, PaintEventArgs e) { }
        private void lblAracTuru_Click(object sender, EventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}