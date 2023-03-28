using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

System.Console.WriteLine("Hello");

#region Default Convention
// İki entity arasınadaki entity arasındaki ilişkiyi navigation propertyler üzerinden çoğul olarak kurmalıyız.
// (ICollection, List) evet evet list yapısıda oluyor :D 
// Default Convtion'da cross table'ı manuel oluşturmak zorunda değiliz.
// Oluşturulan cross table'ın içerisinde composite primary key'i de otomatik oluşturmuş olacaktır.

//public class Kitap
//{
//    public int Id { get; set; }
//    public string KitapAdi { get; set; }
//    public ICollection<Yazar> Yazarlar { get; set; }
//}

//public class Yazar
//{
//    public int Id { get; set; }
//    public string YazarAdi { get; set; }
//    public ICollection<Kitap> Kitaplar { get; set; }
//}

#endregion

#region Data Anations
// Cross table manuel oluşturulmak zorundadır.
// Entity'lerde oluşturulmuş cross table entitysi ile bire çok ilişki kurulmalı.
// CT'da composite primary key'i data annotationlar ile manuel olarak kuramıyoruz.
// Bunun içinde Fluent API'da çalışma yapmamız gerekiyor.
// Cross Table'a karşılık bir entity oluşturuyorsak DbSet oluşturmak zorunda değiliz. 
//public class Kitap
//{
//    public int Id { get; set; }
//    public string KitapAdi { get; set; }
//    public ICollection<KitapYazar> Yazarlar { get; set; }
//}

// Cross Table

// Normalde aşşağıdaki gibi foreign key tanımlaya gerek yok ancak isim farklı olursa mecburuz örnek KKitaplarDDID 
//public class KitapYazar
//{
//    [ForeignKey(nameof(Kitap))]
//    public int KitapId { get; set; }
//    [ForeignKey(nameof(Yazar))]
//    public int YazarId { get; set; }
//    public Kitap Kitap { get; set; }
//    public Yazar Yazar { get; set; }
//}

//public class Yazar
//{
//    public int Id { get; set; }
//    public string YazarAdi { get; set; }
//    public ICollection<KitapYazar> Kitaplar { get; set; }
//}

#endregion

#region Fluent API
// Cross Table manuel oluşturulmalı
// DbSet olarak eklenmesine lüzüm yok
// Composite PK Haskey metodu ile kurulmalı !


public class Kitap
{
    public int Id { get; set; }
    public string KitapAdi { get; set; }
    public ICollection<KitapYazar> Yazarlar { get; set; }
}

public class KitapYazar
{
    public int KitapId { get; set; }
    public int YazarId { get; set; }
    public Kitap Kitap { get; set; }
    public Yazar Yazar { get; set; }
} 

public class Yazar
{
    public int Id { get; set; }
    public string YazarAdi { get; set; }
    public ICollection<KitapYazar> Kitaplar { get; set; }
}

#endregion




public class EKitapDbContext : DbContext
{
    public DbSet<Kitap> Kitaplar { get; set; }
    public DbSet<Yazar> Yazarlar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");

    }
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    // Bu durumda anonim key yapısı oluşturuyoruz o şekilde data annotation yoluyla primary key oluşturabiliyoruz.
    //    modelBuilder.Entity<KitapYazar>()
    //        .HasKey(ky => new { ky.KitapId, ky.YazarId });
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KitapYazar>()
            .HasOne(ky => ky.Kitap)
            .WithMany(y => y.Yazarlar)
            .HasForeignKey(ky => ky.KitapId);

        modelBuilder.Entity<KitapYazar>()
            .HasOne(ky => ky.Yazar)
            .WithMany(y => y.Kitaplar)
            .HasForeignKey(ky => ky.YazarId);

    }

}