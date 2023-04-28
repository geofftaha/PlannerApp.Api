using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PlannerApp.Api.Data;
using PlannerApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Api.Tests.Integration.UpdatePlannerItem
{
    public class UpdatePlannerItem_WithValidData : IClassFixture<PlannerAppApiFactory>
    {
        private readonly PlannerAppApiFactory _factory;

        public UpdatePlannerItem_WithValidData(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PlannerItemUpdatedSuccessfully()
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
            var response = await httpClient.PutAsJsonAsync($"/planner", updatedPlannerItem);
            var plannerItemAfterUpdate = await response.Content.ReadFromJsonAsync<PlannerItem>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            plannerItemAfterUpdate.Should().BeEquivalentTo(updatedPlannerItem);
        }
    }
}
