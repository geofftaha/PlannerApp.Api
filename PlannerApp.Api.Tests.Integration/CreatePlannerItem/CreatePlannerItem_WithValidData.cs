using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using FluentAssertions;
using PlannerApp.Api.Models;

namespace PlannerApp.Api.Tests.Integration.CreatePlannerItem
{
    public class CreatePlannerItem_WithValidData : IClassFixture<PlannerAppApiFactory>
    {
        private readonly PlannerAppApiFactory _factory;

        public CreatePlannerItem_WithValidData(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PlannerItemIsCreatedSuccessfully()
        {
            //Arrange
            var httpClient = _factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", "SuperSecretApiKey");
            var plannerItem = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);

            //Act
            var response = await httpClient.PostAsJsonAsync("/planner", plannerItem);
            var createdPlannerItem = await response.Content.ReadFromJsonAsync<PlannerItem>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            createdPlannerItem.Should().BeEquivalentTo(plannerItem);
        }
    }
}
