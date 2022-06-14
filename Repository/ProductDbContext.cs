using BusinessObjectLayer.ProductOperations;
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
        public DbSet<PrivacyPolicy> privacy { get; set; }
        public DbSet<About> about { get; set; }
        public DbSet<Checkout> checkOut { get; set; }
        public DbSet<AdminContact> contact { get; set; }
        public DbSet<Roles> roles { get; set; }
        public DbSet<ProductSubPart> productSubPart { get; set; }
        public DbSet<Address> address { get; set; }
        public DbSet<MyCart> myCart { get; set; }
        public DbSet<CartDetails> cartDetails { get; set; }


    }


}
