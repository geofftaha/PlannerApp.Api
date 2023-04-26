using PlannerApp.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PlannerApp.Api.Tests.Integration
{
    public class PlannerAppApiHelper
    {
        public PlannerItem GeneratePlannerItem(string testClassName)
        {
            return new PlannerItem()
            {
                Id = Guid.NewGuid().ToString(),
                Title = $"{testClassName} title",
                Description = $"{testClassName} description",
                DateCreated = DateTime.UtcNow,
                DateToAction = DateTime.UtcNow.AddDays(2),
                Completed = false
            };
        }
    }
}
