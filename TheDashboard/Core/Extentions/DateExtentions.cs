using System;

namespace TheDashboard.Core.Extentions
{
    public static class DateExtentions
    {
        public static bool IsSameMinuteAs(this DateTime date, DateTime checkDate)
        {
            return date.Date == checkDate.Date && date.Hour == checkDate.Hour && date.Minute == checkDate.Minute;
        }
    }
}