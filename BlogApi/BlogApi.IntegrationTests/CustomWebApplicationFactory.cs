using BlogInfrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlogApi;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace BlogApi.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real database context registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IDbContextOptionsConfiguration<BlogDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Register in-memory test DB
            services.AddDbContext<BlogDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));

            // Ensure the DB is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
