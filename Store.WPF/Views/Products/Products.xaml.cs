using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Store.WPF.Views.Products
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : Window
    {
        private readonly StoreDBContext dBContext;
        public ObservableCollection<ProductViewModel> ProductsList { get; set; } = new ObservableCollection<ProductViewModel>();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Products()
        {

        }
        public Products(StoreDBContext dBContext)
        {
            this.dBContext = dBContext;
            InitializeComponent();

            GetProducts();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            productsGrid.ItemsSource = ProductsList;
        }
        private void GetProducts()
        {
            try
            {
                ProductsList.Clear();
                var products = dBContext.Products.Where(p => p.IsDeleted == false).AsNoTracking().ToList();
                foreach (var item in products)
                {
                    var type = dBContext.ProductTypes.FirstOrDefault(x => x.Id == item.ProductTypeId);
                    var viewModel = new ProductViewModel()
                    {
                        Barcode = item.Barcode,
                        DeliveryPrice = item.DeliveryPrice,
                        Measure = item.Measure,
                        Name = item.Name,
                        Price = item.Price,
                        ProductTypeName = type.Name,
                        Quantity = item.Quantity,
                        Id = item.Id
                    };
                    ProductsList.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }
        private void Bnt_Insert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Hide();
                new AddProduct(dBContext).ShowDialog();
                GetProducts();
                productsGrid.ItemsSource = ProductsList;
                ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void Bnt_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedProduct = (ProductViewModel)productsGrid.SelectedItem;
                if (selectedProduct != null)
                {
                    var productToDelete = dBContext.Products.FirstOrDefault(p => p.Id == selectedProduct.Id);
                    productToDelete.DateModified = DateTime.Now;
                    productToDelete.IsDeleted = true;
                    dBContext.SaveChanges();
                    GetProducts();
                    productsGrid.ItemsSource = ProductsList;
                    log.Info($"Изтрит е продукт с ID: { productToDelete.Id.ToString()}");
                }
                else
                {
                    MessageBox.Show("Изберете продукт", "Информация");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }

        private void Bnt_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedProduct = productsGrid.SelectedItem;
                if (selectedProduct != null)
                {
                    Hide();
                    UpdateProduct dialogBox = new UpdateProduct((ProductViewModel)selectedProduct);

                    dialogBox.ShowDialog();
                    GetProducts();
                    productsGrid.ItemsSource = ProductsList;
                    ShowDialog();

                }
                else
                {
                    MessageBox.Show("Изберете продукт", "Информация");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }

        private void Bnt_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
