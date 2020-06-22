using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Store.WPF.Views.Payments
{
    /// <summary>
    /// Interaction logic for Bills.xaml
    /// </summary>
    public partial class Bills : Window
    {
        private readonly StoreDBContext dBContext;
        public ObservableCollection<BillsViewModel> BillsList { get; set; } = new ObservableCollection<BillsViewModel>();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Bills()
        {

        }
        public Bills(StoreDBContext dBContext)
        {
            this.dBContext = dBContext;
            InitializeComponent();

            GetBills();

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            billsGrid.ItemsSource = BillsList;
        }
        private void GetBills()
        {
            try
            {
                BillsList.Clear();
                var bills = dBContext.Bills.Where(p => p.IsDeleted == false).AsNoTracking().ToList();
                foreach (var item in bills)
                {                    
                    var viewModel = new BillsViewModel()
                    {
                       Bulstat=item.Bulstat,
                       Number=item.Number,
                       PriceWithDDS=item.PriceWithDDS,
                       PriceWithoutDDS=item.PriceWithoutDDS
                    };
                    BillsList.Add(viewModel);
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
