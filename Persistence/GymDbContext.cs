using GymTrack.Areas.Identity.Data;
using GymTrack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymTrack.Persistence;

public class GymDbContext : IdentityDbContext<GymUser>
{
    public GymDbContext(DbContextOptions<GymDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Training>(entity =>
        {
            entity
            .Property(t => t.Date)
            .HasColumnType("timestamp without time zone");

            entity
            .HasOne(t => t.GymUser)
            .WithMany(u => u.Trainings)
            .HasForeignKey(t => t.GymUserId);
        });

        builder.Entity<ExerciseData>()
            .HasOne(ed => ed.Exercise)
            .WithMany(e => e.Datas)
            .HasForeignKey(ed => ed.ExerciseId);

        builder.Entity<ExerciseData>()
            .HasOne(ed => ed.Training)
            .WithMany(t => t.Exercises)
            .HasForeignKey(ed => ed.TrainingId);
    }

    public DbSet<Training> Trainings { get; set; }
    public DbSet<Exercise> Exercise { get; set; }
    public DbSet<ExerciseData> ExerciseDatas { get; set; }
}
