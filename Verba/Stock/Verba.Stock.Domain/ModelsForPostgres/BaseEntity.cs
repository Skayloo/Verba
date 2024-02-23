using System;

namespace Verba.Stock.Domain.ModelsForPostgres
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public DateTime? CreatedDatetime { get; set; } = DateTime.Now;

        public DateTime? ModifiedDatetime { get; set; } = DateTime.Now;

        public bool? IsDeleted { get; set; } = false;
    }
}
