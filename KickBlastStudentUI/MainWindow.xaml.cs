using System.Windows;
using System.Windows.Controls;
using KickBlastStudentUI.Services;
using KickBlastStudentUI.ViewModels;

namespace KickBlastStudentUI;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow(PricingService pricingService)
    {
        InitializeComponent();
        _viewModel = new MainViewModel(pricingService);
        DataContext = _viewModel;
        SetActiveButton(DashboardButton);
        Closed += (_, _) => _viewModel.Dispose();
    }

    private void DashboardButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.ShowDashboard();
        SetActiveButton(DashboardButton);
    }

    private void AthletesButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.ShowAthletes();
        SetActiveButton(AthletesButton);
    }

    private void CalculatorButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.ShowCalculator();
        SetActiveButton(CalculatorButton);
    }

    private void HistoryButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.ShowHistory();
        SetActiveButton(HistoryButton);
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.ShowSettings();
        SetActiveButton(SettingsButton);
    }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        var login = new Views.LoginWindow(_viewModel.PricingService);
        login.Show();
        Close();
    }

    private void SetActiveButton(Button active)
    {
        DashboardButton.Tag = "Inactive";
        AthletesButton.Tag = "Inactive";
        CalculatorButton.Tag = "Inactive";
        HistoryButton.Tag = "Inactive";
        SettingsButton.Tag = "Inactive";
        active.Tag = "Active";
    }
}
