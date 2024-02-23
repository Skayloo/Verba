using Verba.Stock.Domain.ModelsForElastic.Entities.Languages;

namespace Verba.Stock.Domain.ModelsForElastic.ToUpdate
{
    public class LanguageUpdate
    {
        public List<Language> Language { get; set; } = new List<Language>();
    }
}
