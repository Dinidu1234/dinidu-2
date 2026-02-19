using System.Collections.ObjectModel;
using KickBlastStudentUI.Data;
using KickBlastStudentUI.Helpers;
using KickBlastStudentUI.Models;
using Microsoft.EntityFrameworkCore;

namespace KickBlastStudentUI.ViewModels;

public class HistoryViewModel : ObservableObject
{
    private readonly AppDbContext _db = new();
    private Athlete? _selectedAthleteFilter;
    private int? _monthFilter;
    private int? _yearFilter;
    private MonthlyCalculation? _selectedCalculation;

    public HistoryViewModel()
    {
        Athletes = new ObservableCollection<Athlete>(_db.Athletes.OrderBy(x => x.Name).ToList());
        Calculations = new ObservableCollection<MonthlyCalculation>();
        Load();
    }

    public ObservableCollection<Athlete> Athletes { get; }
    public ObservableCollection<MonthlyCalculation> Calculations { get; }

    public Athlete? SelectedAthleteFilter { get => _selectedAthleteFilter; set { SetProperty(ref _selectedAthleteFilter, value); Load(); } }
    public int? MonthFilter { get => _monthFilter; set { SetProperty(ref _monthFilter, value); Load(); } }
    public int? YearFilter { get => _yearFilter; set { SetProperty(ref _yearFilter, value); Load(); } }

    public MonthlyCalculation? SelectedCalculation
    {
        get => _selectedCalculation;
        set
        {
            SetProperty(ref _selectedCalculation, value);
            OnPropertyChanged(nameof(DetailTraining));
            OnPropertyChanged(nameof(DetailCoaching));
            OnPropertyChanged(nameof(DetailCompetition));
            OnPropertyChanged(nameof(DetailTotal));
            OnPropertyChanged(nameof(DetailWeight));
            OnPropertyChanged(nameof(DetailSecondSaturday));
        }
    }

    public string DetailTraining => CurrencyHelper.ToLkr(SelectedCalculation?.TrainingCost ?? 0);
    public string DetailCoaching => CurrencyHelper.ToLkr(SelectedCalculation?.CoachingCost ?? 0);
    public string DetailCompetition => CurrencyHelper.ToLkr(SelectedCalculation?.CompetitionCost ?? 0);
    public string DetailTotal => CurrencyHelper.ToLkr(SelectedCalculation?.TotalCost ?? 0);
    public string DetailWeight => SelectedCalculation?.WeightStatusMessage ?? "-";
    public string DetailSecondSaturday => SelectedCalculation?.SecondSaturdayDate.ToString("dd MMM yyyy") ?? "-";

    private void Load()
    {
        var query = _db.MonthlyCalculations.Include(x => x.Athlete).AsQueryable();

        if (SelectedAthleteFilter != null) query = query.Where(x => x.AthleteId == SelectedAthleteFilter.Id);
        if (MonthFilter.HasValue) query = query.Where(x => x.Month == MonthFilter.Value);
        if (YearFilter.HasValue) query = query.Where(x => x.Year == YearFilter.Value);

        Calculations.Clear();
        foreach (var row in query.OrderByDescending(x => x.CreatedAt).ToList())
        {
            Calculations.Add(row);
        }
    }
}
