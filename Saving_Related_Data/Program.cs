// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
ApplicationDbContext context = new();

#region  One To One İlişkisel Senaryolarda Veri Ekleme


#region 1.Yöntem -> Principal Entity Üzerinden Dependent Entity Veri Ekleme


// Burda durum şu şekilde Person nesnesi bir Principal Entity'dir. 
// Person nesnesi aynı zamanda içinde address adında dependent entity barındırır.
// Dependent entity kendi içinde person nesnesine ihtyiaç duyar person nesnesi oluştururken bunu kendimiz sağlamak yerine Person'ın oluştururken tanımlarsak ekstra bir daha Person'ın adress prop'unda person nesnesini tanımlamak zorunda kalmıyoruz.
// Person person = new();
// person.Name = "Hasan";
// person.Adress = new()
// {
//     PersonAdress = "Istanbul/Kagithane",
//     // Person = new() demek zorunda kalmıyoruz.
// };

// await context.Persons.AddAsync(person);


#endregion

// Eğer ki principal entity üzerinden ekleme gerçekleştiriliyorsa dependent entity nenesi verilmek zoruunda değildir! 
// Amma velakin, dependent entity üzerinden ekleme gerçekleştiriyorsak principal entity verilmek zorundadır. 

#region 2.Yöntem -> Dependent Entity Üzerinden Principal Entity Verisi Ekleme
// Yukarıdakinin tam tersini yaptık aslında ekstra bir durum gerçekleştirmedik tersten gittik sadece
// Address address = new()
// {
//     PersonAdress = "Istanbul",
//     Person = new() {Name = "Turgay"}
// };

// await context.Addresses.AddAsync(address);
// await context.SaveChangesAsync();

#endregion

// class Person
// {
//     public int Id { get; set; }
//     public string Name { get; set; }
//     public Address Adress { get; set; }

// }

// class Address
// {
//     public int Id { get; set; }
//     public string PersonAdress { get; set; }
//     public Person Person { get; set; }

// }

// class ApplicationDbContent : DbContext
// {
//     public DbSet<Person> Persons { get; set; }
//     public DbSet<Address> Addresses { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//         optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
//     }

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder.Entity<Address>()
//             .HasOne(a => a.Person)
//             .WithOne(p => p.Adress)
//             .HasForeignKey<Address>(a => a.Id);

//     }


// }



#endregion

#region One To Many İlişkisel Senaryolarda Veri Ekleme

#region 1.Yöntem -> Principal Entity Üzerinden Dependent Verisi Ekleme

#region Nesne Referansı Üzerinden Ekleme

// Bunu yapabilmek için ilgili entity'nin constructor'ına gidip objeyi new lemek gerekiyor.

// Blog blog = new() { Name = "gencayyildiz.com Blog" };
// blog.Posts.Add(new() { Title = "Post 1" });
// blog.Posts.Add(new() { Title = "Post 2" });
// blog.Posts.Add(new() { Title = "Post 3" });

// await context.AddAsync(blog);
// await context.SaveChangesAsync();

#endregion

#region Object Initializer Üzerinden Ekleme

// Blog blog2 = new()
// {
//     Name = "A Blog",
//     Posts = new HashSet<Post>(){
//         new (){ Title = "Post 4"},
//         new (){Title = "Post 5 "}
//     }
// };

// await context.AddAsync(blog2);
// await context.SaveChangesAsync();

#endregion

#endregion

#region 2.Yöntem -> Dependent Entity Üzerinden Principal Entity Verisi Ekleme
// Genel olarak Dependent Entity üzerinden principal entity verisi eklemek doğru bir davranış değil çünkü sadece bir tane post için new değeri ile veri eklemiş olucaksın principal entityden eklersek birden fazla postu içinde barındırabilir bu şekilde.

// Az önce yazdıklarım 1-1 ilişki için tabi ki geçerli değil.


// Post post = new(){
//     Title = "Post 6",
//     Blog = new(){Name = "B Blog"}
// };

// await context.AddAsync(post);
// await context.SaveChangesAsync();



#endregion

#region 3.Yöntem -> Foreign Key Kolonu Üzerinden Veri Ekelem 

// 1. ve 2. yöntemler hiç olmayan verilerin ilişkisel olarak eklenmesini sağlarken, bu 3. yöntem önceden eklenmiş olan bir principal entity verisiyle yeni dependent entitylerin ilişkisel olarak eşleştirilmesini sağlamaktadır.

// Post post = new(){
//     BlogId = 1,
//     Title = "Post 7"
// };

// await context.AddAsync(post);
// await context.SaveChangesAsync();

#endregion

// class Blog
// {

//     public Blog()
//     {

//         // Burada ki Post'u neden new ile constructor da oluşturduk dersek bunun sebebi biz eğer burda new demez isek ve sonra Blog'un içindeki Posts'a veri eklemeye çalışırsak bellek de yer ayrılmamış bir alana birşeyler eklemeye çalışıyoruz bundan dolayı null reference hatası alıyoruz.

//         // HashSet List'e göre hem uniqe bir yapıdadır hemde çok daha performanslı çalışmaktadır.
//         Posts = new HashSet<Post>();
//     }

//     public int Id { get; set; }
//     public string Name { get; set; }
//     public ICollection<Post> Posts { get; set; }

// }

// class Post
// {
//     public int Id { get; set; }
//     public int BlogId { get; set; }
//     public string Title { get; set; }
//     public Blog Blog { get; set; }

// }

// class ApplicationDbContext : DbContext
// {
//     public DbSet<Post> Posts { get; set; }
//     public DbSet<Blog> Blogs { get; set; }
//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//         optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
//     }
// }

#endregion

#region  Many to Many İlişkisel Senaryolarda Veri Ekleme

#region 1.Yöntem

// n - n ilişkisi eğer ki default convention üzerinden tasarlanmışsa kullanılan bir yöntemdir.

// Composite bir class 'a gereği olmadığı durumda ne yapıyoruz

// Book book = new(){
//     BookName = "A Kitabı",
//     Authors = new HashSet<Author>(){
//         new Author(){AuthorName = "Hilmi"},
//         new Author(){AuthorName = "Ayşe"},
//         new Author(){AuthorName = "Fatma"}
//     }
// };

// await context.Books.AddAsync(book);
// await context.SaveChangesAsync();

// class Book
// {

//     public Book(){
//         Authors = new HashSet<Author>();
//     }
//     public int Id { get; set; }
//     public string BookName { get; set; }
//     public ICollection<Author> Authors { get; set; }

// }

// class Author
// {
//     public Author(){
//         Books = new HashSet<Book>();
//     }
//     public int Id { get; set; }
//     public string AuthorName { get; set; }
//     public ICollection<Book> Books {get;set;}
// }

#endregion

#region 2.Yöntem
// // n - n ilişkisi eğer ki fluent api ile tasarlanmışsa kullanılan bir yöntemdir.

// Aralarında ne fark var bunların diye sorarsan şöyle açıklayım bir tanesinde composite bir tabloyu kullanarak işlem yapyıyoruz diğerinde direk kendi içinde new leyip işlem yapıyoruz başkada bir halt yok.

Author author = new(){
    AuthorName = "Mustafa",
    Books = new HashSet<BookAuthor>(){
        new BookAuthor(){Book = new(){BookName = "B Kitap"}}
    }
};

await context.AddAsync(author);
await context.SaveChangesAsync();

class Book
{

    public Book(){
        Authors = new HashSet<BookAuthor>();
    }
    public int Id { get; set; }
    public string BookName { get; set; }
    public ICollection<BookAuthor> Authors { get; set; }

}

class BookAuthor
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }
    public Book Book { get; set; }
    public Author Author { get; set; }
}

class Author
{
    public Author(){
        Books = new HashSet<BookAuthor>();
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public ICollection<BookAuthor> Books {get;set;}
}

#endregion

class ApplicationDbContext : DbContext
{   
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new {ba.AuthorId,ba.BookId});

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.Authors)
            .HasForeignKey(ba => ba.BookId);

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(ba => ba.AuthorId);


    }

}

#endregion
