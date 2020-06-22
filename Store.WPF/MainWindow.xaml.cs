

namespace Store.WPF
{
    using Store.Data;
    using Store.WPF.ViewModels;
    using Store.WPF.Views;
    using Store.WPF.Views.Checkouts;
    using Store.WPF.Views.Payments;
    using Store.WPF.Views.Products;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StoreDBContext dBContext;
       
        public MainWindow(UserInfo userInfo,StoreDBContext dBContext)
        {
            this.dBContext = dBContext;
            InitializeComponent();

            if (userInfo.Role.Id == 2)
            {
                AddProductButton.Visibility = Visibility.Collapsed;
                PaymentsButton.Visibility = Visibility.Collapsed;
                ReportsButton.Visibility = Visibility.Collapsed;
                ProductsButton.Visibility = Visibility.Collapsed;
                CheckoutButton.Margin = new Thickness(30, 32, 190, 50);
                ExitButton.Margin = new Thickness(193, 32, 23, 50);
                Application.Current.MainWindow = this;
                Application.Current.MainWindow.Height = 120;
            }
        }
        public MainWindow(StoreDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PaymentsButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new PaymentsWindow(dBContext).ShowDialog();
            ShowDialog();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new AddProduct(dBContext).ShowDialog();
            ShowDialog();
        }

        private void PoductsButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new Products(dBContext).ShowDialog();
            ShowDialog();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new Checkout(dBContext).ShowDialog();
            ShowDialog();
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
         
        }
    }
}
