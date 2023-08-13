
using Microsoft.EntityFrameworkCore;
using System.Reflection;

ApplicationDbContext context = new();

#region Complext Query Operators

#region Join

#region Query Syntax
//var query = from photo in context.Photos
//            join person in context.Persons
//                on photo.PersonId equals person.PersonId
//            select new
//            {
//                person.Name,
//                photo.Url
//            };
//var datas = await query.ToListAsync();
#endregion
#region Method Syntax
//var query = context.Photos
//    .Join(context.Persons,
//    photo => photo.PersonId,
//    person => person.PersonId,
//    (photo, person) => new
//    {
//        person.Name,
//        photo.Url
//    });

//var datas = await query.ToListAsync();
#endregion

#region Multiple Columns Join

#region Query Syntax
// Multiple bir bir join işlemi gerçekleştireceksek kesinlikle anonim class lar kullanmak lazım.
//var query = from photo in context.Photos
//            join person in context.Persons
//                on new { photo.PersonId, photo.Url } equals new { person.PersonId, Url = person.Name }
//            select new
//            {
//                person.Name,
//                photo.Url
//            };
//var datas = await query.ToListAsync();
#endregion
#region Method Syntax
// Hemen aşağıda neden bir URL = person.Name yazıp yaptık dersek çünkü normalde url parametresi person da olmadığı için sanki person.name  = url miş gibi davranmamız gerekiyor ki joinleme işlemi gerçekleşsin. Bundan dolayı anonim classların gücünü kullanıyoruz.

//var query = context.Photos
//    .Join(context.Persons,
//    photo => new
//    {
//        photo.PersonId,
//        photo.Url
//    },
//    person => new
//    {
//        person.PersonId,
//        Url = person.Name
//    },
//    (photo, person) => new
//    {
//        person.Name,
//        photo.Url
//    });

//var datas = await query.ToListAsync();
#endregion
#endregion

#region 2'den Fazla Tabloyla Join

#region Query Syntax
//var query = from photo in context.Photos
//            join person in context.Persons
//                on photo.PersonId equals person.PersonId
//            join order in context.Orders
//                on person.PersonId equals order.PersonId
//            select new
//            {
//                person.Name,
//                photo.Url,
//                order.Description
//            };

//var datas = await query.ToListAsync();
#endregion
#region Method Syntax
//var query = context.Photos
//    .Join(context.Persons,
//    photo => photo.PersonId,
//    person => person.PersonId,
//    (photo, person) => new
//    {
//        person.PersonId,
//        person.Name,
//        photo.Url
//    })
//    .Join(context.Orders,
//    personPhotos => personPhotos.PersonId,
//    order => order.PersonId,
//    (personPhotos, order) => new
//    {
//        personPhotos.Name,
//        personPhotos.Url,
//        order.Description
//    });

//var datas = await query.ToListAsync();
#endregion
#endregion

#region Group Join - GroupBy Değil!
//var query = from person in context.Persons
//            join order in context.Orders
//                on person.PersonId equals order.PersonId into personOrders
//            //from order in personOrders
//            select new
//            {
//                person.Name,
//                Count = personOrders.Count(),
//                personOrders
//            };
//var datas = await query.ToListAsync();
#endregion
#endregion

//DefaultIfEmpty : Sorgulama sürecinde ilişkisel olarak karşılığı olmayan verilere default değerini yazdıran yani LEFT JOIN sorgusunu oluşturtan bir fonksiyondur.

#region Left Join
//var query = from person in context.Persons
//            join order in context.Orders
//                on person.PersonId equals order.PersonId into personOrders
//            from order in personOrders.DefaultIfEmpty()
//            select new
//            {
//                person.Name,
//                order.Description
//            };

//var datas = await query.ToListAsync();
#endregion

#region Right Join
// Efcore da right join yapmak normalde mümkün değil böyle bir geliştirme yok ancak arka planda right joini çalıştırıyor yinede.
// Ancak biz yukarıda left join yaparken kullandığımız yapılanmayı şöyle ters den yazarsak yani baz almak istediğimiz entity'nin sırasını değiştirirsek bunu right join olarak kabul ediyoruz. Böyle güzel kaçamak bir yöntemimiz var.
// Yukarıda ki left join region'unda ne yapmıştık person'ı baz almıştık şimdi order'ı baz alıcaz.
//var query = from order in context.Orders
//            join person in context.Persons
//                on order.PersonId equals person.PersonId into orderPersons
//            from person in orderPersons.DefaultIfEmpty()
//            select new
//            {
//                person.Name,
//                order.Description
//            };

//var datas = await query.ToListAsync();
#endregion

#region Full Join
// Full join yapılanmasını da desteklemiyor ef core ancak biz hem left ve right join'i yazıp sonra bunları leftQuery.Union(rightQuery) bu şekilde birleştirince olay tamamlanıyor.

//var leftQuery = from person in context.Persons
//                join order in context.Orders
//                    on person.PersonId equals order.PersonId into personOrders
//                from order in personOrders.DefaultIfEmpty()
//                select new
//                {
//                    person.Name,
//                    order.Description
//                };


//var rightQuery = from order in context.Orders
//                 join person in context.Persons
//                     on order.PersonId equals person.PersonId into orderPersons
//                 from person in orderPersons.DefaultIfEmpty()
//                 select new
//                 {
//                     person.Name,
//                     order.Description
//                 };

//var fullJoin = leftQuery.Union(rightQuery);

//var datas = await fullJoin.ToListAsync();
#endregion

#region Cross Join
// Cross join sağ daki kolondaki her bir satır için soldaki bütün satırları eşleyerek ilerler.
// örnek renk tablomuz olsun bu tabloda bir tane kırmızı diye bir değer olsun tek bir row, sağdaki tabloda renklerin tonajları olsun bu durumda kartezyen çarpım şeklinde ilerleyecek yani = kırızmı - pj101, kırmızı - pj102, kırmızı - pj103

//var query = from order in context.Orders
//            from person in context.Persons
//            select new
//            {
//                order,
//                person
//            };

//var datas = await query.ToListAsync();
#endregion

#region Collection Selector'da Where Kullanma Durumu
//var query = from order in context.Orders
//            from person in context.Persons.Where(p => p.PersonId == order.PersonId)
//            select new
//            {
//                order,
//                person
//            };

//var datas = await query.ToListAsync();
#endregion

#region Cross Apply
//Inner Join
// Cross apply da inner join de ki gibi sadece eşleşen verileri getirir. Outer appyl'da ise eşleşmeyen veriler olsada onları null değer olarak yine de getirir.

//var query = from person in context.Persons
//            from order in context.Orders.Select(o => person.Name)
//            select new
//            {
//                person,
//                order
//            };

//var datas = await query.ToListAsync();
#endregion

#region Outer Apply
//Left Join
// Left join deki gibi eşleşmeyen veriler olsada eşleşmeyen değerleri yinede getirir ve null yazar.
//var query = from person in context.Persons
//            from order in context.Orders.Select(o => person.Name).DefaultIfEmpty()
//            select new
//            {
//                person,
//                order
//            };

//var datas = await query.ToListAsync();
#endregion
#endregion
Console.WriteLine();
public class Photo
{
    public int PersonId { get; set; }
    public string Url { get; set; }

    public Person Person { get; set; }
}
public enum Gender { Man, Woman }
public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
    public Gender Gender { get; set; }

    public Photo Photo { get; set; }
    public ICollection<Order> Orders { get; set; }
}
public class Order
{
    public int OrderId { get; set; }
    public int PersonId { get; set; }
    public string Description { get; set; }

    public Person Person { get; set; }
}

class ApplicationDbContext : DbContext
{
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Photo>()
            .HasKey(p => p.PersonId);

        modelBuilder.Entity<Person>()
            .HasOne(p => p.Photo)
            .WithOne(p => p.Person)
            .HasForeignKey<Photo>(p => p.PersonId);

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