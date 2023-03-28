using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

System.Console.WriteLine("Hello");

#region Default Convention
// Her iki entity'de Navigation Property ile birbirlerini tekil olarak referans ederek fiziksel bir ilişkinin olacağı ifade edilir.
// One to One ilişki türünde dependent entity'nin hangisi olduğunu default olarak belirleyebilmek pek kolay değildir.
// Bu durumda fiziksel olarak bir foreign key'e karşılık property/kolon tanımlayarak çözüm getirebiliyoruz.
// Böylece foreign key'e karşılık property tanımlayarak lüzumsuz bir kolon oluşturmuş oluyoruz.


//public class Calisan
//{
//    public int Id { get; set; }
//    public string Adi { get; set; }
//    public CalisanAdresi CalisanAdresi { get; set; }
//}
//public class CalisanAdresi
//{
//    public int Id { get; set; }
//    public int CalisanId { get; set; }    // Bu kısım bu konunun özeti çalışan adresi çalışana bağlı olduğu için foreign key tanımlıyoruz.
//    public string Adres { get; set; }
//    public Calisan Calisan { get; set; }
//}

#endregion

#region Data Anations
// Navigation Property'ler tanımlanmalıdır.
// Foreign key kolonunun ismi default convention dışında bir kolon olacaksa eğer Foreignkey attribute ile bunu bildirebiliriz.
// Foreign key kolonu oluşturmak zorunda değiliz.
// 1-1 ilişkide ekstradan foreign key kolonuna ihtiyaç olmayacağından dolayı dependent entitydeki id kolonunun hem foreign key hemde primary key olarak
// kullanmayı tercih ediyoruz ve bu duruma özen gösterilmelidir.

//public class Calisan
//{
//    public int Id { get; set; }
//    public string Adi { get; set; }
//    public CalisanAdresi CalisanAdresi { get; set; }
//}
//public class CalisanAdresi
//{
//    // Bu tanımlama yöntemiyle default convention yöntemindeki gibi ekstra column masrafından kurtulmuş olduk. 

//    [Key,ForeignKey(nameof(Calisan))] 
//    public int Id { get; set; }
//    public string Adres { get; set; }
//    public Calisan Calisan { get; set; }
//}

#endregion

#region Fluent API
// Navigation Propertyler tanımlanmalı !
// Fluent API yönteminde entityler arasındaki ilişki context sınıfı içerisinde OnModelCreating fonksiyonun içersinde
// override edilerek metotlar aracılığıyla tasarlanması gerekmektedir. Yani tüm sorumluluk bu fonksiyon içerisindedir.


public class Calisan
{
    public int Id { get; set; }
    public string Adi { get; set; }
    public CalisanAdresi CalisanAdresi { get; set; }
}
public class CalisanAdresi
{
    public int Id { get; set; }
    public string Adres { get; set; }
    public Calisan Calisan { get; set; }
}

#endregion

public class ETicaretContext : DbContext
{
    public DbSet<Calisan> Calisanlar { get; set; }
    public DbSet<CalisanAdresi> CalisanAdresleri { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }


    // Model'larn (entity) veri tabanında generate edilecek yapıları bu fonksyino içerisinde konfigure ediyoruz.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CalisanAdresi>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Calisan>()
            .HasOne(c => c.CalisanAdresi)
            .WithOne(c => c.Calisan)
            .HasForeignKey<CalisanAdresi>(c => c.Id);
    }


}

