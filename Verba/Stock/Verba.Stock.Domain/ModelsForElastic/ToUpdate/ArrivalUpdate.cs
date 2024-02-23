using Verba.Stock.Domain.ModelsForElastic.Entities.Arrivals;

namespace Verba.Stock.Domain.ModelsForElastic.ToUpdate
{
    public class ArrivalUpdate
    {
        public List<Arrival> Arrival { get; set; } = new List<Arrival>();
    }
}
