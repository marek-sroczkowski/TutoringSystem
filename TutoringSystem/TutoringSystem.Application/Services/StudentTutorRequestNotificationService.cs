using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Threading.Tasks;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class StudentTutorRequestNotificationService : IStudentTutorRequestNotificationService
    {
        private readonly IPushNotificationTokenRepository tokenRepository;
        private readonly IStudentRepository studentRepository;

        public StudentTutorRequestNotificationService(IPushNotificationTokenRepository tokenRepository,
            IStudentRepository studentRepository)
        {
            this.tokenRepository = tokenRepository;
            this.studentRepository = studentRepository;
        }

        public async Task<bool> SendNotificationToTutorDevice(long studentId, long tutorId)
        {
            try
            {
                var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));
                string notificationContent = $"{student.Username} chce dołączyć do Twoich uczniów!";

                return await TrySentNotification(tutorId, notificationContent);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> TrySentNotification(long tutorId, string notificationContent)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("firebase_private_key.json")
            });

            var registrationToken = await tokenRepository.GetTokenAsync(t => t.UserId.Equals(tutorId));
            var message = new Message()
            {
                Token = registrationToken.Token,
                Android = new AndroidConfig { Priority = Priority.High },
                Notification = new Notification()
                {
                    Title = "Nowy uczeń",
                    Body = notificationContent
                }
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            return !string.IsNullOrWhiteSpace(response);
        }
    }
}