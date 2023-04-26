using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlannerApp.Api.Data;
using Testcontainers.PostgreSql;

namespace PlannerApp.Api.Tests.Integration
{
    public class PlannerAppApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        public string ConnectionString { get; set; } = "";
        private readonly PostgreSqlContainer _postgreSqlContainer;
        private int PostgresqlContainerPort = Random.Shared.Next(10000, 60000);

        public PlannerAppApiFactory()
        {
            _postgreSqlContainer = new PostgreSqlBuilder()
                .WithImage("postgres:latest")              
                .WithPortBinding(PostgresqlContainerPort, true)
                .WithDatabase("planner-app-db-test")
                .WithUsername("postgres")
                .WithPassword("Bennetts123")
                .WithCleanUp(true)
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();
            ConnectionString = _postgreSqlContainer.GetConnectionString();
        }

        public async Task DisposeAsync()
        {
            await _postgreSqlContainer.DisposeAsync().AsTask();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(async services =>
            {
                //Remove PlannerContext
                services.RemoveDbContext<PlannerContext>();

                //Add PlannerContext pointing to test container
                services.AddDbContext<IPlannerContext, PlannerContext>(options =>
                { options.UseNpgsql(_postgreSqlContainer.GetConnectionString()); });

                //Ensure schema gets created
                services.EnsureDbCreated<PlannerContext>();
            });
        }
    }
}
