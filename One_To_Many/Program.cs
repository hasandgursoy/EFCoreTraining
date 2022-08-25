using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

#region Default Convetion
// Default convetion yönteminde bire çok ilişki kurarken foreign key kolonuna karşı gelen bir property
// tanımlamak mecburiyetinde değiliz. Eğer tanımlamassak EF Core bunu kendisi tamamlayacak yok eğer tanımlarsak,
// tanımladığımızı baz alacaktır.

//public class Calisan // Dependent Entity
//{
//    public int Id { get; set; }
//    public string Adi { get; set; }
//    public int DepartmanId { get; set; } // Zorunlu değil EF Core kendiside tanımlar.
//    public Departman Departman { get; set; }
//}
//public class Departman
//{
//    public int Id { get; set; }
//    public string DepartmanName { get; set; }
//    public ICollection<Calisan> Calisanlar { get; set; }
//}

#endregion

#region Data Annotations
// Default yönteminde foreign key kolonuna karşılık gelen property'i tanımladığımızda bu property ismi temel geleneksel entity tanımlama
// kurallarına uymuyorsa eğer Data Annotations'lar ile müdahelede bulunabiliriz.

//public class Calisan // Dependent Entity
//{
//    public int Id { get; set; }
//    [ForeignKey(nameof(Departman))]
//    public int DId { get; set; }
//    public string Adi { get; set; }
//    public Departman Departman { get; set; }
//}
//public class Departman
//{
//    public int Id { get; set; }
//    public string DepartmanName { get; set; }
//    public ICollection<Calisan> Calisanlar { get; set; }
//}

#endregion

#region Fluent API

public class Calisan // Dependent Entity
{
    public int Id { get; set; }
    public string Adi { get; set; }
    public Departman Departman { get; set; }
}
public class Departman
{
    public int Id { get; set; }
    public string DepartmanName { get; set; }
    public ICollection<Calisan> Calisanlar { get; set; }
}

#endregion

public class ETicaretContext : DbContext
{
    public DbSet<Calisan> Calisanlar { get; set; }
    public DbSet<Departman> Departmanlar { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }


    // Model'larn (entity) veri tabanında generate edilecek yapıları bu fonksyino içerisinde konfigure ediyoruz.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calisan>()
             .HasOne(c => c.Departman)
             .WithMany(d => d.Calisanlar);

        // Normal de alttaki gibi foreign key tanımlamamıza gerek yok ancak örnek olsun yapalım biz.
        //modelBuilder.Entity<Calisan>()
        //     .HasOne(c => c.Departman)
        //     .WithMany(d => d.Calisanlar)
        //     .HasForeignKey(c => c.DId);
    }


}
