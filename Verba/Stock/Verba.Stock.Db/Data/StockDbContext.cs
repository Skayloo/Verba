using Verba.Stock.Db.Data.EntityConfigurations;
using Verba.Stock.Domain.Models.AccountNumbers;
using Verba.Stock.Domain.Models.CardNumbers;
using Verba.Stock.Domain.Models.EwalletNumber;
using Verba.Stock.Domain.Models.ForeignAccounts;
using Verba.Stock.Domain.Models.Inn;
using Verba.Stock.Domain.Models.Lots;
using Verba.Stock.Domain.Models.Passports;
using Verba.Stock.Domain.Models.PhoneNumbers;
using Verba.Stock.Domain.Models.Snilses;
using Verba.Stock.Domain.Models.Terrorists;
using Microsoft.EntityFrameworkCore;

namespace Verba.Stock.Db.Data
{
    public class StockDbContext : DbContext
    {
        public DbSet<AccountNumbers> AccountNumbers { get; set; }
        public DbSet<CardNumbers> CardNumbers { get; set; }
        public DbSet<EwalletNumbers> EwalletNumbers { get; set; }
        public DbSet<ForeignAccounts> ForeignAccounts { get; set; }
        public DbSet<Inns> Inns { get; set; }
        public DbSet<Lots> Lots { get; set; }
        public DbSet<PassportsHash> PassportsHash { get; set; }
        public DbSet<PhoneNumbers> PhoneNumbers { get; set; }
        public DbSet<SnilsesHash> SnilsesHash { get; set; }
        public DbSet<Terrorists> Terrorists { get; set; }

        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AccountNumbersEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CardNumbersEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EwalletNumbersEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ForeignAccountsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InnsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LotsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PassportsHashEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneNumbersEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SnilsesHashEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TerroristsEntityTypeConfiguration());
        }
    }
}
