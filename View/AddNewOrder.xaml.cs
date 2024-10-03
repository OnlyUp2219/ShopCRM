using ShopCRM.ViewModel;
using System.Windows;

namespace ShopCRM.View
{
   /// <summary>
   /// Логика взаимодействия для AddNewOrder.xaml
   /// </summary>
   public partial class AddNewOrder : Window
   {
      public AddNewOrder()
      {
         InitializeComponent();
         DataContext = new DataManageVM();
      }
   }
}
