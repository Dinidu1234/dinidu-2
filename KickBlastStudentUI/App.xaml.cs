using System.Windows;
using KickBlastStudentUI.Data;
using KickBlastStudentUI.Services;
using KickBlastStudentUI.Views;

namespace KickBlastStudentUI;

public partial class App : Application
{
    private PricingService? _pricingService;

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        _pricingService = new PricingService();
        using var db = new AppDbContext();
        DbInitializer.Initialize(db, _pricingService);

        var login = new LoginWindow(_pricingService);
        login.Show();
    }
}
