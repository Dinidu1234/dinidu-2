using System.IO;
using KickBlastStudentUI.Models;
using Microsoft.EntityFrameworkCore;

namespace KickBlastStudentUI.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Athlete> Athletes => Set<Athlete>();
    public DbSet<TrainingPlan> TrainingPlans => Set<TrainingPlan>();
    public DbSet<MonthlyCalculation> MonthlyCalculations => Set<MonthlyCalculation>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDir);
        var dbPath = Path.Combine(dataDir, "kickblast_student.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();

        modelBuilder.Entity<Athlete>()
            .HasOne(a => a.TrainingPlan)
            .WithMany(p => p.Athletes)
            .HasForeignKey(a => a.TrainingPlanId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MonthlyCalculation>()
            .HasOne(m => m.Athlete)
            .WithMany(a => a.MonthlyCalculations)
            .HasForeignKey(m => m.AthleteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
