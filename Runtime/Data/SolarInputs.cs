using UnityEngine;

namespace Chimp.Solar.Data
{
    [System.Serializable]
    public class SolarInputs
    {
        [Header("Geographic Location")]
        public float Latitude = 40.72f;
        public float Longitude = 29.47f;
        public float TimeZone = 3.0f;

        [Header("Date")]
        public int Year = 2023;
        public int Month = 9;
        public int Day = 1;

        [Header("Time")]
        [Range(0, 23)] public int Hour = 0;
        [Range(0, 59)] public int Minute = 0;
        [Range(0, 59)] public int Second = 0;

        public System.DateTime GetDateTime()
        {
            return new System.DateTime(Year, Month, Day, Hour, Minute, Second);
        }

        public void SetDateTime(System.DateTime dateTime)
        {
            Year = dateTime.Year;
            Month = dateTime.Month;
            Day = dateTime.Day;
            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Second;
        }
    }
}
