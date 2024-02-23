using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Verba.Stock.Domain.Models.Lots;

namespace Verba.Stock.Db.Data.EntityConfigurations
{
    public class LotsEntityTypeConfiguration : IEntityTypeConfiguration<Lots>
    {
        public void Configure(EntityTypeBuilder<Lots> builder)
        {
            builder.ToTable("lots");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.CreatedDatetime).HasColumnName("created_datetime").IsRequired(false);
            builder.Property(p => p.ModifiedDatetime).HasColumnName("modified_datetime").IsRequired(false);
            builder.Property(p => p.IsDeleted).HasColumnName("is_deleted").IsRequired(false);
            builder.Property(p => p.DateOfRequest).HasColumnName("date_of_request").IsRequired(false);
            builder.Property(p => p.NameOfOrganization).HasColumnName("name_of_organization").IsRequired(false);
            builder.Property(p => p.FioOfInn).HasColumnName("fio_of_inn").IsRequired(false);
            builder.Property(p => p.AddressOrAccountNumber).HasColumnName("address_or_account_number").IsRequired(false);
            builder.Property(p => p.FioOfForeigner).HasColumnName("fio_of_foreigner").IsRequired(false);
        }
    }
}
