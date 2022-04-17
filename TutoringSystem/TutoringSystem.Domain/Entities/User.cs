using System;
using System.Collections.Generic;
using TutoringSystem.Domain.Entities.Base;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Extensions;

namespace TutoringSystem.Domain.Entities
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsEnable { get; set; }
        public Role Role { get; set; }
        public string ProfilePictureFirebaseUrl { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual Address Address { get; set; }
        public virtual PushNotificationToken PushNotificationToken { get; set; }

        public virtual ICollection<ActivationToken> ActivationTokens { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

        public User()
        {
            IsActive = true;
            IsEnable = false;
            RegistrationDate = DateTime.Now.ToLocal();
        }
    }
}