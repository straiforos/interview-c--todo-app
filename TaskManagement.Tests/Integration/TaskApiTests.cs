using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Api.Data;
using Testcontainers.PostgreSql;
using Xunit;

namespace TaskManagement.Tests.Integration;

public class TaskApiTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:18-alpine")
        .Build();

    public async Task InitializeAsync() => await _dbContainer.Start();
    public async Task DisposeAsync() => await _dbContainer.DisposeAsync();

    [Fact]
    public async Task GetTasks_ReturnsUnauthorized_WhenNoToken()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<AppDbContext>(options =>
                        options.UseNpgsql(_dbContainer.GetConnectionString()));
                });
            });

        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/tasks");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
