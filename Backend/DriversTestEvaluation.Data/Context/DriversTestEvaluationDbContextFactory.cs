using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DriversTestEvaluation.Data.Context;

public class DriversTestEvaluationDbContextFactory
    : IDesignTimeDbContextFactory<DriversTestEvaluationDbContext>
{
    public DriversTestEvaluationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DriversTestEvaluationDbContext>();

        optionsBuilder.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection")
        );

        return new DriversTestEvaluationDbContext(optionsBuilder.Options);
    }
}