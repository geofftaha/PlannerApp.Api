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
    public class UpdatePlannerItem_Unauthenticated : IClassFixture<PlannerAppApiFactory>
    {
        private readonly PlannerAppApiFactory _factory;

        public UpdatePlannerItem_Unauthenticated(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RespondsWithUnauthorized()
        {
            //Arrange
            var httpClient = _factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", "SuperSecretApiKey");
            var plannerItem = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);
            await httpClient.PostAsJsonAsync("/planner", plannerItem);

            //Act
            var updatedPlannerItem = new PlannerItem
            {
                Id = plannerItem.Id,
                Title = plannerItem.Title,
                Description = plannerItem.Description,
                DateCreated = plannerItem.DateCreated,
                DateToAction = plannerItem.DateToAction,
                Completed = true
            };
            httpClient.DefaultRequestHeaders.Remove("X-Api-Key");
            var response = await httpClient.PutAsJsonAsync($"/planner", updatedPlannerItem);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
