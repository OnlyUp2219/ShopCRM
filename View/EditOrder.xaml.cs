using ShopCRM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShopCRM.View
{
   /// <summary>
   /// Логика взаимодействия для EditOrder.xaml
   /// </summary>
   public partial class EditOrder : Window
   {
      public EditOrder()
      {
         InitializeComponent();
         DataContext = new DataManageVM();
      }
   }
}
