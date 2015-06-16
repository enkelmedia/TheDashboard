using System;

namespace TheDashboard.Core.Extensions
{
    public static class DateExtensions
    {
        public static bool IsSameMinuteAs(this DateTime date, DateTime checkDate)
        {
            return date.Date == checkDate.Date && date.Hour == checkDate.Hour && date.Minute == checkDate.Minute;
        }
    }
}