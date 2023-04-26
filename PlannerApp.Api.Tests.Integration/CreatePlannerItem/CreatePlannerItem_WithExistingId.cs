using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace PlannerApp.Api.Tests.Integration.CreatePlannerItem
{
    public class CreatePlannerItem_WithExistingId : IClassFixture<PlannerAppApiFactory>
    {
        private readonly PlannerAppApiFactory _factory;

        public CreatePlannerItem_WithExistingId(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task BadRequestResponseExpected()
        {
            //Arrange
            var httpClient = _factory.CreateClient();
            var plannerItem = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);
            await httpClient.PostAsJsonAsync("/planner", plannerItem);

            //Act
            var response = await httpClient.PostAsJsonAsync("/planner", plannerItem);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
