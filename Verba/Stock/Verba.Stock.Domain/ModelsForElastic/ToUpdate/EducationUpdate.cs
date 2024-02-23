using Verba.Stock.Domain.ModelsForElastic.Entities.Educations;

namespace Verba.Stock.Domain.ModelsForElastic.ToUpdate
{
    public class EducationUpdate
    {
        public List<Education> Education { get; set; } = new List<Education>();
    }
}
