using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static GP_Project.parkingAreaForm;

namespace GP_Project
{
    public partial class UC_KpiKartlari : UserControl
    {
        public UC_KpiKartlari()
        {
            InitializeComponent();
        }

        public void BilgileriGuncelle(List<ParkYeriModel> veriler)
        {
            if (veriler == null) return;

            // Hesaplamalar
            int dolu = veriler.Count(x => x.MevcutDurum == "DOLU");
            decimal ciro = veriler.Sum(x => x.TahminiUcret);
            int kapasite = veriler.Count;
            double oran = kapasite > 0 ? ((double)dolu / kapasite) * 100 : 0;

            // Ekrana Yazdırma
            lblAracSayisi.Text = $"{dolu} / {kapasite}";
            lblCiro.Text = $"{ciro:N0} ₺";
            lblDoluluk.Text = $"%{oran:F1}";
        }

        private void UC_KpiKartlari_Load(object sender, EventArgs e)
        {
            // Tasarımcı hatasını önlemek için boş metot
        }
    }
}