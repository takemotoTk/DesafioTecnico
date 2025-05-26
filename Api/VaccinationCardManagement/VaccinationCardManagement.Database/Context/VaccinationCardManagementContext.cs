using Microsoft.EntityFrameworkCore;
using VaccinationCardManagement.Domain.Entities;

namespace VaccinationCardManagement.Database.Context;

public class VaccinationCardManagementContext : DbContext
{
    public VaccinationCardManagementContext(DbContextOptions options) : base(options)
    {
    }

    public async Task<int> Commit(CancellationToken cancellationToken = new CancellationToken())
    {
        base.ChangeTracker.AutoDetectChangesEnabled = false;
        var result = await base.SaveChangesAsync(cancellationToken);
        base.ChangeTracker.Clear();

        return result;
    }

    public DbSet<User> User { get; set; }
    public DbSet<Person> Person { get; set; }
    public DbSet<Vaccine> Vaccine { get; set; }
    public DbSet<VaccinationCard> VaccinationCard { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VaccinationCard>(entity =>
        {
            entity.Property(v => v.AppliedDoseTypeName).HasMaxLength(50);

            entity.HasOne(vc => vc.Person)
                .WithMany()
                .HasForeignKey(vc => vc.IdPerson)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(vc => vc.Vaccine)
                .WithMany()
                .HasForeignKey(vc => vc.IdVaccine)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
