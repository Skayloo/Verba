using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Verba.Stock.Domain.Models.Inn;

namespace Verba.Stock.Db.Data.EntityConfigurations
{
    public class InnsEntityTypeConfiguration : IEntityTypeConfiguration<Inns>
    {
        public void Configure(EntityTypeBuilder<Inns> builder)
        {
            builder.ToTable("inns");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.CreatedDatetime).HasColumnName("created_datetime").IsRequired(false);
            builder.Property(p => p.ModifiedDatetime).HasColumnName("modified_datetime").IsRequired(false);
            builder.Property(p => p.IsDeleted).HasColumnName("is_deleted").IsRequired(false);
            builder.Property(p => p.Inn).HasColumnName("inn").IsRequired(false);
            builder.Property(p => p.Date).HasColumnName("date").IsRequired(false);
            builder.Property(p => p.Count).HasColumnName("count").IsRequired(false);
            builder.Property(p => p.Country).HasColumnName("country").IsRequired(false);
        }
    }
}
