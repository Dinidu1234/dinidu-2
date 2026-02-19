using System.Windows;
using KickBlastStudentUI.Services;
using KickBlastStudentUI.ViewModels;

namespace KickBlastStudentUI.Views;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _viewModel;
    private readonly PricingService _pricingService;

    public LoginWindow(PricingService pricingService)
    {
        InitializeComponent();
        _pricingService = pricingService;
        _viewModel = new LoginViewModel(new AuthService());
        _viewModel.LoginSucceeded += LoginSucceeded;
        DataContext = _viewModel;
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        _viewModel.Password = PasswordBox.Password;
    }

    private void LoginSucceeded()
    {
        var main = new MainWindow(_pricingService);
        main.Show();
        Close();
    }
}
