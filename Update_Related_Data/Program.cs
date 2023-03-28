// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

ApplicationDbContext context = new ApplicationDbContext();

#region One To One İlişkisel Senaryolarda Veri Güncelleme

#region Saving

// Burdaki senaryo şu hasan person'u adress'i var birebir ilişki ama Hilminin yok hilminin adress bilgisini güncellemek istersek ne olur ? 

// Person person = new()
// {
//     Name = "Hasan",
//     Address = new()
//     {
//         PersonAddress = "Çankaya/Fransa"
//     }
// };

// Person person2 = new()
// {
//     Name = "Hilmi"
// };

// await context.AddAsync(person);
// await context.AddAsync(person2);
// await context.SaveChangesAsync();

#endregion

#region 1.Durum | Esas tablodaki veriye bağımlı veri değiştirme

// Include Props'u ilişki sahibi olunan tablolar'ı ilişkisi olan yapıya dahil edip kullanmamızı sağlar.

// Person? person = await context.Persons
//     .Include(p => p.Address)
//     .FirstOrDefaultAsync(p => p.Id == 1);

// context.Addresses.Remove(person.Address);
// person.Address = new()
// {
//     PersonAddress = "Yeni Adress"
// };
// await context.SaveChangesAsync();

#endregion

#region 2.Durum | Bağımlı verinin ilişkisel olduğu ana veriyi güncelleme

// Address? address = await context.Addresses.FindAsync(6);
// var p1 = await context.Addresses.Include(p => p.Person).FirstOrDefaultAsync(a => a.Id == address.Id);
// context.Addresses.Remove(address);
// await context.SaveChangesAsync();


// Buraya kadar id si 1 olan adres i sildik
// şimdi gidip in memoryde ki adressle person nesnesi arasında bağlantı kuralım

// İlişki kuracağımız person'ı  ya seçelim yada oluşturalım sonra gidip ilişki kuralım

// address.Person = new(){
//     Id = 1,
//     Name = "Selami"
// };

// await context.Addresses.AddAsync(address);
// await context.SaveChangesAsync();
// Address dd = await context.Addresses.Include(p => p.Person).FirstOrDefaultAsync(x => x.Id == 6);
// System.Console.WriteLine(dd.Person.Name);

#endregion

#endregion

#region One To Many İlişkisel Senaryolarda Veri Güncelleme

#region Saving
// Blog blog = new()
// {
//     Name = "www.google.com",
//     Posts = new List<Post>
//     {
//         new(){Title = "1.Post"},
//         new(){Title = "2.Post"},
//         new(){Title = "3.Post"}
//     }
// };

// await context.Blogs.AddAsync(blog);
// await context.SaveChangesAsync();
#endregion

#region 1.Durum | Esas tablodaki veriye bağımlı veri değiştirme

// Blog? blog = await context.Blogs.Include(b => b.Posts).FirstOrDefaultAsync(b => b.Id == 1);

// Post? silinecekPost =  blog?.Posts.FirstOrDefault(p => p.Id == 2);

// if (blog != null && silinecekPost != null)
// {
// blog?.Posts.Remove(silinecekPost);    
// }

// blog?.Posts.Add(new(){Title = "4. Post"});
// blog?.Posts.Add(new(){Title = "5. Post"});

// await context.SaveChangesAsync();

#endregion

#region 2.Durum | Bağımlı verinin ilişkisel olduğu ana veriyi güncelleme

// 4. post'u 2. blog adında yeni bir blog la güncelleyelim
// Post? post = await context.Posts.FindAsync(4);
// post.Blog = new(){
//     Name = "2. BLog"
// };

// await context.SaveChangesAsync();

// 5. post'un bloğunu direkt olarak var olan 2.blog ile güncelleyelim.
// Post? post = await context.Posts.FindAsync(5);
// Blog? blog = await  context.Blogs.FindAsync(2);

// post.Blog = blog;

// await context.SaveChangesAsync();


#endregion

#endregion

#region  Many To Many İlişkisel Senaryolrda Veri Güncelleme

#region Saving

// Book book1 = new(){BookName = "1. Kitap"};
// Book book2 = new(){BookName = "2. Kitap"};
// Book book3 = new(){BookName = "3. Kitap"};

// Author autoh1 = new (){AuthorName = "1. Yazar"};
// Author autoh2 = new (){AuthorName = "2. Yazar"};
// Author autoh3 = new (){AuthorName = "3. Yazar"};

// book1.Authors.Add(autoh1);
// book1.Authors.Add(autoh2);

// book2.Authors.Add(autoh1);
// book2.Authors.Add(autoh2);
// book2.Authors.Add(autoh3);

// book3.Authors.Add(autoh3);

// await context.AddAsync(book1);
// await context.AddAsync(book2);
// await context.AddAsync(book3);
// await context.SaveChangesAsync();

#endregion

#region 1. Örnek

// Bu senaryoda id si 1 olan kitabın yazarını güncelleyelim.

// Book? book = await context.Books.FindAsync(1);
// Author? author = await context.Authors.FindAsync(3);
// book.Authors.Add(author);

// await context.SaveChangesAsync();

#endregion

#region 2. Örnek 

// Bu senaryoda id si 3 olan yazarın id si 1 olmayan bütün kitaplarını silelim ( EFCore otomatik algılayıp cross table'dan siliyor no problem ) 

// Author? author = await context.Authors.Include(b => b.Books).FirstOrDefaultAsync(a => a.Id == 3);

// foreach (var book in author.Books)
// {
//     if (book.Id != 1)
//     {
//         author.Books.Remove(book);
//     }
// }
// await context.SaveChangesAsync();

#endregion

#region 3. Örnek

// Bu örnek de id si 2 olan kitabın id si 1 olan yazarla ilişkisini kesip 3. yazarla ve yeni oluşacak 4. yazarla ilişki kurmasını sağlamak.


// Book? book = await context.Books.Include(b => b.Authors).FirstOrDefaultAsync(b => b.Id == 2);

// Author? silinecekYazar = book.Authors.FirstOrDefault(a => a.Id == 1);

// book.Authors.Remove(silinecekYazar);

// Author author = await context.Authors.FindAsync(3);
// book.Authors.Add(author);
// book.Authors.Add(new(){AuthorName = "4. Yazar"});

// await context.SaveChangesAsync();


#endregion

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
    public int BlogId { get; set; }
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
            .HasForeignKey<Address>(a => a.Id);
    }
}