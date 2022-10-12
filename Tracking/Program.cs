using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

var context = new ETicaretContext();


#region AsNoTracking Methodu
// Context üzerinden gelen tüm datalar Change Tracker mekanizması tarafından takip edilmektedir.
// Change Tracker takip ettiği nesnelerin sayısıyla doğru orantılı maliyete sahiptir.
// Bu yüzden işlem yapılmayacak verilerin takip edilmesi bizlere lüzumsuz maliyet demektir.
// AsNoTracking metodu, context üzerinden sorgu neticesinde gelecek olan verilerin takip edilmesini engeller.
// AsNoTracking metodu ile Change Tracker'ın ihtiyaç olmayan verilerde ki maliyetini törpülemiş oluruz.
// AsNoTracking methodu ile yapılan sorgulamalarda, verileri elde edebilir ve kullanabiliriz lakin veriler üzerinde
// herhangi bir değişiklik işlemi yapmayız.

// Aşşağıda herhangi değişiklik işlemi yapmayacağım için AsNoTracking'i IQuerayable durumundayken tetikledim.
//var kullanicilar = await context.Kullanicilar.AsNoTracking().ToListAsync();

//foreach (var kullanici  in kullanicilar)
//{
//    Console.WriteLine(kullanici.Adi);
//}



#endregion

#region AsNoTrackingWithIdentityResolution
// CT (Change Tracker) mekanizması yinelenen verileri tekil insatance olarak getirir.
// Buradan ekstra bir performans kazancı söz konusudur.
// Bizler yarattığımız sorgularda takip mekanizmasının AsNoTracking metodu ile maliyeti kırmak isterken 
// bazen maliyete sebebiyet verebiliriz. (Özellikle ilişkisel tabloları sorgularken bu duruma dikkat etmemiz gerekiyor.) Resme BAK !!

// AsNoTracking ile elde edilen veriler takip edilmeyeceğinden dolayı yinelenen verilerin ayrı instancelarda olmasına sebebiyet veriyoruz.
// Çünkü CT mekanizması takip ettiği nesneden bellekte varsa eğer aynı nesneden bir daha oluşturma gereği duymaksızın o nesneye ayrı noktalardaki
// ihtiyacı aynı instance üzerinden gidermektedir.

// Böyle bir durumda hem takip mekanizmasının maliyetini ortadan kaldırmak için hemde yinelenen
// dataları tek bir instance üzerinde karşılamak için AsNoTrackingWithIdentityResolution fonksiyonu kullanılabilir.

// İlişki kurduğumuz yapılarda daha faydalıdır.
// Sorgu yapılarında track mekanizmasına ihtiyac yoktur bunuda doğru yolla yapmak lazım 
// Her yazara bir kitap yada her kitaba bir yazar atamak doğru değil bunun için bu yapıyı kullanıyoruz.
// Çünkü bir yazarın birden fazla kitabı olabilir.Bu yüzden her kitap için aynı yazarı bir daha oluşturmanın bir manası yok.

//var kitaplar = context.Kitaplar.Include(k => k.Yazarlar).AsNoTrackingWithIdentityResolution().ToListAsync();

// AsNoTrackingWithIdentitResolution fonksiyonu AsNoTracking fonksiyonuna nazaran görece yavaştır ve maliyetlidir.
// Lakin CT'a nazaran daha performanslı ve az maliyetlidir.
// Kısaca yinelenen dataların ayrı instancelarda olmasını engeller.

#endregion

#region AsTracking
// context üzerinden gelen dataların CT tarafından takip edilmesini iradeli bir şekilde ifade etmemizi sağlayan fonksiyondur.
// Peki neden kullanmalıyız ?
// Bir sonraki inceleyeceğimiz UseQueryTrackingBehaviour metodunun davranışının gereği uygulama olarak devrede olup olmamasını
// ayarlıyor olacağız. Eğer ki default olarak pasif hale getirilirse böyle durumda takip mekanizmasının ihtiyac olduğu sorgularda
// AsTracking fonksiyonunu kullanabilir ve böylece takip mekanizmasının iradeli bir  şekilde devreye sokmuş oluruz.

// var kitaplar = await context.Kitaplar.AsTracking().ToListAsync();


#endregion

#region UseQueryTrackingBehavior
// EF Core seviyesinde/uygulama seviyesinde ilgili contexten gelen verilerin üzerinde CT mekanizmasının davranışını temel seviyede belirlememizi sağlayan fonksiyondur.
// Yani konfigurasyon fonksiyonudur. OnConfigutaion içinde kullanıyoruz.


#endregion

public class ETicaretContext : DbContext
{


    public DbSet<Kullanici> Kullanicilar { get; set; }
    public DbSet<Rol> Roller { get; set; }
    public DbSet<Kitap> Kitaplar { get; set; }
    public DbSet<Yazar> Yazarlar { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");

        // Bunu diyerek CT mekanizmasına hiçbirşeyi takip etme demiş oluyoruz bunu istediğimiz gibi kullanabiliriz.
        // Eğer bu şekilde NoTracking de bırakırsak sürekli AsTracking kullanmak zorunda kalabiliriz.
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            

    }


}

public class Kullanici
{
    public int Id { get; set; }
    public string Adi { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public ICollection<Rol> Roller { get; set; }
}
public class Rol
{
    public int Id { get; set; }
    public string RolAdi { get; set; }
    public ICollection<Kullanici> Kullanicilar { get; set; }
}
public class Kitap
{
    public Kitap() => Console.WriteLine("Kitap nesnesi oluşturuldu.");
    public int Id { get; set; }
    public string KitapAdi { get; set; }
    public int SayfaSayisi { get; set; }

    public ICollection<Yazar> Yazarlar { get; set; }
}
public class Yazar
{
    public Yazar() => Console.WriteLine("Yazar nesnesi oluşturuldu.");
    public int Id { get; set; }
    public string YazarAdi { get; set; }

    public ICollection<Kitap> Kitaplar { get; set; }
}