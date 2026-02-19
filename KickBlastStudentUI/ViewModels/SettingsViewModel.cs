using KickBlastStudentUI.Helpers;
using KickBlastStudentUI.Services;

namespace KickBlastStudentUI.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private readonly PricingService _pricingService;

    public event Action<string, bool>? Toast;

    public SettingsViewModel(PricingService pricingService)
    {
        _pricingService = pricingService;
        Load();
        SaveCommand = new RelayCommand(_ => Save());
        ReloadCommand = new RelayCommand(_ => Reload());
    }

    public decimal BeginnerWeeklyFee { get; set; }
    public decimal IntermediateWeeklyFee { get; set; }
    public decimal EliteWeeklyFee { get; set; }
    public decimal CompetitionFee { get; set; }
    public decimal CoachingHourlyRate { get; set; }

    public RelayCommand SaveCommand { get; }
    public RelayCommand ReloadCommand { get; }

    private void Load()
    {
        var pricing = _pricingService.GetPricing();
        BeginnerWeeklyFee = pricing.BeginnerWeeklyFee;
        IntermediateWeeklyFee = pricing.IntermediateWeeklyFee;
        EliteWeeklyFee = pricing.EliteWeeklyFee;
        CompetitionFee = pricing.CompetitionFee;
        CoachingHourlyRate = pricing.CoachingHourlyRate;
        OnPropertyChanged(nameof(BeginnerWeeklyFee));
        OnPropertyChanged(nameof(IntermediateWeeklyFee));
        OnPropertyChanged(nameof(EliteWeeklyFee));
        OnPropertyChanged(nameof(CompetitionFee));
        OnPropertyChanged(nameof(CoachingHourlyRate));
    }

    private void Save()
    {
        _pricingService.Save(new PricingModel
        {
            BeginnerWeeklyFee = BeginnerWeeklyFee,
            IntermediateWeeklyFee = IntermediateWeeklyFee,
            EliteWeeklyFee = EliteWeeklyFee,
            CompetitionFee = CompetitionFee,
            CoachingHourlyRate = CoachingHourlyRate
        });
        Toast?.Invoke("Pricing saved and reloaded.", false);
    }

    private void Reload()
    {
        _pricingService.Reload();
        Load();
        Toast?.Invoke("Pricing reloaded.", false);
    }
}
