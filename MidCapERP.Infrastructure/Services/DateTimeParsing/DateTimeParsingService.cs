namespace MidCapERP.Infrastructure.Services.DateTimeParsing
{
    public static class DateTimeParsingService
    {
        public static DateTime ConvertToUserTime(DateTime dt, DateTimeKind sourceDateTimeKind = DateTimeKind.Local)
        {
            dt = DateTime.SpecifyKind(dt, sourceDateTimeKind);
            if (sourceDateTimeKind == DateTimeKind.Local && TimeZoneInfo.Local.IsInvalidTime(dt))
                return dt;
            var currentUserTimeZoneInfo = TimeZoneInfo.Local;
            return TimeZoneInfo.ConvertTime(dt, currentUserTimeZoneInfo);
        }

        //private static TimeZoneInfo GetCurrentTimeZone()
        //{
        //    return GetUserTimeZone();
        //}

        //private static TimeZoneInfo GetUserTimeZone()
        //{
        //    return TimeZoneInfo.Local;
        //    TimeZoneInfo timeZoneInfo = null;
        //    var timeZoneId = string.Empty;
        //    if (!string.IsNullOrEmpty(timeZoneId))
        //        timeZoneInfo = FindTimeZoneById(timeZoneId);
        //    return timeZoneInfo ?? TimeZoneInfo.Local;
        //}

        //private TimeZoneInfo FindTimeZoneById(string id)
        //{
        //    return TimeZoneInfo.FindSystemTimeZoneById(id);
        //}
    }
}