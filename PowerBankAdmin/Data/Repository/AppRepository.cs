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

        public AppRepository(DbContextOptions<AppRepository> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserModel>().HasData(new UserModel { Id = 1, Login = "admin", Password = "admin" });
        }
    }
}
