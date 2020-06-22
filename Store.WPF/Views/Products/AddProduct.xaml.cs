using log4net.Repository.Hierarchy;
using Store.Data;
using Store.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private readonly StoreDBContext dbContext;
        private static readonly Regex digitsOnly = new Regex("[^0-9.-]+");
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool IsTextAllowed(string text)
        {
            return !digitsOnly.IsMatch(text);
        }
        private void PreviewTextInputHandler(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        public AddProduct()
        {

        }
        public AddProduct(StoreDBContext dBContext)
        {
            this.dbContext = dBContext;
            InitializeComponent();
            TypeBox.ItemsSource = dBContext.ProductTypes.Select(x => x.Name).ToList();
            TypeBox.SelectedItem = TypeBox.Items[0];
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string measure = null;
            try
            {
                var type = dbContext.ProductTypes.FirstOrDefault(pt => pt.Name == TypeBox.SelectedItem.ToString());
                if (Count.IsChecked == true)
                {
                    measure = Count.Content.ToString();
                }
                else
                {
                    measure = Kilograms.Content.ToString();
                }
                var product = new Product()
                {
                    Barcode = BarcodeTxt.Text,
                    DeliveryPrice = decimal.Parse(DeliveryPriceTxt.Text),
                    Measure = measure,
                    Name = ProductNameTxt.Text,
                    Price = decimal.Parse(PriceTxt.Text),
                    ProductType = type,
                    Quantity = decimal.Parse(QuantityTxt.Text),
                    DateCreated = DateTime.Now,
                };
                dbContext.Add(product);
                dbContext.SaveChanges();
                log.Info($"Добавен е продукт { ProductNameTxt.Text.ToString()}");

                MessageBox.Show($"Добавен е продукт {ProductNameTxt.Text.ToString()}", "Информация");
            }
            catch (Exception ex)
            {
                log.Error($"Грешка {ex.Message}");
                MessageBox.Show("Моля опитайте отново", "Информация");
            }
            finally
            {
                BarcodeTxt.Text = "";
                DeliveryPriceTxt.Text = "";
                PriceTxt.Text = "";
                ProductNameTxt.Text = "";
                QuantityTxt.Text = "";
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            BarcodeTxt.Text = "";
            DeliveryPriceTxt.Text = "";
            PriceTxt.Text = "";
            ProductNameTxt.Text = "";
            QuantityTxt.Text = "";
        }
    }
}
