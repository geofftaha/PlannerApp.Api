using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PlannerApp.Api.Data;
using PlannerApp.Api.Models;
using System.Net.Http.Json;

namespace PlannerApp.Api.Tests.Integration.CreatePlannerItem
{
    public class CreatePlannerItem_CheckDatabase_WithValidData : IClassFixture<PlannerAppApiFactory>
    {
        private readonly PlannerAppApiFactory _factory;

        public CreatePlannerItem_CheckDatabase_WithValidData(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PlannerItemIsSavedSuccesfully()
        {
            //Arrange
            var httpClient = _factory.CreateClient();
            var plannerItem = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", "SuperSecretApiKey");
            await httpClient.PostAsJsonAsync("/planner", plannerItem);

            //Act
            using var context = new PlannerContext(new DbContextOptionsBuilder<PlannerContext>()
           .UseNpgsql(_factory.ConnectionString!)
           .Options);
            await context.Database.EnsureCreatedAsync();
            var savedPlannerItem = await context.PlannerItems.FirstOrDefaultAsync();

            //Assert
            savedPlannerItem.Id.Should().BeEquivalentTo(plannerItem.Id);
            savedPlannerItem.Title.Should().BeEquivalentTo(plannerItem.Title);
            savedPlannerItem.Description.Should().BeEquivalentTo(plannerItem.Description);
            savedPlannerItem.DateCreated.Date.Should().Be(plannerItem.DateCreated.Date);
            savedPlannerItem.DateToAction?.Date.Should().Be(plannerItem.DateToAction?.Date);
            savedPlannerItem.Completed.Should().BeFalse();
        }
    }
}
