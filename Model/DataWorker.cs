using ShopCRM.Model.Data;

namespace ShopCRM.Model
{
   internal class DataWorker
   {
      #region CREATE
      public static string CreateOrder(Order order)
      {
         string result = "Заказ уже существует";
         using (ApplicationContext db = new ApplicationContext())
         {
            // Проверяем, существует ли заказ с таким же ClientId, OrderDate и Product
            bool checkIsExist = db.Orders.Any(o => o.ClientId == order.ClientId && o.OrderDate == order.OrderDate && o.Product == order.Product);
            if (!checkIsExist && order != null)
            {
               db.Orders.Add(order); // Добавляем новый заказ
               db.SaveChanges(); // Сохраняем изменения в базе данных
               result = "Заказ успешно создан";
            }
         }
         return result;
      }

      public static string CreateClient(Client client)
      {
         string result = "Клиент уже существует";
         
         using (ApplicationContext db = new ApplicationContext())
         {
            // Проверяем, существует ли клиент с таким же Email или Phone
            bool checkIsExist = db.Clients.Any(c => c.Email == client.Email || c.Phone == client.Phone);
            if (!checkIsExist && client != null)
            {
               client.Phone = client.Phone.Replace(" ","");
               db.Clients.Add(client); // Добавляем нового клиента
               db.SaveChanges(); // Сохраняем изменения в базе данных
               result = "Клиент успешно создан";
            }
         }
         return result;
      }
      #endregion

      #region EDIT
      public static string EditOrder(Order oldOrder, string newProduct, int newQuantity, decimal newPrice, string newOrderDate)
      {
         string result = "Такого заказа не существует";
         using (ApplicationContext db = new ApplicationContext())
         {
            // Находим существующий заказ по Id
            Order order = db.Orders.FirstOrDefault(o => o.Id == oldOrder.Id);
            if (order != null)
            {
               // Обновляем свойства заказа
               order.Product = newProduct;
               order.Quantity = newQuantity;
               order.Price = newPrice;
               order.OrderDate = newOrderDate;

               db.SaveChanges(); // Сохраняем изменения в базе данных
               result = "Заказ успешно обновлен";
            }
         }
         return result;
      }

      public static string EditClient(Client oldClient, string newName, string newSurname, string newPhone, string newEmail, string newCompany)
      {
         string result = "Такого клиента не существует";
         using (ApplicationContext db = new ApplicationContext())
         {
            // Находим существующего клиента по Email или Phone
            Client client = db.Clients.FirstOrDefault(c => c.Id == oldClient.Id);
            if (client != null)
            {
               
               // Обновляем свойства клиента
               client.Name = newName;
               client.Surname = newSurname;
               client.Phone = newPhone.Replace(" ", ""); ;
               client.Email = newEmail;
               client.Company = newCompany;
               db.SaveChanges(); // Сохраняем изменения в базе данных
               result = "Клиент успешно обновлен";
            }
         }
         return result;
      }
      #endregion

      #region DELETE
      public static string DeleteClient(Client client)
      {
         string result = "Такого клиента не существует";
         using (ApplicationContext db = new ApplicationContext())
         {
            db.Clients.Remove(client);
            db.SaveChanges();
            result = "Сделано! Клиент " + client.Name + " " + client.Surname + " удален";
         }
         return result;
      }

      public static string DeleteOrder(Order order)
      {
         string result = "Такого заказа не существует";
         using (ApplicationContext db = new ApplicationContext())
         {
            db.Orders.Remove(order);
            db.SaveChanges();
            result = "Сделано! Заказ " + order.Product + " на " + order.OrderDate + " удален";
         }
         return result;
      }
      #endregion

      #region ID
      // Получение клиента по ID
      public static Client GetClientById(int id)
      {
         using (ApplicationContext db = new ApplicationContext())
         {
            return db.Clients.FirstOrDefault(c => c.Id == id);
         }
      }

      // Получение заказа по ID
      public static Order GetOrderById(int id)
      {
         using (ApplicationContext db = new ApplicationContext())
         {
            return db.Orders.FirstOrDefault(o => o.Id == id);
         }
      }

      // Получение всех заказов по ID клиента
      public static List<Order> GetAllOrdersByClientId(int clientId)
      {
         using (ApplicationContext db = new ApplicationContext())
         {
            return db.Orders.Where(o => o.ClientId == clientId).ToList();
         }
      }

      // Получение всех клиентов
      public static List<Client> GetAllClients()
      {
         using (ApplicationContext db = new ApplicationContext())
         {
            return db.Clients.ToList();
         }
      }

      // Получение всех заказов
      public static List<Order> GetAllOrders()
      {
         using (ApplicationContext db = new ApplicationContext())
         {
            return db.Orders.ToList();
         }
      }
      #endregion

   }
}
