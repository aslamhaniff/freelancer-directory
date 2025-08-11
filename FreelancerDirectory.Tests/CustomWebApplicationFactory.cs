using FreelancerDirectory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FreelancerDirectory.Tests
{
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly string _databaseName = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test"); // Match the check in Program.cs

            builder.ConfigureServices(services =>
            {
                // Remove any existing DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<FreelancerDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<FreelancerDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName)
                           .EnableSensitiveDataLogging(false) // Disable sensitive data logging
                           .LogTo(message => { }, LogLevel.None); // Suppress all EF logs
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FreelancerDbContext>();
                db.Database.EnsureCreated();
            });

            // Suppress noisy logs but keep important ones
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddFilter("Microsoft.EntityFrameworkCore.Update", LogLevel.None);
                logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
                logging.AddFilter("Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware", LogLevel.None);
                logging.SetMinimumLevel(LogLevel.Warning);
            });
        }
    }
}