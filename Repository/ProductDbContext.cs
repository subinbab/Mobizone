using DomainLayer;
using DomainLayer.ProductModel;
using DomainLayer.ProductModel.Master;
using DomainLayer.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Text;

namespace Repository
{
    public class ProductDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ProductDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Product> products { get; set; }
        public DbSet<UserRegistration> userRegistrations { get; set; }
        public DbSet<MasterTable> masterData { get; set; }
        public DbSet<ProductEntity> product { get; set; }
        public DbSet<Login> login { get; set; }
        public DbSet<Order> order { get; set; }
        public DbSet<PrivacyPolicy> privacy { get; set; }
        public DbSet<About> about { get; set; }
        public DbSet<Checkout> checkOut { get; set; }
        public DbSet<Contact> contact { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedUser(builder);

        }
        public void SeedUser(ModelBuilder builder)
        {
            Roles role1 = new Roles()
            {
                id = 1,
                name = "User",
                createdOn = DateTime.Now,
                createdBy = "Subin",
                modifiedOn = DateTime.Now,
                modifiedBy = "Subin"
            };
            Roles role2 = new Roles()
            {
                id = 2,
                name = "Admin",
                createdOn = DateTime.Now,
                createdBy = "Subin",
                modifiedOn = DateTime.Now,
                modifiedBy = "Subin"
            };
            builder.Entity<Roles>().HasData(role1);
            builder.Entity<Roles>().HasData(role2);
        }
    }
    
    
}
