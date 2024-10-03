using ShopCRM.ViewModel;
using System.Windows;
using System.Windows.Controls;


namespace ShopCRM.View
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public static DataGrid AllClients;
      public static DataGrid AllOrders;
      public MainWindow()
      {
         InitializeComponent();
         DataContext = new DataManageVM();
         AllClients = ViewClientsList;
         AllOrders = ViewOrdersList;
      }
   }
}