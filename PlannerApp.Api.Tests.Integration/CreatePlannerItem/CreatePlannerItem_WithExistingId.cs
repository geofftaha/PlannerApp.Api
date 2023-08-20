using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

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
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", "SuperSecretApiKey");
            await httpClient.PostAsJsonAsync("/planner", plannerItem);

            //Act
            var response = await httpClient.PostAsJsonAsync("/planner", plannerItem);
            var responseContent = await response.Content.ReadAsStringAsync();
            var problemDetail = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            //Assert
            Assert.Multiple(
                () => response.StatusCode.Should().Be(HttpStatusCode.BadRequest),
                () => problemDetail!.Title.Should().Be("This item already exists")
                );
        }
    }
}
