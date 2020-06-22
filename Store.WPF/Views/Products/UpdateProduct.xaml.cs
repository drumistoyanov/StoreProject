using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.WPF.ViewModels;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Store.WPF.Views.Products
{
    /// <summary>
    /// Interaction logic for UpdateProduct.xaml
    /// </summary>
    public partial class UpdateProduct : Window
    {
        private readonly StoreDBContext dBContext = new StoreDBContext();
        private readonly Product product;
        private static readonly Regex digitOnly = new Regex("[^0-9.-]+");
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool IsTextAllowed(string text)
        {
            return !digitOnly.IsMatch(text);
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
        public UpdateProduct()
        {
            InitializeComponent();
        }
        public UpdateProduct(ProductViewModel product)
        {
            try
            {
                this.product = dBContext.Products.AsNoTracking().FirstOrDefault(p => p.Id == product.Id);
                InitializeComponent();
                if (product.Measure == "Кг.")
                {
                    Kilograms.IsChecked = true;
                }

                BarcodeTxt.Text = product.Barcode;
                DeliveryPriceTxt.Text = product.DeliveryPrice.ToString();
                ProductNameTxt.Text = product.Name;
                PriceTxt.Text = product.Price.ToString();
                QuantityTxt.Text = product.Quantity.ToString();
                TypeBox.ItemsSource = dBContext.ProductTypes.Select(x => x.Name).ToList();
                TypeBox.SelectedItem = dBContext.ProductTypes.FirstOrDefault(x => x.Id == this.product.ProductTypeId).Name;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            BarcodeTxt.Text = product.Barcode;
            DeliveryPriceTxt.Text = product.DeliveryPrice.ToString();
            ProductNameTxt.Text = product.Name;
            PriceTxt.Text = product.Price.ToString();
            QuantityTxt.Text = product.Quantity.ToString();
            TypeBox.ItemsSource = dBContext.ProductTypes.Select(x => x.Name).ToList();
            TypeBox.SelectedItem = dBContext.ProductTypes.FirstOrDefault(x => x.Id == this.product.ProductTypeId).Name;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string measure = null;
            String message = "Грешни данни";
            try
            {
                if (Count.IsChecked == true)
                {
                    measure = Count.Content.ToString();
                }
                else
                {
                    measure = Kilograms.Content.ToString();
                }
                var type = dBContext.ProductTypes.FirstOrDefault(pt => pt.Name == TypeBox.SelectedItem.ToString());
                product.Barcode = BarcodeTxt.Text;
                product.DeliveryPrice = decimal.Parse(DeliveryPriceTxt.Text);
                product.Measure = measure;
                product.Name = ProductNameTxt.Text;
                product.Price = decimal.Parse(PriceTxt.Text);
                product.ProductType = type;
                product.Quantity = decimal.Parse(QuantityTxt.Text);
                product.DateModified = DateTime.Now;
                var productToChange = dBContext.Products.AsNoTracking().FirstOrDefault(p => p.Id == product.Id);
                productToChange = product;
                dBContext.Update(productToChange);
                dBContext.SaveChanges();

                log.Info($"Product {this.product.Name} is updated.");


                MessageBox.Show($"Продуктът с име {ProductNameTxt.Text.ToString()} е променен", "Информация");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                message = ex.Message.ToString();
                MessageBox.Show(message, "Информация");
            }
            finally
            {
                this.Close();
            }

        }

    }
}
