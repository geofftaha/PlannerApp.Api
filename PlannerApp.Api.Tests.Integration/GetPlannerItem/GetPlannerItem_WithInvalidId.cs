using FluentAssertions;
using PlannerApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Api.Tests.Integration.GetPlannerItem
{
    public class GetPlannerItem_WithInvalidId : IClassFixture<PlannerAppApiFactory>
    {
        private readonly PlannerAppApiFactory _factory;

        public GetPlannerItem_WithInvalidId(PlannerAppApiFactory factory)
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
            var invalidId = Guid.NewGuid().ToString();
            var response = await httpClient.GetAsync($"/planner/{invalidId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
