# EnglishCourse Projesi

## 1. PROJE ÖZETİ
**EnglishCourse**, kullanıcıların İngilizce kelime dağarcığını geliştirmelerine, çeviri yapmalarına ve seviye tespit sınavları ile bilgilerini ölçmelerine olanak tanıyan kapsamlı bir web tabanlı eğitim platformudur.  
Proje, modern web standartlarına uygun, hızlı ve kullanıcı dostu bir deneyim sunmayı amaçlamaktadır.

---

## 2. KULLANILAN TEKNOLOJİLER
Bu projenin geliştirilmesinde aşağıdaki teknolojiler ve araçlar kullanılmıştır:

- **Framework:** .NET 9 (ASP.NET Core MVC)
- **Programlama Dili:** C#
- **Veritabanı:** Microsoft SQL Server (MSSQL)
- **ORM:** Entity Framework Core
- **Frontend:** HTML5, CSS3, JavaScript
- **API Entegrasyonu:** MyMemory API (Çeviri servisi)
- **Hosting:** SmarterASP.NET

---

## 3. PROJE MİMARİSİ VE TASARIM (MVC)
Proje, uygulamanın veri (**Model**), arayüz (**View**) ve iş mantığı (**Controller**) katmanlarını birbirinden ayıran **MVC (Model-View-Controller)** tasarım deseni kullanılarak geliştirilmiştir.

### Katmanlar
- **Models:**  
  Veritabanı tablolarını temsil eden sınıflar bulunur.  
  Örnek: `User`, `Word`, `Courses`

- **Views:**  
  Kullanıcı arayüzünü oluşturan `.cshtml` dosyalarıdır.  
  Razor sözdizimi kullanılarak dinamik içerik üretilir.

- **Controllers:**  
  Gelen istekleri karşılayan, iş mantığını çalıştıran ve uygun View'ı döndüren sınıflardır.  
  Örnek: `HomeController`, `AdminController`

### İlişki Örneği
Kullanıcı bir kurs sayfasına girdiğinde, `CoursesController` veritabanından (**Model**) veriyi çeker ve bu veriyi `Index.cshtml` (**View**) sayfasına göndererek ekranda gösterir.

---

## 4. TEMEL ÖZELLİKLER
- **Kullanıcı İşlemleri:**  
  Kayıt olma, giriş yapma ve yetkilendirme (Admin / User rolleri)

- **Sözlük ve Çeviri:**  
  Veritabanından kelime arama ve MyMemory API ile anlık çeviri

- **Eğitim Modülleri:**  
  Farklı seviyelere yönelik kurs içerikleri  
  (Başlangıç, Orta, İleri)

- **Yönetici Paneli:**  
  Admin yetkisine sahip kullanıcılar:
  - Yeni kurs ekleyebilir
  - Kelime ekleyebilir
  - Güncelleme ve silme işlemleri yapabilir

---

## 5. VERİTABANI TASARIMI
Proje veritabanı **Code-First** yaklaşımı ile tasarlanmıştır.  
Veritabanı bağlantısı `appsettings.json` dosyası üzerinden yapılandırılmıştır.

### Temel Tablolar
- **Courses:** Kurs başlığı, içeriği ve resim yolu
- **Topics:** Kurslara bağlı alt konular
- **Words:** Sözlükteki kelimeler ve anlamları

---

## 6. CRUD İŞLEMLERİ VE ENDPOINT MANTIĞI
Projede veriler üzerinde tam kontrol sağlamak amacıyla **Güncelleme (Update)** ve **Silme (Delete)** işlemleri MVC mimarisine uygun endpoint yapısı ile gerçekleştirilmiştir.

### 6.1 Güncelleme (Update) Mantığı
- **GET İsteği:**  
  `Edit(int id)` metodu çalışır.  
  Güncellenecek veri ID’ye göre bulunur ve form doldurularak kullanıcıya gösterilir.

- **POST İsteği:**  
  Kullanıcı formu gönderdiğinde `[HttpPost] Edit` metodu çalışır.  
  `_context.Update()` komutu ile değişiklikler veritabanına kaydedilir.

### 6.2 Silme (Delete) Mantığı
- **GET İsteği:**  
  `Delete(int id)` metodu ile kullanıcıya silme işlemi için onay sayfası gösterilir.

- **POST İsteği:**  
  Onay verildiğinde `[HttpPost, ActionName("Delete")]` metodu tetiklenir ve  
