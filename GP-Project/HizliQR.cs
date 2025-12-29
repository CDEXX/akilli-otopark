using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics; // Linki açmak için gerekli kütüphane
using QRCoder;

public static class HizliQR
{
    public static void Goster(string url)
    {
        // 1. AYARLAR
        int genislik = 250;
        int yukseklik = 300;
        Bitmap bitmap = new Bitmap(genislik, yukseklik);

        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // 2. QR KODU OLUŞTUR
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrImage = qrCode.GetGraphic(20);

            // 3. QR'I ORTALA
            int qrBoyut = 200;
            int qrX = (genislik - qrBoyut) / 2;
            g.DrawImage(qrImage, qrX, 20, qrBoyut, qrBoyut);

            // 4. SAATİ YAZ
            string saatMetni = DateTime.Now.ToString("HH:mm");
            Font saatFont = new Font("Segoe UI", 24, FontStyle.Bold);

            SizeF metinBoyutu = g.MeasureString(saatMetni, saatFont);
            float yaziX = (genislik - metinBoyutu.Width) / 2;
            float yaziY = 230;

            g.DrawString(saatMetni, saatFont, Brushes.Black, yaziX, yaziY);

            // "Tıkla Git" yazısı (İstersen ekle, yoksa silebilirsin)
            Font bilgiFont = new Font("Segoe UI", 8, FontStyle.Regular);
            g.DrawString("(Gitmek için tıkla)", bilgiFont, Brushes.Gray, (genislik - g.MeasureString("(Gitmek için tıkla)", bilgiFont).Width) / 2, yaziY + 40);

            g.DrawRectangle(Pens.Black, 0, 0, genislik - 1, yukseklik - 1);
        }

        // 5. POPUP FORMU
        using (Form popup = new Form())
        {
            popup.FormBorderStyle = FormBorderStyle.None;
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.Size = new Size(genislik, yukseklik);
            popup.BackgroundImage = bitmap;
            popup.BackgroundImageLayout = ImageLayout.Stretch;
            popup.Cursor = Cursors.Hand; // El işareti çıksın ki tıklanacağı belli olsun
            popup.TopMost = true;

            // --- BURAYI DEĞİŞTİRDİK ---
            // Tıklayınca tarayıcıyı aç ve formu kapat
            popup.Click += (s, e) =>
            {
                try
                {
                    // Varsayılan tarayıcıda linki aç
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Link açılamadı: " + ex.Message);
                }
                popup.Close(); // İşlem bitince pencereyi kapat
            };

            // ESC tuşuyla çıkış imkanı kalsın
            popup.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) popup.Close(); };

            popup.ShowDialog();
        }
    }
}