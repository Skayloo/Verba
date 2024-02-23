using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Verba.Stock.Domain.Models.ForeignAccounts;

namespace Verba.Stock.Db.Data.EntityConfigurations
{
    public class ForeignAccountsEntityTypeConfiguration : IEntityTypeConfiguration<ForeignAccounts>
    {
        public void Configure(EntityTypeBuilder<ForeignAccounts> builder)
        {
            builder.ToTable("foreign_accounts");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.CreatedDatetime).HasColumnName("created_datetime").IsRequired(false);
            builder.Property(p => p.ModifiedDatetime).HasColumnName("modified_datetime").IsRequired(false);
            builder.Property(p => p.IsDeleted).HasColumnName("is_deleted").IsRequired(false);
            builder.Property(p => p.AccountNumber).HasColumnName("account_number").IsRequired(false);
            builder.Property(p => p.Description).HasColumnName("description").IsRequired(false);
        }
    }
}
