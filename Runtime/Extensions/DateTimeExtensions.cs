namespace Chimp.Solar.Extensions
{
    public static class DateTimeExtensions
    {
        public static double ToJulianDay(this System.DateTime dateTime, double timeZone)
        {
            return dateTime.ToOADate() + 2415018.5 - timeZone / 24.0;
        }
    }
}
