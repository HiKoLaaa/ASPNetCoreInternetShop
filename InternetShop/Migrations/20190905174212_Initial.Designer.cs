﻿// <auto-generated />
using System;
using InternetShop.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InternetShop.Migrations
{
    [DbContext(typeof(ProductDbContext))]
    [Migration("20190905174212_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InternetShop.Models.Customer", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<int>("Discount");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("InternetShop.Models.Order", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CustomerID");

                    b.Property<DateTime>("OrderDate");

                    b.Property<int>("OrderNumber");

                    b.Property<int>("ProductCount");

                    b.Property<DateTime>("ShipmentDate");

                    b.Property<int>("Status");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("InternetShop.Models.OrderProduct", b =>
                {
                    b.Property<Guid>("OrderID");

                    b.Property<Guid>("ProductID");

                    b.HasKey("OrderID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("OrderProduct");
                });

            modelBuilder.Entity("InternetShop.Models.Product", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category")
                        .HasMaxLength(25);

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.HasKey("ID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("InternetShop.Models.Order", b =>
                {
                    b.HasOne("InternetShop.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InternetShop.Models.OrderProduct", b =>
                {
                    b.HasOne("InternetShop.Models.Order", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InternetShop.Models.Product", "Product")
                        .WithMany("Orders")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
