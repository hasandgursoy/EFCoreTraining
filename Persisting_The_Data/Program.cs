using Microsoft.EntityFrameworkCore;


ETicaretContext context = new();

//Product product = new Product()
//{
//    Name = "Mouse",
//    Price = 1000
//};

#region Eklenen Verinin ID'si nasıl elde ediliyor ? 

// INSERT Products Values('H ürünü',2000)
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

//Product p1 = new()
//{
//    Name = "product 1",
//    Price = 2000
//};
//Product p2 = new()
//{
//    Name = "product 2",
//    Price = 2000
//};
//Product p3 = new()
//{
//    Name = "product 3",
//    Price = 2000
//};

//await context.AddRangeAsync(p1, p2, p3);

//// Tek seferde saveChanges edip verimli kullanıcaz ayrı ayrı değil.
//await context.SaveChangesAsync();

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

#region Veri Nasıl Güncellenir ?

//Product? tablet = await context.Products.FirstOrDefaultAsync(p => p.Id == 3);

//if (tablet is not null)
//    tablet.Name = "Tablet";
//    await context.SaveChangesAsync();



#endregion

#region ChangeTracker Nedir ? Kısaca!
// ChangeTracker, context üzerinden gelen verilerin takibinden sorumlu bir mekanizmadır. Bu mekanizma sayesinde 
// context üzerinden gelen verilerle ilgili işlemler neticesinde insert, update ve delete sorgularının oluşturulacağı anlaşılır.


#endregion

#region Takip edilmeyen Nesneler Nasıl Güncellenir ?


//Product elma = new()
//{
//    Id = 3,
//    Name = "Elma",
//    Price = 1231
//};

#region Update Fonksiyonu
// ChangeTacker mekanizması tarafından takip edilmeyen nesnelerin güncellenebilmesi için Update fonksiyonu kullanılır !
// Update fonksiyonu kullanabilmek için kesinlikle ilgili nesnede Id değeri verilmelidir. Bu değer güncellenecek
// (update sorgusu oluşturulacak) verinin hangisi olduğunu ifade edecektir.

//context.Products.Update(elma);
//await context.SaveChangesAsync();
#endregion


#endregion

#region EntityState Nedir ? 
// Bir entity instance'ının durumunu ifade eden bir referansdır.
// EntityState state = context.Entry(elma).State;


#endregion

#region EF Core açısından bir veri nasıl güncellenmeli ? 

//Product? pro = await context.Products.FirstOrDefaultAsync(p => p.Id == 3);
//pro.Name = "hilmi";
//context.Products.Update(pro);
//await context.SaveChangesAsync();


#endregion

#region Birden Fazla Veri Güncellenirken Nelere Dikkat Edilmelidir ? 

// Bütün verileri al demek için toListAsync() 
//var products = await context.Products.ToListAsync();

//foreach (var product in products)
//{
//    product.Name += "*";
//}
//await context.SaveChangesAsync();

#endregion

#region Veri Nasıl Silinir ?
// Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == 3);
//context.Products.Remove(product);
//await context.SaveChangesAsync();



#endregion

#region Silme İşleminde ChangeTracker'ın Rolü
// ChangeTracker, context üzerinden gelen verilerin takibinden sorumlu bir mekanizmadır. Bu takip mekanizması sayesinde
// context üzerinden gelen verilerle ilgili işlemler neticesinde update yahut delete sorgularının oluşturlacağı anlaşılır.


#endregion

#region Takip Edilmeyen Nesneler Nasıl Silinir ?

//Product product = new()
//{
//    Id = 2
//};

//context.Products.Remove(product);
//await context.SaveChangesAsync();

#endregion

#region EntityState ile Silme İşlemi
//Product product = new Product { Id = 1 };
//context.Entry(product).State = EntityState.Deleted;
//await context.SaveChangesAsync();

#endregion

#region Veri Silerken SaveChanges'ı Verimli Kullanma
//Product p1 = new Product { Id = 5 };
//Product p2 = new Product { Id = 1 };
//List<Product> products = new()
//{
//    p1,p2
//};

//foreach (var product in products)
//{
//    context.Products.Remove(product);
//}

//await context.SaveChangesAsync();


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



