using ShopCRM.Model;
using ShopCRM.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ShopCRM.ViewModel
{
   public class DataManageVM : INotifyPropertyChanged
   {

      public DataManageVM()
      {
         // Инициализация вкладок
         SelectedTabItem = App.Current.MainWindow.FindName("ClientsTab") as TabItem; // Получение ссылки на TabItem
         IsElementVisibleClient = Visibility.Collapsed;
         IsElementVisibleDate = Visibility.Collapsed;
         UpdateFilterVisibility();
      }

      #region ALL LIST

      private List<Order> allOrders = DataWorker.GetAllOrders();
      public List<Order> AllOrders
      {
         get { return allOrders; }
         set
         {
            allOrders = value;
            OnPropertyChanged("AllOrders");
         }
      }

      private List<Client> allClients = DataWorker.GetAllClients();
      public List<Client> AllClients
      {
         get { return allClients; }
         set
         {
            allClients = value;
            OnPropertyChanged("AllClients");
         }
      }

      #endregion

      #region PROPERtY
      //свойства для Client
      public static string ClientName { get; set; }
      public static string ClientSurname { get; set; }
      public static string ClientPhone { get; set; }
      public static string ClientEmail { get; set; }
      public static string ClientCompany { get; set; }
      //свойства для Order
      public static string Date { get; set; }
      public static string OrderProduct { get; set; }
      public static int OrderQuantity { get; set; }
      public static decimal OrderPrice { get; set; }
      public static Client Client { get; set; }

      //свойства для выделенных элементов
      private TabItem selectedTabItem;

      public TabItem SelectedTabItem
      {
         get { return selectedTabItem; }
         set
         {
            selectedTabItem = value;
            OnPropertyChanged(nameof(SelectedTabItem));
         }
      }
      public static Client SelectedClient { get; set; }
      public static Order SelectedOrder { get; set; }

      //свойства для Order Filter 
      public string FromDateFilter { get; set; }
      public string ToDateFilter { get; set; }

      //свойства для Client Filter
      public string NameFilter { get; set; }
      public string SurnameFilter { get; set; }
      #endregion

      #region OPEN WINDOW
      private void OpenAddNewOrderWnd()
      {
         AddNewOrder addNewOrder = new AddNewOrder();
         SetCenterPositionAndOpen(addNewOrder);
      }

      private void OpenAddNewClientWnd()
      {
         AddNewClient addNewClient = new AddNewClient();
         SetCenterPositionAndOpen(addNewClient);
      }

      //окна редактирования
      private void OpenEditClientWindowMethod(Client client)
      {
         EditClient editClientWindow = new EditClient();
         PopulateClientFields();
         SetCenterPositionAndOpen(editClientWindow);
      }

      private void OpenEditOrderWindowMethod(Order order)
      {
         EditOrder editOrderWindow = new EditOrder();
         PopulateOrderFields();
         SetCenterPositionAndOpen(editOrderWindow);
      }

      private void SetCenterPositionAndOpen(Window wnd)
      {
         wnd.Owner = Application.Current.MainWindow;
         wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
         wnd.ShowDialog();
      }

      #endregion

      private async Task SetRedBlockControll(Window wnd, string blockName)
      {
         // Используем Dispatcher для доступа к элементам управления на UI-потоке
         await Application.Current.Dispatcher.InvokeAsync(() =>
         {
            Control block = wnd.FindName(blockName) as Control;

            if (block != null)
            {
               var originalBrush = new SolidColorBrush(Color.FromArgb(255, 112, 112, 112));
               block.BorderBrush = Brushes.Red;
               Task.Delay(300).ContinueWith(t =>
               {
                  // Возвращаем исходный цвет на UI-потоке
                  Application.Current.Dispatcher.Invoke(() =>
                  {
                     block.BorderBrush = originalBrush;
                  });
               });
            }
         });
      }

      #region UPDATE

      private void UpdateAllDataView()
      {
         UpdateAllClientsView();
         UpdateAllOrdersView();
      }
      private void UpdateAllClientsView()
      {
         AllClients = DataWorker.GetAllClients();
         MainWindow.AllClients.ItemsSource = null;
         MainWindow.AllClients.Items.Clear();
         MainWindow.AllClients.ItemsSource = AllClients;
         MainWindow.AllClients.Items.Refresh();
      }
      private void UpdateAllOrdersView()
      {
         AllOrders = DataWorker.GetAllOrders();
         MainWindow.AllOrders.ItemsSource = null;
         MainWindow.AllOrders.Items.Clear();
         MainWindow.AllOrders.ItemsSource = AllOrders;
         MainWindow.AllOrders.Items.Refresh();
      }

      private void SetNullValuesToProperties()
      {
         //для пользователя
         ClientName = null;
         ClientSurname = null;
         ClientPhone = null;
         ClientEmail = null;
         ClientCompany = null;
         //для позиции
         OrderProduct = null;
         OrderQuantity = 0;
         OrderPrice = 0;
         Date = $"01.01.{DateTime.Now.Year}";
      }

      #endregion

      #region UPDATE FILTER
      private void UpdateFilteredClientsView(List<Client> filteredClients)
      {
         // Обновляем привязку данных для отображения клиентов
         MainWindow.AllClients.ItemsSource = null;
         MainWindow.AllClients.Items.Clear();
         MainWindow.AllClients.ItemsSource = filteredClients;
         MainWindow.AllClients.Items.Refresh();
      }
      private void UpdateFilteredOrdersView(List<Order> filteredOrders)
      {
         // Обновляем привязку данных для отображения заказов
         MainWindow.AllOrders.ItemsSource = null;
         MainWindow.AllOrders.Items.Clear();
         MainWindow.AllOrders.ItemsSource = filteredOrders;
         MainWindow.AllOrders.Items.Refresh();
      }
      #endregion


      #region COMMANDS TO OPEN WINDOWS
      private RelayCommand openAddNewClient;
      public RelayCommand OpenAddNewClient
      {
         get
         {
            return openAddNewClient ?? new RelayCommand(obj =>
            {
               OpenAddNewClientWnd();
            });
         }
      }

      private RelayCommand openAddNewOrder;
      public RelayCommand OpenAddNewOrder
      {
         get
         {
            return openAddNewOrder ?? new RelayCommand(obj =>
            {
               OpenAddNewOrderWnd();
            });
         }
      }

      private RelayCommand openEditItemWnd;
      public RelayCommand OpenEditItemWnd
      {
         get
         {
            return openEditItemWnd ?? new RelayCommand(obj =>
            {
               string resultStr = "Ничего не выбрано";
               if (SelectedTabItem.Name == "ClientsTab" && SelectedClient != null)
               {
                  OpenEditClientWindowMethod(SelectedClient);
               }
               if (SelectedTabItem.Name == "OrdersTab" && SelectedOrder != null)
               {
                  OpenEditOrderWindowMethod(SelectedOrder);
               }
            }
                );
         }
      }

      #endregion

      #region ADD COMMANDS 

      private RelayCommand addNewClient;
      public RelayCommand AddNewClient
      {
         get
         {
            return addNewClient ??= new RelayCommand(async obj =>
            {
               Window wnd = obj as Window;
               string resultStr = "";
               bool isValidOK = true;

               // Проверяем, что введены все необходимые данные для клиента
               if (string.IsNullOrWhiteSpace(ClientName))
               {
                  Task.Run(() => SetRedBlockControll(wnd, "NameBlock"));
                  isValidOK = false;
               }
               if (string.IsNullOrWhiteSpace(ClientSurname))
               {
                  Task.Run(() => SetRedBlockControll(wnd, "SurnameBlock"));
                  isValidOK = false;
               }
               if (string.IsNullOrWhiteSpace(ClientPhone) || !IsValidPhone(ClientPhone))
               {
                  Task.Run(() => SetRedBlockControll(wnd, "PhoneBlock"));
                  isValidOK = false;
               }
               if (string.IsNullOrWhiteSpace(ClientEmail) || !IsValidEmail(ClientEmail))
               {
                  Task.Run(() => SetRedBlockControll(wnd, "EmailBlock"));
                  isValidOK = false;
               }
               if (string.IsNullOrWhiteSpace(ClientCompany))
               {
                  SetRedBlockControll(wnd, "CompanyBlock");
                  isValidOK = false;
               }
               if (isValidOK)
               {
                  // Создаем нового клиента и сохраняем его в БД
                  var newClient = new Client
                  {
                     Name = ClientName,
                     Surname = ClientSurname,
                     Phone = ClientPhone,
                     Email = ClientEmail,
                     Company = ClientCompany
                  };

                  resultStr = DataWorker.CreateClient(newClient);
                  UpdateAllDataView();
                  MessageBox.Show(resultStr);
                  SetNullValuesToProperties();
                  wnd.Close();
               }
            });
         }
      }

      private RelayCommand addNewOrder;
      public RelayCommand AddNewOrder
      {
         get
         {
            return addNewOrder ??= new RelayCommand(async obj =>
            {
               Window wnd = obj as Window;
               Client client = new Client();
               string resultStr = "";
               bool isValidOK = true;
               // Проверяем, что введены все необходимые данные для заказа
               if (string.IsNullOrWhiteSpace(OrderProduct))
               {
                  Task.Run(() => SetRedBlockControll(wnd, "ProductBlock"));
                  isValidOK = false;
               }
               if (string.IsNullOrWhiteSpace(Date) || !IsValidDate(Date))
               {
                  Task.Run(() => SetRedBlockControll(wnd, "DateBlock"));
                  isValidOK = false;
               }
               if (OrderQuantity <= 0)
               {
                  Task.Run(() => SetRedBlockControll(wnd, "QuantityBlock"));
                  isValidOK = false;
               }
               if (OrderPrice <= 0)
               {
                  Task.Run(() => SetRedBlockControll(wnd, "PriceBlock"));
                  isValidOK = false;
               }
               if (Client == null)
               {
                  MessageBox.Show("Укажите клиента");
                  isValidOK = false;
               }
               if (isValidOK)
               {
                  // Создаем новый заказ и сохраняем его в БД
                  var newOrder = new Order
                  {
                     ClientId = Client.Id,
                     Product = OrderProduct,
                     Quantity = OrderQuantity,
                     Price = OrderPrice,
                     OrderDate = Date
                  };

                  resultStr = DataWorker.CreateOrder(newOrder);
                  UpdateAllDataView();
                  MessageBox.Show(resultStr);
                  SetNullValuesToProperties();
                  wnd.Close();
               }
            });
         }
      }
      #endregion

      #region EDIT COMMANDS

      private RelayCommand editClient;
      public RelayCommand EditClient
      {
         get
         {
            return editClient ?? new RelayCommand(obj =>
            {
               Window window = obj as Window;
               string resultStr = "Не выбран клиент";
               if (SelectedClient != null)
               {
                  resultStr = DataWorker.EditClient(SelectedClient, ClientName, ClientSurname, ClientPhone, ClientEmail, ClientCompany);
                  UpdateAllDataView();
                  SetNullValuesToProperties();
                  MessageBox.Show(resultStr);
                  window.Close();
               }
               else MessageBox.Show(resultStr);
            }
            );
         }
      }

      private RelayCommand editOrder;
      public RelayCommand EditOrder
      {
         get
         {
            return editOrder ?? new RelayCommand(obj =>
            {
               Window window = obj as Window;
               string resultStr = "Не выбран заказ";
               if (SelectedOrder != null && Client != null)
               {
                  resultStr = DataWorker.EditOrder(SelectedOrder, OrderProduct, OrderQuantity, OrderPrice, Date);
                  UpdateAllDataView();
                  SetNullValuesToProperties();
                  MessageBox.Show(resultStr);
                  window.Close();
               }
               else MessageBox.Show(resultStr);
            }
            );
         }
      }
      #endregion

      #region DELETE COMMANDS
      private RelayCommand deleteItem;
      public RelayCommand DeleteItem
      {
         get
         {
            return deleteItem ?? new RelayCommand(obj =>
            {
               string resultStr = "Ничего не выбрано";
               if (SelectedTabItem.Name == "ClientsTab" && SelectedClient != null)
               {
                  resultStr = DataWorker.DeleteClient(SelectedClient);
                  UpdateAllDataView();
               }
               if (SelectedTabItem.Name == "OrdersTab" && SelectedOrder != null)
               {
                  resultStr = DataWorker.DeleteOrder(SelectedOrder);
                  UpdateAllDataView();
               }
               //обновление
               SetNullValuesToProperties();
               MessageBox.Show(resultStr);
            }
                );
         }
      } 
      #endregion

      #region REGEX
      // Проверка телефона в формате #######
      public bool IsValidPhone(string phone)
      {
         if (string.IsNullOrEmpty(phone))
            return false;

         // Регулярное выражение для формата #######
         string phonePattern = @"^[+ ]?\d*(?:\s?\d){7,}$";
         Regex regex = new Regex(phonePattern);
         return regex.IsMatch(phone);
      }

      // Проверка Email
      public bool IsValidEmail(string email)
      {
         if (string.IsNullOrEmpty(email))
            return false;

         // Пример регулярного выражения для проверки email
         string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
         Regex regex = new Regex(emailPattern);
         return regex.IsMatch(email);
      }

      // Проверка даты в формате ##.##.####
      public bool IsValidDate(string date)
      {
         if (string.IsNullOrEmpty(date))
            return false;

         // Регулярное выражение для формата даты ##.##.####
         string datePattern = @"^\d{2}\.\d{2}\.\d{4}$";
         Regex regex = new Regex(datePattern);
         return regex.IsMatch(date);
      }
      #endregion

      #region POPULATE

      private void PopulateOrderFields()
      {
         Date = SelectedOrder.OrderDate;
         OrderProduct = SelectedOrder.Product;
         OrderQuantity = SelectedOrder.Quantity;
         OrderPrice = SelectedOrder.Price;
      }

      private void PopulateClientFields()
      {
         ClientName = SelectedClient.Name;
         ClientSurname = SelectedClient.Surname;
         ClientPhone = SelectedClient.Phone;
         ClientEmail = SelectedClient.Email;
         ClientCompany = SelectedClient.Company;
      }
      #endregion

      #region VISIBLE
      private Visibility _isElementVisibleDate;

      public Visibility IsElementVisibleDate
      {
         get { return _isElementVisibleDate; }
         set { _isElementVisibleDate = value; OnPropertyChanged(nameof(IsElementVisibleDate)); }
      }

      private Visibility _isElementVisibleClient;

      public Visibility IsElementVisibleClient
      {
         get { return _isElementVisibleClient; }
         set { _isElementVisibleClient = value; OnPropertyChanged(nameof(IsElementVisibleClient)); }
      }

      private int _selectedFilterIndex;
      public int SelectedFilterIndex
      {
         get => _selectedFilterIndex;
         set
         {
            _selectedFilterIndex = value;
            OnPropertyChanged(nameof(SelectedFilterIndex));
            UpdateFilterVisibility();
         }
      }

      private void UpdateFilterVisibility()
      {
         if (SelectedFilterIndex == 0) // По дате
         {
            IsElementVisibleDate = Visibility.Visible;
            IsElementVisibleClient = Visibility.Collapsed;
         }
         else if (SelectedFilterIndex == 1) // По клиенту
         {
            IsElementVisibleDate = Visibility.Collapsed;
            IsElementVisibleClient = Visibility.Visible;
         }
      }
      #endregion

      #region FILTER
      private RelayCommand resetGridView;
      public RelayCommand ResetGridView
      {
         get
         {
            return resetGridView ?? new RelayCommand(obj =>
            {
               UpdateAllDataView();
            });
         }
      }

      private RelayCommand filterByDateCommand;
      public RelayCommand FilterByDateCommand
      {
         get
         {
            return filterByDateCommand ??= new RelayCommand(obj =>
            {
               Window wnd = obj as Window;
               bool isValidOK = true;
               if (string.IsNullOrWhiteSpace(FromDateFilter) || !IsValidDate(FromDateFilter))
               {
                  Task.Run(() => SetRedBlockControll(wnd, "FromDateFilterBlock"));
                  isValidOK = false;
               }
               // Проверка валидности ToDateFilter
               if (string.IsNullOrWhiteSpace(ToDateFilter))
               {
                  // Если ToDateFilter пустой, использовать FromDateFilter
                  ToDateFilter = FromDateFilter;
               }
               else if (!IsValidDate(ToDateFilter))
               {
                  Task.Run(() => SetRedBlockControll(wnd, "ToDateFilterBlock"));
                  isValidOK = false;
               }

               if (isValidOK)
               {
                  // Передача скорректированных значений в метод фильтрации
                  var filteredOrders = Filter.GetFilteredOrdersByDate(FromDateFilter, ToDateFilter);
                  UpdateFilteredOrdersView(filteredOrders);
               }
            });
         }
      }


      private RelayCommand filterByClientCommand;
      public RelayCommand FilterByClientCommand
      {
         get
         {
            return filterByClientCommand ??= new RelayCommand(obj =>
            {
               Window wnd = obj as Window;
               bool isValidOK = true;
               if (string.IsNullOrWhiteSpace(NameFilter))
               {
                  Task.Run(() => SetRedBlockControll(wnd, "NameFilterBlock"));
                  isValidOK = false;

               }
               if (string.IsNullOrWhiteSpace(SurnameFilter))
               {
                  Task.Run(() => SetRedBlockControll(wnd, "SurnameFilterBlock"));
                  isValidOK = false;
               }
               if (isValidOK)
                  {
                     var filteredClients = Filter.GetFilteredClientsByFullName(NameFilter, SurnameFilter);
                     UpdateFilteredClientsView(filteredClients);
                  }
            });

         }
      }

      #endregion


      public event PropertyChangedEventHandler PropertyChanged;
      public void OnPropertyChanged([CallerMemberName] string prop = "")
      {
         if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
      }
   }
}

