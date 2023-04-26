using FluentAssertions;
using PlannerApp.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace PlannerApp.Api.Tests.Integration.GetPlannerItem
{
    public class GetPlannerItem_WithValidId : IClassFixture<PlannerAppApiFactory>
    {
        private readonly PlannerAppApiFactory _factory;

        public GetPlannerItem_WithValidId(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PlannerItemReturnedSuccessfully()
        {
            //Arrange
            var httpClient = _factory.CreateClient();
            var plannerItem = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);
            await httpClient.PostAsJsonAsync("/planner", plannerItem);

            //Act
            var response = await httpClient.GetAsync($"/planner/{plannerItem.Id}");
            var existingPlannerItem = response.Content.ReadFromJsonAsync<PlannerItem>().Result;

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            existingPlannerItem.Id.Should().BeEquivalentTo(plannerItem.Id);
            existingPlannerItem.Title.Should().BeEquivalentTo(plannerItem.Title);
            existingPlannerItem.Description.Should().BeEquivalentTo(plannerItem.Description);
            existingPlannerItem.DateCreated.Date.Should().Be(plannerItem.DateCreated.Date);
            existingPlannerItem.DateToAction?.Date.Should().Be(plannerItem.DateToAction?.Date);
            existingPlannerItem.Completed.Should().Be(plannerItem.Completed);
        }
    }
}
