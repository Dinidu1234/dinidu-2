using System.Collections.ObjectModel;
using System.Windows;
using KickBlastStudentUI.Data;
using KickBlastStudentUI.Helpers;
using KickBlastStudentUI.Models;
using Microsoft.EntityFrameworkCore;

namespace KickBlastStudentUI.ViewModels;

public class AthletesViewModel : ObservableObject
{
    private readonly AppDbContext _db = new();
    private Athlete? _selectedAthlete;
    private string _searchText = string.Empty;
    private string _validationMessage = string.Empty;
    private TrainingPlan? _selectedPlanFilter;
    private int _editId;
    private string _name = string.Empty;
    private decimal _currentWeight;
    private decimal _competitionWeight;
    private TrainingPlan? _selectedPlan;

    public event Action<string, bool>? Toast;

    public AthletesViewModel()
    {
        Plans = new ObservableCollection<TrainingPlan>(_db.TrainingPlans.OrderBy(x => x.Name).ToList());
        Athletes = new ObservableCollection<Athlete>();
        LoadAthletes();

        AddOrUpdateCommand = new RelayCommand(_ => AddOrUpdate());
        DeleteCommand = new RelayCommand(_ => DeleteSelected(), _ => SelectedAthlete != null);
        ResetCommand = new RelayCommand(_ => ResetForm());
    }

    public ObservableCollection<Athlete> Athletes { get; }
    public ObservableCollection<TrainingPlan> Plans { get; }

    public Athlete? SelectedAthlete
    {
        get => _selectedAthlete;
        set
        {
            if (SetProperty(ref _selectedAthlete, value) && value != null)
            {
                EditId = value.Id;
                Name = value.Name;
                CurrentWeight = value.CurrentWeightKg;
                CompetitionWeight = value.CompetitionCategoryKg;
                SelectedPlan = Plans.FirstOrDefault(p => p.Id == value.TrainingPlanId);
            }
            DeleteCommand.RaiseCanExecuteChanged();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set { SetProperty(ref _searchText, value); LoadAthletes(); }
    }

    public TrainingPlan? SelectedPlanFilter
    {
        get => _selectedPlanFilter;
        set { SetProperty(ref _selectedPlanFilter, value); LoadAthletes(); }
    }

    public int EditId { get => _editId; set => SetProperty(ref _editId, value); }
    public string Name { get => _name; set => SetProperty(ref _name, value); }
    public decimal CurrentWeight { get => _currentWeight; set => SetProperty(ref _currentWeight, value); }
    public decimal CompetitionWeight { get => _competitionWeight; set => SetProperty(ref _competitionWeight, value); }
    public TrainingPlan? SelectedPlan { get => _selectedPlan; set => SetProperty(ref _selectedPlan, value); }
    public string ValidationMessage { get => _validationMessage; set => SetProperty(ref _validationMessage, value); }

    public RelayCommand AddOrUpdateCommand { get; }
    public RelayCommand DeleteCommand { get; }
    public RelayCommand ResetCommand { get; }

    private void LoadAthletes()
    {
        var query = _db.Athletes.Include(a => a.TrainingPlan).AsQueryable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            query = query.Where(x => x.Name.Contains(SearchText));
        }

        if (SelectedPlanFilter != null)
        {
            query = query.Where(x => x.TrainingPlanId == SelectedPlanFilter.Id);
        }

        Athletes.Clear();
        foreach (var athlete in query.OrderByDescending(x => x.CreatedAt).ToList())
        {
            Athletes.Add(athlete);
        }
    }

    private void AddOrUpdate()
    {
        ValidationMessage = Validators.ValidateAthlete(Name, CurrentWeight, CompetitionWeight, SelectedPlan?.Id);
        if (!string.IsNullOrEmpty(ValidationMessage)) return;

        if (EditId == 0)
        {
            _db.Athletes.Add(new Athlete
            {
                Name = Name.Trim(),
                CurrentWeightKg = CurrentWeight,
                CompetitionCategoryKg = CompetitionWeight,
                TrainingPlanId = SelectedPlan!.Id,
                CreatedAt = DateTime.Now
            });
            Toast?.Invoke("Athlete added.", false);
        }
        else
        {
            var athlete = _db.Athletes.First(x => x.Id == EditId);
            athlete.Name = Name.Trim();
            athlete.CurrentWeightKg = CurrentWeight;
            athlete.CompetitionCategoryKg = CompetitionWeight;
            athlete.TrainingPlanId = SelectedPlan!.Id;
            Toast?.Invoke("Athlete updated.", false);
        }

        _db.SaveChanges();
        LoadAthletes();
        ResetForm();
    }

    private void DeleteSelected()
    {
        if (SelectedAthlete == null) return;

        var result = MessageBox.Show($"Delete {SelectedAthlete.Name}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result != MessageBoxResult.Yes) return;

        _db.Athletes.Remove(SelectedAthlete);
        _db.SaveChanges();
        LoadAthletes();
        ResetForm();
        Toast?.Invoke("Athlete deleted.", false);
    }

    private void ResetForm()
    {
        EditId = 0;
        Name = string.Empty;
        CurrentWeight = 0;
        CompetitionWeight = 0;
        SelectedPlan = null;
        ValidationMessage = string.Empty;
        SelectedAthlete = null;
    }
}
