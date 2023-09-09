// See https://aka.ms/new-console-template for more information
using System.Reflection;
using Microsoft.EntityFrameworkCore;

ApplicationDbContext context = new();

#region EF Core Select Sorgularını Güçlendirme Teknikleri

#region  IQueryable - IEnumerable Farkı
// IQueryable, bu arayüz üzerinde yapılan işlemler direkt generate edilecek olan sorguya yansıtılacaktır.
// IEnumerable, bu arayüz üzerinde yapılan işlemler temel sorgu neticesinde gelen ve in-memory'e yüklenen instancelar üzerinde gerçekleştirilir.Yani Sorguya yansıtılmaz.

// IQueryable ile yapılan sorgulama çalışmalarında sql sorguyu hedef verileri elde edecek şekilde generate edilecekken,IEnumerable ile yapılan sorgulama çalışmalarında sql daha geniş verileri getirebilecek şekilde execute edilerek hedef veriler in-memory'de ayıklanır.

// IQueryable, hedef verileri getirirken, IEnumerable hedef verilerden daha fazlasını getirip in-memory'de ayıkalr.

// Her ikiside Deffered Execution davranışı sağlar. ToListAsync() , Foreach gibi yapılarla bunları çalışır hale getiriyoruz.

// Biz sorgularımızı IQueryable da inşa etmeye önem göstermeliyiz. 
// IQueryable dediğimiz gibi hedef'e yönelik hareket eder en başından beri ancak IEnumerable ilk önce gider bütün dataları getirir sonra bunlar içerisinde eleme yapar.
// Örnek vermek gerekirse aile ferdlerimiz bizden tuz getirmemizi istediler , biz ilk önce mutfaga gideriz ve orada tuzu bulup getiririz. Bu durum IQueryable'ı anlatır.
// IEnumerable'da ise ilk önce mutfaktaki bütün malzemeleri içeriye taşırız (in-memory) sonra içinden tuzu alıp aile ferdlerine veririz.

#region  IQueryable 
// ToListAsync yapısı IQueryable 'ı execute edebiliyor ancak bu durum IEnumerable için geçerli değil async olmayan bir method ile execute edilmesi gerekiyor.
// var persons = await context.Persons
//         .Where(p => p.Name.Contains("a"))
//         .Take(3)
//         .ToListAsync();

#endregion

#region IEnumerable
// var persons2 = context.Persons
//         .Where(p => p.Name.Contains("a"))
//         .AsEnumerable()
//         .Take(3)
//         .ToList();

// System.Console.WriteLine("Stop Hear Debugging Area");

// System.Console.WriteLine("");
#endregion

#endregion

#region Yalnızca ihtiyac olan kolonlari listele - Select
// Birden fazla kolon çekiceksek bu şekilde yapıyoruz.
// var persons = await context.Persons.Select(p => new {
//     p.Name,
//     p.PersonId
// }).ToListAsync();

#endregion

#region Result'i Limitleyin - Take
// await context.Persons.Take(20).ToListAsync();

#endregion

#region  Join Sorgularında Eager Loading Sürecinde Verileri Filtreleyin
//  Order'ı çağırdığımız noktada filtreleyip getiriyoruz.
// var persons = await context.Persons.
//     Include(x => x.Orders.Where(o => o.OrderId % 2 == 0).OrderByDescending(o => o.OrderId).Take(4))
//     .ToListAsync();

#endregion

#region Eger sartlara bagli join yapilacaksa explicit loading kullanin
// var person = await context.Persons.FirstOrDefaultAsync(p => p.PersonId == 1);

// if (person?.Name == "Ayşe")
// {
//  Orderları getir. En başta person'u oluşturduğumuz da ordersları getirebilirdik ancak bu kötü bir seçim olacak çünkü bizim bütün insanların orderslarına ihtiyacımız yok.
//  Asagida ki yapıda tekli veri ise Collection yerine Reference ile birlikte alıyorduk.
//  Asagidaki kod yapisi ile Orderslari artik InMemory'e almis oluyoruz.
//  Kritik bir davranisdir.
//  await context.Entry(person).Collection(p  => p.Orders).LoadAsync();
// }

#endregion

#region Lazy Loading Kullanırken Dikkatli olmak gerekiyor bana kalsa hiç kullanmamak lazım :D 
// Mermory' veriyi her seferinde tekrar tekrar alıp duracak explicit loading varken kim kopek la bu lazyloading
#endregion

#region Ihtiyac noktalarinda Ham SQL kullanin - FromSQL

#endregion

#region Asenkron Fonksiyonları Tercih Edin

#endregion

#endregion

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }

    public virtual Person Person { get; set; }
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
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=DenemeDB;User Id=postgres;Password=123456;");
            // .UseLazyLoadingProxies();
    }
}