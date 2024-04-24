using FireBrigade.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FireBrigade.Data;

public class FireBrigadeContext : DbContext
{
    public DbSet<UserBrigade> Users { get; set; }
    public DbSet<EmergencyBrigade> EmergencyBrigades { get; set; }

    public FireBrigadeContext(DbContextOptions<FireBrigadeContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var data = "FireBrigade.db";
        var databasePath = Path.Combine(folderPath, data);

        optionsBuilder.UseSqlite($"Filename={databasePath}");
        Console.WriteLine(databasePath);
    }
}
