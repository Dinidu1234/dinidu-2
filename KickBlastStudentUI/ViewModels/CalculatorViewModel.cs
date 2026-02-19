using System.Collections.ObjectModel;
using KickBlastStudentUI.Data;
using KickBlastStudentUI.Helpers;
using KickBlastStudentUI.Models;
using KickBlastStudentUI.Services;
using Microsoft.EntityFrameworkCore;

namespace KickBlastStudentUI.ViewModels;

public class CalculatorViewModel : ObservableObject
{
    private readonly AppDbContext _db = new();
    private readonly PricingService _pricingService;
    private readonly FeeCalculatorService _calculatorService = new();
    private Athlete? _selectedAthlete;
    private int _month = DateTime.Now.Month;
    private int _year = DateTime.Now.Year;
    private int _competitionsCount;
    private double _coachingHoursPerWeek;
    private string _validationMessage = string.Empty;

    public event Action<string, bool>? Toast;

    public CalculatorViewModel(PricingService pricingService)
    {
        _pricingService = pricingService;
        Athletes = new ObservableCollection<Athlete>(_db.Athletes.Include(x => x.TrainingPlan).OrderBy(x => x.Name).ToList());
        CalculateCommand = new RelayCommand(_ => Calculate());
        SaveCommand = new RelayCommand(_ => Save(), _ => Result != null);
    }

    public ObservableCollection<Athlete> Athletes { get; }
    public Athlete? SelectedAthlete { get => _selectedAthlete; set => SetProperty(ref _selectedAthlete, value); }
    public int Month { get => _month; set => SetProperty(ref _month, value); }
    public int Year { get => _year; set => SetProperty(ref _year, value); }
    public int CompetitionsCount { get => _competitionsCount; set => SetProperty(ref _competitionsCount, value); }
    public double CoachingHoursPerWeek { get => _coachingHoursPerWeek; set => SetProperty(ref _coachingHoursPerWeek, value); }
    public string ValidationMessage { get => _validationMessage; set => SetProperty(ref _validationMessage, value); }

    private MonthlyCalculation? _result;
    public MonthlyCalculation? Result
    {
        get => _result;
        set
        {
            if (SetProperty(ref _result, value))
            {
                OnPropertyChanged(nameof(TrainingCostText));
                OnPropertyChanged(nameof(CoachingCostText));
                OnPropertyChanged(nameof(CompetitionCostText));
                OnPropertyChanged(nameof(TotalCostText));
                OnPropertyChanged(nameof(WeightStatus));
                OnPropertyChanged(nameof(SecondSaturdayText));
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public string TrainingCostText => CurrencyHelper.ToLkr(Result?.TrainingCost ?? 0);
    public string CoachingCostText => CurrencyHelper.ToLkr(Result?.CoachingCost ?? 0);
    public string CompetitionCostText => CurrencyHelper.ToLkr(Result?.CompetitionCost ?? 0);
    public string TotalCostText => CurrencyHelper.ToLkr(Result?.TotalCost ?? 0);
    public string WeightStatus => Result?.WeightStatusMessage ?? "-";
    public string SecondSaturdayText => Result?.SecondSaturdayDate.ToString("dd MMM yyyy") ?? "-";

    public RelayCommand CalculateCommand { get; }
    public RelayCommand SaveCommand { get; }

    private void Calculate()
    {
        if (SelectedAthlete == null)
        {
            ValidationMessage = "Select an athlete.";
            return;
        }

        ValidationMessage = Validators.ValidateCalculator(CoachingHoursPerWeek, CompetitionsCount, SelectedAthlete.TrainingPlan?.Name ?? string.Empty);
        if (!string.IsNullOrEmpty(ValidationMessage)) return;

        var pricing = _pricingService.GetPricing();
        Result = _calculatorService.Calculate(SelectedAthlete, pricing, Month, Year, CompetitionsCount, CoachingHoursPerWeek);
        ValidationMessage = string.Empty;
    }

    private void Save()
    {
        if (Result == null) return;
        _db.MonthlyCalculations.Add(Result);
        _db.SaveChanges();
        Toast?.Invoke("Calculation saved.", false);
    }
}
