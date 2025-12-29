using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GP_Project
{
    public class DashboardService
    {
        // 1. ARAÇ SAYISI / KAPASİTE
        public string GetKapasiteMetni(List<ParkYeriModel> veriler)
        {
            if (veriler == null) return "0/0";
            int dolu = veriler.Count(x => x.MevcutDurum == "DOLU");
            int toplam = veriler.Count;
            return $"{dolu}/{toplam}";
        }

        // 2. DOLULUK YÜZDESİ
        public string GetDolulukYuzdesi(List<ParkYeriModel> veriler)
        {
            if (veriler == null || veriler.Count == 0) return "%0";
            int dolu = veriler.Count(x => x.MevcutDurum == "DOLU");
            double yuzde = ((double)dolu / veriler.Count) * 100;
            return $"%{yuzde:0}";
        }

        // 3. BEKLEYEN CİRO
        public string GetTahminiCiro(List<ParkYeriModel> veriler)
        {
            if (veriler == null) return "0 TL";
            decimal ciro = (decimal)veriler.Where(x => x.MevcutDurum == "DOLU").Sum(x => x.TahminiUcret);
            return $"{ciro:N0} TL";
        }

        // 4. KAT BAZLI DOLULUK (DÜZELTİLDİ: ALT ALTA SIRALAR)
        public string GetKatDurumu(List<ParkYeriModel> veriler)
        {
            if (veriler == null || veriler.Count == 0) return "-";

            var katGruplari = veriler.GroupBy(x => x.Tip);
            List<string> sonuclar = new List<string>();

            foreach (var grup in katGruplari.OrderBy(g => g.Key))
            {
                string katAdi = grup.Key;
                // İsim kısaltma (Kat 1 -> K1)
                var match = Regex.Match(katAdi, @"\d+");
                string kisaAd = match.Success ? "K" + match.Value : katAdi.Substring(0, Math.Min(3, katAdi.Length));

                int toplam = grup.Count();
                int dolu = grup.Count(x => x.MevcutDurum == "DOLU");
                double yuzde = toplam > 0 ? ((double)dolu / toplam) * 100 : 0;

                sonuclar.Add($"{kisaAd}: %{yuzde:0}");
            }

            // DÜZELTME BURADA: "  " YERİNE "\n" KULLANILDI (Alt satıra geçer)
            return string.Join("\n", sonuclar);
        }

        // 5. GÜNLÜK GERÇEK KAZANÇ
        public string GetGunlukKazanc()
        {
            double kazanc = DatabaseHelper.GetGunlukCiro();
            return kazanc.ToString("C0");
        }
    }
}