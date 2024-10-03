using System.ComponentModel.DataAnnotations.Schema;

namespace ShopCRM.Model
{
   public class Order
   {
      public int Id { get; set; }
      public int ClientId { get; set; }
      public string OrderDate { get; set; }
      public string Product { get; set; }
      public int Quantity { get; set; }
      public decimal Price { get; set; }
      public Client Client { get; set; }
      [NotMapped]
      public Client OrderClient
      {
         get
         {
            {
               return DataWorker.GetClientById(ClientId);
            }
         }
      }

   }
}
