using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Verba.Stock.Domain.Models.AccountNumbers;

namespace Verba.Stock.Db.Data.EntityConfigurations;

public class AccountNumbersEntityTypeConfiguration : IEntityTypeConfiguration<AccountNumbers>
{
    public void Configure(EntityTypeBuilder<AccountNumbers> builder)
    {
        builder.ToTable("account_numbers");
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(p => p.CreatedDatetime).HasColumnName("created_datetime").IsRequired(false);
        builder.Property(p => p.ModifiedDatetime).HasColumnName("modified_datetime").IsRequired(false);
        builder.Property(p => p.IsDeleted).HasColumnName("is_deleted").IsRequired(false);
        builder.Property(p => p.AccountNumber).HasColumnName("account_number").IsRequired(false);
        builder.Property(p => p.BIK).HasColumnName("bik").IsRequired(false);
        builder.Property(p => p.Date).HasColumnName("date").IsRequired(false);
        builder.Property(p => p.Count).HasColumnName("count").IsRequired(false);
        builder.Property(p => p.Country).HasColumnName("country").IsRequired(false);
    }
}
