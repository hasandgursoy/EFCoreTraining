using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

var context = new ETicaretContext();

#region Relationsships Terimleri

#region Principal Entityy (Asıl Entity)
// Kendi başına var olabilen tabloyu modelleyen entity'e denir.
// Departmanlar tablosunu modelleyen 'Departman' entity'sidir.

#endregion

#region Dependent Entity (Bağımlı)
// Kendi başına var olamayan, bir başka tabloya bağımlı (ilişkisel olarak bağımlı) olan tabloyu modelleyen entity'e denir.
// Çalışanlar tablosu modelleyen 'Calisan' entity'sidir.

#endregion

#region Foreign Key
// Principal Entity ile Dependent Entity arasındaki ilişkiyi sağlayan key'dir.
// Dependent Entity'de tanımlanır.
#endregion

#region Principal Key
// Principal Entity'deki id'nin kendisidir.
// Principal Entity'nin kimliği olan kolonu ifade eden propertydir.

#endregion

#region Navigation Property Nedir ?
// İlişkisel tablolar arasındaki fiziksel erişimi entity class'ları üzerinden sağlayan property'dir.
// Örnek : Departman Departman, ICollecton<Calisanlar> Calisanlar gibi 
// Navigation propertyler N -> N yahut 1 -> N şeklinde ilişki türlerini ifade etmektedirler.

#endregion

#endregion

#region İlişki Türleri

#region One To One
// Çalışan ile adresi
// Karı-Koca ilişkisi gibi :D bu örnek gayet yeterli

#endregion

#region One To Many
// Departman-Calisan ilişkisi 
// Anne ve çocukları

#endregion

#region Many To Many
// Çalışanlar ile projeler
// Kardeşler arasındaki ilişki

#endregion

#endregion

#region EF Core'da İlişki Yapılandırma Yöntemleri

#region Default Conventions
// Varsayılan Entity kurallarını kullanarak yapılan ilişki yapılandırma yöntemleridir.
// Navigation property'leri kullanarak ilişki şablonlarını çıkarmaktadır.
#endregion

#region Data Annotations Attributes
// Attribute yapılanması ile yaptığımız uygulama 
// [Key], [ForeignKey]

#endregion

#region Fluent API
// Entity modellerindeki ilişkileri yapılandırırken daha detaylı çalışmamızı sağlayan yöntemdir.

#region HasOne
// İlgili entity'nin ilişkisel entity'ye birebir ya da bire çok olacak şekilde ilişkisini yapılandırmaya başlayan metottur.
#endregion

#region HasMany
// İlgili entity'nin ilişkisel entity'e çoka çok ya da çoka bir olacak şekilde ilişki yapılandırmaya başlayan metottur.
#endregion

#region WithOne
//HasOne ya da HasMany'den sonra bire bir ya da çoka bir olacak şekilde ilişki yapılandırmasını tamamlayan metottur.
#endregion

#region WithMany
// HasOne ya da HasMany'den sonra bire bir ya da çoka bir olacak şekilde ilişki yapılandırmasını tamamlayan metottur.
#endregion

#endregion



#endregion


public class ETicaretContext : DbContext
{
    public DbSet<Urun> Urunler { get; set; }
    public DbSet<Parca> Parcalar { get; set; }
    public DbSet<UrunParca> UrunParca { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrunParca>().HasKey(up => new { up.UrunId, up.ParcaId });
    }
}
public class Urun
{
    public int Id { get; set; }
    public string UrunAdi { get; set; }
    public float Fiyat { get; set; }

    public ICollection<Parca> Parcalar { get; set; }
}
public class Parca
{
    public int Id { get; set; }
    public string ParcaAdi { get; set; }
}
public class UrunParca
{
    public int UrunId { get; set; }
    public int ParcaId { get; set; }

    public Urun Urun { get; set; }
    public Parca Parca { get; set; }
}

public class UrunDetay
{
    public int Id { get; set; }
    public float Fiyat { get; set; }
}