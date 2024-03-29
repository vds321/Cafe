﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Storage.Context;

namespace Storage.Migrations
{
    [DbContext(typeof(CafeDB))]
    [Migration("20211007163304_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Storage.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("category");
                });

            modelBuilder.Entity("Storage.Entities.Composition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DishId")
                        .HasColumnType("int")
                        .HasColumnName("Dish_Id");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("Product_Id");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.HasIndex("ProductId");

                    b.ToTable("composition");
                });

            modelBuilder.Entity("Storage.Entities.Dish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("Category_Id");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("dish");
                });

            modelBuilder.Entity("Storage.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DishId")
                        .HasColumnType("int")
                        .HasColumnName("Dish_Id");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("Order_Date");

                    b.Property<int>("OrderNumber")
                        .HasColumnType("int")
                        .HasColumnName("Order_Number");

                    b.Property<int>("Qty")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.ToTable("order");
                });

            modelBuilder.Entity("Storage.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Remains")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("product");
                });

            modelBuilder.Entity("Storage.Entities.Composition", b =>
                {
                    b.HasOne("Storage.Entities.Dish", "Dish")
                        .WithMany("Compositions")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Storage.Entities.Product", "Product")
                        .WithMany("Composition")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Storage.Entities.Dish", b =>
                {
                    b.HasOne("Storage.Entities.Category", "Category")
                        .WithMany("Dishes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Storage.Entities.Order", b =>
                {
                    b.HasOne("Storage.Entities.Dish", "Dish")
                        .WithMany("Orders")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("Storage.Entities.Category", b =>
                {
                    b.Navigation("Dishes");
                });

            modelBuilder.Entity("Storage.Entities.Dish", b =>
                {
                    b.Navigation("Compositions");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Storage.Entities.Product", b =>
                {
                    b.Navigation("Composition");
                });
#pragma warning restore 612, 618
        }
    }
}
