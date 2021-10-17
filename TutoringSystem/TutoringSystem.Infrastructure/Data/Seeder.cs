using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Infrastructure.Data
{
    public class Seeder
    {
        private readonly AppDbContext dbContext;
        private readonly IPasswordHasher<User> passwordHasher;

        public Seeder(AppDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
        }

        public void Seed()
        {
            if (!dbContext.Users.Any())
                InsertSampleData();
        }

        private void InsertSampleData()
        {
            var orders = new List<AdditionalOrder>
            {
                new AdditionalOrder
                {
                    Name = "Projekt C++ Terminarz",
                    Cost = 300.0,
                    IsPaid = false,
                    ReceiptDate = new DateTime(2021, 06, 12),
                    Deadline = new DateTime(2021, 07, 01),
                    Status = AdditionalOrderStatus.Realized,
                    Description = "Projekt programistyczny dotyczący stworzenia bazodanowego projektu okienkowego \"Terminarz\""
                },
                new AdditionalOrder
                {
                    Name = "Projekt C# Biuro matrymonialne",
                    Cost = 150.0,
                    IsPaid = false,
                    ReceiptDate = new DateTime(2021, 06, 28),
                    Deadline = new DateTime(2021, 07, 16),
                    Status = AdditionalOrderStatus.Realized,
                    Description = "Projekt programistyczny mający na celu stworzenie aplikacji do obsługi biura matrymonialnego"
                }
            };

            var programming = new Subject
            {
                Name = "Programowanie",
                Category = SubjectCategory.Informatics,
                Description = "Podstawy programowania w językach C++, C# oraz Java",
                Place = SubjectPlace.Online
            };
            var math = new Subject
            {
                Name = "Matematyka",
                Category = SubjectCategory.Math,
                Description = "Matematyka ma poziomie szkoły podstawowej oraz podstawy w szkole średniej",
                Place = SubjectPlace.AtTutor
            };

            var tutor1 = new Tutor
            {
                AdditionalOrders = orders,

                Contact = new Contact
                {
                    DiscordName = "marekes99#3923",
                    Email = "marekes97@gmail.com",
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber
                        {
                            Owner = "Me",
                            Number = "100200300"
                        }
                    }
                },
                Address = new Address
                {
                    Street = "Ulica",
                    HouseAndFlatNumber = "1/1",
                    City = "Gliwice",
                    PostalCode = "44-100"
                },
                Subjects = new List<Subject>
                {
                    programming,
                    math
                },

                Role = Role.Tutor,
                FirstName = "Marek",
                LastName = "Sroczkowski",
                Username = "Admin"
            };
            tutor1.PasswordHash = passwordHasher.HashPassword(tutor1, "1234");

            var janKowalski = new Student
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Username = "JanKow",
                Role = Role.Student,
                HourlRate = 40,
                Contact = new Contact
                {
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber
                        {
                            Owner = "Mama Jana",
                            Number = "111111222"
                        }
                    }
                },
            };
            janKowalski.PasswordHash = passwordHasher.HashPassword(janKowalski, "1234");

            var jacekNowak = new Student
            {
                FirstName = "Jacek",
                LastName = "Nowak",
                Username = "JacNow",
                Role = Role.Student,
                HourlRate = 50.0,
                Contact = new Contact
                {
                    DiscordName = "jacek#1234"
                },
            };
            jacekNowak.PasswordHash = passwordHasher.HashPassword(jacekNowak, "1234");

            var r1 = new SingleReservation
            {
                StartTime = new DateTime(2021, 09, 04),
                Description = "Ciągi arytmetyczne",
                Duration = 90,
                Student = janKowalski,
                Tutor = tutor1,
                Place = ReservationPlace.AtTutor,
                Subject = math
            };
            r1.Cost = (r1.Duration / 60.0) * r1.Student.HourlRate;
            math.Reservations = new List<Reservation> { r1 };

            var r2 = new SingleReservation
            {
                StartTime = new DateTime(2021, 09, 05),
                Description = "Omówienie tablic w języku C++",
                Duration = 120,
                Student = jacekNowak,
                Tutor = tutor1,
                Place = ReservationPlace.Online
            };
            r2.Cost = (r2.Duration / 60.0) * r2.Student.HourlRate;
            programming.Reservations = new List<Reservation> { r2 };

            tutor1.Reservations = new List<Reservation> { r1, r2 };
            janKowalski.Reservations = new List<Reservation> { r1 };
            jacekNowak.Reservations = new List<Reservation> { r2 };

            tutor1.Students = new List<Student> { janKowalski, jacekNowak };

            janKowalski.Tutors = new List<Tutor> { tutor1 };
            jacekNowak.Tutors = new List<Tutor> { tutor1 };

            dbContext.Tutors.Add(tutor1);
            dbContext.Students.Add(janKowalski);
            dbContext.Students.Add(jacekNowak);

            dbContext.SaveChanges();
        }
    }
}
