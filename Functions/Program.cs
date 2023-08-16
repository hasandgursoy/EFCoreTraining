﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

ApplicationDbContext context = new();
#region Scalar Functions Nedir?
// Veritabanı seviyesinde çaşacak ve geriye herhangi bir türde değer döndüren fonksiyonlardır.
#endregion
#region Scalar Function Oluşturma
// Artık buraya kadar 50 kere tekrarladığımız için oturmuştur veri tabanındaki objelerle, procedurelerle vs uğraşıcaksak boş migrations basıyoruz.
//1. adım : boş bir migration oluşturulmalı.
//2. adım : bu migration içerisinde Up metodunda Sql metodu eşliğinde fonksiyonun create kodları yazılacak, Down metodu içerisinde de Drop kodları yazılacaktır.
//3. adım : migrate edilmeli.
//  protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.Sql($@"
//                     CREATE FUNCTION getPersonTotalOrderPrice(@personId INT)
// 	                    RETURNS INT
//                     AS
//                     BEGIN
// 	                    DECLARE @totalPrice INT
// 	                    SELECT @totalPrice = SUM(o.Price) FROM Persons p
// 	                    JOIN Orders o
// 		                    ON p.PersonId = o.PersonId
// 	                    WHERE p.PersonId = @personId
// 	                    RETURN @totalPrice
//                     END
//                     ");
//         }

//         /// <inheritdoc />
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.Sql($@"DROP FUNCTION getPersonTotalOrderPrice");
//         }
#endregion
#region Scalar Function'ı EF Core'a Entegre Etme

#region HasDbFunction
//Veritabanı seviyesindeki herhangi bir fonksiyonu EF Core/yazılım kısmında bir metoda bind etmemizi sağlayan fonksiyondur.
#endregion
 
//var persons = await (from person in context.Persons
//                     where context.GetPersonTotalOrderPrice(person.PersonId) > 500
//                     select person).ToListAsync();

//Console.WriteLine();

#endregion

#region Inline Functions Nedir?
//Geriye bir değer değil, tablo döndüren fonksiyonlardır.
#endregion
#region Inline Function Oluşturma
//1. adım : boş bir migration oluşturunuz.
//2. adım : bu migration içerisindeki Up fonksiyonunda Create işlemini,  down fonksiyonunda ise drop işlemlerini gerçekleştiriniz.
//3. adım : migrate ediniz.
// protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.Sql($@"
//                 CREATE FUNCTION bestSellingStaff(@totalOrderPrice INT = 10000)
// 	                RETURNS TABLE
//                 AS
//                 RETURN 
//                 SELECT TOP 1 p.Name, COUNT(*) OrderCount, SUM(o.Price) TotalOrderPrice FROM Persons p
//                 JOIN Orders o
// 	                ON p.PersonId = o.PersonId
//                 GROUP By p.Name
//                 HAVING SUM(o.Price) < @totalOrderPrice
//                 ORDER By OrderCount DESC
//                     ");
//         }

//         /// <inheritdoc />
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.Sql($@"DROP FUNCTION bestSellingStaff");
//         }
#endregion
#region Inline Function'ı EF Core'a Entegre Etme
var persons = await context.BestSellingStaff(3000).ToListAsync();
foreach (var person in persons)
{
    Console.WriteLine($"Name : {person.Name} | Order Count : {person.OrderCount} | Total Order Price : {person.TotalOrderPrice}");
}
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
public class BestSellingStaff
{
    public string Name { get; set; }
    public int OrderCount { get; set; }
    public int TotalOrderPrice { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Scalar
        // Array icerisinde int bir array vermemizin sebebi en yukarida bos migrations icerisinden aldigmiz degerin donus turu integer array !!
        // Hemen assagida verdigimiz HasName degeri ile beraber DB de ki method ile bizim burada yazdigimiz method eslesecek ve ne zaman biz GetPersonTotalOrderPrice bu methodu cagirirsak DB deki method tetiklenecek.
        modelBuilder.HasDbFunction(typeof(ApplicationDbContext).GetMethod(nameof(ApplicationDbContext.GetPersonTotalOrderPrice), new[] { typeof(int) }))
            .HasName("getPersonTotalOrderPrice");
        #endregion
        #region Inline
        // Scalar da olduğu gibi burada da aynı konfigurasyonları yapıoyruz.
        modelBuilder.HasDbFunction(typeof(ApplicationDbContext).GetMethod(nameof(ApplicationDbContext.BestSellingStaff), new[] { typeof(int) }))
            .HasName("bestSellingStaff");

        // Burada da DB'den Inline method aracılığı ile bize dönecek olan table'ı karşılamak için oluşturduğumuz Nesnenin bir primary key'i olmayacağını bildirmek için çalışma HasNoKey diyoruz ve bitiyor.
        modelBuilder.Entity<BestSellingStaff>()
            .HasNoKey();
        #endregion

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Person)
            .HasForeignKey(o => o.PersonId);
    }


    #region Scalar Functions
    // Asagidaki function'ı sadece OnModelCreating icerisinde modelBuilder.HasDbFunction'da da nameof ile beraber cagirabilmek icin yaptik baska hic bir anlami yok. (STAMP islemi)
    public int GetPersonTotalOrderPrice(int personId)
        => throw new Exception();
    #endregion
    #region Inline Functions
    // Sclar func ta olduğu gibi burada da STAMP için bir yapı oluşturuyoruz. Sonra DB deki func dan dönen değer için bir model oluşturuyoruz.
    public IQueryable<BestSellingStaff> BestSellingStaff(int totalOrderPrice = 10000)
         => FromExpression(() => BestSellingStaff(totalOrderPrice));
    #endregion


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
         optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }
}