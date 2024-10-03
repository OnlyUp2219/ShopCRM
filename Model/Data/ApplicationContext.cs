using Microsoft.EntityFrameworkCore;

namespace ShopCRM.Model.Data
{
   class ApplicationContext : DbContext
   {
      public DbSet<Client> Clients { get; set; }
      public DbSet<Order> Orders { get; set; }

      public ApplicationContext()
      {
         Database.EnsureCreated(); // Создает базу данных, если она не существует
      }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         if (!optionsBuilder.IsConfigured)
         {
            // Замените строку подключения на вашу
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocaldb;Database=sqlsShopCRM;Trusted_Connection=True;");
         }
      }
   }
}
