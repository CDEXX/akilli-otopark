using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static GP_Project.UIHelper;

namespace GP_Project
{
    public partial class parkingAreaForm : Form, ITitledPage
    {
        public string PageTitle => "Parking Area";
        public Image PageIcon => Properties.Resources.parkingslateblue;

        // Context menu variable
        private ContextMenuStrip _katMenu;

        public parkingAreaForm()
        {
            InitializeComponent();
            // Bind Mouse Wheel event
            this.MouseWheel += new MouseEventHandler(panel1_MouseWheel);
        }

        public static List<ParkYeriModel> _sahteVeritabani = new List<ParkYeriModel>();

        // Variables
        private bool _isDragging = false;
        private Point _draggingStartPoint;
        private Control _draggedControl;
        private Point _originalDragLocation;
        private int _gridSize = 15; // Grid square size
        private List<string> _availableFloors = new List<string>();
        private int _currentFloorIndex = 0;
        private int _minMargin = 5;
        private float _zoomFactor = 1.0f;
        private bool _isPanning = false;
        private Point _panLastMousePos;

        private void parkingAreaForm_Load(object sender, EventArgs e)
        {
            if (panel1 != null)
            {
                UIHelper.SetRoundedRegion(panel1, 20);
                panel1.AutoScroll = false; // We control it manually at start
            }

            // --- Enable Double Buffering to prevent Grid flickering ---
            EnableDoubleBuffering(pnlWorld);
            EnableDoubleBuffering(panel1);

            DatabaseHelper.TablolariOlustur();
            OlusturSahteVeri();

            // Events
            this.pnlWorld.Paint += new PaintEventHandler(this.pnlWorld_Paint);
            this.pnlWorld.MouseDown += new MouseEventHandler(this.pnlWorld_MouseDown);
            this.pnlWorld.MouseMove += new MouseEventHandler(this.pnlWorld_MouseMove);
            this.pnlWorld.MouseUp += new MouseEventHandler(this.pnlWorld_MouseUp);

            // Prepare Floors
            KatListesiniGuncelle();

            _currentFloorIndex = 0;

            // --- Restore Settings ---
            try { _zoomFactor = Properties.Settings.Default.LastZoom; } catch { _zoomFactor = 1.0f; }
            if (_zoomFactor < 0.5f) _zoomFactor = 1.0f;

            // Set Edit Mode
            chkDuzenlemeModu_CheckedChanged(this, EventArgs.Empty);

            // Restore Scroll Position
            RestoreScrollPosition();

            // Create Bottom Panel
            AltMenuyuOlusturVeSabitle();

            // --- Prepare Floor Menu ---
            KatMenusunuOlustur();
        }

        // --- NEW: Create Context Menu ---
        private void KatMenusunuOlustur()
        {
            _katMenu = new ContextMenuStrip();

            // Add Option
            ToolStripMenuItem itemEkle = new ToolStripMenuItem("➕ Add New Floor");
            itemEkle.Click += (s, e) => KatEkleIslemi();

            // Delete Option
            ToolStripMenuItem itemSil = new ToolStripMenuItem("🗑️ Delete Current Floor");
            itemSil.Click += (s, e) => KatSilIslemi();
            itemSil.ForeColor = Color.Red;

            _katMenu.Items.Add(itemEkle);
            _katMenu.Items.Add(new ToolStripSeparator());
            _katMenu.Items.Add(itemSil);
        }

        // --- Function to Prevent Grid Flickering ---
        public static void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void RestoreScrollPosition()
        {
            if (panel1 == null) return;

            int x = Properties.Settings.Default.LastScrollX;
            int y = Properties.Settings.Default.LastScrollY;

            if (pnlWorld.Width > panel1.Width || pnlWorld.Height > panel1.Height)
            {
                int maxX = Math.Max(0, pnlWorld.Width - panel1.Width);
                int maxY = Math.Max(0, pnlWorld.Height - panel1.Height);

                if (x > maxX) x = maxX;
                if (y > maxY) y = maxY;

                panel1.AutoScrollPosition = new Point(x, y);
            }
        }

        private void SaveSettings()
        {
            if (panel1 != null)
            {
                Properties.Settings.Default.LastScrollX = Math.Abs(panel1.AutoScrollPosition.X);
                Properties.Settings.Default.LastScrollY = Math.Abs(panel1.AutoScrollPosition.Y);
                Properties.Settings.Default.LastZoom = _zoomFactor;
                Properties.Settings.Default.Save();
            }
        }

        private void KatListesiniGuncelle()
        {
            _availableFloors = _sahteVeritabani
                                .Select(p => p.Tip)
                                .Distinct()
                                .OrderBy(k => KatNumarasiVer(k))
                                .ToList();
        }

        private void UpdateFloorDisplay()
        {
            SaveSettings();

            if (_availableFloors.Count == 0)
            {
                // Clear screen if no floors
                pnlWorld.Controls.Clear();
                if (lblCurrentFloor != null) lblCurrentFloor.Text = "None";
                return;
            }

            if (_currentFloorIndex >= _availableFloors.Count) _currentFloorIndex = _availableFloors.Count - 1;
            if (_currentFloorIndex < 0) _currentFloorIndex = 0;

            string currentFloorName = _availableFloors[_currentFloorIndex];

            // UI Updates
            if (lblCurrentFloor != null) lblCurrentFloor.Text = $" {_currentFloorIndex + 1}";
            if (btnPrevFloor != null) btnPrevFloor.Enabled = (_currentFloorIndex > 0);
            if (btnNextFloor != null) btnNextFloor.Enabled = (_currentFloorIndex < _availableFloors.Count - 1);

            LoadParkingArea(currentFloorName, chkDuzenlemeModu.Checked);
        }

        public static void OlusturSahteVeri()
        {
            _sahteVeritabani.Clear();
            var dbParkYerleri = DatabaseHelper.ParkYerleriniGetir();
            if (dbParkYerleri.Count > 0)
                _sahteVeritabani.AddRange(dbParkYerleri);
        }

        private void LoadParkingArea(string floorName, bool isEditMode)
        {
            if (string.IsNullOrEmpty(floorName)) return;

            int currentScrollX = Math.Abs(panel1.AutoScrollPosition.X);
            int currentScrollY = Math.Abs(panel1.AutoScrollPosition.Y);

            pnlWorld.Controls.Clear();
            pnlWorld.SuspendLayout();

            var gosterilecekler = _sahteVeritabani.Where(p => p.Tip == floorName && !p.ParkYeriId.StartsWith("TEST"));

            int maxWorldX = 0, maxWorldY = 0;

            foreach (var parkYeri in gosterilecekler)
            {
                Button parkSpot = new Button();
                parkSpot.Text = parkYeri.ParkYeriId;
                parkSpot.Tag = parkYeri.ParkYeriId;

                int finalX = (int)(parkYeri.PosX * _zoomFactor);
                int finalY = (int)(parkYeri.PosY * _zoomFactor);
                int finalW = (int)(parkYeri.Genislik * _zoomFactor);
                int finalH = (int)(parkYeri.Yukseklik * _zoomFactor);

                parkSpot.Location = new Point(finalX, finalY);
                parkSpot.Size = new Size(finalW, finalH);
                parkSpot.Font = new Font("Arial", (int)(10 * _zoomFactor * 0.8), FontStyle.Bold);
                parkSpot.ForeColor = Color.White;
                parkSpot.FlatStyle = FlatStyle.Flat;
                parkSpot.FlatAppearance.BorderSize = 1;

                if (finalX + finalW > maxWorldX) maxWorldX = finalX + finalW;
                if (finalY + finalH > maxWorldY) maxWorldY = finalY + finalH;

                if (isEditMode)
                {
                    parkSpot.BackColor = Color.Gray;
                    parkSpot.Cursor = Cursors.Hand;
                    parkSpot.MouseDown += Spot_MouseDown;
                    parkSpot.MouseMove += Spot_MouseMove;
                    parkSpot.MouseUp += Spot_MouseUp;
                }
                else
                {
                    switch (parkYeri.MevcutDurum)
                    {
                        case "BOŞ": parkSpot.BackColor = Color.ForestGreen; break;
                        case "DOLU": parkSpot.BackColor = Color.DarkRed; break;
                        case "ENGELLİ": parkSpot.BackColor = Color.DarkBlue; break; // OR "Engelli"
                        case "ARIZALI": parkSpot.BackColor = Color.DarkOrange; break;
                        default:
                            // Fallback for English logic if DB stores English
                            if (parkYeri.MevcutDurum == "Engelli") parkSpot.BackColor = Color.DarkBlue;
                            else parkSpot.BackColor = Color.Gray;
                            break;
                    }
                    parkSpot.MouseUp += ParkSpot_NormalClick;
                }

                parkSpot.MouseWheel += panel1_MouseWheel;
                pnlWorld.Controls.Add(parkSpot);
            }

            int padding = isEditMode ? 500 : 100;
            pnlWorld.Size = new Size(Math.Max(panel1.Width, maxWorldX + padding), Math.Max(panel1.Height, maxWorldY + padding));

            pnlWorld.ResumeLayout(false);
            pnlWorld.Invalidate();

            if (isEditMode)
            {
                int safeX = Math.Min(currentScrollX, Math.Max(0, pnlWorld.Width - panel1.Width));
                int safeY = Math.Min(currentScrollY, Math.Max(0, pnlWorld.Height - panel1.Height));
                panel1.AutoScrollPosition = new Point(safeX, safeY);
            }
        }

        private void pnlWorld_Paint(object sender, PaintEventArgs e)
        {
            if (!chkDuzenlemeModu.Checked) return;

            Pen gridPen = new Pen(Color.FromArgb(50, 200, 200, 200), 1);

            int zoomedGridSize = (int)(_gridSize * _zoomFactor);
            if (zoomedGridSize < 5) zoomedGridSize = 5;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            for (int x = 0; x <= pnlWorld.Width; x += zoomedGridSize)
            {
                e.Graphics.DrawLine(gridPen, x, 0, x, pnlWorld.Height);
            }

            for (int y = 0; y <= pnlWorld.Height; y += zoomedGridSize)
            {
                e.Graphics.DrawLine(gridPen, 0, y, pnlWorld.Width, y);
            }

            gridPen.Dispose();
        }

        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!chkDuzenlemeModu.Checked) return;

            ((HandledMouseEventArgs)e).Handled = true;

            Point mouseLocation = e.Location;
            if (sender != pnlWorld && sender != panel1 && sender is Control)
            {
                mouseLocation = panel1.PointToClient(((Control)sender).PointToScreen(e.Location));
            }

            Point currentScroll = panel1.AutoScrollPosition;
            int worldX_before = (Math.Abs(currentScroll.X) + mouseLocation.X);
            int worldY_before = (Math.Abs(currentScroll.Y) + mouseLocation.Y);

            float oldZoom = _zoomFactor;
            float zoomStep = 0.1f;

            if (e.Delta > 0) _zoomFactor += zoomStep;
            else _zoomFactor -= zoomStep;

            _zoomFactor = Math.Max(0.5f, Math.Min(_zoomFactor, 3.0f));

            if (oldZoom == _zoomFactor) return;

            if (_availableFloors.Count > 0)
            {
                string currentFloorName = _availableFloors[_currentFloorIndex];
                LoadParkingArea(currentFloorName, true);
            }

            float scaleRatio = _zoomFactor / oldZoom;
            int worldX_after = (int)(worldX_before * scaleRatio);
            int worldY_after = (int)(worldY_before * scaleRatio);

            int newScrollX = Math.Max(0, worldX_after - mouseLocation.X);
            int newScrollY = Math.Max(0, worldY_after - mouseLocation.Y);

            panel1.AutoScrollPosition = new Point(newScrollX, newScrollY);
        }

        // --- Drag & Drop ---
        private void Spot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _draggedControl = (Control)sender;
                _draggingStartPoint = e.Location;
                _originalDragLocation = _draggedControl.Location;
                _draggedControl.BringToFront();
            }
            else if (e.Button == MouseButtons.Right)
            {
                Control btn = (Control)sender;
                int temp = btn.Width;
                btn.Width = btn.Height;
                btn.Height = temp;
                btn.BringToFront();
            }
        }

        private void Spot_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && _draggedControl != null)
            {
                Point mousePosInPanel = pnlWorld.PointToClient(Control.MousePosition);
                int newX = mousePosInPanel.X - _draggingStartPoint.X;
                int newY = mousePosInPanel.Y - _draggingStartPoint.Y;

                int zoomedGridSize = (int)(_gridSize * _zoomFactor);
                if (zoomedGridSize == 0) zoomedGridSize = 1;

                int snappedX = (int)(Math.Round((double)newX / zoomedGridSize) * zoomedGridSize);
                int snappedY = (int)(Math.Round((double)newY / zoomedGridSize) * zoomedGridSize);

                snappedX = Math.Min(Math.Max(0, snappedX), pnlWorld.Width - _draggedControl.Width);
                snappedY = Math.Min(Math.Max(0, snappedY), pnlWorld.Height - _draggedControl.Height);

                _draggedControl.Location = new Point(snappedX, snappedY);
            }
        }

        private void Spot_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_isDragging || _draggedControl == null) return;

            _isDragging = false;
            Point finalLocation = _draggedControl.Location;
            Rectangle proposedRect = new Rectangle(finalLocation, _draggedControl.Size);
            proposedRect.Inflate(_minMargin, _minMargin);

            bool collisionDetected = false;
            foreach (Control ctrl in pnlWorld.Controls)
            {
                if (ctrl != _draggedControl && ctrl is Button)
                {
                    if (proposedRect.IntersectsWith(ctrl.Bounds))
                    {
                        collisionDetected = true;
                        break;
                    }
                }
            }

            if (collisionDetected)
            {
                MessageBox.Show("Collision detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _draggedControl.Location = _originalDragLocation;
            }
            _draggedControl = null;
        }

        private void ParkSpot_NormalClick(object sender, MouseEventArgs e)
        {
            Button tiklananSpot = (Button)sender;
            string parkYeriId = tiklananSpot.Tag.ToString();

            if (e.Button == MouseButtons.Left)
                ParkYeriBilgiPopupGoster(parkYeriId);
            else if (e.Button == MouseButtons.Right)
            {
                ParkYeriModel tiklananYer = _sahteVeritabani.FirstOrDefault(p => p.ParkYeriId == parkYeriId);
                if (tiklananYer != null) ShowParkSpotMenu(tiklananYer);
            }
        }

        private void ShowParkSpotMenu(ParkYeriModel parkYeri)
        {
            ParkSpotMenu menu = new ParkSpotMenu();
            menu.ParkYeriID = parkYeri.ParkYeriId;
            menu.MevcutDurum = parkYeri.MevcutDurum;
            menu.Location = Control.MousePosition;
            menu.EylemSecildi += (sender, e) => ParkSpotMenu_EylemSecildi(sender, parkYeri);
            menu.Show();
        }

        private void ParkSpotMenu_EylemSecildi(object sender, ParkYeriModel parkYeri)
        {
            ParkSpotMenu menu = (ParkSpotMenu)sender;
            if (menu.SecilenEylem == "NONE" || string.IsNullOrEmpty(menu.SecilenEylem)) return;

            // ... (Your DELETE code can remain here if it exists) ...

            // --- UPDATE DB ---
            parkYeri.MevcutDurum = menu.SecilenEylem;
            DatabaseHelper.ParkYeriDurumGuncelle(parkYeri.ParkYeriId, menu.SecilenEylem);

            UpdateFloorDisplay();

            // Refresh Dashboard if open
            foreach (Form f in Application.OpenForms)
            {
                if (f is dashboardForm d) d.VerileriGuncelle(_sahteVeritabani);
            }

            // 2. LOG TO HISTORY FOR DASHBOARD (Auto)
            if (menu.SecilenEylem == "DOLU")
            {
                // Quick Entry via Right Click (Defaulting to Guest for test)
                DatabaseHelper.HareketEkle("34-HZL-01", parkYeri.ParkYeriId, "Giriş", "Misafir");
            }
            else if (menu.SecilenEylem == "BOŞ")
            {
                // Exit Log
                DatabaseHelper.HareketEkle("34-HZL-01", parkYeri.ParkYeriId, "Çıkış", "Misafir", 50); // Example 50 TL
            }

            // 3. Refresh Screen
            UpdateFloorDisplay();

            // 4. Update Dashboard Immediately
            foreach (Form f in Application.OpenForms)
            {
                if (f is dashboardForm d)
                {
                    d.VerileriGuncelle(_sahteVeritabani);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var buttons = pnlWorld.Controls.OfType<Button>().ToList();

            for (int i = 0; i < buttons.Count; i++)
            {
                Rectangle rectA = buttons[i].Bounds;
                rectA.Inflate(_minMargin, _minMargin);
                for (int j = i + 1; j < buttons.Count; j++)
                {
                    if (rectA.IntersectsWith(buttons[j].Bounds))
                    {
                        MessageBox.Show("Collision detected! Could not save: " + buttons[i].Tag, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            try
            {
                foreach (Button btn in buttons)
                {
                    string id = btn.Tag.ToString();
                    var model = _sahteVeritabani.FirstOrDefault(p => p.ParkYeriId == id);

                    if (model != null)
                    {
                        model.PosX = (int)Math.Round(btn.Location.X / _zoomFactor);
                        model.PosY = (int)Math.Round(btn.Location.Y / _zoomFactor);
                        model.Genislik = (int)Math.Round(btn.Width / _zoomFactor);
                        model.Yukseklik = (int)Math.Round(btn.Height / _zoomFactor);
                        DatabaseHelper.ParkYeriKonumGuncelle(model);
                    }
                }
                MessageBox.Show("Locations and Orientations Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chkDuzenlemeModu.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save error: " + ex.Message);
            }
        }

        private void chkDuzenlemeModu_CheckedChanged(object sender, EventArgs e)
        {
            bool isEditMode = chkDuzenlemeModu.Checked;

            if (btnDuzenlemeyiKaydet != null) btnDuzenlemeyiKaydet.Visible = isEditMode;
            if (btnYerEkle_Click != null) btnYerEkle_Click.Visible = isEditMode;
            if (btnKatEkle != null) btnKatEkle.Visible = isEditMode;

            panel1.AutoScroll = isEditMode;

            UpdateFloorDisplay();
            pnlWorld.Refresh();
        }

        private void btnPrevFloor_Click(object sender, EventArgs e)
        {
            if (_currentFloorIndex > 0)
            {
                _currentFloorIndex--;
                UpdateFloorDisplay();
            }
        }

        private void btnNextFloor_Click(object sender, EventArgs e)
        {
            if (_currentFloorIndex < _availableFloors.Count - 1)
            {
                _currentFloorIndex++;
                UpdateFloorDisplay();
            }
        }

        private void pnlWorld_MouseDown(object sender, MouseEventArgs e)
        {
            if (!chkDuzenlemeModu.Checked) return;
            if (e.Button == MouseButtons.Middle)
            {
                _isPanning = true;
                _panLastMousePos = panel1.PointToClient(Control.MousePosition);
                pnlWorld.Cursor = Cursors.SizeAll;
            }
        }

        private void pnlWorld_MouseMove(object sender, MouseEventArgs e)
        {
            if (!chkDuzenlemeModu.Checked) return;
            if (_isPanning)
            {
                Point currentMousePos = panel1.PointToClient(Control.MousePosition);
                int deltaX = currentMousePos.X - _panLastMousePos.X;
                int deltaY = currentMousePos.Y - _panLastMousePos.Y;

                int currentX = Math.Abs(panel1.AutoScrollPosition.X);
                int currentY = Math.Abs(panel1.AutoScrollPosition.Y);

                int newX = currentX - deltaX;
                int newY = currentY - deltaY;

                panel1.AutoScrollPosition = new Point(newX, newY);
                _panLastMousePos = currentMousePos;
            }
        }

        private void pnlWorld_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                _isPanning = false;
                pnlWorld.Cursor = Cursors.Default;
            }
        }

        private void ParkYeriBilgiPopupGoster(string parkYeriId)
        {
            string dbYolu = "Data Source=parkwise.db;Version=3;";
            string plaka = "-", girisSaati = "-", durum = "BOŞ";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbYolu))
                {
                    conn.Open();
                    string sql = "SELECT Plaka, GirisSaati, MevcutDurum FROM ParkYerleri WHERE ParkYeriId = @id";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", parkYeriId);
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                plaka = dr["Plaka"]?.ToString();
                                girisSaati = dr["GirisSaati"]?.ToString();
                                durum = dr["MevcutDurum"]?.ToString();
                            }
                        }
                    }
                }
            }
            catch { }

            Form popup = new Form() { Text = "Parking Details", Size = new Size(300, 280), StartPosition = FormStartPosition.CenterScreen, BackColor = Color.White };
            Label lblBaslik = new Label() { Text = parkYeriId, Font = new Font("Arial Black", 20), Location = new Point(10, 10), AutoSize = true };

            // Translate status logic for Display ONLY
            string displayStatus = durum;
            if (durum == "DOLU") displayStatus = "OCCUPIED";
            else if (durum == "BOŞ") displayStatus = "EMPTY";
            else if (durum == "ENGELLİ" || durum == "Engelli") displayStatus = "DISABLED";

            Label lblDurum = new Label() { Text = displayStatus, Font = new Font("Arial", 12, FontStyle.Bold), Location = new Point(220, 15), AutoSize = true, ForeColor = (durum == "DOLU" ? Color.Red : Color.Green) };
            Label lblBilgi = new Label() { Text = $"Plate: {plaka}\nTime: {girisSaati}", Location = new Point(15, 60), AutoSize = true, Font = new Font("Arial", 10) };

            Button btnGit = new Button() { Text = (durum == "DOLU" ? "EXIT / INVOICE" : "ENTER"), Size = new Size(260, 50), Location = new Point(12, 180), BackColor = (durum == "DOLU" ? Color.DarkRed : Color.SeaGreen), ForeColor = Color.White, Font = new Font("Arial", 10, FontStyle.Bold) };

            btnGit.Click += (s, e) =>
            {
                popup.Close();
                OverviewSayfasinaGecVeDoldur(parkYeriId);
            };

            popup.Controls.AddRange(new Control[] { lblBaslik, lblDurum, lblBilgi, btnGit });
            popup.ShowDialog();
        }

        private void OverviewSayfasinaGecVeDoldur(string id)
        {
            mainForm anaForm = this.TopLevelControl as mainForm;
            if (anaForm != null) anaForm.DisaridanOverviewAc(id);
        }

        private string SiradakiParkYeriIsminiBul(string katAdi, int katNumarasi)
        {
            string sonIsim = "";
            string dbYolu = "Data Source=parkwise.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(dbYolu))
            {
                conn.Open();
                string sql = "SELECT ParkYeriId FROM ParkYerleri WHERE KatBilgisi = @kat ORDER BY length(ParkYeriId) DESC, ParkYeriId DESC LIMIT 1";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kat", katAdi);
                    object result = cmd.ExecuteScalar();
                    if (result != null) sonIsim = result.ToString();
                }
            }

            if (string.IsNullOrEmpty(sonIsim)) return $"K{katNumarasi}-A01";

            var match = System.Text.RegularExpressions.Regex.Match(sonIsim, @"^(.*?)(\d+)$");
            if (match.Success)
            {
                string prefix = match.Groups[1].Value;
                string numberStr = match.Groups[2].Value;
                int number = int.Parse(numberStr);
                number++;
                string newNumberStr = number.ToString(new string('0', numberStr.Length));
                return prefix + newNumberStr;
            }
            return sonIsim + "-1";
        }

        private int KatNumarasiVer(string katAdi)
        {
            if (string.IsNullOrEmpty(katAdi)) return -1;
            string sayi = new string(katAdi.Where(char.IsDigit).ToArray());
            if (int.TryParse(sayi, out int no)) return no;
            return 999;
        }

        public void VerileriYenile()
        {
            OlusturSahteVeri();
            KatListesiniGuncelle();
            UpdateFloorDisplay();
        }

        public void KataGit(string katAdi)
        {
            VerileriYenile();
            int index = _availableFloors.IndexOf(katAdi);
            if (index != -1)
            {
                _currentFloorIndex = index;
                UpdateFloorDisplay();
            }
        }

        private void btnYerEkle_Click_Click(object sender, EventArgs e)
        {
            if (_availableFloors.Count == 0 || _currentFloorIndex < 0) return;

            string suankiKat = _availableFloors[_currentFloorIndex];
            int katNo = KatNumarasiVer(suankiKat);
            string yeniId = SiradakiParkYeriIsminiBul(suankiKat, katNo);
            Point konum = YeniKonumHesapla(suankiKat);

            try
            {
                DatabaseHelper.ParkYeriEkle(yeniId, suankiKat, konum.X, konum.Y, 60, 120);
                foreach (Form f in Application.OpenForms)
                {
                    if (f is parkingAreaForm p) p.VerileriYenile();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private Point YeniKonumHesapla(string katAdi)
        {
            int w = 120; int h = 120; int bosluk = 20; int maxGenislik = 1000;
            var mevcutKutular = _sahteVeritabani.Where(p => p.Tip == katAdi).ToList();
            if (mevcutKutular.Count == 0) return new Point(20, 20);

            var sonKutu = mevcutKutular.OrderByDescending(p => p.PosY).ThenByDescending(p => p.PosX).First();

            int yeniX = sonKutu.PosX + w + bosluk;
            int yeniY = sonKutu.PosY;

            if (yeniX + w > maxGenislik)
            {
                yeniX = 20;
                yeniY = sonKutu.PosY + h + bosluk;
            }
            return new Point(yeniX, yeniY);
        }

        // --- BUTTON CLICK NOW OPENS MENU ---
        private void btnKatEkle_Click(object sender, EventArgs e)
        {
            if (_katMenu != null)
            {
                // Show menu right below the button
                _katMenu.Show(btnKatEkle, 0, btnKatEkle.Height);
            }
        }

        // --- ADD NEW FLOOR ---
        private void KatEkleIslemi()
        {
            int nextFloorNum = 1;
            string dbYolu = "Data Source=parkwise.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbYolu))
                {
                    conn.Open();
                    // Keeping "Kat" in SQL check to ensure we find existing floors correctly
                    string sqlMax = "SELECT MAX(CAST(SUBSTR(KatBilgisi, 5, INSTR(KatBilgisi, ' -') - 5) AS INTEGER)) FROM ParkYerleri WHERE KatBilgisi LIKE 'Kat % - Standart'";
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlMax, conn))
                    {
                        object res = cmd.ExecuteScalar();
                        if (res != DBNull.Value && res != null) nextFloorNum = Convert.ToInt32(res) + 1;
                    }

                    // Keeping internal name "Kat ..." to match database consistency with other forms
                    string yeniKatAdi = $"Kat {nextFloorNum} - Standart";
                    string ilkParkYeriAdi = $"K{nextFloorNum}-A01";
                    string sqlInsert = @"INSERT INTO ParkYerleri (ParkYeriId, KatBilgisi, MevcutDurum, PosX, PosY, Genislik, Yukseklik, Plaka, MusteriTipi, GirisSaati, TahminiUcret) VALUES (@id, @kat, 'BOŞ', 20, 20, 60, 120, '', '', '', 0)";

                    using (SQLiteCommand cmdInsert = new SQLiteCommand(sqlInsert, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("@id", ilkParkYeriAdi);
                        cmdInsert.Parameters.AddWithValue("@kat", yeniKatAdi);
                        cmdInsert.ExecuteNonQuery();
                    }
                }
                MessageBox.Show($"Floor {nextFloorNum} created.");

                // Refresh
                VerileriYenile();
                KataGit($"Kat {nextFloorNum} - Standart");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // --- NEW: DELETE FLOOR ---
        private void KatSilIslemi()
        {
            if (_availableFloors.Count == 0) return;
            string silinecekKat = _availableFloors[_currentFloorIndex];

            DialogResult cevap = MessageBox.Show(
                $"{silinecekKat} and ALL PARKING SPOTS on this floor will be deleted.\n\nAre you sure?",
                "Delete Floor Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (cevap == DialogResult.Yes)
            {
                string dbYolu = "Data Source=parkwise.db;Version=3;";
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(dbYolu))
                    {
                        conn.Open();
                        string sql = "DELETE FROM ParkYerleri WHERE KatBilgisi = @kat";
                        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@kat", silinecekKat);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Floor successfully deleted.");
                    VerileriYenile(); // Refresh
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Deletion error: " + ex.Message);
                }
            }
        }

        // --- UI SYSTEM: BOTTOM NAVIGATION PANEL ---
        private void AltMenuyuOlusturVeSabitle()
        {
            // 1. Check if exists
            Control[] mevcutPanel = this.Controls.Find("pnlBottomNav", true);
            if (mevcutPanel.Length > 0) return;

            // 2. Create New Bottom Panel
            Panel pnlBottomNav = new Panel();
            pnlBottomNav.Name = "pnlBottomNav";
            pnlBottomNav.Height = 55;
            pnlBottomNav.Dock = DockStyle.Bottom;
            pnlBottomNav.BackColor = Color.FromArgb(9, 9, 11);

            // Add to form
            this.Controls.Add(pnlBottomNav);
            pnlBottomNav.BringToFront();

            // 3. Move Buttons
            Control[] kontroller = { btnPrevFloor, lblCurrentFloor, btnNextFloor };

            foreach (var ctrl in kontroller)
            {
                if (ctrl == null) continue;

                ctrl.Parent = pnlBottomNav;
                ctrl.BackColor = Color.FromArgb(9, 9, 11);
                ctrl.ForeColor = Color.White;
                ctrl.Visible = true;
            }

            // 4. Align
            AltMenuyuHizala(pnlBottomNav);
            pnlBottomNav.Resize += (s, e) => AltMenuyuHizala(pnlBottomNav);
        }

        private void AltMenuyuHizala(Panel pnl)
        {
            if (btnPrevFloor == null || btnNextFloor == null || lblCurrentFloor == null) return;

            int centerX = pnl.Width / 2;
            int centerY = pnl.Height / 2;

            lblCurrentFloor.Location = new Point(centerX - (lblCurrentFloor.Width / 2), centerY - (lblCurrentFloor.Height / 2));
            btnPrevFloor.Location = new Point(lblCurrentFloor.Left - btnPrevFloor.Width - 15, centerY - (btnPrevFloor.Height / 2));
            btnNextFloor.Location = new Point(lblCurrentFloor.Right + 15, centerY - (btnNextFloor.Height / 2));
        }

        private void parkingAreaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint_1(object sender, PaintEventArgs e) { }
        private void pnlWorld_MouseEnter(object sender, EventArgs e) { }
        private void pnlWorld_Paint_1(object sender, PaintEventArgs e) { }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}