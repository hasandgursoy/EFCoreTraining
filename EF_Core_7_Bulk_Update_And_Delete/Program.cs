﻿using Microsoft.EntityFrameworkCore;
using System.Reflection;

ApplicationDbContext context = new();

#region EF Core 7 Öncesi Toplu Güncelleme
//var persons = await context.Persons.Where(p => p.PersonId > 5).ToListAsync();
//foreach (var person in persons)
//{
//    person.Name = $"{person.Name}...";
//}
//await context.SaveChangesAsync();
#endregion
#region EF Core 7 Öncesi Toplu Silme
//var persons = await context.Persons.Where(p => p.PersonId > 5).ToListAsync();
//context.RemoveRange(persons);
//await context.SaveChangesAsync();
#endregion


#region ExecuteUpdate
// SetProperty yapısında string interpolation kullanırsak expression method yapısından dolayı hata alacağız.
await context.Persons.Where(p => p.PersonId > 3).ExecuteUpdateAsync(p => p.SetProperty(p => p.Name, v => v.Name + " yeni"));
//await context.Persons.Where(p => p.PersonId > 3).ExecuteUpdateAsync(p => p.SetProperty(p => p.Name, v => $"{v.Name} yeni"));
#endregion
#region ExecuteDelete
//await context.Persons.Where(p => p.PersonId > 3).ExecuteDeleteAsync();
#endregion

//ExecuteUpdate ve ExecuteDelete fonksiyonları ile bulk(toplu) veri güncelleme ve silme işlemleri gerçekleştirirken SaveChanges fonksiyonunu çağırmanız gerekmemektedir. Çünkü b fonksiyonlar adları üzerinde Execute... fonksiyonlarıdır. Yani direkt veritabanına fiziksel etkide bulunurlar.

//Eğer ki istyorsanız transaction kontrolünü ele alarak bu fonksiyonların işlevlerini de süreçte kontrol edebilirsiniz.

public class Person
{
    public int PersonId { get; set; }
    public string Name { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
               optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=DenemeDB;User Id=postgres;Password=123456;");
    }
}