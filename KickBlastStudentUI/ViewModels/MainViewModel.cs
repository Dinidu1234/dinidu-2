using System.Windows.Threading;
using KickBlastStudentUI.Helpers;
using KickBlastStudentUI.Services;
using KickBlastStudentUI.Views;

namespace KickBlastStudentUI.ViewModels;

public class MainViewModel : ObservableObject, IDisposable
{
    private readonly DispatcherTimer _timer;
    private readonly ToastService _toastService;
    private object _currentView;
    private string _currentPageTitle = "Dashboard";
    private string _currentDateText = DateTime.Now.ToString("dddd, dd MMM yyyy");

    public MainViewModel(PricingService pricingService)
    {
        PricingService = pricingService;
        _toastService = new ToastService();
        _toastService.PropertyChanged += (_, _) => OnPropertyChanged(nameof(ToastMessage));

        ShowDashboard();

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(1) };
        _timer.Tick += (_, _) => CurrentDateText = DateTime.Now.ToString("dddd, dd MMM yyyy");
        _timer.Start();
    }

    public PricingService PricingService { get; }

    public object CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public string CurrentPageTitle
    {
        get => _currentPageTitle;
        set => SetProperty(ref _currentPageTitle, value);
    }

    public string CurrentDateText
    {
        get => _currentDateText;
        set => SetProperty(ref _currentDateText, value);
    }

    public string ToastMessage => _toastService.Message;

    public void ShowDashboard()
    {
        CurrentPageTitle = "Dashboard";
        CurrentView = new DashboardView { DataContext = new DashboardViewModel() };
    }

    public void ShowAthletes()
    {
        CurrentPageTitle = "Athletes";
        var vm = new AthletesViewModel();
        vm.Toast += ShowToast;
        CurrentView = new AthletesView { DataContext = vm };
    }

    public void ShowCalculator()
    {
        CurrentPageTitle = "Calculator";
        var vm = new CalculatorViewModel(PricingService);
        vm.Toast += ShowToast;
        CurrentView = new CalculatorView { DataContext = vm };
    }

    public void ShowHistory()
    {
        CurrentPageTitle = "History";
        CurrentView = new HistoryView { DataContext = new HistoryViewModel() };
    }

    public void ShowSettings()
    {
        CurrentPageTitle = "Settings";
        var vm = new SettingsViewModel(PricingService);
        vm.Toast += ShowToast;
        CurrentView = new SettingsView { DataContext = vm };
    }

    private void ShowToast(string message, bool isError)
    {
        if (isError) _toastService.Error(message);
        else _toastService.Success(message);
    }

    public void Dispose() => _timer.Stop();
}
