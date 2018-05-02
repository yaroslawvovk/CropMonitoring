using CropMonitoring.Downloaders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CropMonitoring.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();

            //Emp emp = new Emp() {Id=1,Name="John", Job = "Xuy" };

            //DataGridXaml.Items.Add(emp);
            //DataGridXaml.Items.Add(emp);
            //DataGridXaml.Items.Add(emp);
            //DataGridXaml.Items.Add(emp);
            //DataGridXaml.Items.Add(emp);
            //DataGridXaml.Items.Add(emp);
            //DataGridXaml.Items.Add(emp);
            //DataGridXaml.Items.Add(emp);
            //DataGridXaml.Items.Add(emp);
            //DataGridXaml.Items.Add(emp);

            DataGridXaml.ItemsSource = Customer.GetCustomerList();
            

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Loader load = new VHIDataLoader(ProgressBar);
            await load.DownloadAndSaveData("Piska3", "11");
        }
    }

    class Emp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
    }
    public enum OrderStatus
    {
        InProgress, Delivered
    }

    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsMember { get; set; }
        public OrderStatus Status { get; set; }

        public static ObservableCollection<Customer> GetCustomerList()
        {
            ObservableCollection<Customer> collection = new ObservableCollection<Customer>();
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            collection.Add(new Customer() { FirstName = "Jhon", LastName = "Doe", Email = "jhon.doe@mail.com", IsMember = true, Status = OrderStatus.InProgress });
            return collection;
        }
    }


}
