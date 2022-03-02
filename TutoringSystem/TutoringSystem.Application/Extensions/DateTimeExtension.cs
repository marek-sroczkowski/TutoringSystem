using System;

namespace TutoringSystem.Application.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToLocal(this DateTime dateTime)
        {
            DateTime result = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, "Europe/Warsaw");

            return result;
        }
    }
}