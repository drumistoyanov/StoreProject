
namespace Store.WPF
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Store.Data;
    using Store.WPF.Views;
    using Store.WPF.Views.Users;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Xml;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
        }
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            XmlDocument log4netConfig = new XmlDocument();
            var currentDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory.ToString()).ToString()).ToString());
            log4netConfig.Load(File.OpenRead(currentDirectory.ToString()+"\\log4net.config"));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                       typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.GlobalContext.Properties["LogFileName"] = currentDirectory.ToString()+"\\Files\\storeLogger.log"; //log file path
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var login = ServiceProvider.GetRequiredService<Login>();
            login.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // ...
            services.AddDbContext<StoreDBContext>(options =>
            options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddTransient(typeof(Login));
        }
    }
}
