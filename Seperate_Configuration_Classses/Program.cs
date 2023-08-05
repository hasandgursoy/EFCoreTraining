using System.IO.Compression;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

Console.WriteLine("Hello, World!");


#region OnModelCreating

// Entityler üzerinde konfigurasyonel çalışmalar yapmamızı sağlayan bir fonksiyondur.
// Genel anlamda veritabanı ilgili konfigurasyonel operasyonların dışında entityler üzerinde konfigurasyonel çalışmalar
// yapmamızı sağlayan bir fonksiyondur.

#endregion

#region IEntityTypeConfiguration<T> Arayüzü

// Entity bazlı yapılacak olan konfigurasyonları o entity'e özel harici bir dosya üzerinde yapmamızı sağlayan  bir arayüzdür.

// Harici bir dosyada konfigurasyonların yürütülmesi merkezi bir yapılandırma noktası oluşturmamızı sağlamaktadır.
// Harici bir dosyada konfigurasyonların yürütlmesi entity sayısının fazla oluduğu senaryolarda yönetilebilirliği
// arttıracak ve yapılandırma ile ilgii geliştiricinin yükünü azaltacaktır.

#endregion

#region ApplyConfiguration Metodu

// Harici konfigurasyonel sınıflarımızı EFCore'a bildirmek için kullandığımzı bir metotdur.

#endregion

#region ApplyConfigurationsFromAssembly Metodu

// Uygulama bazında oluşturulan harici konfigurasyonel sınıfların" her birini OnModelCreating netodunda ApplyConfiguraiton ile tek tek bildirmek yerine bu sınıfların bulunduğu Assembly'i bildirerek IEntityTypeConfiguration arayüzünden türeyen tüm sınıfları ilgili entity'e karşılık konfigurasyonel değer olarak baz almasını tek kalemde gerçekleştirmemizi sağlayan metotdur.

#endregion

class Order
{
    public int OrderId { get; set; }
    public string Description { get; set; }
    public DateTime OrderDate { get; set; }
}

class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.OrderId);
        builder.Property(p => p.Description).HasMaxLength(13);
        builder.Property(p => p.OrderDate)
            .HasDefaultValue("GETDATE()");
    }
}


class ApplicationDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfiguration(new OrderConfiguration());      
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
          
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }
}