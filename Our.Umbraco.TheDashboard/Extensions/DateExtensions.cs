using System;

namespace Our.Umbraco.TheDashboard.Extensions
{
    public static class DateExtensions
    {
        public static bool IsSameMinuteAs(this DateTime date, DateTime checkDate)
        {
            //TODO: Why do we need this?
            return date.Date == checkDate.Date && date.Hour == checkDate.Hour && date.Minute == checkDate.Minute;
        }
    }
}