﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoreManagement.Data;

#nullable disable

namespace StoreManagement.Migrations
{
    [DbContext(typeof(StoreContext))]
    [Migration("20231024043135_fixedWrongDataType")]
    partial class fixedWrongDataType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("StoreManagement.Shared.Models.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("StoreManagement.Shared.Models.MarkdownPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly?>("CurrentSaleDate")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FinalReduction")
                        .HasColumnType("TEXT");

                    b.Property<bool>("InitialCompleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InitialInventory")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("InitialReduction")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IntermediateCompleted")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("IntermidiateReduction")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("SaleEnded")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("MarkdownPlans");
                });

            modelBuilder.Entity("StoreManagement.Shared.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("Upc")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("StoreManagement.Shared.Models.SalesDatum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Margin")
                        .HasColumnType("TEXT");

                    b.Property<int>("MarkdownPlanId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RemainingInventory")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("SalesDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalProfit")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalSold")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MarkdownPlanId");

                    b.ToTable("SalesData");
                });

            modelBuilder.Entity("StoreManagement.Shared.Models.Inventory", b =>
                {
                    b.HasOne("StoreManagement.Shared.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("StoreManagement.Shared.Models.MarkdownPlan", b =>
                {
                    b.HasOne("StoreManagement.Shared.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("StoreManagement.Shared.Models.SalesDatum", b =>
                {
                    b.HasOne("StoreManagement.Shared.Models.MarkdownPlan", "MarkdownPlan")
                        .WithMany()
                        .HasForeignKey("MarkdownPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MarkdownPlan");
                });
#pragma warning restore 612, 618
        }
    }
}