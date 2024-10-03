using ShopCRM.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopCRM.Model
{
   internal class Filter
   {
      public static List<Client> GetFilteredClientsByFullName(string name, string surname)
      {
         using (ApplicationContext db = new ApplicationContext())
         {
            // Получаем всех клиентов из базы данных
            List<Client> allClients = db.Clients.ToList();

            // Фильтруем клиентов по имени и фамилии
            var filteredClients = allClients
                .Where(c => (string.IsNullOrEmpty(name) || c.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                            (string.IsNullOrEmpty(surname) || c.Surname.Contains(surname, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return filteredClients;
         }
      }

      public static List<Order> GetFilteredOrdersByDate(string fromDateFilter, string toDateFilter)
      {
         using (ApplicationContext db = new ApplicationContext())
         {
            // Определяем формат даты
            string dateFormat = "dd.MM.yyyy"; // Формат "дд.мм.гггг"
            DateTime fromDate;
            DateTime toDate;

            // Проверяем, удалось ли преобразовать строки в даты с заданным форматом
            bool isFromDateValid = DateTime.TryParseExact(fromDateFilter, dateFormat,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out fromDate);

            bool isToDateValid = DateTime.TryParseExact(toDateFilter, dateFormat,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out toDate);

            // Если даты валидны
            if (isFromDateValid && isToDateValid)
            {
               // Извлекаем все заказы из базы данных
               var allOrders = db.Orders.ToList();

               // Фильтруем заказы по диапазону дат
               return allOrders
                   .Where(o => DateTime.TryParseExact(o.OrderDate, dateFormat,
                       System.Globalization.CultureInfo.InvariantCulture,
                       System.Globalization.DateTimeStyles.None,
                       out DateTime orderDate) && orderDate >= fromDate && orderDate <= toDate)
                   .ToList();
            }
            else
            {
               // Если одна из дат невалидна, возвращаем пустой список
               return new List<Order>();
            }
         }
      }




   }
}
