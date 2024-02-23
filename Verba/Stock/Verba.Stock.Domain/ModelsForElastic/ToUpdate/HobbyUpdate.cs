using Verba.Stock.Domain.ModelsForElastic.Entities.Hobbies;

namespace Verba.Stock.Domain.ModelsForElastic.ToUpdate
{
    public class HobbyUpdate
    {
        public List<Hobby> Hobby { get; set; } = new List<Hobby>();
    }
}
