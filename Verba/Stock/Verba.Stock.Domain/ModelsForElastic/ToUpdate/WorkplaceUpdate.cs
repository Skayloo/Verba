using Verba.Stock.Domain.ModelsForElastic.Entities.Workplaces;

namespace Verba.Stock.Domain.ModelsForElastic.ToUpdate
{
    public class WorkplaceUpdate
    {
        public List<Workplace> Workplace { get; set; } = new List<Workplace>();
    }
}
