using Verba.Identity.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verba.Identity.Db.Data.EntityConfigurations;

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(b => b.ConcurrencyStamp)
            .HasColumnName("concurrency_stamp")
            .IsConcurrencyToken();
        builder.Property(b => b.Name).HasColumnName("name").HasMaxLength(256);
        builder.Property(b => b.NormalizedName).HasColumnName("normalized_name").HasMaxLength(256);

        builder.HasIndex("NormalizedName")
            .IsUnique()
            .HasDatabaseName("RoleNameIndex")
            .HasFilter("normalized_name IS NOT NULL");
    }
}
