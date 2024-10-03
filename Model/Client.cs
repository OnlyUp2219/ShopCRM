using System.ComponentModel.DataAnnotations.Schema;

namespace ShopCRM.Model
{
   public class Client
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Surname { get; set; }
      public string Phone { get; set; }
      public string Email { get; set; }
      public string Company { get; set; }
      public List<Order> Orders { get; set; } = new List<Order>();
      [NotMapped]
      public string FullName => $"{Name} {Surname}";

   }
}
