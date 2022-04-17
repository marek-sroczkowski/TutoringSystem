namespace TutoringSystem.Application.Helpers
{
    public class AppSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireDays { get; set; }
        public int RefreshTokenExpireDays { get; set; }
        public int ActivationTokenExpireDays { get; set; }
        public string SmtpEmail { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string ActivationEmailSubject { get; set; }
    }
}