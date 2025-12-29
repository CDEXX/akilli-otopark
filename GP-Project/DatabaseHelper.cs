using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace GP_Project
{
    public static class DatabaseHelper
    {
        // Veritabanı dosya yolu
        private static string dbFile = "parkwise.db";
        private static string connectionString = $"Data Source={dbFile};Version=3;";

        // -------------------------------------------------------------------------
        // 1. GÜNLÜK CİRO (DÜZELTİLDİ: Tarih Formatı Sorununu Çözer)
        // -------------------------------------------------------------------------
        public static double GetGunlukCiro()
        {
            double toplam = 0;
            string dbYolu = "Data Source=parkwise.db;Version=3;";

            // Veritabanında tarihler farklı formatlarda olabilir, ikisini de kontrol edelim
            string format1 = DateTime.Now.ToString("yyyy-MM-dd"); // Örn: 2025-12-27
            string format2 = DateTime.Now.ToString("dd.MM.yyyy"); // Örn: 27.12.2025

            using (SQLiteConnection conn = new SQLiteConnection(dbYolu))
            {
                conn.Open();

                // 1. Araç Çıkış Gelirleri (GecmisHareketler tablosu)
                // IslemTipi 'Çıkış', 'ÇIKIŞ', 'CIKIS' vb. olan ve Zamanı bugünle başlayanlar
                string sqlArac = @"SELECT SUM(Ucret) FROM GecmisHareketler 
                                   WHERE IslemTipi IN ('Çıkış', 'ÇIKIŞ', 'Cikis', 'CIKIS') 
                                   AND (Zaman LIKE @f1 OR Zaman LIKE @f2)";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlArac, conn))
                {
                    cmd.Parameters.AddWithValue("@f1", format1 + "%");
                    cmd.Parameters.AddWithValue("@f2", format2 + "%");
                    object sonuc = cmd.ExecuteScalar();
                    if (sonuc != DBNull.Value && sonuc != null)
                    {
                        toplam += Convert.ToDouble(sonuc);
                    }
                }

                // 2. Abone Satış Gelirleri (AboneOdemeGecmisi tablosu)
                // IslemTarihi bugünle başlayanlar
                string sqlAbone = @"SELECT SUM(Tutar) FROM AboneOdemeGecmisi 
                                    WHERE (IslemTarihi LIKE @f1 OR IslemTarihi LIKE @f2)";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlAbone, conn))
                {
                    cmd.Parameters.AddWithValue("@f1", format1 + "%");
                    cmd.Parameters.AddWithValue("@f2", format2 + "%");
                    object sonuc = cmd.ExecuteScalar();
                    if (sonuc != DBNull.Value && sonuc != null)
                    {
                        toplam += Convert.ToDouble(sonuc);
                    }
                }
            }
            return toplam;
        }

        // -------------------------------------------------------------------------
        // 2. TABLO OLUŞTURMA
        // -------------------------------------------------------------------------
        public static void TablolariOlustur()
        {
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
            }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // 1. PARK YERLERİ TABLE
                string sqlPark = @"CREATE TABLE IF NOT EXISTS ParkYerleri (
                                ParkYeriId TEXT PRIMARY KEY,
                                KatBilgisi TEXT,
                                MevcutDurum TEXT DEFAULT 'BOŞ',
                                PosX INTEGER,
                                PosY INTEGER,
                                Genislik INTEGER,
                                Yukseklik INTEGER,
                                Plaka TEXT,
                                MusteriTipi TEXT,
                                GirisSaati TEXT,
                                TahminiUcret REAL
                            );";

                // 2. GEÇMİŞ HAREKETLER TABLE
                string sqlGecmis = @"CREATE TABLE IF NOT EXISTS GecmisHareketler (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Plaka TEXT,
                                ParkYeriId TEXT,
                                IslemTipi TEXT,
                                Zaman TEXT,
                                Ucret REAL,
                                MusteriTipi TEXT
                            );";

                // 3. ABONELER TABLE
                string sqlAbone = @"CREATE TABLE IF NOT EXISTS Aboneler (
                                AboneId INTEGER PRIMARY KEY AUTOINCREMENT,
                                AdSoyad TEXT,
                                Plaka TEXT,
                                Telefon TEXT,
                                BaslangicTarihi TEXT,
                                BitisTarihi TEXT,
                                KaraListe INTEGER DEFAULT 0,
                                Tarife TEXT,
                                OdenenUcret REAL DEFAULT 0
                            );";

                // 4. KULLANICILAR TABLE
                string sqlUser = @"CREATE TABLE IF NOT EXISTS Kullanicilar (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                AdSoyad TEXT,
                                KullaniciAdi TEXT UNIQUE,
                                Sifre TEXT,
                                Email TEXT,
                                Telefon TEXT
                            );";

                // 5. TARIFELER TABLE
                string sqlTarife = @"CREATE TABLE IF NOT EXISTS Tarifeler (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                TarifeAdi TEXT,
                                Fiyat REAL
                            );";

                // 6. ABONE ÖDEME GEÇMİŞİ TABLE
                string sqlOdeme = @"CREATE TABLE IF NOT EXISTS AboneOdemeGecmisi (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                AboneId INTEGER,
                                IslemDetayi TEXT,
                                SubeId INTEGER DEFAULT 1,
                                IslemTarihi TEXT,
                                Tutar REAL
                            );";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlPark, conn)) cmd.ExecuteNonQuery();
                using (SQLiteCommand cmd = new SQLiteCommand(sqlGecmis, conn)) cmd.ExecuteNonQuery();
                using (SQLiteCommand cmd = new SQLiteCommand(sqlAbone, conn)) cmd.ExecuteNonQuery();
                using (SQLiteCommand cmd = new SQLiteCommand(sqlUser, conn)) cmd.ExecuteNonQuery();
                using (SQLiteCommand cmd = new SQLiteCommand(sqlTarife, conn)) cmd.ExecuteNonQuery();
                using (SQLiteCommand cmd = new SQLiteCommand(sqlOdeme, conn)) cmd.ExecuteNonQuery();

                // Initialize Default Tariffs
                string checkTariff = "SELECT COUNT(*) FROM Tarifeler";
                using (SQLiteCommand cmd = new SQLiteCommand(checkTariff, conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        string insertTariffs = "INSERT INTO Tarifeler (TarifeAdi, Fiyat) VALUES ('0-1 Saat', 20), ('1-3 Saat', 40), ('Günlük', 100)";
                        using (SQLiteCommand ins = new SQLiteCommand(insertTariffs, conn)) ins.ExecuteNonQuery();
                    }
                }
            }
        }

        // -------------------------------------------------------------------------
        // 3. VERİ ÇEKME VE EKLEME
        // -------------------------------------------------------------------------
        public static List<ParkYeriModel> ParkYerleriniGetir()
        {
            var list = new List<ParkYeriModel>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM ParkYerleri";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new ParkYeriModel
                            {
                                ParkYeriId = dr["ParkYeriId"].ToString(),
                                MevcutDurum = dr["MevcutDurum"].ToString(),
                                Tip = dr["KatBilgisi"].ToString(),
                                PosX = Convert.ToInt32(dr["PosX"]),
                                PosY = Convert.ToInt32(dr["PosY"]),
                                Genislik = Convert.ToInt32(dr["Genislik"]),
                                Yukseklik = Convert.ToInt32(dr["Yukseklik"]),

                                // HATA ÇÖZÜMÜ: Convert.ToDouble YERİNE Convert.ToDecimal KULLANILDI
                                TahminiUcret = dr["TahminiUcret"] != DBNull.Value ? Convert.ToDecimal(dr["TahminiUcret"]) : 0m,

                                MusteriTipi = dr["MusteriTipi"] != DBNull.Value ? dr["MusteriTipi"].ToString() : ""
                            });
                        }
                    }
                }
            }
            return list;
        }

        public static void ParkYeriEkle(string id, string kat, int x, int y, int w = 120, int h = 120)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO ParkYerleri (ParkYeriId, KatBilgisi, PosX, PosY, Genislik, Yukseklik, MevcutDurum) VALUES (@id, @kat, @x, @y, @w, @h, 'BOŞ')";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@kat", kat);
                    cmd.Parameters.AddWithValue("@x", x);
                    cmd.Parameters.AddWithValue("@y", y);
                    cmd.Parameters.AddWithValue("@w", w);
                    cmd.Parameters.AddWithValue("@h", h);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // -------------------------------------------------------------------------
        // 4. KULLANICI İŞLEMLERİ
        // -------------------------------------------------------------------------

        public static bool KullaniciKaydet(string adSoyad, string kAdi, string sifre, string email, string telefon)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                try
                {
                    string sql = "INSERT INTO Kullanicilar (AdSoyad, KullaniciAdi, Sifre, Email, Telefon) VALUES (@ad, @kadi, @sifre, @email, @tel)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ad", adSoyad);
                        cmd.Parameters.AddWithValue("@kadi", kAdi);
                        cmd.Parameters.AddWithValue("@sifre", sifre);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@tel", telefon);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (SQLiteException)
                {
                    return false;
                }
            }
        }

        public static bool KullaniciGirisKontrol(string kAdi, string sifre)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi=@kadi AND Sifre=@sifre";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kadi", kAdi);
                    cmd.Parameters.AddWithValue("@sifre", sifre);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        // -------------------------------------------------------------------------
        // 5. GÜNCELLEME METOTLARI
        // -------------------------------------------------------------------------

        public static void ParkYeriDurumGuncelle(string id, string yeniDurum)
        {
            string dbYolu = "Data Source=parkwise.db;Version=3;";

            string eskiDurum = "";
            using (SQLiteConnection conn = new SQLiteConnection(dbYolu))
            {
                conn.Open();
                string sqlCheck = "SELECT MevcutDurum FROM ParkYerleri WHERE ParkYeriId = @id";
                using (SQLiteCommand cmd = new SQLiteCommand(sqlCheck, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    object result = cmd.ExecuteScalar();
                    if (result != null) eskiDurum = result.ToString();
                }
            }

            if (eskiDurum == yeniDurum) return;

            using (SQLiteConnection conn = new SQLiteConnection(dbYolu))
            {
                conn.Open();
                string sql = "UPDATE ParkYerleri SET MevcutDurum = @d WHERE ParkYeriId = @id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@d", yeniDurum);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            // OTOMATİK GEÇMİŞE EKLEME
            if (yeniDurum == "DOLU")
            {
                HareketEkle("PLAKA-GİRİŞ", id, "GİRİŞ", "Bilinmiyor", 0);
            }
            else if (yeniDurum == "BOŞ" && eskiDurum == "DOLU")
            {
                HareketEkle("PLAKA-ÇIKIŞ", id, "ÇIKIŞ", "Bilinmiyor", 0);
            }
        }

        public static void ParkYeriKonumGuncelle(ParkYeriModel model)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sqlCheck = "SELECT COUNT(*) FROM ParkYerleri WHERE ParkYeriId = @id";
                using (SQLiteCommand cmd = new SQLiteCommand(sqlCheck, conn))
                {
                    cmd.Parameters.AddWithValue("@id", model.ParkYeriId);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        string sqlUpdate = @"UPDATE ParkYerleri 
                                             SET PosX=@x, PosY=@y, Genislik=@w, Yukseklik=@h 
                                             WHERE ParkYeriId=@id";
                        using (SQLiteCommand upCmd = new SQLiteCommand(sqlUpdate, conn))
                        {
                            upCmd.Parameters.AddWithValue("@x", model.PosX);
                            upCmd.Parameters.AddWithValue("@y", model.PosY);
                            upCmd.Parameters.AddWithValue("@w", model.Genislik);
                            upCmd.Parameters.AddWithValue("@h", model.Yukseklik);
                            upCmd.Parameters.AddWithValue("@id", model.ParkYeriId);
                            upCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string sqlInsert = @"INSERT INTO ParkYerleri (ParkYeriId, KatBilgisi, PosX, PosY, Genislik, Yukseklik, MevcutDurum) 
                                             VALUES (@id, @kat, @x, @y, @w, @h, 'BOŞ')";
                        using (SQLiteCommand insCmd = new SQLiteCommand(sqlInsert, conn))
                        {
                            insCmd.Parameters.AddWithValue("@id", model.ParkYeriId);
                            insCmd.Parameters.AddWithValue("@kat", model.Tip);
                            insCmd.Parameters.AddWithValue("@x", model.PosX);
                            insCmd.Parameters.AddWithValue("@y", model.PosY);
                            insCmd.Parameters.AddWithValue("@w", model.Genislik);
                            insCmd.Parameters.AddWithValue("@h", model.Yukseklik);
                            insCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        // -------------------------------------------------------------------------
        // 6. DASHBOARD & İSTATİSTİK METOTLARI (DÜZELTİLMİŞ)
        // -------------------------------------------------------------------------

        public static List<string> SonHareketleriGetir(string islemTipi)
        {
            var liste = new List<string>();
            string dbYolu = "Data Source=parkwise.db;Version=3;";

            // Büyük/Küçük harf duyarlılığını kaldırmak için tüm varyasyonları arıyoruz
            string[] aranacaklar;
            if (islemTipi.StartsWith("G") || islemTipi.StartsWith("g"))
                aranacaklar = new string[] { "Giriş", "GİRİŞ", "Giris", "GIRIS", "giriş" };
            else
                aranacaklar = new string[] { "Çıkış", "ÇIKIŞ", "Cikis", "CIKIS", "cikis", "Çıkıs" };

            using (SQLiteConnection conn = new SQLiteConnection(dbYolu))
            {
                conn.Open();
                string sql = $@"SELECT Plaka, ParkYeriId, Zaman 
                                FROM GecmisHareketler 
                                WHERE IslemTipi IN ('{string.Join("','", aranacaklar)}') 
                                ORDER BY Id DESC LIMIT 5";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string zamanStr = dr["Zaman"].ToString();
                            // Saati temizle (Sadece HH:mm al)
                            try { zamanStr = DateTime.Parse(zamanStr).ToString("HH:mm"); } catch { }

                            string veri = $"{dr["Plaka"]} - {dr["ParkYeriId"]} - {zamanStr}";
                            liste.Add(veri);
                        }
                    }
                }
            }
            return liste;
        }

        public static List<string> KullanimIstatistikleriniGetir(bool enCok)
        {
            var liste = new List<string>();
            string dbYolu = "Data Source=parkwise.db;Version=3;";
            string siralama = enCok ? "DESC" : "ASC";

            using (SQLiteConnection conn = new SQLiteConnection(dbYolu))
            {
                conn.Open();
                string sql = $@"SELECT ParkYeriId, COUNT(*) as KullanimSayisi 
                                FROM GecmisHareketler 
                                WHERE IslemTipi IN ('Giriş', 'GİRİŞ', 'Giris', 'GIRIS') 
                                GROUP BY ParkYeriId 
                                ORDER BY KullanimSayisi {siralama} LIMIT 5";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string veri = $"{dr["ParkYeriId"]} ({dr["KullanimSayisi"]})";
                            liste.Add(veri);
                        }
                    }
                }
            }
            return liste;
        }

        public static void HareketEkle(string plaka, string parkYeriId, string islemTipi, string musteriTipi, double ucret = 0)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                // Tarihi standart ISO formatında (yyyy-MM-dd) kaydediyoruz ki aramalar kolay olsun
                string zaman = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string sql = @"INSERT INTO GecmisHareketler (Plaka, ParkYeriId, IslemTipi, Zaman, Ucret, MusteriTipi) 
                               VALUES (@plaka, @yer, @islem, @zaman, @ucret, @mtip)";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@plaka", plaka);
                    cmd.Parameters.AddWithValue("@yer", parkYeriId);
                    cmd.Parameters.AddWithValue("@islem", islemTipi);
                    cmd.Parameters.AddWithValue("@zaman", zaman);
                    cmd.Parameters.AddWithValue("@ucret", ucret);
                    cmd.Parameters.AddWithValue("@mtip", musteriTipi);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void ParkYeriSil(string parkYeriId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM ParkYerleri WHERE ParkYeriId = @id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", parkYeriId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}