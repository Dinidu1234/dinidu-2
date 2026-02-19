namespace KickBlastStudentUI.Helpers;

public static class Validators
{
    public static string ValidateAthlete(string name, decimal currentWeight, decimal categoryWeight, int? planId)
    {
        if (string.IsNullOrWhiteSpace(name)) return "Name is required.";
        if (currentWeight <= 0) return "Current weight must be greater than 0.";
        if (categoryWeight <= 0) return "Competition category must be greater than 0.";
        if (!planId.HasValue || planId.Value <= 0) return "Training plan is required.";
        return string.Empty;
    }

    public static string ValidateCalculator(double coachingHours, int competitionsCount, string planName)
    {
        if (coachingHours < 0 || coachingHours > 5) return "Coaching hours must be between 0 and 5.";
        if (competitionsCount < 0) return "Competitions cannot be negative.";
        if (planName.Equals("Beginner", StringComparison.OrdinalIgnoreCase) && competitionsCount > 0)
            return "Beginner plan athletes cannot have competitions.";
        return string.Empty;
    }
}
