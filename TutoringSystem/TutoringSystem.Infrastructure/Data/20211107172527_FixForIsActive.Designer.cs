﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20211107172527_FixForIsActive")]
    partial class FixForIsActive
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StudentTutor", b =>
                {
                    b.Property<long>("StudentsId")
                        .HasColumnType("bigint");

                    b.Property<long>("TutorsId")
                        .HasColumnType("bigint");

                    b.HasKey("StudentsId", "TutorsId");

                    b.HasIndex("TutorsId");

                    b.ToTable("StudentTutor");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.ActivationToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("TokenContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ActivationTokens");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.AdditionalOrder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReceiptDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long>("TutorId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TutorId");

                    b.ToTable("AdditionalOrders");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Address", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HouseAndFlatNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Availability", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BreakTime")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<long>("TutorId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TutorId");

                    b.ToTable("Availabilities");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Contact", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DiscordName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Interval", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AvailabilityId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AvailabilityId");

                    b.ToTable("Intervals");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Message", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateRead")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<bool>("RecipientDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("RecipientId")
                        .HasColumnType("bigint");

                    b.Property<bool>("SenderDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("SenderId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.PhoneNumber", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ContactId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Owner")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("PhoneNumbers");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.RepeatedReservation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Frequency")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastAddedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NextAddedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("RepeatedReservations");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Reservation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<int>("Place")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("StudentId")
                        .HasColumnType("bigint");

                    b.Property<long>("SubjectId")
                        .HasColumnType("bigint");

                    b.Property<long>("TutorId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TutorId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Subject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Place")
                        .HasColumnType("int");

                    b.Property<long>("TutorId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TutorId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEnable")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.RecurringReservation", b =>
                {
                    b.HasBaseType("TutoringSystem.Domain.Entities.Reservation");

                    b.Property<int>("Frequency")
                        .HasColumnType("int");

                    b.Property<long>("RepeatedReservationId")
                        .HasColumnType("bigint");

                    b.HasIndex("RepeatedReservationId");

                    b.ToTable("RecurringReservations");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.SingleReservation", b =>
                {
                    b.HasBaseType("TutoringSystem.Domain.Entities.Reservation");

                    b.ToTable("SingleReservations");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Student", b =>
                {
                    b.HasBaseType("TutoringSystem.Domain.Entities.User");

                    b.Property<double>("HourlRate")
                        .HasColumnType("float");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Tutor", b =>
                {
                    b.HasBaseType("TutoringSystem.Domain.Entities.User");

                    b.ToTable("Tutors");
                });

            modelBuilder.Entity("StudentTutor", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TutoringSystem.Domain.Entities.Tutor", null)
                        .WithMany()
                        .HasForeignKey("TutorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.ActivationToken", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.User", "User")
                        .WithMany("ActivationTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.AdditionalOrder", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.Tutor", "Tutor")
                        .WithMany("AdditionalOrders")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tutor");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Address", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.User", "User")
                        .WithOne("Address")
                        .HasForeignKey("TutoringSystem.Domain.Entities.Address", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Availability", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.Tutor", "Tutor")
                        .WithMany("Availabilities")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tutor");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Contact", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.User", "User")
                        .WithOne("Contact")
                        .HasForeignKey("TutoringSystem.Domain.Entities.Contact", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Interval", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.Availability", "Availability")
                        .WithMany("Intervals")
                        .HasForeignKey("AvailabilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Availability");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Message", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.User", "Recipient")
                        .WithMany("MessagesRecived")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TutoringSystem.Domain.Entities.User", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Recipient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.PhoneNumber", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.Contact", "Contact")
                        .WithMany("PhoneNumbers")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Reservation", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.Student", "Student")
                        .WithMany("Reservations")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TutoringSystem.Domain.Entities.Subject", "Subject")
                        .WithMany("Reservations")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TutoringSystem.Domain.Entities.Tutor", "Tutor")
                        .WithMany("Reservations")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Subject");

                    b.Navigation("Tutor");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Subject", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.Tutor", "Tutor")
                        .WithMany("Subjects")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tutor");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.RecurringReservation", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.Reservation", null)
                        .WithOne()
                        .HasForeignKey("TutoringSystem.Domain.Entities.RecurringReservation", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("TutoringSystem.Domain.Entities.RepeatedReservation", "Reservation")
                        .WithMany("Reservations")
                        .HasForeignKey("RepeatedReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.SingleReservation", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.Reservation", null)
                        .WithOne()
                        .HasForeignKey("TutoringSystem.Domain.Entities.SingleReservation", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Student", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.User", null)
                        .WithOne()
                        .HasForeignKey("TutoringSystem.Domain.Entities.Student", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Tutor", b =>
                {
                    b.HasOne("TutoringSystem.Domain.Entities.User", null)
                        .WithOne()
                        .HasForeignKey("TutoringSystem.Domain.Entities.Tutor", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Availability", b =>
                {
                    b.Navigation("Intervals");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Contact", b =>
                {
                    b.Navigation("PhoneNumbers");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.RepeatedReservation", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Subject", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.User", b =>
                {
                    b.Navigation("ActivationTokens");

                    b.Navigation("Address");

                    b.Navigation("Contact");

                    b.Navigation("MessagesRecived");

                    b.Navigation("MessagesSent");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Student", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("TutoringSystem.Domain.Entities.Tutor", b =>
                {
                    b.Navigation("AdditionalOrders");

                    b.Navigation("Availabilities");

                    b.Navigation("Reservations");

                    b.Navigation("Subjects");
                });
#pragma warning restore 612, 618
        }
    }
}
