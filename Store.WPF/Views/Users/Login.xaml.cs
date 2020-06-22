using Store.Data;
using Store.WPF.ViewModels;
using System;
using System.Collections.Generic;
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

namespace Store.WPF.Views.Users
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly StoreDBContext dbContext;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Login()
        {

        }
        public Login(StoreDBContext storeDBContext)
        {
            this.dbContext = storeDBContext;
            InitializeComponent();
        }

        private void BtnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            UserInfo userInfo = new UserInfo();

            var user = dbContext.Users.FirstOrDefault(u => u.Name == txtUserName.Text.ToString());
            try
            {
                if (user == null)
                {
                    throw new Exception();
                }
                else if (user.Password == txtPassword.Password.ToString())
                {
                    var role = dbContext.Roles.FirstOrDefault(r => r.Id == user.RoleId);
                    userInfo = new UserInfo() { Name = user.Name, Role = role };
                    log.Info("Потребител с име " + user.Name + " влезе в системата");
                    MainWindow mainWindow = new MainWindow(userInfo, dbContext);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show("Грешен потребител", "Информация");
            }

        }

        private void BtnClose_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
