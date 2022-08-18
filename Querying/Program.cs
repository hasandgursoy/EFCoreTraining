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










public class ETicaretContext : DbContext
{


    public DbSet<Product> Products { get; set; }
    public DbSet<Part> Parts { get; set; }

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
    public ICollection<Part> Parts { get; set; }

}

public class Part
{
    public int Id { get; set; }
    public string PartName { get; set; }

}