using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace KickBlastStudentUI.Services;

public class PricingService
{
    private readonly string _settingsPath;
    private PricingModel _pricing;

    public PricingService()
    {
        _settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        EnsureSettingsFile();
        _pricing = Load();
    }

    public PricingModel GetPricing() => _pricing;

    public void Reload() => _pricing = Load();

    public void Save(PricingModel model)
    {
        var root = new AppSettingsRoot { Pricing = model };
        var json = JsonSerializer.Serialize(root, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_settingsPath, json);
        _pricing = model;
    }

    private PricingModel Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var model = new PricingModel();
        config.GetSection("Pricing").Bind(model);
        return model;
    }

    private void EnsureSettingsFile()
    {
        if (File.Exists(_settingsPath)) return;

        var defaultJson = """
{
  "Pricing": {
    "BeginnerWeeklyFee": 2500,
    "IntermediateWeeklyFee": 3500,
    "EliteWeeklyFee": 5000,
    "CompetitionFee": 1500,
    "CoachingHourlyRate": 800
  }
}
""";
        File.WriteAllText(_settingsPath, defaultJson);
    }
}

public class PricingModel
{
    public decimal BeginnerWeeklyFee { get; set; }
    public decimal IntermediateWeeklyFee { get; set; }
    public decimal EliteWeeklyFee { get; set; }
    public decimal CompetitionFee { get; set; }
    public decimal CoachingHourlyRate { get; set; }
}

public class AppSettingsRoot
{
    public PricingModel Pricing { get; set; } = new();
}
