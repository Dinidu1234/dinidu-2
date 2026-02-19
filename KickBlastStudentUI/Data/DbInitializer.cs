using KickBlastStudentUI.Models;
using KickBlastStudentUI.Services;

namespace KickBlastStudentUI.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext db, PricingService pricingService)
    {
        db.Database.EnsureCreated();

        if (!db.Users.Any())
        {
            db.Users.Add(new User
            {
                Username = "rashii",
                PasswordPlain = "123456",
                CreatedAt = DateTime.Now
            });
        }

        if (!db.TrainingPlans.Any())
        {
            var pricing = pricingService.GetPricing();
            db.TrainingPlans.AddRange(
                new TrainingPlan { Name = "Beginner", WeeklyFee = pricing.BeginnerWeeklyFee },
                new TrainingPlan { Name = "Intermediate", WeeklyFee = pricing.IntermediateWeeklyFee },
                new TrainingPlan { Name = "Elite", WeeklyFee = pricing.EliteWeeklyFee }
            );
            db.SaveChanges();
        }

        if (!db.Athletes.Any())
        {
            var plans = db.TrainingPlans.ToList();
            var beginner = plans.First(p => p.Name == "Beginner");
            var intermediate = plans.First(p => p.Name == "Intermediate");
            var elite = plans.First(p => p.Name == "Elite");

            db.Athletes.AddRange(
                new Athlete { Name = "Nimal Perera", TrainingPlanId = beginner.Id, CurrentWeightKg = 59, CompetitionCategoryKg = 60 },
                new Athlete { Name = "Sadeepa Silva", TrainingPlanId = beginner.Id, CurrentWeightKg = 48, CompetitionCategoryKg = 50 },
                new Athlete { Name = "Mihira Jay", TrainingPlanId = intermediate.Id, CurrentWeightKg = 67, CompetitionCategoryKg = 65 },
                new Athlete { Name = "Kavindu Fernando", TrainingPlanId = intermediate.Id, CurrentWeightKg = 73, CompetitionCategoryKg = 70 },
                new Athlete { Name = "Rashmika Wijesinghe", TrainingPlanId = elite.Id, CurrentWeightKg = 81, CompetitionCategoryKg = 80 },
                new Athlete { Name = "Chamath Niroshan", TrainingPlanId = elite.Id, CurrentWeightKg = 90, CompetitionCategoryKg = 86 }
            );
        }

        db.SaveChanges();
    }
}
