using GymTrack.Areas.Identity.Data;
using GymTrack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymTrack.Data;

public class GymDbContext : IdentityDbContext<GymUser>
{
    public GymDbContext(DbContextOptions<GymDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Training> Trainings { get; set; }
    public DbSet<Exercise> Excercise { get; set; } 
    public DbSet<ExerciseData> ExcerciseDatas { get; set; }
}
