using Store.Data;
using Store.Entities;
using Store.WPF.Utils;
using Store.WPF.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Store.WPF.Views.Bills
{
    /// <summary>
    /// Interaction logic for Bill.xaml
    /// </summary>
    public partial class BillWindow : Window
    {
        private readonly StoreDBContext dBContext;
        private Sale sale;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ObservableCollection<CheckoutProductViewModel> ProductsList { get; set; } = new ObservableCollection<CheckoutProductViewModel>();
        public BillWindow()
        {
            InitializeComponent();
        }
        public BillWindow(ObservableCollection<CheckoutProductViewModel> ProductsList, StoreDBContext dBContext, Sale sale)
        {
            this.sale = sale;
            this.ProductsList = ProductsList;
            this.dBContext = dBContext;
            InitializeComponent();
            ProductsListGrid.ItemsSource = this.ProductsList;
        }

        private void BilBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var priceWithoutDDS = sale.TotalPrice - (sale.TotalPrice / 6);
                var bill = new Bill()
                {
                    Number = RandomNumber.BillNumber(dBContext),
                    SaleId = sale.Id,
                    Bulstat = this.BulstadTxt.Text,
                    PriceWithDDS = sale.TotalPrice,
                    PriceWithoutDDS = priceWithoutDDS
                };
                dBContext.Bills.Add(bill);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            finally
            {
                dBContext.SaveChanges();
                //PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
                //if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
                //{
                //    Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                //    // sizing of the element.
                //    ProductsListGrid.Measure(pageSize);
                //    ProductsListGrid.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                //    Printdlg.PrintVisual(ProductsListGrid, Title);
                //}
                //var pd = new PrintDialog();
                //var result = pd.ShowDialog();
                //if (result.HasValue && result.Value)
                //    pd.PrintVisual(this, "My WPF printing a DataGrid");
                this.Close();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
