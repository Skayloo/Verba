using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verba.Identity.Domain.Models;

namespace Verba.Identity.Db.Data.EntityConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.Property(b => b.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.HasKey(b => b.Id);
        builder.Property(b => b.AccessFailedCount).HasColumnName("access_failed_count");
        builder.Property(b => b.ConcurrencyStamp).HasColumnName("concurrency_stamp")
            .IsConcurrencyToken();

        builder.Property(b => b.CreatedDatetime).HasColumnName("created_date_time");
        builder.Property(b => b.Email).HasColumnName("email").HasMaxLength(256);
        builder.Property(b => b.EmailConfirmed).HasColumnName("email_confirmed");
        builder.Property(b => b.LockoutEnabled).HasColumnName("lockout_enabled");
        builder.Property(b => b.LockoutEnd).HasColumnName("lockout_end");
        builder.Property(b => b.NormalizedEmail).HasColumnName("normalized_email").HasMaxLength(256);
        builder.Property(b => b.NormalizedUserName).HasColumnName("normalized_user_name").HasMaxLength(256);
        builder.Property(b => b.PasswordHash).HasColumnName("password_hash");
        builder.Property(b => b.PhoneNumber).HasColumnName("phone_number");
        builder.Property(b => b.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
        builder.Property(b => b.SecurityStamp).HasColumnName("security_stamp");
        builder.Property(b => b.TwoFactorEnabled).HasColumnName("two_factor_enabled");
        builder.Property(b => b.Inn).HasColumnName("inn");
        builder.Property(b => b.OrgName).HasColumnName("org_name");
        builder.Property(b => b.UserName).HasColumnName("user_name").HasMaxLength(256);


        builder.HasIndex("NormalizedEmail")
            .HasDatabaseName("EmailIndex");

        builder.HasIndex("NormalizedUserName")
            .IsUnique()
            .HasDatabaseName("UserNameIndex")
            .HasFilter("normalized_user_name IS NOT NULL");
    }
}
