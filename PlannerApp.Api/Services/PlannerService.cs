using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlannerApp.Api.Data;
using PlannerApp.Api.Models;

namespace PlannerApp.Api.Services
{
    public class PlannerService : IPlannerService
    {
        private readonly IPlannerContext _plannerContext;

        public PlannerService(IPlannerContext plannerContext)
        {
            _plannerContext = plannerContext;
        }

        public async Task<CreatePlannerItemResponse> CreatePlannerItemAsync(PlannerItem plannerItem)
        {
            var existingPlannerItem = await GetPlannerItemAsync(plannerItem.Id);
            if(existingPlannerItem is not null) return new CreatePlannerItemResponse { IsSuccess = false, StatusMessage = "This item already exists"};

            await _plannerContext.PlannerItems.AddAsync(plannerItem);
            var result = await _plannerContext.SaveChangesAsync();

            return result > 0 ? new CreatePlannerItemResponse { IsSuccess = true } : new CreatePlannerItemResponse { IsSuccess = false, StatusMessage = "Unable to save item"};
        }        

        public async Task<PlannerItem> GetPlannerItemAsync(string id)
        {
            var result = await _plannerContext.PlannerItems
                            .AsNoTracking()
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();
            return result!;
        }

        public async Task<IEnumerable<PlannerItem>> GetAllPlannerItemsAsync()
        {
            return await _plannerContext.PlannerItems.ToListAsync();
        }

        public async Task<bool> UpdatePlannerItemAsync(PlannerItem plannerItem)
        {
            var plannerItemExists = await GetPlannerItemAsync(plannerItem.Id);
            if (plannerItemExists is null) return false;

            _plannerContext.PlannerItems.Update(plannerItem);            
            var result = await _plannerContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeletePlannerItemAsync(string id)
        {
            var plannerItemToDelete = await GetPlannerItemAsync(id);
            if (plannerItemToDelete is null) return false;

            _plannerContext.PlannerItems.Remove(plannerItemToDelete);
            var result = await _plannerContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
