// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

BackingFieldsDbContext context = new();

// Ayrı bir şekilde bilinmesi gereken bir konu mu bilmiyorum çünkü farkında olmadan herkes kullanıyor aslında

#region Backing Fields
// Tablo içerisindeki kolonları, entity class'ları içerisinde propertyler ile değil fieldlarla temsil etmemizi sağlayan bir özelliktir.


// 1. yöntem budur. get ve set de kendin halledersin

// class Persona
// {
//     public int Id { get; set; } // property

//     // Kapsülleme yapabilme şansına sahip olmuş oluyoruz.
//     public string name; // field
//     public string Name { get => name.Substring(0,3); set => name = value; }
//     public string Departmans { get; set; }
// }
#endregion

#region BackingField Attribute
// 2. yöntem BackingFields Attribute'u kullanarak yapılabilir.

// class Persona
// {
//     [BackingField(nameof(name))]
//     public string Name { get; set; }
//     public int Id { get; set; } // property
//     // Kapsülleme yapabilme şansına sahip olmuş oluyoruz.
//     public string name; // field

//     public string Departmans { get; set; }
// }
#endregion

#region FluentAPI HasField
// 3. yöntem onModelCreating içerisinde yapılabilir.
// Fluent API'de HasField metodu BackingField özelliğine karşılık gelmektedir.
// protected override void OnModelCreating(ModelBuilder modelBuilder)
// {

//     // HasField özelliği BackingField yapısını kurar.

//     modelBuilder.Entity<Persona>()
//     .Property(p => p.Name)
//     .HasField(nameof(Persona.name));
// }

#endregion

#region Field And Property Access
// EFCore sorgulama sürecinde entity içerisindeki propertyleri ya da fieldları kullanıp kullanılmayacağının davranışlarını bizlere belirtmektedir.

// EFCore , hiç bir ayarlama yoksa varsıyaln olarak propertyler üzerinden verileri işler, eğer ki backing field bildiriliyorsa field üzerinden işler yok eğer backing field bildirildiği halde davranış belirtiliyorsa ne belirtilmişse ona göre işlemeyi devam ettirir.

// Fluent API ile yapıyoruzz bunu db context de OnModelCreating'de UsePropertyAccessMode üzerinden gerçekleştiriyoruz.
#endregion

#region Field-Only Properties
// Entitylerde değerleri almak için propertyler yerine metotların kullanıldığı veya belirli alanların hiç gösterilmemesi gerektiği durumlarda (örneğin primary key kolonu) kullanılabilir.

// class Persona
// {
//     public int Id { get; set; } // property
//     // Kapsülleme yapabilme şansına sahip olmuş oluyoruz.
//     public string name; // field

//     public string Departmans { get; set; }
// }

// yukarıdaki class da name değeri bir field ama biz bunu property gibi kullanmak istiyoruz o halde gidip modelBuilder da işlem yapmamız lazım aşşağıya hemen kodları bırakıyorum.

// modelBuilder.Entity<Persona>()
//     .Property(nameof(Persona.name));
#endregion



Persona persona = new Persona
{
    Name = "MotoGP",
    Departmans = "SSS"
};

await context.Personsas.AddAsync(persona);
var person = await context.Personsas.FindAsync(1);
Persona person2 = new()
{
    Name = "Person  101",
    Departmans = "Departmant 101"
};
await context.Personsas.AddAsync(person2);


await context.SaveChangesAsync();


Console.Read();





class Persona
{
    [BackingField(nameof(name))]
    public string Name { get; set; }
    public int Id { get; set; } // property
    // Kapsülleme yapabilme şansına sahip olmuş oluyoruz.
    public string name; // field

    public string Departmans { get; set; }
}

class BackingFieldsDbContext : DbContext
{
    public DbSet<Persona> Personsas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ETicaretDb;User Id=postgres;Password=postgres;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // HasField özelliği BackingField yapısını kurar.

        modelBuilder.Entity<Persona>()
        .Property(p => p.Name)
        .HasField(nameof(Persona.name))
        .UsePropertyAccessMode(PropertyAccessMode.PreferField);




        //Field : Veri erişim süreçlerinde sadece field'ların kullanılmasını söyler. Eğer field'ın kullanılamayacağı durum söz konusu olursa bir exception fırlatır.
        //FieldDuringConstruction : Veri erişim süreçlerinde ilgili entityden bir nesne oluşturulma sürecinde field'ların kullanılmasını söyler.,
        //Property : Veri erişim sürecinde sadece propertynin kullanılmasını söyler. Eğer property'nin kullanılamayacağı durum söz konusuysa (read-only, write-only) bir exception fırlatır.
        //PreferField,
        //PreferFieldDuringConstruction,
        //PreferProperty

    }
}