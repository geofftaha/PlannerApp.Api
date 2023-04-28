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
    public class GetAllPlannerItems_Unauthenticated : IClassFixture<PlannerAppApiFactory>
    {
        PlannerAppApiFactory _factory;

        public GetAllPlannerItems_Unauthenticated(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async void RespondsWithUnauthorized()
        {
            //Arrange
            var httpClient = _factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", "SuperSecretApiKey");
            var plannerItemOne = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);
            var plannerItemTwo = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);
            await httpClient.PostAsJsonAsync("/planner", plannerItemOne);
            await httpClient.PostAsJsonAsync("/planner", plannerItemTwo);

            //Act
            httpClient.DefaultRequestHeaders.Remove("X-Api-Key");
            var response = await httpClient.GetAsync("/planner");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
