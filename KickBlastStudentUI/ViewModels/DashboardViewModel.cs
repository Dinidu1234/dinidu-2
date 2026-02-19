using KickBlastStudentUI.Data;
using KickBlastStudentUI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace KickBlastStudentUI.ViewModels;

public class DashboardViewModel : ObservableObject
{
    public DashboardViewModel()
    {
        using var db = new AppDbContext();
        var month = DateTime.Now.Month;
        var year = DateTime.Now.Year;

        TotalAthletes = db.Athletes.Count;
        CalculationsThisMonth = db.MonthlyCalculations.Count(x => x.Month == month && x.Year == year);
        TotalRevenueThisMonth = CurrencyHelper.ToLkr(db.MonthlyCalculations
            .Where(x => x.Month == month && x.Year == year)
            .Sum(x => x.TotalCost));

        var nextCalc = db.MonthlyCalculations
            .Where(x => x.Month == month && x.Year == year)
            .OrderBy(x => x.SecondSaturdayDate)
            .FirstOrDefault();
        NextCompetitionDate = nextCalc?.SecondSaturdayDate.ToString("dd MMM yyyy") ?? "Not available";

        RecentCalculations = db.MonthlyCalculations
            .Include(x => x.Athlete)
            .OrderByDescending(x => x.CreatedAt)
            .Take(8)
            .Select(x => new
            {
                x.Id,
                Athlete = x.Athlete != null ? x.Athlete.Name : "Athlete",
                Period = $"{x.Month}/{x.Year}",
                Total = CurrencyHelper.ToLkr(x.TotalCost),
                x.WeightStatusMessage
            }).Cast<object>().ToList();
    }

    public int TotalAthletes { get; }
    public int CalculationsThisMonth { get; }
    public string TotalRevenueThisMonth { get; }
    public string NextCompetitionDate { get; }
    public List<object> RecentCalculations { get; }
}
