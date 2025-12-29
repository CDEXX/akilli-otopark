using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GP_Project
{
    public partial class UC_GenelDurum : UserControl
    {
        private DashboardService _servis = new DashboardService();

        public UC_GenelDurum()
        {
            InitializeComponent();
        }

        public void VerileriYansit(List<ParkYeriModel> gelenVeri)
        {
            // 1. Kapasite (Örn: 15/50)
            lblAracSayisi.Text = _servis.GetKapasiteMetni(gelenVeri);

            // 2. Doluluk Oranı (Örn: %30)
            lblDoluluk.Text = _servis.GetDolulukYuzdesi(gelenVeri);

            // 3. CİRO (Örn: ₺1.500) - ARTIK GÜNLÜK KAZANCI YAZIYOR
            lblCiro.Text = _servis.GetGunlukKazanc();
            lblCiro.ForeColor = System.Drawing.Color.LightGreen; // Kazanç olduğu için yeşil yapalım

            // 4. Kat Durumları (Örn: K1:%50 K2:%20)
            lblKatlar.Text = _servis.GetKatDurumu(gelenVeri);
        }

        private void UC_GenelDurum_Load(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
    }
}