
using Microsoft.EntityFrameworkCore;

ApplicationDbContext context = new();


#region One To One İlişkisel Senaryolarda Veri Silme
// Include özünde bir join işlemidir.
// Person'u adress bilgisi ile birlikte çağırmak istiyorsak Include'u kullanmak zorundayız.

// Person? person = await context.Persons
// .Include(x => x.Address)
// .FirstOrDefaultAsync(p => p.Id == 1);

// // Person'a karşılık gelen address'i sildik
// if (person != null)
// {
//     context.Addresses.Remove(person.Address);
//     await context.SaveChangesAsync();
// }

#endregion

#region One To Many İlişkisel Senaryolarda Veri Silme 
// Blog'un 1. ve 3. postu dursun ama 2. postu silmek istiyoruz diyelim

// Blog? blog = await context.Blogs.Include(b => b.Posts).FirstOrDefaultAsync(b => b.Id == 1);
// Post? post = blog.Posts.FirstOrDefault(x => x.Id == 2);

// if (post != null)
// {
//     context.Posts.Remove(post);
//     await context.SaveChangesAsync();
// }

#endregion

#region Many To Many İlişkisel Senaryolarda Veri Silme
// Çok a çok senaryolarda örnek kitapların yazarını sil dediğimizde kitapları da komple götüreceği için sadece ara tabloyu siliyoruz yani gidip yazarı silmiyoruz arasındaki ilişkiyi siliyoruz. Mantık bu şekilde olmalı efcore da bu şekilde hareket eder.

// Book? book = await context.Books
//     .Include(b => b.Authors)
//     .FirstOrDefaultAsync(b => b.Id == 1);

// Author? author = book.Authors.FirstOrDefault(a => a.Id == 2);

// if (author != null)
// {
//     // Context üzerinden gidip authors'ı silmeye kalkmadık gidip aralarındaki ilişikiyi kopardık.
//     book.Authors.Remove(author);
//     await context.SaveChangesAsync();
// }


#endregion

#region Cascade Delete
// Bağımlı tablolar arasında bir veri silmeye kalkıtığımız zaman silinen verinin kullanıldığı diğer tabloda bu veri anlamsız bir şekilde beklemeyeceği için bu işleme izin vermez bunun için cascade yapılanmasını kullanıyoruz.

// Bu davranış modelleri Fluent API ile konfigüre ediliyor. ÖNELİ !!!
// İlişkileri tanımlarken OnDelete() methodu ile yapıyoruz.

// Çok'a çok ilişkide cascade dan başka bir yöntem kullanılmıyor. EF Core izin vermez aralarındaki bağlantıyı uçurur bırakır.

#region Cascade
// Esas tablodan silinen veriyle karşı/bağımlı tabloda bulunan ilişkili verilerin silinmesini sağlar.
// Blog blog = await context.Blogs.FindAsync(1);
// context.Blogs.Remove(blog);
// await context.SaveChangesAsync();

#endregion

#region SetNull
// Esas tablodan silinen veriyle karşı/bağımlı tabloda bulunan ilişkili verilerle null değerin atanmasını sağlar.

// One to One senaryolarda eğer ki Foreign Key ve Primary Key kolonları aynı ise o zaman SetNull davranışını KULLANAMAYIZ !!!
#endregion

#region Restrict
// Esas tablodan herhangi bir veri silinmeye çalışıldığından o veriye karşılık dependent table'da ilişkisel veriler varsa eğer bu sefer silme işlemini engellemesini sağlar.
#endregion

// Çok a çok ilişkiler veri silmeye çalışırkende ara tablo daki verileri uçurur cascade dan başka bir şeye de izin vermez cascade = direk sil gitsin :D
// Author? author = context.Authors.FirstOrDefault(i => i.Id == 1);
// if (author != null)
// {
//     context.Authors.Remove(author);
//     await context.SaveChangesAsync();
// }

#endregion

#region Saving Data
// Person person = new()
// {
//     Name = "Gençay",
//     Address = new()
//     {
//         PersonAddress = "Yenimahalle/ANKARA"
//     }
// };

// Person person2 = new()
// {
//     Name = "Hilmi"
// };

// await context.AddAsync(person);
// await context.AddAsync(person2);

// Blog blog = new()
// {
//     Name = "Gencayyildiz.com Blog",
//     Posts = new List<Post>
//    {
//        new(){ Title = "1. Post" },
//        new(){ Title = "2. Post" },
//        new(){ Title = "3. Post" },
//    }
// };

// await context.Blogs.AddAsync(blog);

// Book book1 = new() { BookName = "1. Kitap" };
// Book book2 = new() { BookName = "2. Kitap" };
// Book book3 = new() { BookName = "3. Kitap" };

// Author author1 = new() { AuthorName = "1. Yazar" };
// Author author2 = new() { AuthorName = "2. Yazar" };
// Author author3 = new() { AuthorName = "3. Yazar" };

// book1.Authors.Add(author1);
// book1.Authors.Add(author2);

// book2.Authors.Add(author1);
// book2.Authors.Add(author2);
// book2.Authors.Add(author3);

// book3.Authors.Add(author3);

// await context.AddAsync(book1);
// await context.AddAsync(book2);
// await context.AddAsync(book3);
// await context.SaveChangesAsync();
#endregion

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Address Address { get; set; }
}
class Address
{
    public int Id { get; set; }
    public string PersonAddress { get; set; }

    public Person Person { get; set; }
}
class Blog
{
    public Blog()
    {
        Posts = new HashSet<Post>();
    }
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Post> Posts { get; set; }
}
class Post
{
    public int Id { get; set; }
    public int? BlogId { get; set; }
    public string Title { get; set; }

    public Blog Blog { get; set; }
}
class Book
{
    public Book()
    {
        Authors = new HashSet<Author>();
    }
    public int Id { get; set; }
    public string BookName { get; set; }

    public ICollection<Author> Authors { get; set; }
}
class Author
{
    public Author()
    {
        Books = new HashSet<Book>();
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }

    public ICollection<Book> Books { get; set; }
}


class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithOne(p => p.Address)
            .HasForeignKey<Address>(a => a.Id)
            .OnDelete(DeleteBehavior.Restrict); // Cascade delete davranışı yukarıda açıklamasını yaptım.

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Blog)
            .WithMany(b => b.Posts)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false); // IsRequired => foreign key kolonu gerekli değil anlamına geliyor.

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books);
    }
}