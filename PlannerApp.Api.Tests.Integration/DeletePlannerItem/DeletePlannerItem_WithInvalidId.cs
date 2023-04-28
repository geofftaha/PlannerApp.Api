using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Api.Tests.Integration.DeletePlannerItem
{
    public class DeletePlannerItem_WithInvalidId : IClassFixture<PlannerAppApiFactory>
    {
        private readonly PlannerAppApiFactory _factory;

        public DeletePlannerItem_WithInvalidId(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RespondsWithNotFound()
        {
            //Arrange
            var httpClient = _factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", "SuperSecretApiKey");
            var plannerItem = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);
            await httpClient.PostAsJsonAsync("/planner", plannerItem);

            //Act
            var response = await httpClient.DeleteAsync($"/planner/{Guid.NewGuid}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
