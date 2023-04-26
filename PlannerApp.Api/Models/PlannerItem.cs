namespace PlannerApp.Api.Models
{
    public class PlannerItem
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime? DateToAction { get; set; }
        public bool Completed { get; set; } = false;
    }
}
