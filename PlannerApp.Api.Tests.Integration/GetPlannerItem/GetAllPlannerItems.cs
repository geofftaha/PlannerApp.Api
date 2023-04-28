using FluentAssertions;
using PlannerApp.Api.Models;
using System.Net;
using System.Net.Http.Json;

namespace PlannerApp.Api.Tests.Integration.GetPlannerItem
{
    public class GetAllPlannerItems : IClassFixture<PlannerAppApiFactory>
    {
        PlannerAppApiFactory _factory;

        public GetAllPlannerItems(PlannerAppApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async void ReturnsAllPlannerItems()
        {
            //Arrange
            var httpClient = _factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", "SuperSecretApiKey");
            var plannerItemOne = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);
            var plannerItemTwo = new PlannerAppApiHelper().GeneratePlannerItem(this.GetType().Name);
            await httpClient.PostAsJsonAsync("/planner", plannerItemOne);
            await httpClient.PostAsJsonAsync("/planner", plannerItemTwo);

            //Act
            var response = await httpClient.GetAsync("/planner");
            var plannerItems = await response.Content.ReadFromJsonAsync<List<PlannerItem>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            plannerItems.Count.Should().Be(2);
            plannerItems[0].Id.Should().Be(plannerItemOne.Id);
            plannerItems[0].Title.Should().Be(plannerItemOne.Title);
            plannerItems[0].Description.Should().Be(plannerItemOne.Description);
            plannerItems[0].DateCreated.Date.Should().Be(plannerItemOne.DateCreated.Date);
            plannerItems[0].DateToAction?.Date.Should().Be(plannerItemOne.DateToAction?.Date);
            plannerItems[0].Completed.Should().BeFalse();
            plannerItems[1].Id.Should().Be(plannerItemTwo.Id);
            plannerItems[1].Title.Should().Be(plannerItemOne.Title);
            plannerItems[1].Description.Should().Be(plannerItemOne.Description);
            plannerItems[1].DateCreated.Date.Should().Be(plannerItemOne.DateCreated.Date);
            plannerItems[1].DateToAction?.Date.Should().Be(plannerItemOne.DateToAction?.Date);
            plannerItems[1].Completed.Should().BeFalse();
        }
    }
}
