namespace PlannerApp.Api.Models
{
    public class CreatePlannerItemResponse
    {
        public bool IsSuccess { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
    }
}
