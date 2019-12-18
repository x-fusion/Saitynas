﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Models;

namespace WebApi.Migrations
{
    [DbContext(typeof(WarehouseContext))]
    [Migration("20191218060554_Orders")]
    partial class Orders
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApi.Models.Inventory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("MonetaryValue")
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("Revenue")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("TotalRentDuration")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Inventories");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Inventory");
                });

            modelBuilder.Entity("WebApi.Models.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BicycleRackID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("CreationTime")
                        .HasColumnType("time");

                    b.Property<string>("Customer")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("InventoryID")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("OrderStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RoofRackID")
                        .HasColumnType("int");

                    b.Property<int?>("WheelChainID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("BicycleRackID");

                    b.HasIndex("InventoryID");

                    b.HasIndex("RoofRackID");

                    b.HasIndex("WheelChainID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("WebApi.Models.BicycleRack", b =>
                {
                    b.HasBaseType("WebApi.Models.Inventory");

                    b.Property<int>("Assertion")
                        .HasColumnType("int");

                    b.Property<int>("BikeLimit")
                        .HasColumnType("int");

                    b.Property<double>("LiftPower")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue("BicycleRack");
                });

            modelBuilder.Entity("WebApi.Models.RoofRack", b =>
                {
                    b.HasBaseType("WebApi.Models.Inventory");

                    b.Property<string>("AppearenceDescription")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<bool>("IsLockable")
                        .HasColumnType("bit");

                    b.Property<double>("LiftPower")
                        .HasColumnName("RoofRack_LiftPower")
                        .HasColumnType("float");

                    b.Property<int>("Opening")
                        .HasColumnType("int");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue("RoofRack");
                });

            modelBuilder.Entity("WebApi.Models.WheelChain", b =>
                {
                    b.HasBaseType("WebApi.Models.Inventory");

                    b.Property<double>("ChainThickness")
                        .HasColumnType("float");

                    b.Property<string>("TireDimensions")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("WheelChain");
                });

            modelBuilder.Entity("WebApi.Models.Order", b =>
                {
                    b.HasOne("WebApi.Models.BicycleRack", "BicycleRack")
                        .WithMany()
                        .HasForeignKey("BicycleRackID");

                    b.HasOne("WebApi.Models.Inventory", "Inventory")
                        .WithMany()
                        .HasForeignKey("InventoryID");

                    b.HasOne("WebApi.Models.RoofRack", "RoofRack")
                        .WithMany()
                        .HasForeignKey("RoofRackID");

                    b.HasOne("WebApi.Models.WheelChain", "WheelChain")
                        .WithMany()
                        .HasForeignKey("WheelChainID");
                });
#pragma warning restore 612, 618
        }
    }
}
