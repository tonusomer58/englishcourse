# 🎓 EnglishCourse - Web Tabanlı Dil Eğitim Platformu

![.NET](https://img.shields.io/badge/.NET%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white) ![Ubuntu](https://img.shields.io/badge/Ubuntu-E95420?style=for-the-badge&logo=ubuntu&logoColor=white) ![Nginx](https://img.shields.io/badge/Nginx-009639?style=for-the-badge&logo=nginx&logoColor=white) ![Google Cloud](https://img.shields.io/badge/Google_Cloud-4285F4?style=for-the-badge&logo=google-cloud&logoColor=white)

---

## 🚀 1. Proje Özeti

**EnglishCourse**, kullanıcıların İngilizce kelime dağarcığını geliştirmelerine, çeviri yapmalarına ve seviye tespit sınavları ile bilgilerini ölçmelerine olanak tanıyan kapsamlı bir web platformudur.

Bu proje, sadece bir web sitesi değil, **Google Cloud Platform (GCP)** üzerinde, **Linux (Ubuntu)** sunucu altyapısında ve **Nginx** teknolojisi kullanılarak canlıya alınmış, ölçeklenebilir modern bir mimaridir.

🌐 **Canlı Demo:** [http://webprogramlama.com.tr](http://webprogramlama.com.tr)

---

## 🛠️ 2. Kullanılan Teknolojiler ve Altyapı

### 💻 Backend & Frontend

- **Framework:** .NET 9 (ASP.NET Core MVC)
- **Dil:** C#
- **Veritabanı:** Microsoft SQL Server (MSSQL)
- **ORM:** Entity Framework Core (Code-First Yaklaşımı)
- **Arayüz:** HTML5, CSS3, JavaScript, Bootstrap 5
- **API Entegrasyonu:** MyMemory API (Anlık Çeviri Servisi)

### ☁️ Sunucu & DevOps (Yayınlama Ortamı)

- **Bulut Sağlayıcı:** Google Cloud Platform (Compute Engine)
- **İşletim Sistemi:** Ubuntu Linux Server 22.04 LTS
- **Web Sunucusu:** Nginx (Reverse Proxy Yapılandırması)
- **Servis Yönetimi:** Systemd (Linux Daemon Service)
- **Alan Adı (DNS):** METUnic

---

## 🏗️ 3. Proje Mimarisi (MVC & Server)

Proje, **MVC (Model-View-Controller)** tasarım deseni ile geliştirilmiştir. Ancak projenin en güçlü yanı sunucu mimarisidir:

1.  **İstek (Request):** Kullanıcı `webprogramlama.com.tr` adresine girer.
2.  **Firewall:** Google Cloud Güvenlik Duvarı isteği karşılar (Port 80).
3.  **Reverse Proxy (Nginx):** Gelen isteği Linux sunucu içinde çalışan `.NET Kestrel` sunucusuna (`localhost:5000`) yönlendirir.
4.  **Cevap (Response):** İşlenen veri aynı güvenli yoldan kullanıcıya döner.

---

## ✨ 4. Temel Özellikler

### 👤 Kullanıcı İşlemleri

- ✅ **Kayıt & Giriş:** Güvenli kimlik doğrulama sistemi.
- ✅ **Yetkilendirme:** Admin ve User rolleri ile sayfa erişim kısıtlamaları.

### 📚 Sözlük ve Çeviri

- ✅ **Kişisel Sözlük:** Kullanıcılar kendi kelimelerini ekleyebilir.
- ✅ **Anlık Çeviri:** API desteği ile hızlı İngilizce-Türkçe çeviri.

### 🎓 Eğitim ve Sınav

- ✅ **Test Modülleri:** Başlangıç, Orta ve İleri seviye testler.
- ✅ **Puanlama:** Sınav sonucuna göre anlık seviye belirleme.

### 🛡️ Yönetici (Admin) Paneli

- ✅ **İçerik Yönetimi:** Yeni kurs, konu ve kelime ekleme/silme.
- ✅ **Kullanıcı Denetimi:** Kullanıcıların eklediği içerikleri onaylama veya reddetme.

---

## 🗄️ 5. Veritabanı Tasarımı (Code-First)

Veritabanı bağlantısı `appsettings.json` üzerinden güvenli bir şekilde yönetilmektedir.

| Tablo Adı       | Açıklama                                               |
| :-------------- | :----------------------------------------------------- |
| **Users**       | Kullanıcı adı, şifre ve rol bilgileri.                 |
| **Words**       | İngilizce kelimeler, Türkçe karşılıkları ve durumları. |
| **Courses**     | Eğitim kursları ve içerik detayları.                   |
| **TestResults** | Kullanıcıların sınav skorları ve tarihleri.            |

---

## 🔄 6. CRUD Mantığı ve Endpoint Yapısı

Projede veri bütünlüğü için **MVC Endpoint** yapısı kullanılmıştır:

- **Güncelleme (Update):** `[HttpGet]` ile mevcut veri forma doldurulur, `[HttpPost]` ile değişiklikler veritabanına `_context.Update()` komutuyla işlenir.
- **Silme (Delete):** Kullanıcıya önce bir onay ekranı gösterilir, onay alındığında `ActionName("Delete")` metodu çalışır ve veri silinir.

---

## 🐧 7. Linux Kurulum ve Yayınlama Adımları

Proje yerel bilgisayardan (Localhost) bulut sunucuya (Production) şu adımlarla taşınmıştır:

1.  **Publish:** Visual Studio'da `linux-x64` için derleme alındı.
2.  **SSH Bağlantısı:** Terminal üzerinden Google Cloud sunucusuna bağlanıldı.
3.  **Deploy:** Dosyalar `/var/www/sozluk` dizinine yüklendi.
4.  **Servis (Daemon):** Uygulamanın sunucu yeniden başlasa bile çalışması için `sozluk.service` yazıldı.
5.  **Nginx Config:** Domain yönlendirmesi için `/etc/nginx/sites-available/default` dosyası yapılandırıldı.

---

👨‍💻 **Geliştirici:** Ömer Tonus
