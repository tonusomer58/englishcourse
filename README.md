SozlukApp Proje Raporu
1. Proje Özeti
SozlukApp, kullanıcıların İngilizce kelime dağarcığını geliştirmelerine, çeviri yapmalarına ve seviye tespit sınavları ile bilgilerini ölçmelerine olanak tanıyan kapsamlı bir web tabanlı eğitim ve sözlük platformudur. ASP.NET Core MVC mimarisi üzerine inşa edilen proje, modern web standartlarına uygun, hızlı ve kullanıcı dostu bir deneyim sunmayı amaçlamaktadır.

2. Kullanılan Teknolojiler
Bu projenin geliştirilmesinde aşağıdaki teknolojiler ve araçlar kullanılmıştır:

Framework: .NET 9 (ASP.NET Core MVC)
Dil: C#
Veritabanı: Microsoft SQL Server
ORM (Nesne İlişkisel Eşleme): Entity Framework Core
Frontend: HTML5, CSS3, JavaScript (Razor Views)
API Entegrasyonu: MyMemory API (Ücretsiz çeviri servisi için)
Kimlik Doğrulama: Cookie Based Authentication (Çerez tabanlı kimlik doğrulama)
3. Proje Mimarisi ve Tasarım
Proje, uygulamanın veri (Model), arayüz (View) ve iş mantığı (Controller) katmanlarını birbirinden ayıran MVC (Model-View-Controller) tasarım deseni kullanılarak geliştirilmiştir.

Katmanlar:
Models: Veritabanı tablolarını temsil eden sınıflar (User, Word, TestResult) burada bulunur.
Views: Kullanıcı arayüzünü oluşturan .cshtml dosyalarıdır. Razor sözdizimi kullanılarak dinamik içerik üretilir.
Controllers: Gelen istekleri karşılayan, iş mantığını çalıştıran ve uygun View'ı döndüren sınıflardır (HomeController, AccountController, WordController).
Services: Dış servislerle iletişimi sağlayan katmandır (Örn: FreeTranslationService).
4. Temel Özellikler
4.1. Kullanıcı İşlemleri (Account Management)
Kayıt ve Giriş: Kullanıcılar sisteme kayıt olabilir ve güvenli bir şekilde giriş yapabilirler.
Yetkilendirme: "Admin" ve "User" olmak üzere iki temel rol bulunmaktadır. Admin kullanıcılar site yönetimi için ek yetkilere sahiptir.
4.2. Sözlük ve Çeviri (Dictionary & Translation)
Kelime Arama: Kullanıcılar veritabanındaki kelimeleri arayabilir ve anlamlarını öğrenebilir.
Çeviri Servisi: Harici bir API kullanılarak (MyMemory API) anlık İngilizce-Türkçe / Türkçe-İngilizce çeviri yapılabilir.
Örnek Cümleler: Kelimelerin kullanımını pekiştirmek için örnek cümleler sunulur (Örnek cümlelerin geliştirilmesi devam etmektedir).
4.3. Eğitim Modülleri
Seviye Testleri: Kullanıcılar İngilizce seviyelerini belirlemek için testler çözebilir.
Kurslar: Farklı seviyelere yönelik kurs içerikleri sunulmaktadır. (Başlangıç, Orta, İleri).
4.4. Yönetici Paneli (Admin Panel)
Yöneticiler, kullanıcıları ve sözlük verilerini yönetebilir.
Yeni kelimeler eklenebilir veya mevcut içerikler düzenlenebilir.
5. Veritabanı Tasarımı
Proje veritabanı "Code-First" yaklaşımı ile tasarlanmıştır. Temel tablolar şunlardır:

Users: Kullanıcı bilgileri (Ad, Soyad, E-posta, Şifre, Rol).
Words: Sözlükteki kelimeler, tanımları ve örnek kullanımları.
TestResults: Kullanıcıların çözdüğü testlerin sonuçları ve tarihçesi.
6. Sonuç
SozlukApp, dil öğrenimini dijitalleştiren, kullanıcı etkileşimini ön planda tutan ve genişletilebilir bir yapıya sahip modern bir web uygulamasıdır. Hem bireysel öğreniciler hem de eğitim kurumları için temel bir kaynak oluşturma potansiyeline sahiptir.
