using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

ApplicationDbContext context = new();

#region Table Per Type (TPT) Nedir?
//Entitylerin aralarında kalıtımsal ilişkiye sahip olduğu durumlarda her bir türe/entitye/tip/referans karşılık bir tablo generate eden davranıştır.
//Her generate edilen bu tablolar hiyerarşik düzlemde kendi aralarında birebir ilişkiye sahiptir.

// Performans istiyorsak ve maliyetin düşük olmasını istiyorsak Table Per Hirearchy kullanmak gerekiyor.
// Table Per Type'a muhtemelen pek ihtiyaç olmayacak şahsi not.

// Aşağıda 4 farklı class var bunlar birbirlerinden inheritance alıyor. Person tablosundaki Name değerini düşünün efcore bu değeri Customer tablosunda da oluşturmuyor çünkü sen bunu zaten miras aldın diyor ve bu person tablosunda dursa yeterlidir diyor. Sadece Person class'ın da olmayan propertyleri colon olarak Customer tablosunda açıyor.

// Şu soru akla gelebilir Customer daki ad ve soy ad değerini gidip person tablosuna yazacaksa yada diğer değerleri nereden kalıtım aldıysa oraya yazacaksa aralarındaki ilişkiyi nasıl bilicem ad soy ad hangi customer'a ait vs. cevap tabi ki Id parametresinden. ID ler bütün tablolarda ortak tutuluyor.
#endregion
#region TPT Nasıl Uygulanır?
//TPT'yi uygulayabilmek için öncelikle entitylerin kendi aralarında olması gereken mantıkta inşa edilmesi gerekmektedir.
//Entityler DbSet olarak bildirilmelidir.
//Hiyerarşik olarak aralarında kalıtımsal ilişki olan tüm entityler OnModelCreating fonksiyonunda ToTable metodu ile konfigüre edilmelidir. Böylece EF Core kalıtımsal ilişki olan bu tablolar arasında TPT davranışının olduğunu anlayacaktır.
#endregion
#region TPT'de Veri Ekleme
//Technician technician = new() { Name = "Şuayip", Surname = "Yıldız", Department = "Yazılım", Branch = "Kodlama" };
//await context.Technicians.AddAsync(technician);

//Customer customer = new() { Name = "Hilmi", Surname = "Celayir", CompanyName = "Çaykur" };
//await context.Customers.AddAsync(customer);
//await context.SaveChangesAsync();
#endregion
#region TPT'de Veri Silme
// Herhangi bir veriyi siliyorsak onunla ilişkili olan bütün datalar silinecektir. Cascade davranışını sergileyecektir.

//Employee? silinecek = await context.Employees.FindAsync(3);
//context.Employees.Remove(silinecek);
//await context.SaveChangesAsync();

//Person? silinecekPerson = await context.Persons.FindAsync(1);
//context.Persons.Remove(silinecekPerson);
//await context.SaveChangesAsync();
#endregion
#region TPT'de Veri Güncelleme
//Technician technician = await context.Technicians.FindAsync(2);
//technician.Name = "Mehmet";
//await context.SaveChangesAsync();
#endregion
#region TPT'de Veri Sorgulama
//Employee employee = new() { Name = "Fatih", Surname = "Yavuz", Department = "ABC" };
//await context.Employees.AddAsync(employee);
//await context.SaveChangesAsync();

//var technicians = await context.Technicians.ToListAsync();
//var employees = await context.Employees.ToListAsync();

//Console.WriteLine();
#endregion



abstract class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}
class Employee : Person
{
    public string? Department { get; set; }
}
class Customer : Person
{
    public string? CompanyName { get; set; }
}
class Technician : Employee
{
    public string? Branch { get; set; }
}

class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Technician> Technicians { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().ToTable("Persons");
        modelBuilder.Entity<Employee>().ToTable("Employees");
        modelBuilder.Entity<Customer>().ToTable("Customers");
        modelBuilder.Entity<Technician>().ToTable("Technicians");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
         optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }
}