using KickBlastStudentUI.Helpers;
using KickBlastStudentUI.Models;

namespace KickBlastStudentUI.Services;

public class FeeCalculatorService
{
    public MonthlyCalculation Calculate(Athlete athlete, PricingModel pricing, int month, int year, int competitions, double coachingHours)
    {
        var weeklyFee = athlete.TrainingPlan?.WeeklyFee ?? 0;
        var trainingCost = weeklyFee * 4;
        var coachingCost = (decimal)coachingHours * 4 * pricing.CoachingHourlyRate;
        var competitionCost = competitions * pricing.CompetitionFee;

        var status = athlete.CurrentWeightKg > athlete.CompetitionCategoryKg
            ? "Over target"
            : athlete.CurrentWeightKg < athlete.CompetitionCategoryKg
                ? "Under target"
                : "On target";

        return new MonthlyCalculation
        {
            AthleteId = athlete.Id,
            Month = month,
            Year = year,
            TrainingCost = trainingCost,
            CoachingCost = coachingCost,
            CompetitionCost = competitionCost,
            TotalCost = trainingCost + coachingCost + competitionCost,
            CompetitionsCount = competitions,
            CoachingHoursPerWeek = coachingHours,
            WeightStatusMessage = status,
            SecondSaturdayDate = DateHelper.GetSecondSaturday(new DateTime(year, month, 1)),
            CreatedAt = DateTime.Now
        };
    }
}
