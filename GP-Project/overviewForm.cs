using ClosedXML.Excel;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GP_Project
{
    public partial class overviewForm : Form, ITitledPage
    {
        public string PageTitle => "Overview";

        // ERROR FIX: If 'whiteoverview' icon doesn't exist, use null or fix the resource name. 
        public Image PageIcon => null;

        string dbPath = "Data Source=parkwise.db;Version=3;";
        string websiteUrl = "https://parkwisedb.web.app";

        // List to store rates in memory for easy access
        List<AboneTarifeModel> subscriberRatesList = new List<AboneTarifeModel>();

        public overviewForm()
        {
            InitializeComponent();
            SetupGridSettings();
            LoadRates();
        }

        private void overviewForm_Load(object sender, EventArgs e)
        {
            if (txtGirisPlaka != null) UIHelper.SetPlaceholder(txtGirisPlaka, "PLATE");

            FillSubscriberRates(); // Load rates into ComboBox

            // Design rounding logic
            try
            {
                UIHelper.SetRoundedRegion(this, 20);
                if (panel1 != null) UIHelper.SetRoundedRegion(panel1, 20);
                if (panel9 != null) UIHelper.SetRoundedRegion(panel9, 20);
                if (panel7 != null) UIHelper.SetRoundedRegion(panel7, 20);
                if (panel10 != null) UIHelper.SetRoundedRegion(panel10, 15);
                if (panel11 != null) UIHelper.SetRoundedRegion(panel11, 15);
                if (panel13 != null) UIHelper.SetRoundedRegion(panel13, 15);
                if (panel14 != null) UIHelper.SetRoundedRegion(panel14, 15);
                if (panel16 != null) { UIHelper.SetRoundedRegion(panel16, 15); panel16.Hide(); }
            }
            catch { }

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

        // =======================================================================
        //  METHOD CALLED FROM PARKING AREA
        // =======================================================================
        public void DisaridanParkYeriSec(string parkSpotId)
        {
            if (textBox1 != null)
            {
                textBox1.Text = parkSpotId;
                textBox1.Focus();
            }
            if (txtGirisPlaka != null)
            {
                txtGirisPlaka.Text = "";
            }
        }

        // =======================================================================
        //  1. ENTRY PROCESS
        // =======================================================================
        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            string plate = CleanText(txtGirisPlaka.Text);
            string requestedSpot = CleanText(textBox1.Text);

            if (string.IsNullOrEmpty(plate) || plate.Contains("PLATE"))
            {
                ShowCustomWarning("Warning", "Please enter a LICENSE PLATE for entry.");
                return;
            }

            bool manualSelection = !string.IsNullOrEmpty(requestedSpot) && !requestedSpot.Contains("PARK");
            StartEntryProcess(plate, manualSelection ? requestedSpot : "");
        }

        private void StartEntryProcess(string plate, string manualSpot)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();
                    string assignedSpotId = "";

                    // --- 1. SUBSCRIBER CHECK ---
                    string customerType = "Misafir"; // Default: Guest 

                    string cleanPlate = plate.Replace(" ", "").Replace("-", "").ToUpper();

                    string subscriberCheckSQL = @"SELECT COUNT(*) FROM Aboneler 
                                                 WHERE REPLACE(REPLACE(Plaka, '-', ''), ' ', '') = @p 
                                                 AND KaraListe = 0 
                                                 AND BitisTarihi >= @today";

                    using (SQLiteCommand cmd = new SQLiteCommand(subscriberCheckSQL, conn))
                    {
                        cmd.Parameters.AddWithValue("@p", cleanPlate);
                        cmd.Parameters.AddWithValue("@today", DateTime.Now.ToString("yyyy-MM-dd"));
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0) customerType = "Abone";
                    }

                    // --- 2. DETERMINE PARKING SPOT ---
                    if (!string.IsNullOrEmpty(manualSpot))
                    {
                        string cleanId = manualSpot.Replace(" ", "").Replace("-", "");
                        string checkSQL = "SELECT ParkYeriId, MevcutDurum FROM ParkYerleri WHERE REPLACE(REPLACE(ParkYeriId, '-', ''), ' ', '') = @id";

                        using (SQLiteCommand cmd = new SQLiteCommand(checkSQL, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", cleanId);
                            using (SQLiteDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    string status = dr["MevcutDurum"].ToString();

                                    // Check occupied status
                                    if (status == "DOLU")
                                    {
                                        ShowCustomWarning("Warning", $"Spot {manualSpot} is currently OCCUPIED!");
                                        return;
                                    }
                                    assignedSpotId = dr["ParkYeriId"].ToString();
                                }
                                else
                                {
                                    ShowCustomWarning("Error", $"Parking spot '{manualSpot}' not found!");
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Automatic Selection
                        string autoFindSQL = "SELECT ParkYeriId FROM ParkYerleri WHERE MevcutDurum = 'BOŞ' ORDER BY ParkYeriId ASC LIMIT 1";
                        using (SQLiteCommand cmd = new SQLiteCommand(autoFindSQL, conn))
                        {
                            var result = cmd.ExecuteScalar();
                            if (result != null) assignedSpotId = result.ToString();
                            else
                            {
                                ShowCustomWarning("Warning", "Parking lot is FULL!");
                                return;
                            }
                        }
                    }

                    // --- 3. SAVE TO DATABASE ---
                    DateTime now = DateTime.Now;
                    string dateStr = now.ToString("yyyy-MM-dd HH:mm:ss");

                    string updateSQL = @"UPDATE ParkYerleri SET MevcutDurum = 'DOLU', Plaka = @p, GirisSaati = @t, MusteriTipi = @mtype WHERE ParkYeriId = @id";
                    using (SQLiteCommand cmd = new SQLiteCommand(updateSQL, conn))
                    {
                        cmd.Parameters.AddWithValue("@p", cleanPlate);
                        cmd.Parameters.AddWithValue("@t", dateStr);
                        cmd.Parameters.AddWithValue("@mtype", customerType);
                        cmd.Parameters.AddWithValue("@id", assignedSpotId);
                        cmd.ExecuteNonQuery();
                    }

                    string logSQL = "INSERT INTO GecmisHareketler (Plaka, ParkYeriId, IslemTipi, Zaman, MusteriTipi) VALUES (@p, @id, 'Giriş', @t, @mtype)";
                    using (SQLiteCommand cmd = new SQLiteCommand(logSQL, conn))
                    {
                        cmd.Parameters.AddWithValue("@p", cleanPlate);
                        cmd.Parameters.AddWithValue("@id", assignedSpotId);
                        cmd.Parameters.AddWithValue("@t", dateStr);
                        cmd.Parameters.AddWithValue("@mtype", customerType);
                        cmd.ExecuteNonQuery();
                    }

                    // --- 4. QR CODE ---
                    if (pbQRCode != null)
                    {
                        string urlParams = $"?plaka={cleanPlate}&yer={assignedSpotId}&saat={dateStr.Replace(" ", "%20")}";
                        pbQRCode.Tag = websiteUrl + urlParams;
                        pbQRCode.Image = GenerateTicketImage(assignedSpotId, now, websiteUrl + urlParams);
                        pbQRCode.SizeMode = PictureBoxSizeMode.Zoom;
                        pbQRCode.Visible = true;
                        pbQRCode.BringToFront();
                    }

                    string message = $"Entry Successful: {assignedSpotId}";
                    if (customerType == "Abone") message += "\n(SUBSCRIBER ENTRY)";

                    ShowCustomWarning("Success", message);

                    if (textBox1 != null) textBox1.Text = "";
                    if (txtGirisPlaka != null) txtGirisPlaka.Text = "";
                }
                catch (Exception ex)
                {
                    ShowCustomWarning("Error", "Entry Error: " + ex.Message);
                }
            }
        }

        // =======================================================================
        //  2. EXIT PROCESS
        // =======================================================================
        private void btnManuelCikis_Click(object sender, EventArgs e)
        {
            string enteredSpot = CleanText(textBox1.Text);
            string enteredPlate = CleanText(txtGirisPlaka.Text);

            if (!string.IsNullOrEmpty(enteredSpot) && !enteredSpot.Contains("PARK"))
                PerformExitProcess("LOCATION", enteredSpot);
            else if (!string.IsNullOrEmpty(enteredPlate) && !enteredPlate.Contains("PLATE"))
                PerformExitProcess("PLATE", enteredPlate);
            else
                ShowCustomWarning("Info", "Please enter 'Spot ID' or 'License Plate' to exit.");
        }

        private void PerformExitProcess(string searchType, string value)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();
                    string sql = "";

                    if (searchType == "LOCATION")
                        sql = "SELECT ParkYeriId, Plaka, GirisSaati, MevcutDurum, MusteriTipi, KatBilgisi FROM ParkYerleri WHERE REPLACE(REPLACE(ParkYeriId, '-', ''), ' ', '') = @val";
                    else
                        sql = "SELECT ParkYeriId, Plaka, GirisSaati, MevcutDurum, MusteriTipi, KatBilgisi FROM ParkYerleri WHERE REPLACE(REPLACE(Plaka, '-', ''), ' ', '') = @val AND MevcutDurum = 'DOLU'";

                    string spotId = "", plate = "", entryStr = "", status = "", customerType = "";
                    string spotType = "";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@val", value);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                spotId = dr["ParkYeriId"].ToString();
                                plate = dr["Plaka"].ToString();
                                entryStr = dr["GirisSaati"].ToString();
                                status = dr["MevcutDurum"].ToString();
                                customerType = dr["MusteriTipi"].ToString();

                                if (dr["KatBilgisi"] != DBNull.Value)
                                    spotType = dr["KatBilgisi"].ToString();
                            }
                            else
                            {
                                ShowCustomWarning("Error", "Record not found.");
                                return;
                            }
                        }
                    }

                    if (status != "DOLU")
                    {
                        ShowCustomWarning("Warning", "This spot is already empty.");
                        return;
                    }

                    DateTime entryTime;
                    if (!DateTime.TryParse(entryStr, out entryTime)) entryTime = DateTime.Now;
                    TimeSpan duration = DateTime.Now - entryTime;
                    double totalHours = duration.TotalHours;
                    double fee = 0;

                    if (customerType == "Abone") fee = 0;
                    else
                    {
                        fee = Math.Ceiling(totalHours) * 50;
                        if (fee == 0) fee = 50;
                    }

                    // --- REVERT TO DISABLED STATUS IF APPLICABLE ---
                    string newStatus = "BOŞ";
                    if (spotType == "Engelli" || spotType.Contains("Engelli"))
                    {
                        newStatus = "Engelli";
                    }

                    // Exit Update
                    string updateSql = "UPDATE ParkYerleri SET MevcutDurum=@newStatus, Plaka=NULL, GirisSaati=NULL, MusteriTipi=NULL WHERE ParkYeriId=@id";
                    using (SQLiteCommand cmd = new SQLiteCommand(updateSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@newStatus", newStatus);
                        cmd.Parameters.AddWithValue("@id", spotId);
                        cmd.ExecuteNonQuery();
                    }

                    // Log History
                    string logSql = "INSERT INTO GecmisHareketler (Plaka, ParkYeriId, IslemTipi, Zaman, Ucret, MusteriTipi) VALUES (@p, @id, 'Çıkış', @t, @u, @mtype)";
                    using (SQLiteCommand cmd = new SQLiteCommand(logSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@p", plate);
                        cmd.Parameters.AddWithValue("@id", spotId);
                        cmd.Parameters.AddWithValue("@t", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@u", fee);
                        cmd.Parameters.AddWithValue("@mtype", customerType);
                        cmd.ExecuteNonQuery();
                    }

                    // QR Code
                    if (pbQRCode != null)
                    {
                        string urlParams = $"?plaka={plate}&yer={spotId}&saat={entryStr.Replace(" ", "%20")}&ucret={fee}";
                        pbQRCode.Tag = websiteUrl + urlParams;
                        pbQRCode.Image = GenerateInvoiceImage(plate, spotId, duration, fee);
                        pbQRCode.Visible = true;
                        pbQRCode.BringToFront();
                    }

                    string msg = customerType == "Abone" ? "SUBSCRIBER EXIT (FREE)" : $"FEE: {fee:C2}";
                    ShowCustomWarning("Success", $"EXIT SUCCESSFUL.\nLocation: {spotId}\n{msg}");

                    if (textBox1 != null) textBox1.Text = "";
                    if (txtGirisPlaka != null) txtGirisPlaka.Text = "";
                }
                catch (Exception ex)
                {
                    ShowCustomWarning("Error", "Error: " + ex.Message);
                }
            }
        }

        // =======================================================================
        //  GRID & RATES HELPERS
        // =======================================================================

        private void SetupGridSettings()
        {
            if (dgvFinansGecmis == null) return;

            StyleDataGrid(dgvFinansGecmis);
            dgvFinansGecmis.Rows.Clear();
            dgvFinansGecmis.Columns.Clear();

            dgvFinansGecmis.Columns.Add("Branch", "Branch");
            dgvFinansGecmis.Columns.Add("Plate", "License Plate");
            dgvFinansGecmis.Columns.Add("Date", "Transaction Date");
            dgvFinansGecmis.Columns.Add("Action", "Action Detail");
            dgvFinansGecmis.Columns.Add("Amount", "Amount");
        }

        private void LoadRates()
        {
            if (dgvTarifeler == null) return;

            StyleDataGrid(dgvTarifeler);
            dgvTarifeler.Rows.Clear();
            dgvTarifeler.Columns.Clear();

            dgvTarifeler.Columns.Add("Tariff", "Tariff Name");
            dgvTarifeler.Columns.Add("Price", "Price (TL)");

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM Tarifeler";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            dgvTarifeler.Rows.Add(
                                dr["TarifeAdi"].ToString(),
                                dr["Fiyat"].ToString()
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowCustomWarning("Error", "Error loading rates: " + ex.Message);
                }
            }
        }

        private void StyleDataGrid(DataGridView dgv)
        {
            if (dgv == null) return;
            dgv.BackgroundColor = System.Drawing.Color.Black;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(1, 4, 9);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            dgv.ColumnHeadersHeight = 35;
            dgv.DefaultCellStyle.BackColor = System.Drawing.Color.Black;
            dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.WhiteSmoke;
            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.SeaGreen;
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // =======================================================================
        //  SUBSCRIBER RATES & LOGIC
        // =======================================================================

        private void FillSubscriberRates()
        {
            if (cmbAbonelikSuresi == null) return;
            cmbAbonelikSuresi.Items.Clear();
            subscriberRatesList.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM Tarifeler WHERE TarifeAdi NOT LIKE '%Saat%'";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string name = dr["TarifeAdi"].ToString();
                            double price = Convert.ToDouble(dr["Fiyat"]);
                            AboneTarifeModel model = new AboneTarifeModel { Ad = name, Fiyat = price };

                            // Add to list and combobox
                            subscriberRatesList.Add(model);
                            cmbAbonelikSuresi.Items.Add(name);
                        }
                    }
                }
                catch { }
            }
        }

        private void cmbAbonelikSuresi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAbonelikSuresi.SelectedIndex != -1 && lblOdenecekTutar != null)
            {
                string selected = cmbAbonelikSuresi.SelectedItem.ToString();
                var t = subscriberRatesList.FirstOrDefault(x => x.Ad == selected);
                if (t != null)
                    lblOdenecekTutar.Text = t.Fiyat.ToString("N0") + " TL";
                else
                    lblOdenecekTutar.Text = "0.00 TL";
            }
        }

        // --- ADD NEW SUBSCRIBER ---
        private void btnAboneEkle_Click_1(object sender, EventArgs e)
        {
            string name = txtAboneAd.Text.Trim();
            string plate = CleanText(txtAbonePlaka.Text);
            string tel = txtAboneTel.Text.Trim();
            string durationText = cmbAbonelikSuresi.Text;
            int blacklist = chkKaraListe.Checked ? 1 : 0;

            // 1. Kontroller
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(plate) || string.IsNullOrEmpty(tel) || string.IsNullOrEmpty(durationText))
            {
                ShowCustomWarning("Warning", "Please fill in Name, Plate, Phone, and Duration.");
                return;
            }

            // 2. Fiyat ve Tarih Hesaplama
            DateTime start = DateTime.Now;
            DateTime end = start.AddMonths(1);
            double fee = 0;

            // Listeden fiyatı çekiyoruz
            var selectedRate = subscriberRatesList.FirstOrDefault(t => t.Ad == durationText);
            if (selectedRate != null)
            {
                end = start.AddDays(selectedRate.GunSayisi);
                fee = selectedRate.Fiyat;
            }
            else
            {
                // Yedek kontrol (Eski veri tipleri için)
                if (durationText.Contains("3 Ay")) end = start.AddMonths(3);
                else if (durationText.Contains("6 Ay")) end = start.AddMonths(6);
                else if (durationText.Contains("1 Yıl")) end = start.AddYears(1);
            }

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();

                    // A. ABONEYİ EKLE
                    string sqlInsertAbone = @"INSERT INTO Aboneler (AdSoyad, Plaka, Telefon, BaslangicTarihi, BitisTarihi, KaraListe, OdenenUcret) 
                                      VALUES (@ad, @plaka, @tel, @bas, @bit, @kl, @ucret)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sqlInsertAbone, conn))
                    {
                        cmd.Parameters.AddWithValue("@ad", name);
                        cmd.Parameters.AddWithValue("@plaka", plate);
                        cmd.Parameters.AddWithValue("@tel", tel);
                        cmd.Parameters.AddWithValue("@bas", start.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@bit", end.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@kl", blacklist);
                        cmd.Parameters.AddWithValue("@ucret", fee);
                        cmd.ExecuteNonQuery();
                    }

                    // B. ÖDEMEYİ GEÇMİŞE EKLE (BURASI DASHBOARD REVENUE İÇİN ŞART)
                    // last_insert_rowid() ile az önce eklenen abonenin ID'sini alıp ödemeyi ona bağlıyoruz.
                    try
                    {
                        string sqlPayment = "INSERT INTO AboneOdemeGecmisi (AboneId, IslemDetayi, IslemTarihi, Tutar) VALUES ((SELECT last_insert_rowid()), 'New Subscription', @t, @u)";
                        using (SQLiteCommand cmd = new SQLiteCommand(sqlPayment, conn))
                        {
                            cmd.Parameters.AddWithValue("@t", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                            cmd.Parameters.AddWithValue("@u", fee); // İşte bu 'fee' Dashboard'a gidecek
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch { }

                    ShowCustomWarning("Success", $"Subscriber Added Successfully!\nExpires: {end:dd.MM.yyyy}");

                    // C. TEMİZLİK VE LABEL SIFIRLAMA
                    txtAboneAd.Clear();
                    txtAbonePlaka.Clear();
                    txtAboneTel.Clear();
                    chkKaraListe.Checked = false;

                    // Seçimi kaldır ve PARAYI SIFIRLA
                    cmbAbonelikSuresi.SelectedIndex = -1;
                    if (lblOdenecekTutar != null) lblOdenecekTutar.Text = "0.00 TL";
                }
                catch (Exception ex)
                {
                    ShowCustomWarning("Error", "Error: " + ex.Message);
                }
            }
        }

        // --- UPDATE SUBSCRIBER ---
        private void btnGuncelle_Click_1(object sender, EventArgs e)
        {
            string plate = CleanText(txtAbonePlaka.Text);
            if (string.IsNullOrEmpty(plate))
            {
                ShowCustomWarning("Warning", "Please enter a license plate to update.");
                return;
            }

            if (cmbAbonelikSuresi.SelectedIndex == -1)
            {
                ShowCustomWarning("Warning", "Please select a duration (Tariff) to extend.");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();

                    string sqlFind = "SELECT AboneId, BitisTarihi FROM Aboneler WHERE REPLACE(REPLACE(Plaka, '-', ''), ' ', '') = @p";
                    int subId = 0;
                    DateTime oldExpiry = DateTime.Now;
                    bool exists = false;

                    using (SQLiteCommand cmd = new SQLiteCommand(sqlFind, conn))
                    {
                        cmd.Parameters.AddWithValue("@p", plate);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                exists = true;
                                subId = Convert.ToInt32(dr["AboneId"]);
                                if (!DateTime.TryParse(dr["BitisTarihi"].ToString(), out oldExpiry)) oldExpiry = DateTime.Now;
                            }
                        }
                    }

                    if (!exists)
                    {
                        ShowCustomWarning("Error", "Subscriber not found! Use 'Add Subscriber' first.");
                        return;
                    }

                    string selectedRateName = cmbAbonelikSuresi.SelectedItem.ToString();
                    var rate = subscriberRatesList.FirstOrDefault(t => t.Ad == selectedRateName);

                    if (rate == null) { ShowCustomWarning("Error", "Selected rate info not found."); return; }

                    DateTime newExpiryDate;
                    if (oldExpiry > DateTime.Now) newExpiryDate = oldExpiry.AddDays(rate.GunSayisi);
                    else newExpiryDate = DateTime.Now.AddDays(rate.GunSayisi);

                    double addedFee = rate.Fiyat;
                    int blacklistStatus = chkKaraListe.Checked ? 1 : 0;

                    string sqlUpdate = @"UPDATE Aboneler SET AdSoyad=@name, Telefon=@tel, BitisTarihi=@end, KaraListe=@kl, OdenenUcret=OdenenUcret+@fee WHERE AboneId=@id";

                    using (SQLiteCommand cmd = new SQLiteCommand(sqlUpdate, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", txtAboneAd.Text.Trim());
                        cmd.Parameters.AddWithValue("@tel", txtAboneTel.Text.Trim());
                        cmd.Parameters.AddWithValue("@end", newExpiryDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@kl", blacklistStatus);
                        cmd.Parameters.AddWithValue("@fee", addedFee);
                        cmd.Parameters.AddWithValue("@id", subId);
                        cmd.ExecuteNonQuery();
                    }

                    try
                    {
                        string logSql = "INSERT INTO AboneOdemeGecmisi (AboneId, IslemDetayi, IslemTarihi, Tutar) VALUES (@id, @detail, @t, @u)";
                        using (SQLiteCommand cmd = new SQLiteCommand(logSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", subId);
                            cmd.Parameters.AddWithValue("@detail", $"Extension ({selectedRateName})");
                            cmd.Parameters.AddWithValue("@t", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                            cmd.Parameters.AddWithValue("@u", addedFee);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch { }

                    ShowCustomWarning("Success", $"Subscription Updated Successfully!\n\nOld Expiry: {oldExpiry:dd.MM.yyyy}\nNew Expiry: {newExpiryDate:dd.MM.yyyy}");

                    txtAboneAd.Clear(); txtAbonePlaka.Clear(); txtAboneTel.Clear();
                    chkKaraListe.Checked = false;
                    cmbAbonelikSuresi.SelectedIndex = -1;
                    if (lblOdenecekTutar != null) lblOdenecekTutar.Text = "0.00 TL";
                }
                catch (Exception ex) { ShowCustomWarning("Error", "Error during update: " + ex.Message); }
            }
        }

        // =======================================================================
        //  REVENUE & EXCEL
        // =======================================================================

        private void btnCiroHesapla_Click(object sender, EventArgs e)
        {
            if (dgvFinansGecmis != null) dgvFinansGecmis.Rows.Clear();

            string startDate = dtpFinansBaslangic.Value.ToString("yyyy-MM-dd 00:00:00");
            string endDate = dtpFinansBitis.Value.ToString("yyyy-MM-dd 23:59:59");

            double totalRevenue = 0;
            int totalTransactions = 0;

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();

                    // 1. Vehicles
                    string sqlVehicles = @"SELECT ParkYeriId, Plaka, Zaman, IslemTipi, Ucret FROM GecmisHareketler WHERE Zaman BETWEEN @s AND @e AND Ucret > 0 ORDER BY Zaman DESC";
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlVehicles, conn))
                    {
                        cmd.Parameters.AddWithValue("@s", startDate);
                        cmd.Parameters.AddWithValue("@e", endDate);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                double amount = 0;
                                double.TryParse(dr["Ucret"].ToString(), out amount);
                                totalRevenue += amount;
                                totalTransactions++;
                                if (dgvFinansGecmis != null)
                                    dgvFinansGecmis.Rows.Add(dr["ParkYeriId"], dr["Plaka"], dr["Zaman"], "Parking Fee", amount.ToString("C2"));
                            }
                        }
                    }

                    // 2. Subs
                    string sqlSubs = @"SELECT A.Plaka, O.IslemDetayi, O.IslemTarihi, O.Tutar FROM AboneOdemeGecmisi O LEFT JOIN Aboneler A ON O.AboneId = A.AboneId WHERE O.IslemTarihi BETWEEN @s AND @e ORDER BY O.IslemTarihi DESC";
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlSubs, conn))
                    {
                        cmd.Parameters.AddWithValue("@s", startDate);
                        cmd.Parameters.AddWithValue("@e", endDate);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                double amount = 0;
                                double.TryParse(dr["Tutar"].ToString(), out amount);
                                totalRevenue += amount;
                                totalTransactions++;
                                string subPlate = dr["Plaka"] != DBNull.Value ? dr["Plaka"].ToString() : "Deleted User";
                                if (dgvFinansGecmis != null)
                                    dgvFinansGecmis.Rows.Add("SUBSCRIPTION", subPlate, dr["IslemTarihi"], dr["IslemDetayi"], amount.ToString("C2"));
                            }
                        }
                    }

                    if (lblToplamCiro != null) lblToplamCiro.Text = totalRevenue.ToString("C2");
                    double avgRevenue = totalTransactions > 0 ? totalRevenue / totalTransactions : 0;
                    if (lblOrtalamaKazanc != null) lblOrtalamaKazanc.Text = avgRevenue.ToString("C2");
                }
                catch (Exception ex) { ShowCustomWarning("Error", "Error: " + ex.Message); }
            }
        }

        private void btnFiyatGuncelle_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();
                    foreach (DataGridViewRow row in dgvTarifeler.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string rateName = row.Cells[0].Value.ToString().Trim();
                        string priceStr = row.Cells[1].Value.ToString().Replace("TL", "").Trim();
                        if (double.TryParse(priceStr, out double newPrice))
                        {
                            string sql = "INSERT OR REPLACE INTO Tarifeler (Id, TarifeAdi, Fiyat) VALUES ((SELECT Id FROM Tarifeler WHERE TarifeAdi = @ad), @ad, @fiyat)";
                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@ad", rateName);
                                cmd.Parameters.AddWithValue("@fiyat", newPrice);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    ShowCustomWarning("Success", "Prices updated!");
                    LoadRates();
                }
                catch (Exception ex) { ShowCustomWarning("Error", "Error: " + ex.Message); }
            }
        }

        private void btnExcelAktar_Click_1(object sender, EventArgs e)
        {
            if (dgvFinansGecmis.Rows.Count == 0) { ShowCustomWarning("Warning", "No data to export!"); return; }

            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                double totalRevenue = 0; // Toplam tutarı tutacak değişken
                int amountColumnIndex = -1; // Amount kolonunun sırasını tutacak

                // 1. Kolonları Oluştur
                foreach (DataGridViewColumn col in dgvFinansGecmis.Columns)
                {
                    if (col.Name == "Amount" || col.HeaderText.Contains("Amount"))
                    {
                        dt.Columns.Add(col.HeaderText, typeof(double));
                        // Excel 1 tabanlı olduğu için index + 1 yapıyoruz ama DataTable 0 tabanlı.
                        // Burada sadece DataTable içindeki index'i bulalım.
                        amountColumnIndex = dt.Columns.Count;
                    }
                    else
                    {
                        dt.Columns.Add(col.HeaderText, typeof(string));
                    }
                }

                // 2. Satırları Dön ve Toplamı Hesapla
                foreach (DataGridViewRow row in dgvFinansGecmis.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        System.Data.DataRow dRow = dt.NewRow();
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            // Eğer Amount kolonu ise sayısal işlem yap
                            if (dgvFinansGecmis.Columns[cell.ColumnIndex].HeaderText.Contains("Amount"))
                            {
                                string cleanVal = cell.Value?.ToString().Replace("₺", "").Replace("TL", "").Trim();
                                double.TryParse(cleanVal, out double numVal);

                                dRow[cell.ColumnIndex] = numVal;

                                // Toplama ekle
                                totalRevenue += numVal;
                            }
                            else
                            {
                                dRow[cell.ColumnIndex] = cell.Value ?? "";
                            }
                        }
                        dt.Rows.Add(dRow);
                    }
                }

                // 3. Excel'e Yazma İşlemi
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string path = Path.Combine(desktop, $"RevenueReport_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Revenue Report");

                    // Tabloyu ekle
                    var table = worksheet.Cell(1, 1).InsertTable(dt);
                    table.Theme = XLTableTheme.TableStyleMedium9;

                    // --- TOPLAM SATIRI EKLEME ---
                    // Tablonun bittiği satırın bir altını buluyoruz
                    int totalRowIndex = table.RangeAddress.LastAddress.RowNumber + 1;

                    if (amountColumnIndex != -1)
                    {
                        // Toplam etiketini yaz ("TOTAL")
                        var labelCell = worksheet.Cell(totalRowIndex, amountColumnIndex - 1);
                        labelCell.Value = "TOTAL REVENUE:";
                        labelCell.Style.Font.Bold = true;
                        labelCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                        // Hesaplanan toplam tutarı yaz
                        var totalCell = worksheet.Cell(totalRowIndex, amountColumnIndex);
                        totalCell.Value = totalRevenue;
                        totalCell.Style.Font.Bold = true;
                        totalCell.Style.Font.FontColor = XLColor.DarkGreen;
                        // Para formatı (Binlik ayraçlı)
                        totalCell.Style.NumberFormat.Format = "#,##0.00";
                    }

                    // Sütun genişliklerini ayarla
                    worksheet.Columns().AdjustToContents();

                    workbook.SaveAs(path);
                }

                ShowCustomWarning("Success", "Excel saved successfully!\n" + path);
            }
            catch (Exception ex) { ShowCustomWarning("Error", "Error: " + ex.Message); }
        }

        // UI Event Handlers
        private void btnGuncelle_Click(object sender, EventArgs e) { }
        private void button3_Click(object sender, EventArgs e) { panel16.Show(); panel16.BringToFront(); }
        private void button1_Click(object sender, EventArgs e) { panel16.Hide(); }

        // Helper Methods
        private string CleanText(string text) { return string.IsNullOrWhiteSpace(text) ? "" : text.Replace("-", "").Replace(" ", "").Trim().ToUpper(); }

        private Bitmap GenerateTicketImage(string location, DateTime time, string qrData)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrImage = qrCode.GetGraphic(5);
            Bitmap ticket = new Bitmap(300, 450);
            using (Graphics g = Graphics.FromImage(ticket))
            {
                g.Clear(Color.White);
                StringFormat c = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString("ENTRY TICKET", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, 150, 30, c);
                g.DrawString(location, new Font("Arial Black", 40, FontStyle.Bold), Brushes.Black, 150, 70, c);
                g.DrawString(time.ToString("HH:mm"), new Font("Arial", 12), Brushes.Gray, 150, 140, c);
                g.DrawImage(qrImage, (300 - qrImage.Width) / 2, 170);
            }
            return ticket;
        }

        private Bitmap GenerateInvoiceImage(string plate, string location, TimeSpan duration, double amount)
        {
            Bitmap invoice = new Bitmap(300, 450);
            using (Graphics g = Graphics.FromImage(invoice))
            {
                g.Clear(Color.White);
                StringFormat c = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString("RECEIPT", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, 150, 30, c);
                g.DrawLine(Pens.Black, 20, 60, 280, 60);
                g.DrawString($"Vehicle: {plate}", new Font("Arial", 11), Brushes.Black, 150, 80, c);
                g.DrawString($"Loc: {location}", new Font("Arial", 11), Brushes.Black, 150, 105, c);
                g.DrawString($"Time: {duration.Hours}h {duration.Minutes}m", new Font("Arial", 11), Brushes.Black, 150, 130, c);
                g.DrawRectangle(Pens.Black, 40, 180, 220, 100);
                g.DrawString("AMOUNT", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, 150, 190, c);
                if (amount == 0) g.DrawString("FREE", new Font("Arial Black", 18, FontStyle.Bold), Brushes.Green, 150, 220, c);
                else g.DrawString($"{amount:N0} TL", new Font("Arial Black", 24, FontStyle.Bold), Brushes.DarkRed, 150, 220, c);
            }
            return invoice;
        }

        private void pbQRCode_Click(object sender, EventArgs e)
        {
            if (pbQRCode.Image == null) return;
            string targetUrl = pbQRCode.Tag != null ? pbQRCode.Tag.ToString() : websiteUrl;
            Form popup = new Form() { Size = new Size(400, 600), StartPosition = FormStartPosition.CenterScreen, Text = "Details" };
            PictureBox bigPic = new PictureBox() { Image = pbQRCode.Image, Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, Cursor = Cursors.Hand };
            bigPic.Click += (s, args) => { try { Process.Start(new ProcessStartInfo(targetUrl) { UseShellExecute = true }); } catch { } };
            popup.Controls.Add(bigPic);
            popup.ShowDialog();
        }

        // Empty Events
        private void panel16_Paint(object sender, PaintEventArgs e) { }
        private void dgvFinansGecmis_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void lblToplamCiro_Click(object sender, EventArgs e) { }
        private void btnfaturauret_Click(object sender, EventArgs e) { }
        private void txtAbonePlaka_KeyDown(object sender, KeyEventArgs e) { }
        private void aboneyapbtn_Click(object sender, EventArgs e) { }
        private void lblOdenecekTutar_Click(object sender, EventArgs e) { }
        private void cmbAbonelikSuresi_SelectedIndexChanged_1(object sender, EventArgs e) {

            // Eğer seçim boşsa veya label yoksa işlem yapma
            if (cmbAbonelikSuresi.SelectedIndex == -1 || lblOdenecekTutar == null)
            {
                if (lblOdenecekTutar != null) lblOdenecekTutar.Text = "0.00 TL";
                return;
            }

            // 1. Seçilen Planın Adını Al
            string secilenPlanAdi = cmbAbonelikSuresi.SelectedItem.ToString();

            // 2. Listeden Bu Planı ve Fiyatını Bul
            var secilenTarife = subscriberRatesList.FirstOrDefault(x => x.Ad == secilenPlanAdi);

            if (secilenTarife != null)
            {
                // 3. Fiyatı Label'a Yazdır (Örn: 500 TL)
                lblOdenecekTutar.Text = secilenTarife.Fiyat.ToString("N0") + " TL";
            }
            else
            {
                lblOdenecekTutar.Text = "0.00 TL";
            }
        }

        private void btnFiyatGuncelle_Click_1(object sender, EventArgs e)
        {
            // Grid üzerindeki düzenleme işlemini bitir (Son değişikliği algılaması için)
            dgvTarifeler.EndEdit();

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                try
                {
                    conn.Open();
                    int guncellenenSayisi = 0;

                    foreach (DataGridViewRow row in dgvTarifeler.Rows)
                    {
                        // Yeni satır (en alttaki boş satır) ise atla
                        if (row.IsNewRow) continue;

                        // Hücre değerlerini al
                        var cellName = row.Cells[0].Value; // Tarife Adı
                        var cellPrice = row.Cells[1].Value; // Fiyat

                        if (cellName == null || cellPrice == null) continue;

                        string tarifeAdi = cellName.ToString().Trim();
                        // "100 TL" gibi yazıldıysa sadece "100" kısmını al
                        string fiyatStr = cellPrice.ToString().Replace("TL", "").Trim();

                        // Sayıya çevirmeyi dene
                        if (double.TryParse(fiyatStr, out double yeniFiyat))
                        {
                            // SQL Update Sorgusu
                            string sql = "UPDATE Tarifeler SET Fiyat = @fiyat WHERE TarifeAdi = @ad";

                            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@fiyat", yeniFiyat);
                                cmd.Parameters.AddWithValue("@ad", tarifeAdi);

                                int etki = cmd.ExecuteNonQuery();
                                if (etki > 0) guncellenenSayisi++;
                            }
                        }
                    }

                    ShowCustomWarning("Success", $"{guncellenenSayisi} Rates updated successfully!");

                    // Listeyi yenile ki formatlar (TL vb.) düzgün görünsün
                    LoadRates();
                }
                catch (Exception ex)
                {
                    ShowCustomWarning("Error", "Update failed: " + ex.Message);
                }
            }
        }

        private void dgvTarifeler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}