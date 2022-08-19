using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

ETicaretContext context = new();

#region En Temel Basit Bir Sorgulama Nasıl Yapılır ? 

#region Method Syntax (Link Methods)
//var products = await context.Products.ToListAsync();
#endregion

#region Query Syntax (Link Queries) (C# üzerinden sorgulama yapyoruz.)

//var products2 = (from product in context.Products
//                select product).ToListAsync();

#endregion



#endregion

#region IQueryable ve IEnurable Nedir ? Basit Olarak!

#region IQueryable
// Sorguya karşılık gelir
// EF Core üzerinden yapılmış olan sorgunun execute edilmemiş halini ifade eder.
#endregion

#region IEnumerable
// Sorgunun çalıştırılıp/execute edilip verilerin in memorye yüklenmiş halini ifade eder.
#endregion



#endregion

#region Sorguyu Execute Etmek İçin Ne Yapmamız Gerekmektedir ? 

// Aşşağıdaki ifade mesela sadece IQueryable modunda ta ki biz bunu ToListAsync diyene kadar.
//var products = (from product in context.Products
//               select product).ToListAsync();

#region Deferred Execution (Ertlenmiş Çalışma)

//var products = from product in context.Products
//               select product;

#region Foreach İle Yapabiliyoruz
// Çalışma zamanında IEnumerable hale dönüyor örnek Console'a yazdırırken.
//foreach (var product in products)
//{
//    Console.WriteLine(product.Name);
//}

#endregion

#region Deferred Execution Yapısına Derin bakış
// IQueryable çalışmalarında ilgili kod yazıldığı noktada çalıştırılmaz/tetiklenmez.
// Nerede eder ? Çalıştırıldığı/execute edildiği noktada tetiklenir bu durumada erlenmiş çalışma denir.
// productId en başta 5 atanmasına rağmen buna göre sorgu yapıldığı düşünebiliriz ancak durum böyle değil 
// execute etmeden hemen önce son değerine bakar aşşağıda 230 var ona göre hareket eder.
// Sorgu yapılır yapılmaz hemen ToListAsync() dersek bu defferred execution olmaz ama sonra yaparsak bu yine deffered execution olur.

//var productId = 5;
//string productName = "2";


//var products = from product in context.Products
//               where product.Id < productId && product.Name.Contains(productName)
//               select product;
//productId = 230;



//foreach (var product in products)
//{
//    Console.WriteLine(product.Name);
//}




#endregion


#endregion


#endregion

#region Çoğul Veri Getiren Sorgulama Fonksiyonları

#region ToListAsync
// Üretilen sorguyu execute etttirmemizi sağlayan bir fonksiyondur.
//var products = context.Products.ToListAsync();
//var products2 = (from product in context.Products
//                select product).ToListAsync();
#endregion

#region Where
// Oluşturulan sorguya where şartı eklememizi sağlayan bir fonksiyondur.
//var products = await context.Products.Where(p => p.Name.Length < 10 && p.Id < 50).ToListAsync();
//var products2 = from product in context.Products
//                where product.Name.Length < 10 && product.Id < 50
//                select product;
//var newProducts2 = await products2.ToListAsync(); // Deffered Execution
#endregion

#region OrderBy
// Sorgu üzerinde sıralama yapmamızı sağlayan bir fonksiyondur. (Ascending)

//var products = await context.Products.Where(p => p.Id > 500 || p.Name.EndsWith("2"))
//    .OrderBy(p => p.Name).ToListAsync();

//var products2 = await (from product in context.Products
//                where product.Id > 500 || product.Name.EndsWith("2")
//                orderby product.Name ascending // default olarak ascendingdir.
//                select product).ToListAsync();
#endregion

#region ThenBy
// OrderBy üzerinde yapılan sıralama işlemini farklı kolonlarada uygulamamızı sağlayan fonksiyondur. (ascending)
// Products'ın içindeki verileri isme göre order by ile sıraladık ama onda da mükerrer olanlar var ise Id'ye baksın
// ondada mükerrer olanlar varsa price'a baksın diyebiliyoruz
//var products = await context.Products.Where(p => p.Id > 500 || p.Name.EndsWith("2"))
//              .OrderBy(p => p.Name).ThenBy(p => p.Id).ThenBy(p => p.Price).ToListAsync();
#endregion

#region OrderByDescending
// Descending yapısında sıralama yapar örnek büyükden küçüğe veya Z -> A 
//var products = await context.Products.OrderByDescending(p => p.Price).ToListAsync();

//var products2 = await (from product in context.Products
//                       orderby product.Price descending
//                       select product).ToListAsync();




#endregion

#region ThenByDescending
//var products = await context.Products.OrderByDescending(p => p.Id).ThenByDescending(p => p.Name).ToListAsync();


#endregion

#endregion

// -----------------------------------------

#region Tekil Veri Getiren Sorgulama Fonksiyonları
// Yapılan sorguda sadece tek bir verinin gelmesini amaçlıyorsak single ya da single or default 
// fonksiyonları kullanılabilir.


#region SingleAsync
// Eğer ki, sorgu neticesinde bir den fazla veri geliyorsa ya da hiç gelmiyorsa her iki durumda da exception fırlatır.
//var product = await context.Products.SingleAsync();

#endregion

#region SingleOrDefaultAsync
// Eğer ki, sorgu neticesinde birden fazla geliyorsa exception, hiç veri gelmiyorsa null döner.
//var product = await context.Products.SingleOrDefaultAsync();

#endregion

#region FirstAsync
// Yapılan sorguda tek bir verinin gelmesi amaçlıyorsak First veya FirstOrDefault fonksiyonları kullanılabilir.
// Sorgu neticesinde elde edilen verilerden ilkini getirir. Eğerki hiç veri gelmiyorsa hata fırlatır.
//var product = await context.Products.FirstAsync();
#endregion

#region FirstOrDefaultAsync
// Sorgu neticesinde elde edilen verilerden ilkini getirir. Eğer ki hiç veri gelmiyorsa null değerini döndürür.
//var product = await context.Products.FirstOrDefaultAsync();
#endregion

#region FindAsync
// Find fonksiyonu, primary key kolonuna özel hızlı bir şekilde sorgulama yapmamızı sağlayan fonksiyondur.
// Sorgulama sürecinde önce context içerisini kontrol eder ve kaydı bulamadığı takdirde sorguyu veritabanına gönderir.


//Product? product = await context.Products.FindAsync(55);

#region Composite Primary Key Durumu
// Composite Key = Birden fazla ID tanımlamamız gereken durumlarda kullanılır.

//var pp = await context.ProductsParts.FindAsync(2,5);


#endregion


#endregion

#region LastAsync and LastOrDefaultAsync
// FirstAsync ile FirstOrDefaultAsync arasında ki tek fark sondaki veriyi alıyor.

#endregion


#endregion






public class ETicaretContext : DbContext
{


    public DbSet<Product> Products { get; set; }
    public DbSet<Part> Parts { get; set; }

    public DbSet<ProductPart> ProductsParts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite PrimaryKey
        // Composite Key = Birden fazla ID tanımlamamız gereken durumlarda kullanılır.

        modelBuilder.Entity<ProductPart>().HasKey(pp => new { pp.ProductId, pp.PartId });
    }


}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public ICollection<Part> Parts { get; set; }

}

public class Part
{
    public int Id { get; set; }
    public string PartName { get; set; }

}

public class ProductPart
{
    // Composite Key = Birden fazla ID tanımlamamız gereken durumlarda kullanılır.
    public int ProductId { get; set; }
    public int PartId { get; set; }
    public Product Product { get; set; }
    public Part Part { get; set; }
}