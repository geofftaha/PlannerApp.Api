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

namespace PlannerApp.Api.Tests.Integration.CreatePlannerItem
{
    public class CreatePlannerItem_Unauthenticated : IClassFixture<PlannerAppApiFactory>
    {
        private readonly PlannerAppApiFactory _factory;

        public CreatePlannerItem_Unauthenticated(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RespondsWithUnauthorized()
        {
            //Arrange
            var httpClient = _factory.CreateClient();
            var plannerItem = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);

            //Act
            var response = await httpClient.PostAsJsonAsync("/planner", plannerItem);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
