using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ScottPlot;
using ScottPlot.WinForms;

namespace GP_Project
{
    public partial class UC_Grafikler : UserControl
    {
        private FormsPlot _formsPlotDoluluk;
        private FormsPlot _formsPlotMusteri;

        public UC_Grafikler()
        {
            InitializeComponent();
        }

        public void SetPlots(FormsPlot formsPlotDoluluk, FormsPlot formsPlotMusteri)
        {
            _formsPlotDoluluk = formsPlotDoluluk;
            _formsPlotMusteri = formsPlotMusteri;

            SetupFormsPlot(_formsPlotDoluluk);
            SetupFormsPlot(_formsPlotMusteri);
        }

        private void SetupFormsPlot(FormsPlot fp)
        {
            if (fp == null) return;

            // Grafik ayarları (Zoom/Pan kapalı)
            fp.Configuration.Pan = false;
            fp.Configuration.Zoom = false;
            fp.Configuration.ScrollWheelZoom = false;
            fp.Configuration.MiddleClickDragZoom = false;
            fp.Configuration.RightClickDragZoom = false;

            ApplyDarkThemeV4(fp.Plot);
            fp.Render();
        }

        // --- ANA HESAPLAMA METODU ---
        public void GrafikleriCiz(List<ParkYeriModel> veriler)
        {
            if (veriler == null) return;
            if (_formsPlotDoluluk == null || _formsPlotMusteri == null) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => GrafikleriCiz(veriler)));
                return;
            }

            // 1. Genel Doluluk (Dolu / Boş) - Arızalı kaldırıldı
            int dolu = veriler.Count(x => x.MevcutDurum == "DOLU");
            int bos = veriler.Count - dolu; // Kalan her yer boş sayılır

            // 2. Müşteri Tipi (Sadece "DOLU" olan araçlara bakılır)
            // Veritabanındaki 'MusteriTipi' sütununa bakar
            int abone = veriler.Count(x => x.MevcutDurum == "DOLU" && x.MusteriTipi == "Abone");
            int misafir = veriler.Count(x => x.MevcutDurum == "DOLU" && x.MusteriTipi == "Misafir");

            // Grafikleri Çiz
            DolulukCiz(dolu, bos);
            MusteriTipiCiz(abone, misafir);
        }

        // --- DOLULUK GRAFİĞİ (Sadece Dolu ve Boş) ---
        private void DolulukCiz(int dolu, int bos)
        {
            // Veri yoksa hata vermemesi için demo veri
            if (dolu + bos == 0) { dolu = 1; bos = 1; }

            var plt = _formsPlotDoluluk.Plot;
            plt.Clear();

            double[] values = { dolu, bos };
            string[] labels = { "Dolu", "Boş" };
            Color[] colors = {
                Color.FromArgb(194, 36, 36),   // Kırmızı (Dolu)
                Color.FromArgb(120, 167, 90)   // Yeşil (Boş)
            };

            var pie = plt.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowPercentages = true;
            pie.SliceFillColors = colors;

            ApplyDarkThemeV4(plt);
            MakePieSmaller(plt);

            _formsPlotDoluluk.Render();
        }

        // --- MÜŞTERİ TİPİ GRAFİĞİ (Abone vs Misafir) ---
        private void MusteriTipiCiz(int abone, int misafir)
        {
            // Eğer içeride hiç araç yoksa grafik boş kalmasın diye geçici çözüm
            // İstersen buraya 'Veri Yok' yazdırabiliriz ama şimdilik demo veri kalsın
            if (abone + misafir == 0) { abone = 1; misafir = 1; }

            var plt = _formsPlotMusteri.Plot;
            plt.Clear();

            var pie = plt.AddPie(new double[] { abone, misafir });
            pie.SliceLabels = new[] { "Abone", "Misafir" };
            pie.ShowPercentages = true;

            // Donut Görünümü (Ortası Delik)
            try { pie.DonutSize = 0.55; } catch { }

            // Renkler (Senin seçtiklerin)
            pie.SliceFillColors = new[]
            {
                Color.FromArgb(89, 133, 225), // Mavi (Abone)
                Color.FromArgb(230, 77, 25)   // Turuncu (Misafir)
            };

            ApplyDarkThemeV4(plt);
            MakePieSmaller(plt);

            _formsPlotMusteri.Render();
        }

        private void MakePieSmaller(ScottPlot.Plot plt)
        {
            try { plt.SetAxisLimits(-1.4, 1.4, -1.4, 1.4); return; } catch { }
        }

        private void ApplyDarkThemeV4(ScottPlot.Plot plt)
        {
            try
            {
                plt.Style(
                    figureBackground: Color.Black,
                    dataBackground: Color.Black,
                    tick: Color.Gainsboro
                );
            }
            catch { }

            try { plt.Grid(enable: false); } catch { }
            try { plt.Grid(false); } catch { }
        }

        private void UC_Grafikler_Load(object sender, EventArgs e) { }
        private void UC_Grafikler_Load_1(object sender, EventArgs e) { }
    }
}