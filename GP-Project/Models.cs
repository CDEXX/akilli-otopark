using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GP_Project
{
    // DİKKAT: Namespace sadece 'GP_Project'. 'Models' yok.

    public class ParkYeriModel
    {
        public string ParkYeriId { get; set; }
        public string Tip { get; set; }
        public string MevcutDurum { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Genislik { get; set; }
        public int Yukseklik { get; set; }
        public string Plaka { get; set; }
        public string MusteriTipi { get; set; }
        public DateTime? GirisSaati { get; set; }
        public decimal TahminiUcret { get; set; }
        public int KullanimSayisi { get; set; } = 0;
    }

    public class AboneModel
    {
        public int AboneId { get; set; }
        public string AdSoyad { get; set; }
        public string Plaka { get; set; }
        public string Telefon { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public bool KaraListe { get; set; }
        public string Tarife { get; set; }
        public decimal OdenenUcret { get; set; }
        public List<string> OdemeGecmisi { get; set; } = new List<string>();
    }

    public class GecmisHareketModel
    {
        public string Plaka { get; set; }
        public string ParkYeriId { get; set; }
        public string IslemTipi { get; set; }
        public DateTime Zaman { get; set; }
        public decimal Ucret { get; set; }
        public string MusteriTipi { get; set; }
    }
    // Çıkış işleminde "3-5 Saat" aralığını hesaplamak için
    public class TarifeAraligi
    {
        public double AltSinirSaat { get; set; }
        public double UstSinirSaat { get; set; }
        public double Fiyat { get; set; }
    }

    // Abonelik ekranındaki Akıllı Model
    public class AboneTarifeModel
    {
        public string Ad { get; set; }
        public double Fiyat { get; set; }

        // Otomatik Gün Hesaplama Motoru
        public int GunSayisi
        {
            get
            {
                return GunHesapla(Ad);
            }
        }

        private int GunHesapla(string tarifeAdi)
        {
            if (string.IsNullOrEmpty(tarifeAdi)) return 0;

            string kucukAd = tarifeAdi.ToLower(new CultureInfo("tr-TR"));

            int miktar = 1;
            var sayiMatch = Regex.Match(kucukAd, @"(\d+)");
            if (sayiMatch.Success) int.TryParse(sayiMatch.Value, out miktar);

            if (kucukAd.Contains("yıl") || kucukAd.Contains("sene")) return miktar * 365;
            else if (kucukAd.Contains("ay")) return miktar * 30;
            else if (kucukAd.Contains("gün") || kucukAd.Contains("gun")) return miktar;

            return 0;
        }
    }
}