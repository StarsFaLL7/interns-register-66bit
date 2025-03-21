using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class ApplicationDbContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<Intern> Interns { get; init; }
    public DbSet<ProbationProject> ProbationProjects { get; init; }
    public DbSet<ProbationCourse> ProbationCourses { get; init; }

    public ApplicationDbContext(IConfiguration configuration)
    {
        var readedConnString = configuration.GetConnectionString("DefaultConnection");
        if (readedConnString is null)
        {
            throw new Exception("Connection string \"DefaultConnection\" wasn't found in appsettings.json");
        }

        _connectionString = readedConnString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseNpgsql(_connectionString);
    }
}