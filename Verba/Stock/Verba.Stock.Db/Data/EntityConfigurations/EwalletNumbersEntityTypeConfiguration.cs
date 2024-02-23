using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Verba.Stock.Domain.Models.EwalletNumber;

namespace Verba.Stock.Db.Data.EntityConfigurations
{
    public class EwalletNumbersEntityTypeConfiguration : IEntityTypeConfiguration<EwalletNumbers>
    {
        public void Configure(EntityTypeBuilder<EwalletNumbers> builder)
        {
            builder.ToTable("ewallet_numbers");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.CreatedDatetime).HasColumnName("created_datetime").IsRequired(false);
            builder.Property(p => p.ModifiedDatetime).HasColumnName("modified_datetime").IsRequired(false);
            builder.Property(p => p.IsDeleted).HasColumnName("is_deleted").IsRequired(false);
            builder.Property(p => p.WalletNumber).HasColumnName("wallet_number").IsRequired(false);
            builder.Property(p => p.PaymentSystemName).HasColumnName("payment_system_name").IsRequired(false);
            builder.Property(p => p.Date).HasColumnName("date").IsRequired(false);
            builder.Property(p => p.Count).HasColumnName("count").IsRequired(false);
            builder.Property(p => p.Country).HasColumnName("country").IsRequired(false);
        }
    }
}
