using PlannerApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace PlannerApp.Api.Tests.Integration.DeletePlannerItem
{
    public class DeletePlannerItem_WithValidId : IClassFixture<PlannerAppApiFactory>
    {
        private readonly PlannerAppApiFactory _factory;

        public DeletePlannerItem_WithValidId(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PlannerItemDeletedSuccessfully()
        {
            //Arrange
            var httpClient = _factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", "SuperSecretApiKey");
            var plannerItem = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);
            await httpClient.PostAsJsonAsync("/planner", plannerItem);

            //Act
            var response = await httpClient.DeleteAsync($"/planner/{plannerItem.Id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
