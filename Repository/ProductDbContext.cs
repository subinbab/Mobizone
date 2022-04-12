using DomainLayer;
using DomainLayer.ProductModel;
using DomainLayer.ProductModel.Master;
using DomainLayer.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Text;

namespace Repository
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Product> products { get; set; }
        public DbSet<UserRegistration> userRegistrations { get; set; }
        public DbSet<MasterTable> masterData { get; set; }
        public DbSet<ProductEntity> product { get; set; }
        public DbSet<Login> login { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedUser(builder);

        }
        public void SeedUser(ModelBuilder builder)
        {
            Role role1 = new Role()
            {
                id = 1,
                name = "User",
                createdOn = DateTime.Now,
                createdBy = "Subin",
                modifiedOn = DateTime.Now,
                modifiedBy = "Subin"
            };
            Role role2 = new Role()
            {
                id = 2,
                name = "Admin",
                createdOn = DateTime.Now,
                createdBy = "Subin",
                modifiedOn = DateTime.Now,
                modifiedBy = "Subin"
            };
            builder.Entity<Role>().HasData(role1);
            builder.Entity<Role>().HasData(role2);
        }
    }
    
    
}
