using Verba.Stock.Domain.ModelsForElastic.Entities.Projects;

namespace Verba.Stock.Domain.ModelsForElastic.ToUpdate
{
    public class ProjectUpdate
    {
        public List<Project> Project { get; set; } = new List<Project>();
    }
}
