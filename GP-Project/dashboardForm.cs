using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace GP_Project
{
    public partial class dashboardForm : Form, ITitledPage
    {
        // UserControl'ler
        private UC_KpiKartlari _ucKpi;
        private UC_Grafikler _ucGrafik;
        private UC_GenelDurum _ucGenelDurum;

        public string PageTitle => "Dashboard";
        public Image PageIcon => Properties.Resources.dashboardslateblue;

        public dashboardForm()
        {
            InitializeComponent();

            _ucKpi = new UC_KpiKartlari();
            _ucGrafik = new UC_Grafikler();
            _ucGenelDurum = new UC_GenelDurum();

            _ucKpi.Dock = DockStyle.Fill;
            _ucGenelDurum.Dock = DockStyle.Fill;

            if (panel2 != null) panel2.Controls.Add(_ucGenelDurum);

            _ucGrafik.Visible = false;
            this.Controls.Add(_ucGrafik);

            _ucGrafik.SetPlots(formsPlotMusteri, formsPlotDoluluk);
        }

        private void dashboardForm_Load(object sender, EventArgs e)
        {
            if (panel2 != null) UIHelper.SetRoundedRegion(panel2, 20);
            if (panel3 != null) UIHelper.SetRoundedRegion(panel3, 20);
            if (panel5 != null) UIHelper.SetRoundedRegion(panel5, 20);

            // Form açıldığında verileri doldur
            DashboardEtiketleriniDoldur();
        }

        // Ana formdan veya başka yerden çağrıldığında çalışır
        public void VerileriGuncelle(List<ParkYeriModel> gelenVeri)
        {
            if (_ucKpi != null) _ucKpi.BilgileriGuncelle(gelenVeri);
            if (_ucGrafik != null) _ucGrafik.GrafikleriCiz(gelenVeri);
            if (_ucGenelDurum != null) _ucGenelDurum.VerileriYansit(gelenVeri);

            // Listeleri de güncelle
            DashboardEtiketleriniDoldur();
        }

        // --- VERİTABANINDAN ÇEKİP LABELLARA YAZAN METOT ---
        public void DashboardEtiketleriniDoldur()
        {
            try
            {
                // 1. SON GİRİŞLER (Veritabanında "Giriş" olarak kaydediyoruz)
                var sonGirisler = DatabaseHelper.SonHareketleriGetir("Giriş");
                Label[] lblGirisler = { label17, label18, label19, label20, label21 };
                VeriyiLabellaraYaz(lblGirisler, sonGirisler);

                // 2. SON ÇIKIŞLAR (Veritabanında "Çıkış" olarak kaydediyoruz)
                var sonCikislar = DatabaseHelper.SonHareketleriGetir("Çıkış");
                Label[] lblCikislar = { label23, label24, label25, label26, label27 };
                VeriyiLabellaraYaz(lblCikislar, sonCikislar);

                // 3. EN ÇOK KULLANILAN YERLER
                var enCok = DatabaseHelper.KullanimIstatistikleriniGetir(true);
                Label[] lblEnCok = { label6, label7, label8, label9, label10 };
                VeriyiLabellaraYaz(lblEnCok, enCok);

                // 4. EN AZ KULLANILAN YERLER
                var enAz = DatabaseHelper.KullanimIstatistikleriniGetir(false);
                Label[] lblEnAz = { label11, label12, label13, label14, label15 };
                VeriyiLabellaraYaz(lblEnAz, enAz);
            }
            catch (Exception ex)
            {
                // Hata olursa sessiz kal veya logla
                Console.WriteLine("Dashboard veri hatası: " + ex.Message);
            }
        }

        // Yardımcı: Listeyi Labellara dağıtır
        private void VeriyiLabellaraYaz(Label[] labellar, List<string> veriler)
        {
            for (int i = 0; i < labellar.Length; i++)
            {
                if (labellar[i] == null) continue;

                if (i < veriler.Count)
                {
                    labellar[i].Text = veriler[i];
                    labellar[i].ForeColor = Color.White;
                }
                else
                {
                    labellar[i].Text = "-"; // Veri yoksa boş
                    labellar[i].ForeColor = Color.Gray;
                }
            }
        }



        private void formsPlotMusteri_Load(object sender, EventArgs e) { }

        private void formsPlotDoluluk_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}