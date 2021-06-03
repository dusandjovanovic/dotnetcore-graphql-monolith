using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GraphQL.Data.Context
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

            var path = Directory.GetCurrentDirectory();
           
            var configuration = new ConfigurationBuilder()
                .SetBasePath(path)              
                .AddJsonFile($"appsettings.Development.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();           

            var connectionString = configuration["ConnectionString"];

            Console.WriteLine($"connectionString:{connectionString}");

            optionsBuilder.UseSqlServer(
                connectionString, b => b.MigrationsAssembly("GraphQL.API"));
            
            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}