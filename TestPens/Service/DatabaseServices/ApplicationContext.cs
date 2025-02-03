namespace TestPens.Service.DatabaseServices;

using System.Text.Json;

using Microsoft.EntityFrameworkCore;

using TestPens.Models.Real;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<BattleDatabase> Battles { get; set; } = null!;
    public DbSet<GenericChangeDatabase> Changes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BattleDatabase>()
           .Property(b => b.Left)
           .HasConversion(v => JsonSerializer.Serialize(v, Program.JsonOptions),
                v => JsonSerializer.Deserialize<BattledPersonModel>(v, Program.JsonOptions)!);

        modelBuilder.Entity<BattleDatabase>()
            .Property(b => b.Right)
            .HasConversion(v => JsonSerializer.Serialize(v, Program.JsonOptions),
                v => JsonSerializer.Deserialize<BattledPersonModel>(v, Program.JsonOptions)!);

        modelBuilder.Entity<GenericChangeDatabase>()
            .Property(b => b.Data)
            .HasConversion(v => JsonSerializer.Serialize(v, Program.JsonOptions),
                v => JsonSerializer.Deserialize<GenericChangeDatabase.ExtraData>(v, Program.JsonOptions)!);
    }
}