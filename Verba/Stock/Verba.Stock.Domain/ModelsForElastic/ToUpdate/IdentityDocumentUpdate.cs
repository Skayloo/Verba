using Verba.Stock.Domain.ModelsForElastic.Entities.IdentityDocuments;

namespace Verba.Stock.Domain.ModelsForElastic.ToUpdate
{
    public class IdentityDocumentUpdate
    {
        public List<IdentityDocument> IdentityDocument { get; set; } = new List<IdentityDocument>();
    }
}
