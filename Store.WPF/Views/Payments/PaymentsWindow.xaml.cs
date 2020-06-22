namespace Store.WPF.Views.Payments
{
    using Store.Data;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    /// <summary>
    /// Interaction logic for Payments.xaml
    /// </summary>
    public partial class PaymentsWindow : Window
    {
        private readonly StoreDBContext dBContext;
        public PaymentsWindow()
        {

        }
        public PaymentsWindow(StoreDBContext dBContext)
        {
            this.dBContext = dBContext;
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BillsButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new Bills(dBContext).ShowDialog();
            ShowDialog();
        }

        private void NotPaidButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
