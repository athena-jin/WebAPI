﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAPI.Data;

#nullable disable

namespace WebAPI.Migrations
{
    [DbContext(typeof(CustomDbContext))]
    partial class CustomDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("WebAPI.Data.Actor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("BirthTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Actors");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c4e2e6dc-ce2f-424d-a5c4-d7b1c64f9a2b"),
                            BirthTime = new DateTime(1985, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Actor One"
                        },
                        new
                        {
                            Id = new Guid("22377d34-8fe0-4feb-b3f7-d58cd0a44196"),
                            BirthTime = new DateTime(1990, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Actor Two"
                        });
                });

            modelBuilder.Entity("WebAPI.Data.Machine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ConnectorType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<uint>("Port")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Machines");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9be31da9-e66e-48d2-a613-dc0aa7757aab"),
                            Address = "192.168.1.10",
                            ConnectorType = 0,
                            Name = "Machine A",
                            Port = 8080u,
                            Status = 0
                        },
                        new
                        {
                            Id = new Guid("f49b9b89-3372-4852-8e8d-e3a8905671dd"),
                            Address = "192.168.1.11",
                            ConnectorType = 0,
                            Name = "Machine B",
                            Port = 8081u,
                            Status = 2
                        },
                        new
                        {
                            Id = new Guid("7c822da6-330c-4948-8261-894f9658b0cd"),
                            Address = "opc.tcp://localhost",
                            ConnectorType = 2,
                            Name = "Video One",
                            Port = 4840u,
                            Status = 0
                        });
                });

            modelBuilder.Entity("WebAPI.Data.Video", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ActorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ActorId");

                    b.ToTable("Videos");

                    b.HasData(
                        new
                        {
                            Id = new Guid("1564043c-4b5d-46d4-9d1d-09fba4b73dc0"),
                            Name = "Video One"
                        },
                        new
                        {
                            Id = new Guid("5433e5f8-dac4-4948-8cb7-99921d22adf0"),
                            Name = "Video Two"
                        });
                });

            modelBuilder.Entity("WebAPI.Data.Video", b =>
                {
                    b.HasOne("WebAPI.Data.Actor", "Actor")
                        .WithMany()
                        .HasForeignKey("ActorId");

                    b.Navigation("Actor");
                });
#pragma warning restore 612, 618
        }
    }
}
