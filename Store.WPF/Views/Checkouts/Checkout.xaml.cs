using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.WPF.Utils;
using Store.WPF.ViewModels;
using Store.WPF.Views.Bills;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Store.WPF.Views.Checkouts
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : Window
    {
        private Sale sale;
        private readonly StoreDBContext dBContext;
        private decimal totalPrice;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ObservableCollection<CheckoutProductViewModel> ProductsList { get; set; } = new ObservableCollection<CheckoutProductViewModel>();
        public ObservableCollection<CheckoutProductViewModel> ProductsForBill { get; set; } = new ObservableCollection<CheckoutProductViewModel>();
        public List<Product> Products { get; set; } = new List<Product>();
        public Checkout()
        {
        }
        public Checkout(StoreDBContext dBContext)
        {
            InitializeComponent();
            this.dBContext = dBContext;
        }


        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckoutProductViewModel viewModel;
                var textBoxContent = CodeAndQuantityTxt.Text;
                //Добавяне на продукт
                if (Regex.Match(textBoxContent, @"^\d+\*{1}\d+$").Success && textBoxContent != "*+")
                {
                    var splittedText = textBoxContent.Split('*').ToList();
                    bool n = false;
                    foreach (var text in splittedText)
                    {
                        if (String.IsNullOrEmpty(text))
                        {
                            n = true;
                        }
                    }
                    if (!n)
                    {
                        var itemIdOrBarcode = 0;
                        int length = (int)(Math.Log10(int.MaxValue) + 1);

                        if (splittedText[1].Length<length)
                        {
                           itemIdOrBarcode = int.Parse(splittedText[1]);
                        }
                      
                        var item = dBContext.Products.AsNoTracking().FirstOrDefault(p => p.Id == itemIdOrBarcode || p.Barcode == splittedText[1]);
                        if (item == null || item.IsDeleted)
                        {
                            SystemSounds.Hand.Play();
                            MessageBox.Show($"Няма такъв продукт с id/barcode {itemIdOrBarcode}!");
                            throw new Exception($"Няма такъв продукт с id/barcode {itemIdOrBarcode}!");
                        }
                        var itemQuantity = decimal.Parse(splittedText[0]);
                        totalPrice = item.Price * itemQuantity;
                        viewModel = new CheckoutProductViewModel()
                        {
                            Barcode = item.Barcode,
                            Id = item.Id,
                            Measure = item.Measure,
                            Name = item.Name,
                            Price = item.Price,
                            Quantity = itemQuantity,
                            TotalPrice = totalPrice
                        };

                        Products.Add(item);
                        ProductsList.Add(viewModel);
                        ProductsListGrid.ItemsSource = ProductsList;

                        totalPrice = ProductsList.Sum(x => x.TotalPrice);

                        TotalPriceTxt.Text = totalPrice.ToString();
                        log.Info($"Product {viewModel.Name} with quantity {viewModel.Quantity} added to the bill.");
                    }
                    else
                    {
                        SystemSounds.Hand.Play();
                    }
                }
                //Изтриване на последен добавен продукт
                else if (textBoxContent == "--")
                {
                    var newEventArgs = new RoutedEventArgs(Button.ClickEvent);
                    ClearLastBtn.RaiseEvent(newEventArgs);
                }
                //Завършване на продажба
                else if (textBoxContent == "*+")
                {
                    var newEventArgs = new RoutedEventArgs(Button.ClickEvent);
                    FinishBtn.RaiseEvent(newEventArgs);
                }
                //Дневен отчет
                else if (textBoxContent == "/////")
                {
                    DailyFile.SaveDailyFile();
                }
                else if (textBoxContent == "*+*")
                {
                    var newEventArgs = new RoutedEventArgs(Button.ClickEvent);
                    BillBtn.RaiseEvent(newEventArgs);
                }
                else
                {
                    SystemSounds.Hand.Play();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException);
            }
            finally
            {
                CodeAndQuantityTxt.Text = string.Empty;
            }
        }

        private void FinishBtn_Click(object sender, RoutedEventArgs e)
        {
            sale = new Sale();
            try
            {
                sale = new Sale()
                {
                    DateCreated = DateTime.Now,
                    TotalPrice = totalPrice
                };

                dBContext.Sales.Add(sale);
                dBContext.SaveChanges();
                sale = dBContext.Sales.ToList().Last();
                var salesProducts = new List<SaleProduct>();
                for (int i = 0; i < Products.Count; i++)
                {
                    var product = dBContext.Products.FirstOrDefault(x => x.Id == Products[i].Id);
                    if (salesProducts.Count == 0)
                    {

                        if (product.Quantity >= ProductsList[i].Quantity)
                        {

                            var saleProduct = new SaleProduct()
                            {
                                ProductId = Products[i].Id,
                                SaleId = sale.Id,
                                Quantity = ProductsList[i].Quantity
                            };
                            salesProducts.Add(saleProduct);
                        }
                        else
                        {
                            MessageBox.Show($"Няма такова количество от {product.Name} налично!");
                            throw new Exception($"Няма такова количество от {product.Name} налично!");
                        }
                    }
                    else
                    {
                        var isItemAdded = salesProducts.Any(sp => sp.ProductId == Products[i].Id);


                        if (isItemAdded)
                        {
                            var saleProduct = salesProducts.First(sp => sp.ProductId == Products[i].Id);
                            var productQuantity = saleProduct.Quantity + ProductsList[i].Quantity;
                            if (product.Quantity >= productQuantity)
                            {
                                salesProducts.Remove(saleProduct);
                                saleProduct.Quantity += ProductsList[i].Quantity;
                                salesProducts.Add(saleProduct);
                            }
                            else
                            {
                                MessageBox.Show($"Няма такова количество от {product.Name} налично!");
                                throw new Exception($"Няма такова количество от {product.Name} налично!");
                            }

                        }
                        else
                        {
                            if (product.Quantity >= ProductsList[i].Quantity)
                            {

                                var saleProduct = new SaleProduct()
                                {
                                    ProductId = Products[i].Id,
                                    SaleId = sale.Id,
                                    Quantity = ProductsList[i].Quantity
                                };
                                salesProducts.Add(saleProduct);
                            }
                            else
                            {
                                MessageBox.Show($"Няма такова количество от {product.Name} налично!");
                                throw new Exception($"Няма такова количество от {product.Name} налично!");
                            }

                        }

                    }

                }
                foreach (var item in salesProducts)
                {
                    var product = dBContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
                    product.Quantity -= item.Quantity;
                    dBContext.Update(product);
                }
                dBContext.SaleProducts.AddRange(salesProducts);

                this.TotalPriceTxt.Text = "0.00";
            }
            catch (Exception ex)
            {
                SystemSounds.Hand.Play();
                var newSale = dBContext.Sales.First(s => s.Id == sale.Id);
                newSale.IsDeleted = true;
                newSale.DateDeleted = DateTime.Now;
                dBContext.Update(newSale);
                dBContext.SaveChanges();
                log.Error(ex.Message + ex.InnerException);
            }
            finally
            {
                dBContext.SaveChanges();
                ProductsForBill=new ObservableCollection<CheckoutProductViewModel>();
                foreach (var item in ProductsList)
                {
                    ProductsForBill.Add(item);
                }
                Products.Clear();
                ProductsList.Clear();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClearLastBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProductsList.Count() != 0)
                {
                    ProductsList.Remove(ProductsList.Last());
                }
                var total = ProductsList.Sum(x => x.TotalPrice);
                TotalPriceTxt.Text = total.ToString();
                log.Info($"Product {ProductsList.Last().Name} removed.");
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException);
            }
        }

        private void BillBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newEventArgs = new RoutedEventArgs(Button.ClickEvent);
                FinishBtn.RaiseEvent(newEventArgs);
                Hide();
                new BillWindow(ProductsForBill, dBContext, sale).ShowDialog();
                ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException);
            }
        }
    }
}
