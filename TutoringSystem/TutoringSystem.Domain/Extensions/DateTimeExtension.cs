using System;

namespace TutoringSystem.Domain.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToLocal(this DateTime dateTime)
        {
            DateTime result = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, "Central European Standard Time");

            return result;
        }
    }
}