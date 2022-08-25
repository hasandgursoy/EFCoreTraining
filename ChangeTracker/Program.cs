using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Reflection.Emit;

ETicaretContext context = new();



#region Change Tracking Nedir ?
// Context nesnesi üzerinden gelen tüm nesneler/veriler otomatik olarak bir takip mekanizması tarafından izlenir.
// İşte bu takip mekanizmasına Change Tracker denir. Change Traker ile nesneler üzerindeki değişiklikler
// takip edilerek netice itibariyle bu işlemlerin fıtratına uygn sql sorguları generate edilir. İşte bu işleme 
// Change Tracking denir.

#endregion

#region ChangeTracker Propertysi
// Takip edilen nesnelere erişebilmemizi sağlayan ve gerektiği taktirde işlemler gerçekleştirmemizi sağlayan bir propertydir.
// Context sınıfının base class'ı olan DbContext sınıfının bir member'ıdır.

//var products = await context.Products.ToListAsync();

//products[6].Price = 123; // Update
//context.Products.Remove(products[7]); // Delete

//Bütün entry'lerin statelerini göstericek 
//var datas =  context.ChangeTracker.Entries();


#endregion

#region DetectChanges Methodu
// EF Core, context nesnesi tarafından izlenen tüm nesnelerdeki değişikleri yakalayarak Change Tracker
// sayesinde takip edebilmekte ve nesnelerde olan verisel değişiklikler yakalanarak bunların anlık görüntüleri (snapshot)'ini oluşturabilir.
// Yapılan değişikliklerin veritabanına gönderilmeden önce algılandığından emin olmak gerekir. SaveChanges fonksiyonu
// çağırıldığı anda nesneler EF Core tarafından otomatik kontrol edilirler.
// Ancak yapılan operasyonlarda güncel tracking verilerinden emin olabilmek için değişikliklerin algılanmasını opsiyonel olarak algılanmasını 
// isteyebiliriz. İşte bunun için DetectChanges fonksiyonu kullanılabilir ve her ne kadar EF Core değişikleri otomatik algılıyor olsa da irademizle
// kontrol'e zorlayabiliriz.

//var product = await context.Products.FirstOrDefaultAsync(p => p.Id == 3);
//product.Price = 123;

// SaveChanges'dan önce her ne kadar çalışıyor olsada yinede biz kontrol edelim diyebiliyoruz.
// Lüzumsuz bir işlem yaptık çünkü normalde DetectChanges'ın davranışı SaveChanges tarafından yönetilmekde. 
// Bazen çok zor durumlar olabilir dağıtık sistemlerde vs o zaman kullanabiliriz.
//context.ChangeTracker.DetectChanges(); 
//await context.SaveChangesAsync();
#endregion

#region AutoDetectChangesEnabled Property
// İlgili methodlar(SaveChanges, Entries) tarafından DetectChanges metodunun otomatik olarak tetiklenmesinin
// konfigurasyonunu yapmamızı sağlayan propertydir.
// SaveChanges fonksiyonu tetiklendiğinde DetectChanges methodu içerisinde default olarak çağırılmaktadır.
// Bu durumda DetectChanges fonksiyonun kullanımını irademizle yönetmek ve maliyet/performans optimizasyonu yapmak
// istediğimiz durumlarda AutoDetectChangesEnabled fonksiyonunu kullanabiliriz.

#endregion

#region Entires Metodu
// Context'te ki Entry metodunun koleksiyonel versiyonudur.
// Change Tracker mekanizması tarafından izlenen her entity nesnesinin bilgisini EntryEntriy türünden elde etmemizi sağlar.
// ve belirli işlemler yapabilmemizi sağlar.
// Entries metodu, DetectChanges metodunu tetikler. Bu durumda tıpkı SaveChanges'da olduğu gibi bir maliyettir.
// Buradaki maliyetten kaçınmak için AutoDetectChangesEnabled özelliğine false değeri verilebilir.

//var products = context.Products.ToList();
//products.FirstOrDefault(p => p.Id == 7).Price = 123;// update
//context.Products.Remove(products.FirstOrDefault(p => p.Id == 8)); // Delete

//context.ChangeTracker.Entries().ToList().ForEach(e =>
//{
//    if (e.State == EntityState.Unchanged)
//    {
//      //....
//    }else if (e.State == EntityState.Added)
//    {
//        //...
//    }
//});

#endregion

#region AcceptAllChanges Methodu
// SaveChagnes() veya SaveChanges(true) tetiklendiğinde EF Core herşeyin yolunda olduğunu varsayarak track ettiği verilerin 
// takibini keser yeni değişikliklerin takip edilmesini bekler. Böyle bir durumda beklenmeyen bir durum
// olası bir hata söz konusu olursa eğer EF Core takip ettiği nesneleri bırakacağı için bir düzeltme mevzu bahis olmayacaktır.

// Haliyle bu durumda devreye SaveChanges(false) yada AcceptAllChanges metotları devreye girecektir.
// SaveChanges(false), EF Core'a gerkeli veritabanı komutlarını yürütmesini söyler ancak gerektiğinde yeniden oynatabilmesi için
// değişiklikleri takip etmeye devam eder. Taa ki AcceptAllChanges metodu irademizle çağırana kadar !

// SaveChanges(true) ile işlemin başarılı olduğundan emin olursanız AcceptAllChanges metodu ile nesnelerden takibi kesebilirsiniz.

//var products = context.Products.ToList();
//products.FirstOrDefault(p => p.Id == 7).Price = 123;// update
//context.Products.Remove(products.FirstOrDefault(p => p.Id == 8)); // Delete

// Verilerin tracking mekanizmasını kesmeden saveChanges'ı tetikledik sonrada bu bağı AcceptAllChanges ile kestik.
//await context.SaveChangesAsync(false);
//context.ChangeTracker.AcceptAllChanges();

#endregion

#region HasCanges Methodu
// Takip edilen nesneler arasından değişiklik yapılanların olup olmadığının bilgisi verir.
// Arka planda DetectChanges metodunu tetikler.

//var result = context.ChangeTracker.HasChanges();
#endregion

#region Entity States
// Entity nesnelerinin durumlarını ifade eder.
// context.Entry(product).State 
#region Detached
// Takip edilmiyor
#endregion

#region Added
// Eklendi
#endregion

#region Unchanged
// Context'de var ama bir değişiklik yok
#endregion

#region Modified
// Veri Değişti
#endregion

#region Deleted
// Veri silindi
#endregion

#endregion

#region Context Nesnesi üzerinden Change Tracker
//var product = await context.Products.FirstOrDefaultAsync(p => p.Id== 55);
//product.Price = 123;
//product.Name = "Silgi"; // Modified 

#region Entry Methodu

#region OrginalValues Property'si
//var fiyat = context.Entry(product).OriginalValues.GetValue<float>(nameof(Product.Price));
//var productName = context.Entry(product).OriginalValues.GetValue<string>(nameof(Product.Name));

#endregion

#region CurrentValues Property'si
//var productName = context.Entry(product).CurrentValues.GetValue<string>(nameof(Product.Name));

#endregion

#region GetDatabaseValues Methodu
// Veri tabanındaki değerleri elde etmek istiyorsak kullanabiliyoruz.
//var dbVal = await context.Entry(product).GetDatabaseValuesAsync();

#endregion


#endregion


#endregion

#region Change Tracker'ın Interceptor Olarak Kullanılması 
// Context 'in içinde tanımlayacağım bunu SaveChanges methodun içine bak.

#endregion


public class ETicaretContext : DbContext
{


    public DbSet<Product> Products { get; set; }
    public DbSet<Part> Parts { get; set; }

    public DbSet<ProductPart> ProductsParts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }

    public override int SaveChanges()
    {
        // Burda bir Interceptor görevi görerek araya girdik ve bu şekilde müdahele ettik.
        IEnumerable<EntityEntry<Product>> entries = ChangeTracker.Entries<Product>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Name = "Added";
            }
        }

        return base.SaveChanges();
    }

}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public ICollection<Part> Parts { get; set; }

}

public class Part
{
    public int Id { get; set; }
    public string PartName { get; set; }

}

public class ProductPart
{
    // Composite Key = Birden fazla ID tanımlamamız gereken durumlarda kullanılır.
    public int ProductId { get; set; }
    public int PartId { get; set; }
    public Product Product { get; set; }
    public Part Part { get; set; }
}


class ProductDetails
{
    public int Id { get; set; }
    public int Price { get; set; }

}