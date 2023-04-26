using PlannerApp.Api.Models;

namespace PlannerApp.Api.Services
{
    public interface IPlannerService
    {
        public Task<bool> CreatePlannerItemAsync(PlannerItem plannerItem);
        public Task<PlannerItem> GetPlannerItemAsync(string id);
        public Task<IEnumerable<PlannerItem>> GetAllPlannerItemsAsync();
        public Task<bool> UpdatePlannerItemAsync(PlannerItem plannerItem);
        public Task<bool> DeletePlannerItemAsync(string id);
    }
}
