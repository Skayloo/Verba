using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Verba.Stock.Domain.Models.Terrorists;

namespace Verba.Stock.Db.Data.EntityConfigurations
{
    public class TerroristsEntityTypeConfiguration : IEntityTypeConfiguration<Terrorists>
    {
        public void Configure(EntityTypeBuilder<Terrorists> builder)
        {
            builder.ToTable("terrorists");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(p => p.CreatedDatetime).HasColumnName("created_datetime").IsRequired(false);
            builder.Property(p => p.ModifiedDatetime).HasColumnName("modified_datetime").IsRequired(false);
            builder.Property(p => p.IsDeleted).HasColumnName("is_deleted").IsRequired(false);
            builder.Property(p => p.FIO).HasColumnName("fio").IsRequired(false);
            builder.Property(p => p.DateOfBirth).HasColumnName("date_of_birth").IsRequired(false);
            builder.Property(p => p.Inn).HasColumnName("inn").IsRequired(false);
            builder.Property(p => p.Ogrn).HasColumnName("ogrn").IsRequired(false);
            builder.Property(p => p.DUL).HasColumnName("dul").IsRequired(false);
            builder.Property(p => p.RegistrationAddress).HasColumnName("registration_address").IsRequired(false);
            builder.Property(p => p.DateOfRequest).HasColumnName("date_of_request").IsRequired(false);
            builder.Property(p => p.DateOfControlStart).HasColumnName("date_of_control_start").IsRequired(false);
            builder.Property(p => p.DateOfControlEnd).HasColumnName("date_of_control_end").IsRequired(false);
            builder.Property(p => p.Note).HasColumnName("note").IsRequired(false);
        }
    }
}
