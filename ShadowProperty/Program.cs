// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

ApplicationDbContext context = new ApplicationDbContext();

#region Shadow Properties - Gölge Özellikler
// Entity sınıflarında fiziksel oalrak tanımlanmayan/ modellenmeyen ancak EFCore tarafından ilgili entity için var olan var olduğu kabul edilen propertylerdir.
// Tabloda gösterilmesini istemdiğimiz lüzumlu görmediğimiz entity instance'ı üzerinde işlem yapmayacağımız kolonlar için shadow propertyler kullanılabilir.
// Shadow propertylerin değerleri ve stateleri Change Tracker tarafından kontrol edilir.

#endregion


#region Foreign Key - Shadow Properties
// var blogs = await context.Blogs
//     .Include(b => b.Posts)
//     .ToListAsync();

// İlişkisel senaryolarda foreign key properysini tanımlamadığımız halde EFCore tarafından dependent entity'e eklenmektedir. İşte bu shadow property'dir.

#endregion

#region Shadow Propery Oluşturma
// Bir entity üzerinde shadow property oluşturmak istiyorsanız eğer Fluent API kullanmanız gerekmetedir . (OnModelCreating içerisinde)

// modelBuilder.Entity<Blog>().Property<DateTime>("CreatedDate");

#endregion

#region Shadow Property'e erişim sağlama
#region Change Tracker İle Erişim
// Shadow property'e erişim sağlayabilmek için Change Tracker dan istifade edinebilir.
var blog = await context.Blogs.FirstAsync();

var createdDate = context.Entry(blog).Property("CreatedDate");
System.Console.WriteLine(createdDate.CurrentValue);
System.Console.WriteLine(createdDate.OriginalValue);

createdDate.CurrentValue = DateTime.Now;
await context.SaveChangesAsync();
#endregion

#region EF.Property İle Erişim
// Özellikle LINQ sorgularında Shadow Propertylerinde erişim için EF.Property static yapılanmasını kullanabiliriz.

var blogs = await context.Blogs.OrderBy(b => EF.Property<DateTime>(b, "CreatedDate")).ToListAsync();

var blogs2  = await context.Blogs.Where(b => EF.Property<DateTime>(b , "CreatedDate").Year > 2020).ToListAsync();


#endregion
#endregion

class Blog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Post> Posts { get; set; }

}

class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime LastUpdated { get; set; }
    // Burada arka planda BlogId yi EFCore gidip kendi oluşturur bunun adı shadow property'dir.
    public Blog Blog { get; set; }
}




class ApplicationDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>().Property<DateTime>("CreatedDate");
    }
}