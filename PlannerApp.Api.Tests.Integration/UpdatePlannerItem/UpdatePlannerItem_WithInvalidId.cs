using PlannerApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace PlannerApp.Api.Tests.Integration.UpdatePlannerItem
{
    public class UpdatePlannerItem_WithInvalidId : IClassFixture<PlannerAppApiFactory>
    {
        private readonly PlannerAppApiFactory _factory;

        public UpdatePlannerItem_WithInvalidId(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RespondsWithNotFound()
        {
            //Arrange
            var httpClient = _factory.CreateClient();
            var plannerItem = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);
            await httpClient.PostAsJsonAsync("/planner", plannerItem);

            //Act
            var updatedPlannerItem = new PlannerItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = plannerItem.Title,
                Description = plannerItem.Description,
                DateCreated = plannerItem.DateCreated,
                DateToAction = plannerItem.DateToAction,
                Completed = true
            };
            var response = await httpClient.PutAsJsonAsync($"/planner", updatedPlannerItem);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
