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
        public DbSet<RentModel> RentModels { get; set; }
        public DbSet<HolderModel> Holders { get; set; }
        public DbSet<HolderRentModel> HolderRents { get; set; }
        public DbSet<PowerbankModel> Powerbanks { get; set; }
        public DbSet<PowerbankSessionModel> PowerbankSessions { get; set; }

        public AppRepository(DbContextOptions<AppRepository> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserModel>()
                .HasData(new UserModel { Id = 1, Login = "admin", Password = "admin" });
            modelBuilder.Entity<RentModel>()
                .HasData(new RentModel { Id = 1, RentStrategy = RentStrategy.Hour, FirstHourFree = false });
            modelBuilder.Entity<RentModel>()
                .HasData(new RentModel { Id = 2, RentStrategy = RentStrategy.Hour, FirstHourFree = true });
            modelBuilder.Entity<RentModel>()
                .HasData(new RentModel { Id = 3, RentStrategy = RentStrategy.Day, FirstHourFree = false });
            modelBuilder.Entity<RentModel>()
                .HasData(new RentModel { Id = 4, RentStrategy = RentStrategy.Day, FirstHourFree = true });
            modelBuilder.Entity<VerificationCodeModel>()
                .HasOne(x => x.Costumer)
                .WithMany(x => x.Verifications)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CostumerAuthorizationModel>()
                .HasOne(x => x.Costumer)
                .WithMany(x => x.Authorizations)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PowerbankSessionModel>()
                .HasOne(x => x.Powerbank)
                .WithMany(x => x.Sessions)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PowerbankModel>()
                .HasOne(x => x.Holder)
                .WithMany(x => x.Powerbanks)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<HolderRentModel>()
                .HasOne(x => x.HolderModel)
                .WithMany(x => x.HolderRentModels)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<HolderRentModel>()
                .HasOne(x => x.RentModel)
                .WithMany(x => x.HolderRentModels)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
