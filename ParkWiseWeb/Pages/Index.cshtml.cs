using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ParkWiseWeb.Pages
{
    public class IndexModel : PageModel
    {
        // Ekranda göstereceðimiz deðiþkenler
        public string Plaka { get; set; } = "Belirtilmedi";
        public string ParkYeri { get; set; } = "-";
        public string GirisSaati { get; set; } = "-";
        public string DurumMesaji { get; set; } = "Veri Bekleniyor...";

        public void OnGet()
        {
            // URL'den gelen verileri (Query String) yakalýyoruz
            // Örn: ?plaka=34ABC&yer=A01&saat=12:00

            string p = Request.Query["plaka"];
            string y = Request.Query["yer"];
            string s = Request.Query["saat"];

            if (!string.IsNullOrEmpty(p) && !string.IsNullOrEmpty(y))
            {
                Plaka = p;
                ParkYeri = y;
                GirisSaati = s;
                DurumMesaji = "Ödeme Bekleniyor";
            }
            else
            {
                DurumMesaji = "Park Bilgisi Bulunamadý";
            }
        }
    }
}