using ShopCRM.ViewModel;
using System.Windows;

namespace ShopCRM.View
{
   /// <summary>
   /// Логика взаимодействия для AddNewClient.xaml
   /// </summary>
   public partial class AddNewClient : Window
   {
      public AddNewClient()
      {
         InitializeComponent();
         DataContext = new DataManageVM();
      }
   }
}
