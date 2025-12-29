
# 🚗 ParkWise - Akıllı Otopark Otomasyon Sistemi

ParkWise, modern otopark yönetimi için geliştirilmiş; **C# (Windows Forms)** tabanlı masaüstü uygulaması ve **Firebase** entegreli web arayüzüne sahip kapsamlı bir otomasyon sistemidir.

Bu proje, araçları otoparka kayıt etmeyi, otopark doluluk oranlarını görsel olarak izlemeyi, abone yönetimini, ücretlendirme tarifelerini ve detaylı raporlamayı tek bir merkezden yönetmeyi sağlar.

## ✨ Özellikler

- **Görsel Otopark Haritası:** Kat ve slot bazlı (K1-A05 gibi) doluluk durumunu anlık renkli butonlarla izleme.

- **Abone Yönetimi:** Müşteri kaydı, abonelik tipi (Aylık/Yıllık) ve kara liste yönetimi.

- **Akıllı Ücretlendirme:** Giriş-çıkış saatlerine göre otomatik tarife hesaplama (0-1 saat, Günlük vb.).

- **Veritabanı Yönetimi:** SQLite ile hızlı ve güvenilir veri saklama (`parkwise.db`).

- **Web Entegrasyonu:** Firebase Hosting sayesinde araç durumunu ve giriş saatlerini web sitesinden anlık takip etme.

- **Raporlama:** Geçmiş hareketler ve ödeme geçmişi logları.

## 🚀 Kurulum

Projeyi yerel makinenizde çalıştırmak için adımları izleyin:

1. **Projeyi Klonlayın:**
   ```bash
   git clone [https://github.com/CDEXX/akilli-otopark.git](https://github.com/CDEXX/ akilli-otopark.git)


 2.    **Veritabanı Kurulumu:**

    Proje içindeki parkwise.db dosyası SQLite browser ile görüntülenebilir.

    Sistem otomatik olarak veritabanı bağlantısını kuracaktır.

 3.   **Masaüstü Uygulaması:**

    ParkWise.sln dosyasını Visual Studio ile açın.

    Start butonuna basarak projeyi derleyin ve çalıştırın.

 4.   **Web Arayüzü (Opsiyonel):**

    Web klasörüne gidin ve Firebase Hosting'e deploy edin:
    bash firebase deploy

  ## 📸 Ekran Görüntüleri

| Masaüstü Görünümü | 
|:-----------------:|
| ![Desktop App](https://github.com/CDEXX/akilli-otopark/blob/main/images/app.png?raw=true) | ![Desktop App](https://github.com/CDEXX/akilli-otopark/blob/main/images/app2.png?raw=true) |   

## Rozetler

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)
![Firebase](https://img.shields.io/badge/Firebase-039BE5?style=for-the-badge&logo=Firebase&logoColor=white)
![HTML5](https://img.shields.io/badge/HTML5-E34F26?style=for-the-badge&logo=html5&logoColor=white)
  