
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

ApplicationDbContext context = new();
#region Query Log Nedir?
//LINQ soprguları neticesinde generate edilen sorguları izleyebilmek ve olası teknik hataları ayıklayabilmek amacıyla query log mekanizmasından istifade etmekteyiz.
// Arka planda hangi queryler generate ediliyor hepsini loglayabiliriz.
#endregion
#region Nasıl Konfigüre Edilir?
//Microsoft.Extensions.Logging.Console

await context.Persons.ToListAsync();

await context.Persons
   .Include(p => p.Orders)
   .Where(p => p.Name.Contains("a"))
   .Select(p => new { p.Name, p.PersonId })
   .ToListAsync();
#endregion
#region Filtreleme Nasıl Yapılır?

#endregion

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }

    public ICollection<Order> Orders { get; set; }
}
public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }

    public Person Person { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }
    // AddFilter'ı silince salt bir query yazdırma işlemi yapıyoruz console'a.
    readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder
    // Eğer category'si command name'e eşitse ve level'i info ise getir
    .AddFilter((category, level) =>
    {
        return category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information;
    })
    .AddConsole());
    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
        optionsBuilder.UseLoggerFactory(loggerFactory);
    }
}