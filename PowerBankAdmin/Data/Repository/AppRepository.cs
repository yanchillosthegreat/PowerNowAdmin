using System;
using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Data.Repository
{
    public class AppRepository : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<AuthorizationModel> Authorizations { get; set; }
        public DbSet<CostumerModel> Costumers { get; set; }
        public DbSet<CostumerAuthorizationModel> CostumerAuthorizations { get; set; }
        public DbSet<VerificationCodeModel> VerificationCodes { get; set; }
        public DbSet<HolderModel> Holders { get; set; }
        public DbSet<PowerbankModel> Powerbanks { get; set; }

        public AppRepository(DbContextOptions<AppRepository> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserModel>()
                .HasData(new UserModel { Id = 1, Login = "admin", Password = "admin" });
            modelBuilder.Entity<VerificationCodeModel>()
                .HasOne(x => x.Costumer)
                .WithMany(x => x.Verifications)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CostumerAuthorizationModel>()
                .HasOne(x => x.Costumer)
                .WithMany(x => x.Authorizations)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
