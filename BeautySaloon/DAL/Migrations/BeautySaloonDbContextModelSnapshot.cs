﻿// <auto-generated />
using System;
using BeautySaloon.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BeautySaloon.DAL.Migrations
{
    [DbContext(typeof(BeautySaloonDbContext))]
    partial class BeautySaloonDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DurationInMinutes")
                        .HasColumnType("integer");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserModifierId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("UserModifierId");

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.CosmeticService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int?>("ExecuteTimeInMinutes")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserModifierId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("CosmeticService");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("InvoiceDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("InvoiceType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserModifierId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.InvoiceMaterial", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal?>("Cost")
                        .HasColumnType("numeric");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("InvoiceId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MaterialId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserModifierId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("MaterialId");

                    b.ToTable("InvoiceMaterial");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Material", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserModifierId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Material");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Number"));

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("integer");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uuid");

                    b.Property<string>("SpInvoiceId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserModifierId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("UserModifierId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserModifierId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.PersonSubscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AppointmentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.HasIndex("OrderId");

                    b.ToTable("PersonSubscription");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("LifeTimeInDays")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserModifierId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.SubscriptionCosmeticService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CosmeticServiceId")
                        .HasColumnType("uuid");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("SubscriptionId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserModifierId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CosmeticServiceId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("SubscriptionCosmeticService");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<Guid?>("RefreshSecretKey")
                        .HasColumnType("uuid");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Appointment", b =>
                {
                    b.HasOne("BeautySaloon.DAL.Entities.Person", "Person")
                        .WithMany("Appointments")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautySaloon.DAL.Entities.User", "Modifier")
                        .WithMany()
                        .HasForeignKey("UserModifierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Modifier");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Invoice", b =>
                {
                    b.HasOne("BeautySaloon.DAL.Entities.User", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.InvoiceMaterial", b =>
                {
                    b.HasOne("BeautySaloon.DAL.Entities.Invoice", "Invoice")
                        .WithMany("InvoiceMaterials")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautySaloon.DAL.Entities.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");

                    b.Navigation("Material");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Order", b =>
                {
                    b.HasOne("BeautySaloon.DAL.Entities.Person", "Person")
                        .WithMany("Orders")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautySaloon.DAL.Entities.User", "Modifier")
                        .WithMany()
                        .HasForeignKey("UserModifierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Modifier");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Person", b =>
                {
                    b.OwnsOne("BeautySaloon.DAL.Entities.ValueObjects.FullName", "Name", b1 =>
                        {
                            b1.Property<Guid>("PersonId")
                                .HasColumnType("uuid");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.Property<string>("MiddleName")
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.HasKey("PersonId");

                            b1.ToTable("Person");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.PersonSubscription", b =>
                {
                    b.HasOne("BeautySaloon.DAL.Entities.Appointment", null)
                        .WithMany("PersonSubscriptions")
                        .HasForeignKey("AppointmentId");

                    b.HasOne("BeautySaloon.DAL.Entities.Order", "Order")
                        .WithMany("PersonSubscriptions")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("BeautySaloon.DAL.Entities.ValueObjects.SubscriptionCosmeticServiceSnapshot", "SubscriptionCosmeticServiceSnapshot", b1 =>
                        {
                            b1.Property<Guid>("PersonSubscriptionId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Count")
                                .HasColumnType("integer");

                            b1.HasKey("PersonSubscriptionId");

                            b1.ToTable("PersonSubscription");

                            b1.WithOwner()
                                .HasForeignKey("PersonSubscriptionId");

                            b1.OwnsOne("BeautySaloon.DAL.Entities.ValueObjects.CosmeticServiceSnapshot", "CosmeticServiceSnapshot", b2 =>
                                {
                                    b2.Property<Guid>("SubscriptionCosmeticServiceSnapshotPersonSubscriptionId")
                                        .HasColumnType("uuid");

                                    b2.Property<string>("Description")
                                        .HasMaxLength(500)
                                        .HasColumnType("character varying(500)");

                                    b2.Property<int?>("ExecuteTimeInMinutes")
                                        .HasColumnType("integer");

                                    b2.Property<Guid>("Id")
                                        .HasColumnType("uuid");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.HasKey("SubscriptionCosmeticServiceSnapshotPersonSubscriptionId");

                                    b2.ToTable("PersonSubscription");

                                    b2.WithOwner()
                                        .HasForeignKey("SubscriptionCosmeticServiceSnapshotPersonSubscriptionId");
                                });

                            b1.OwnsOne("BeautySaloon.DAL.Entities.ValueObjects.SubscriptionSnapshot", "SubscriptionSnapshot", b2 =>
                                {
                                    b2.Property<Guid>("SubscriptionCosmeticServiceSnapshotPersonSubscriptionId")
                                        .HasColumnType("uuid");

                                    b2.Property<Guid>("Id")
                                        .HasColumnType("uuid");

                                    b2.Property<int?>("LifeTimeInDays")
                                        .HasColumnType("integer");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.Property<decimal>("Price")
                                        .HasColumnType("numeric");

                                    b2.HasKey("SubscriptionCosmeticServiceSnapshotPersonSubscriptionId");

                                    b2.ToTable("PersonSubscription");

                                    b2.WithOwner()
                                        .HasForeignKey("SubscriptionCosmeticServiceSnapshotPersonSubscriptionId");
                                });

                            b1.Navigation("CosmeticServiceSnapshot")
                                .IsRequired();

                            b1.Navigation("SubscriptionSnapshot")
                                .IsRequired();
                        });

                    b.Navigation("Order");

                    b.Navigation("SubscriptionCosmeticServiceSnapshot")
                        .IsRequired();
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.SubscriptionCosmeticService", b =>
                {
                    b.HasOne("BeautySaloon.DAL.Entities.CosmeticService", "CosmeticService")
                        .WithMany()
                        .HasForeignKey("CosmeticServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautySaloon.DAL.Entities.Subscription", "Subscription")
                        .WithMany("SubscriptionCosmeticServices")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CosmeticService");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.User", b =>
                {
                    b.OwnsOne("BeautySaloon.DAL.Entities.ValueObjects.FullName", "Name", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.Property<string>("MiddleName")
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.HasKey("UserId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Appointment", b =>
                {
                    b.Navigation("PersonSubscriptions");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Invoice", b =>
                {
                    b.Navigation("InvoiceMaterials");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Order", b =>
                {
                    b.Navigation("PersonSubscriptions");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Person", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("BeautySaloon.DAL.Entities.Subscription", b =>
                {
                    b.Navigation("SubscriptionCosmeticServices");
                });
#pragma warning restore 612, 618
        }
    }
}
