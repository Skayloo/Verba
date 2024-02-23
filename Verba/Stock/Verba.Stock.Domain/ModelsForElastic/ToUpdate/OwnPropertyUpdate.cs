using Verba.Stock.Domain.ModelsForElastic.Entities.Properties;

namespace Verba.Stock.Domain.ModelsForElastic.ToUpdate
{
    public class OwnPropertyUpdate
    {
        public List<OwnProperty> OwnProperty { get; set; } = new List<OwnProperty>();
    }
}
