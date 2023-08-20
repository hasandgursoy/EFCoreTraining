
// using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using System.Transactions;

ApplicationDbContext context = new();
#region Database Property'si
//Database property'si veritabanını temsil eden ve EF Core'un bazı işlevlerinin detaylarına erişmemizi sağlayan bir propertydir.
// Önemli bir yapılanmadır. Transaction yönetimini her ne kadar EFCore kendisi yönetse Database property'si üzerinden ciddi oranda müdahele edebiliyoruz.
// IDbContextTransaction transaction =  context.Database.BeginTransaction();
#endregion
#region BeginTransaction
//EF Core, transaction yönetimini otomatik bir şekilde kendisi gerçekleştirmektedir. Eğer ki transaction yönetimini manuel olarak anlık ele almak istiyorsak BeginTransaction fonksiyonunu kullanabiliriz.

//IDbContextTransaction transaction = context.Database.BeginTransaction();

#endregion
#region CommitTransaction
// Save Changes'da aynı işi yapıyor muhtemelen hiç kullanmayacağız.
//EF Core üzerinde yapılan çalışmaların commit edilebilmesi için kullanılan bir fonksiyondur.
//context.Database.CommitTransaction();

#endregion
#region RollbackTransaction
//EF Core üzerinde yapılan çalışmaların rollback edilebilmesi için kullanılan bir fonksiyondur.
//EF Core temel davranış olarak yazılan kodda bir problem olduğunda rollback methodunu çalıştırarak yapılan işlemleri geri alır bu şekilde DataBase'i tutarsız hale getirecek davranışların önüne geçilmiş olunur.
//context.Database.RollbackTransaction();
#endregion
#region CanConnect
//Verilen connection string'e karşılık bağlantı kurulabilir bir veritabanı var mı yok mu bunun bilgisini bool türde veren bir fonksiyondur.
// Migration işlemi yapılmadan bu method çalıştırılırsa false değerini döner çünkü herhangi bir connection' olmamış olacak.
//bool connect = context.Database.CanConnect();
//Console.WriteLine(connect);
#endregion
#region EnsureCreated
//EF Core'da tasarlanan veritabanını migration kullanmaksızın, runtime'da yani kod üzerinde veritabanı sunucusuna inşa edebilmek için kullanılan bir fonksiyondur. Çok ilginç önemli bir yapılanmadır migration olmadan direkt olarak inşa edebiliyor. HARIKA
//context.Database.EnsureCreated();
#endregion
#region EnsureDeleted
//İnşa edilmiş veritabanını runtime'da silmemizi sağlayan bir fonksiyondur. EnsureCreated'ın tam tersi :D
//context.Database.EnsureDeleted();
#endregion
#region GenerateCreateScript
//Context nesnesinde yapılmış olan veritabanı tasarımı her ne ise ona uygun bir SQL Script'ini string olarak veren metottur.
// Çok ama çok kullanılışlı bir method şöyle düşünün EF Core 'dan vazgeçmeniz gerekiyor yada başka database kullanıcaksanız. Bu context.Database.GenerateCreateScript() methodunu bir çalıştırıyoruz hemen tabloların SQL dili ile inşasını içeren yapılanmayı bize veriyor. Hatta SeedDatas ları bile veriyor uygun bir şekilde insert edebileceğin method ile birlikte.
//var script = context.Database.GenerateCreateScript();
//Console.WriteLine(script);
#endregion
#region ExecuteSql
//Veritabanına yapılacak Insert, Update ve Delete sorgularını yazdığımız bir metottur. Bu metot işlevsel olarak alacağı parametreleri SQL Injection saldırılarına karşı korumaktadır.  ExecuteSql() methodu içine string ifade yazıyoruz gibi gözüksede FormattableString değerini alıyor bu durumda String Interpolation işlemi ile birlikte yazıyoruz ve içine süslü parantezler içerisinde yazdığımız değerleri bir sql parameter'a dönüştürüp Database de öyle çalıştırıyor. Kısaca default olarak SQL Injection önlemi alıyoruz.
//string name = Console.ReadLine();
//var result = context.Database.ExecuteSql($"INSERT Persons VALUES('{name}')");
#endregion
#region ExecuteSqlRaw
//Veritabanına yapılacak Insert, Update ve Delete sorgularını yazdığımız bir metottur. Bu metotta ise sorguyu SQL Injection saldırılarına karşı koruma görevi geliştirinin sorumluluğundadır. Bu methodu mümkün olduğunca kullanmamak lazım SQL Injection koruması default olarak gelen ExecuteSql kullanılmalıdır.
//string name = Console.ReadLine();
//var result = context.Database.ExecuteSqlRaw($"INSERT Persons VALUES('{name}')");
#endregion
#region SqlQuery
//SqlQuery fonksiyonu her ne kadar erişilebilir olsada artık desteklenememktedir. Bunun yerine DbSet propertysi üzerinden erişilebilen FromSql fonksiyonu gelmiştir/kullanılmaktadır.
#endregion
#region SqlQueryRaw
//SqlQueryRaw fonksiyonu her ne kadar erişilebilir olsada artık desteklenememktedir. Bunun yerine DbSet propertysi üzerinden erişilebilen FromSqlRaw fonksiyonu gelmiştir/kullanılmaktadır.
#endregion
#region GetMigrations
//Uygulamada üretilmiş olan tüm migration'ları runtime'da programatik olarak elde etmemizi sağlayan metottur.
//var migs = context.Database.GetMigrations();
//Console.WriteLine();
#endregion
#region GetAppliedMigrations
//Uygulamada migrate edilmiş olan tüm migrationları elde etmemizi sağlayan bir fonksiyondur. DB update methodu çağırılmış migrationsları gösteriyor.
//var migs = context.Database.GetAppliedMigrations();
//Console.WriteLine();
#endregion
#region GetPendingMigrations
//Uygulamada migrate edilmemiş olan tüm migrationları elde etmemizi sağlayan bir fonksiyondur.
//var migs = context.Database.GetPendingMigrations();
//Console.WriteLine();
#endregion
#region Migrate
//Migration'ları programatik olarak runtime'da migrate etmek için kullanılan bir fonksiyondur.
//context.Database.Migrate();
//EnsureCreated fonksiyonu migration'ları kapsamamaktadır. O yüzden migraton'lar içerisinde yapılan çalışmalar ilgili fonksiyonda geçerli olmayacaktır.
#endregion
#region OpenConnection
//Veritabanı bağlantısını manuel açar.
//context.Database.OpenConnection();
#endregion
#region CloseConnection
//Veritabanı bağlantısını manuel kapatır.
//context.Database.CloseConnection();
#endregion
#region GetConnectionString
//İlgili context nesnesinin o anda kullandığı connectionstring değeri ne ise onu elde etmenizi sağlar.
//Console.WriteLine(context.Database.GetConnectionString());
#endregion
#region GetDbConnection
//EF Core'un kullanmış olduğu Ado.NET altyapısının kullandığı DbConnection nesnesini elde etmemizi sağlayan bir fonksiyondur. Yaniiii bizleri Ado.NET kanadına götürür. Ado NET de yapılabilecek bütün operasyonları gerçekleştirebiliyoruz.

//SqlConnection connection = (SqlConnection)context.Database.GetDbConnection();
//Console.WriteLine();
#endregion
#region SetDbConnection
//Özelleştirilmiş connection nesnelerini EF Core mimarisine dahil etmemizi sağlayan bir fonksiyondur.
//context.Database.SetDbConnection();
#endregion
#region ProviderName Property'si
//EF Core'un kullanmış olduğu provider neyse onun bilgisini getiren bir proeprty'dir.
//Console.WriteLine(context.Database.ProviderName);
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }
}