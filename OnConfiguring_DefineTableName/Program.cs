using Microsoft.EntityFrameworkCore;



Console.WriteLine();

public class EticaretContext : DbContext
{
    #region Tablo Adı Belirleme

    #endregion
    public DbSet<Urun> Urunler { get; set; }



    #region OnConfiguring İle Konfigürasyon Ayarlarını Gerçekleştirmek
    // EF Core tool'unu yapılandırmak için kullandığımız bir metottur.
    // Context nesnesinde override edilerek kullanılmaktadır.
    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        // Provider 
        // ConnectionString
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");


        // Lazy Loading

    }

}

#region Basit Düzetde Entity Tanımlama Kuralları
// EF Core, her tablonun default olarak bir primary key kolonu olması gerektiğini kabul eder.
// Haliyle, bu kolonu temsil eden bir property tanımladığımız takdirde hata verecekdir.


#endregion

public class Urun
{
    // Aşşağıdaki yapılardan birini kullandığımız anda PRIMARY KEY olarak tanımlar.

    //public int Id { get; set; }
    //public int ID { get; set; }
    //public int UrunId { get; set; }
    public int UrunID { get; set; }

}






