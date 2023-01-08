namespace BudgetApp.Domain;

public class TimeService
{
    public static DateTime Now => DateTime.UtcNow;

    public static DateTime NowLocal => ConvertToLocalTime(Now);
        
    public static DateTime ConvertToLocalTime(DateTime utc)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZone);
    }
        
    public static DateTime ConvertToUtcTime(DateTime localTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(localTime, TimeZone);
    }
        
    private static TimeZoneInfo _timeZone = null;

    public static TimeZoneInfo TimeZone
    {
        get
        {
            if (_timeZone == null)
            {
                try
                {
                    _timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
                }
                catch
                {
                    _timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
                }
            }

            return _timeZone;
        }
    }
}