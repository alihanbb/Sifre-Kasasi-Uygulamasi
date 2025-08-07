Şifre Kasası Projesi
Bu proje, kullanıcıların web sitelerine ait şifrelerini güvenli bir şekilde saklayıp yönetebileceği bir şifre kasası uygulamasıdır. Yönetici paneli üzerinden kullanıcı rolleri ve yetkileri dinamik olarak güncellenebilir.
Kullanılan Teknolojiler
•	.NET 9: Modern ve hızlı web uygulamaları geliştirmek için.
•	ASP.NET Core Razor Pages: Sayfa tabanlı, hızlı ve kolay yönetilebilir web arayüzü.
•	Entity Framework Core: Veritabanı işlemleri için ORM altyapısı.
•	Microsoft Identity: Kullanıcı kimlik doğrulama ve yetkilendirme.
•	SQL Server: Veritabanı yönetimi.
•	jQuery & AJAX: Dinamik ve hızlı kullanıcı arayüzü işlemleri.
•	Bootstrap 4/5: Responsive ve modern arayüz tasarımı.
•	SweetAlert: Kullanıcı dostu bildirimler ve hata mesajları.
Proje Yapısı
•	AppWeb: Web arayüzü ve API controller'ları.
•	AppRepository: Entity ve veri erişim katmanı.
•	AppServices: Servis ve iş mantığı katmanı.
Temel Özellikler
•	Kullanıcılar şifrelerini ekleyebilir, güncelleyebilir ve silebilir.
•	Yöneticiler, kullanıcı rolleri ve yetkilerini dinamik olarak güncelleyebilir.
•	AJAX ile sayfa yenilemeden işlemler yapılır.
•	Güçlü kimlik doğrulama ve yetkilendirme altyapısı.
•	Modern ve kullanıcı dostu arayüz.
Kurulum
1.	Veritabanı bağlantı ayarlarını appsettings.json dosyasında yapılandırın.
2.	Gerekli NuGet paketlerini yükleyin.
3.	Projeyi Visual Studio veya dotnet CLI ile çalıştırın.
Geliştirici Notları
•	Kodlar C# 13.0 ve .NET 9 standartlarına uygundur.
•	Razor Pages mimarisi kullanılmıştır, MVC veya Blazor değildir.
•	Tüm AJAX işlemlerinde CSRF koruması mevcuttur.
•	Proje modüler ve genişletilebilir şekilde tasarlanmıştır.
