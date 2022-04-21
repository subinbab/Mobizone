﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository;

namespace Repository.Migrations
{
    [DbContext(typeof(ProductDbContext))]
    [Migration("20220419092532_seventhCreate")]
    partial class seventhCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("DomainLayer.Login", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("createdBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createdOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("modifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("modifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("roleId")
                        .HasColumnType("int");

                    b.Property<string>("username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("login");
                });

            modelBuilder.Entity("DomainLayer.Order", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("orderId")
                        .HasColumnType("int");

                    b.Property<int>("paymentId")
                        .HasColumnType("int");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.Property<int?>("productid")
                        .HasColumnType("int");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<int?>("usersUserId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("productid");

                    b.HasIndex("usersUserId");

                    b.ToTable("order");
                });

            modelBuilder.Entity("DomainLayer.PrivacyPolicy", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("content")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Privacy");
                });

            modelBuilder.Entity("DomainLayer.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("products");
                });

            modelBuilder.Entity("DomainLayer.ProductModel.Images", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("ProductEntityId")
                        .HasColumnType("int");

                    b.Property<string>("imagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("ProductEntityId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("DomainLayer.ProductModel.Master.MasterTable", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .UseIdentityColumn();

                    b.Property<string>("masterData")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("MasterData");

                    b.Property<int?>("parantId")
                        .HasColumnType("int")
                        .HasColumnName("PerantId");

                    b.HasKey("id");

                    b.ToTable("Master");
                });

            modelBuilder.Entity("DomainLayer.ProductModel.ProductEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("model")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("Model");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("Name");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.Property<string>("productBrand")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("Brand");

                    b.Property<string>("productType")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("Type");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.Property<int?>("specsid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("specsid");

                    b.ToTable("ProductModel");
                });

            modelBuilder.Entity("DomainLayer.ProductModel.Specificatiion", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("camFeatures")
                        .HasColumnType("int");

                    b.Property<string>("core")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("os")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("processor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ram")
                        .HasColumnType("int");

                    b.Property<string>("simType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("storage")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("Specificatiion");
                });

            modelBuilder.Entity("DomainLayer.Roles", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("createdBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createdOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("modifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("modifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            id = 1,
                            createdBy = "Subin",
                            createdOn = new DateTime(2022, 4, 19, 14, 55, 31, 791, DateTimeKind.Local).AddTicks(1324),
                            modifiedBy = "Subin",
                            modifiedOn = new DateTime(2022, 4, 19, 14, 55, 31, 791, DateTimeKind.Local).AddTicks(2106),
                            name = "User"
                        },
                        new
                        {
                            id = 2,
                            createdBy = "Subin",
                            createdOn = new DateTime(2022, 4, 19, 14, 55, 31, 791, DateTimeKind.Local).AddTicks(2775),
                            modifiedBy = "Subin",
                            modifiedOn = new DateTime(2022, 4, 19, 14, 55, 31, 791, DateTimeKind.Local).AddTicks(2780),
                            name = "Admin"
                        });
                });

            modelBuilder.Entity("DomainLayer.Users.Address", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("UserRegistrationUserId")
                        .HasColumnType("int");

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("district")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pincode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("state")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("UserRegistrationUserId");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("DomainLayer.Users.UserRegistration", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("createdBy")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("CreatedBy");

                    b.Property<DateTime>("createdOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedOn");

                    b.Property<string>("modifiedBy")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("ModifiedBy");

                    b.Property<DateTime>("modifiedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedOn");

                    b.HasKey("UserId");

                    b.ToTable("userRegistrations");
                });

            modelBuilder.Entity("DomainLayer.Order", b =>
                {
                    b.HasOne("DomainLayer.ProductModel.ProductEntity", "product")
                        .WithMany()
                        .HasForeignKey("productid");

                    b.HasOne("DomainLayer.Users.UserRegistration", "users")
                        .WithMany()
                        .HasForeignKey("usersUserId");

                    b.Navigation("product");

                    b.Navigation("users");
                });

            modelBuilder.Entity("DomainLayer.ProductModel.Images", b =>
                {
                    b.HasOne("DomainLayer.ProductModel.ProductEntity", "product")
                        .WithMany("images")
                        .HasForeignKey("ProductEntityId");

                    b.Navigation("product");
                });

            modelBuilder.Entity("DomainLayer.ProductModel.ProductEntity", b =>
                {
                    b.HasOne("DomainLayer.ProductModel.Specificatiion", "specs")
                        .WithMany()
                        .HasForeignKey("specsid");

                    b.Navigation("specs");
                });

            modelBuilder.Entity("DomainLayer.Users.Address", b =>
                {
                    b.HasOne("DomainLayer.Users.UserRegistration", null)
                        .WithMany("address")
                        .HasForeignKey("UserRegistrationUserId");
                });

            modelBuilder.Entity("DomainLayer.ProductModel.ProductEntity", b =>
                {
                    b.Navigation("images");
                });

            modelBuilder.Entity("DomainLayer.Users.UserRegistration", b =>
                {
                    b.Navigation("address");
                });
#pragma warning restore 612, 618
        }
    }
}
