﻿// <auto-generated />
using System;
using HealthMed.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HealthMed.Infra.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HealthMed.Domain.Entities.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("ScheduleId", "StartDate", "EndDate", "PatientId")
                        .IsUnique();

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("HealthMed.Domain.Entities.Doctor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("VARCHAR(11)");

                    b.Property<string>("Crm")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(500)");

                    b.HasKey("Id");

                    b.ToTable("Doctor");
                });

            modelBuilder.Entity("HealthMed.Domain.Entities.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("VARCHAR(11)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(500)");

                    b.HasKey("Id");

                    b.ToTable("Patient");
                });

            modelBuilder.Entity("HealthMed.Domain.Entities.Schedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndAvailabilityDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartAvailabilityDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.ToTable("Schedule");
                });

            modelBuilder.Entity("HealthMed.Domain.Entities.Appointment", b =>
                {
                    b.HasOne("HealthMed.Domain.Entities.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthMed.Domain.Entities.Schedule", "Schedule")
                        .WithMany("Appointments")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("HealthMed.Domain.Entities.Schedule", b =>
                {
                    b.HasOne("HealthMed.Domain.Entities.Doctor", "Doctor")
                        .WithMany("Schedules")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("HealthMed.Domain.Entities.Doctor", b =>
                {
                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("HealthMed.Domain.Entities.Patient", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("HealthMed.Domain.Entities.Schedule", b =>
                {
                    b.Navigation("Appointments");
                });
#pragma warning restore 612, 618
        }
    }
}
