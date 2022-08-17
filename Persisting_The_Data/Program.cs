using Microsoft.EntityFrameworkCore;



#region Veri Nasıl Eklenir ?

ETicaretContext context = new();

//Product product = new Product()
//{
//    Name = "Mouse",
//    Price = 1000
//};

#region Eklenen Verinin ID'si nasıl elde ediliyor ? 

// INSET Products Values('H ürünü',2000)
// @ SELECT @@IDENTİTY 
// Bu sql kod'u arka planda dönüyor.

#endregion



#region context.AddAsync Fonksiyonu
// Tip güvenli değildir.
//await context.AddAsync(product);

#endregion

#region context.DbSet.AddAsync Fonksiyonu
// Tip güvenlidir.
//await context.Products.AddAsync(product);

#endregion

#region SaveChanges Nedir ?
// Insert, Update ve Delete sorgularını oluşturup bir transaction eşliğinde veritabanına gönderip execute eden fonksiyondur.
// Eğer ki oluşturulan sorgulardan biri başarısız olursa tüm işlemleri geri alır.(rollback)
//await context.SaveChangesAsync();
#endregion

#region Birden Fazla Veri Eklerken Nelere Dikkat Edilmeli ?

#region SaveChanges'ı verimli kullanma 

// SaveChanges fonksiyonu her tetiklendiğinde bir transaction oluşturacağından dolayı EF Core ile yapılan her bir 
// işleme özel kullanmakdan kaçınmalıyız. Çünkü her işleme özel transaction veritabanına maliyet oluşturur.
// Tüm işlemleri tek bir transaction da yönetebilmek için aşşağıdaki gibi tek seferde kullanmak gerekiyor.

Product p1 = new()
{
    Name = "product 1",
    Price = 2000
};
Product p2 = new()
{
    Name = "product 2",
    Price = 2000
};
Product p3 = new()
{
    Name = "product 3",
    Price = 2000
};

await context.AddRangeAsync(p1,p2,p3);

// Tek seferde saveChanges edip verimli kullanıcaz ayrı ayrı değil.
await context.SaveChangesAsync();

#endregion



#endregion

#region EFCore açısından bir verinin eklenmesi gerektiğini nasıl anlıyor ?
//Product kalem = new()
//{
//    Name="Kalem",
//    Price =2000
//};

//Console.WriteLine(context.Entry(kalem).State); // Detached
//await context.AddAsync(kalem);

//Console.WriteLine(context.Entry(kalem).State); // Added
//await context.SaveChangesAsync();

//Console.WriteLine(context.Entry(kalem).State); // Unchanged

////await context.AddAsync(kalem);
////await context.SaveChangesAsync();


#endregion

#endregion



public class ETicaretContext : DbContext
{


    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");

    }

    

}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

}



