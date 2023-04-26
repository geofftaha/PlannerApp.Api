using Microsoft.EntityFrameworkCore;
using PlannerApp.Api.Models;

namespace PlannerApp.Api.Data
{
    public interface IPlannerContext
    {
        DbSet<PlannerItem> PlannerItems { get; set; }
        Task<int> SaveChangesAsync();
    }
}
