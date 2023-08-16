﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

ApplicationDbContext context = new();
#region Stored Procedure Nedir?
//SP, view'ler gibi kompleks sorgularımızı daha basit bir şekilde tekrar kullanılabilir  bir hale getirmemiz isağlayan veritabanı nesnesidir. 
//View'ler tablo misali bir davranış sergilerken, SP'lar ise fonksiyonel bir davranış sergilerler (Parametre alır).
//Ve türlü türlü artılarıda vardır.
#endregion 

#region EF Core İle Stored Procedure Kullanımı
#region Stored Procedure Oluşturma
//1. adım : boş bir migration oluşturunuz.
//2. adım : migration'ın içerisindeki Up fonksiyonuna SP'ın Create komutlarını yazınız, Down fonk. ise Drop komutlarını yazınız. (Down'ın olayı versiyon düşerken yapılaması gerekenleri anlatır.)
//3. adım : migrate ediniz.
#endregion
#region Stored Procedure'ü Kullanma
//SP'ı kullanabilmek için bir entity'e ihtiyacımız vardır. Bu entity'nin DbSet propertysi ollarak context nesnesine de eklenmesi gerekmektedir.
//Bu DbSet properytysi üzerinden FromSql fonksiyonunu kullanarak 'Exec ....' komutu eşliğinde SP yapılanmasını çalıştırıp sorgu neticesini elde edebilirsiniz.
#region FromSql
// FromSql'e DB set ile birlikte çalışabiliyor.
//var datas = await context.PersonOrders.FromSql($"EXEC sp_PersonOrders")
//    .ToListAsync();
#endregion
#endregion
#region Geriye Değer Döndüren Stored Procedure'ü Kullanma
//SqlParameter countParameter = new()
//{
//    ParameterName = "count",
//    SqlDbType = System.Data.SqlDbType.Int,
//    Direction = System.Data.ParameterDirection.Output
//};
// Normalde @count değerini burada vermemize gerek yok ancak efcore istiyor geri değer dönecekse bu şekilde hareket etmek gerekiyor.
//await context.Database.ExecuteSqlRawAsync($"EXEC @count = sp_bestSellingStaff", countParameter);
//Console.WriteLine(countParameter.Value); (bu şekilde de Value'e değerine return edilen değer atanıyor.)
#endregion
#region Parametreli Stored Procedure Kullanımı
#region Input Parametreli Stored Procedure'ü Kullanma

#endregion
#region Output Parametreli Stored Procedure'ü Kullanma

#endregion

//SqlParameter nameParameter = new()
//{
//    ParameterName = "name",
//    SqlDbType = System.Data.SqlDbType.NVarChar,
//    Direction = System.Data.ParameterDirection.Output,
//    Size = 1000
//};
//await context.Database.ExecuteSqlRawAsync($"EXECUTE sp_PersonOrders2 7, @name OUTPUT", nameParameter);
//Console.WriteLine(nameParameter.Value);

#endregion
#endregion
Console.WriteLine();
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

    public Person Person { get; set; }
}
// NotMapped attribute'u bu bir tablo olmayacak anlamına geliyor ve bu şekilde db setler çalışmayacak.
[NotMapped]
public class PersonOrder
{
    public string Name { get; set; }
    public int Count { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<PersonOrder> PersonOrders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<PersonOrder>()
            .HasNoKey();

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