using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

ApplicationDbContext context = new();

#region Owned Entity Types Nedir?
//EF Core entity sınıflarını parçalayarak, propertylerini kümesel olarak farklı sınıflarda barındırmamıza ve tüm bu sınıfları ilgili entity'de birlkeştirip bütünsel olarak çalışmamıza izin vermektedir.
//Böylece bir entity, sahip olunan(owned) birden fazla alt sınıfın birleşmesiyle meydana gelebilmektedir.
#endregion
#region Owned Entity Types'ı Neden Kullanırız?
//https://www.gencayyildiz.com/blog/wp-content/uploads/2020/12/Entity-Framework-Core-Owned-Entities-and-Table-Splitting.png

//Domain Driven Design(DDD) yaklaşımında Value Object'lere karşılık olarak Owned Entity Types'lar kullanılır!
// Value Object = Primary Key'i olmayan ve Değeri Değişmeyen Yapılanmalardır.
#endregion
#region Owned Entity Types Nasıl Uygulanır?
//Normal bir entity'de farklı sınıfların referans edilmesi primary key vs. gibi hatalara sebebiyet verecektir. Çünkü direkt bir sınfıın referans olarak alınması ef core tarafından ilişkisel bir tasarım olarak algılanır. Bizlerin entity ieçrisindeki propertyleri kümesel olarak barındıran sınıfları o entity'nin bir parçası olduğunu bildirmemiz özellikle gerekmektedir.

#region OwnsOne Metodu 

#endregion
#region Owned Attribute'u

#endregion
#region IEntityTypeConfiguration<T> Arayüzü

#endregion

#region OwnsMany Metodu
//OwnsMany metodu, entity'nin farklı özelliklerine başka bir sınıftan ICollection türünde Navigation Property aracılığıyla ilişkisel olarak erişebilmemizi sağlayan bir işleve sahiptir.
//Normalde Has ilişki olarak kurulabilecek bu ilişkinin temel farkı, Has ilişkisi DbSet property'si gerektirirken, OwnsMany metodu ise DbSet'e ihtiyaç duymaksızın gerçekleştirmemizi sağlamaktadır.

//var d = await context.Employees.ToListAsync();
//Console.WriteLine();
#endregion
#endregion
#region İç İçe Owned Entity Types

#endregion
#region Sınırlılıklar
// Herhangi bir owned entity type için DbSet<> sınırlamasına ihtiyaç yoktur.
// OnModel creating fonksiyonunda çalışan Entity<T> methodu ile Owned Entity Type türünden bir sınıf üzerinde herhangi bir konfigurasyon gerçekleştirilmez.
// Owned Entity Type'ların kalıtımsal hiyerarşi desteği yoktur.
#endregion

// Normal Şartlarda EmployeeName ve Adress sınıfıları ile bir ilişki barındırmadığı için migration alırken patlayacak bunların primary key'i bile yok diyecek. Bizde bunun bir owned type entity olduğunu söylemek için 3 farklı yol'a sahip olacağız. OwnedOne , OwnedAttribute'u ve IEntityTypeConfiguration<T> arayüzü. Bu şekilde için de bulunduğumuz problemi çözebiliriz.
class Employee
{
    public int Id { get; set; }
    //public string Name { get; set; }
    //public string MiddleName { get; set; }
    //public string LastName { get; set; }
    //public string StreetAddress { get; set; }
    //public string Location { get; set; }
    public bool IsActive { get; set; }

    public EmployeeName EmployeeName { get; set; }
    public Address Adress { get; set; }

    public ICollection<Order> Orders { get; set; }
}
class Order
{
    public string OrderDate { get; set; }
    public int Price { get; set; }
}
//[Owned]
class EmployeeName
{
    public string Name { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public EmployeBilmemneName EmployeBilmemneName { get; set; }
}

class EmployeBilmemneName
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
}
//[Owned]
class Address
{
    public string StreetAddress { get; set; }
    public string Location { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region OwnsOne
        //modelBuilder.Entity<Employee>().OwnsOne(e => e.EmployeeName, builder =>
        //{
        //    builder.Property(e => e.Name).HasColumnName("Name");
        //});
        //modelBuilder.Entity<Employee>().OwnsOne(e => e.Adress);
        #endregion
        #region OwnsMany
        modelBuilder.Entity<Employee>().OwnsMany(e => e.Orders, builder =>
        {
            builder.WithOwner().HasForeignKey("OwnedEmployeeId");
            builder.Property<int>("Id");
            builder.HasKey("Id");
        });
        #endregion
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    }
    protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }
}

class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.OwnsOne(e => e.EmployeeName, builder =>
        {
            builder.Property(e => e.Name).HasColumnName("Name");
        });
        builder.OwnsOne(e => e.Adress);
    }
}