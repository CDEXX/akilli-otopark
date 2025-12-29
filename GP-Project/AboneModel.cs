using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP_Project
{
    public class AboneModel
    {
        public string AdSoyad { get; set; }
        public string Plaka { get; set; }
        public string Telefon { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public bool KaraListe { get; set; }
        public List<string> OdemeGecmisi { get; set; } = new List<string>(); // Ödeme geçmişi için basit liste

        // Tarife bilgisi (1 Ay, 3 Ay, 1 Yıl)
        public string Tarife { get; set; }
        public decimal OdenenUcret { get; set; }
    }
}
