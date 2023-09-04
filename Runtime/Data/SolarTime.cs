namespace Chimp.Solar.Data
{
    [System.Serializable]
    public class SolarTime
    {
        /// <summary>
        ///     All of them are in Local Solar Time (LST)
        /// </summary>
        public string SolarNoon;
        public string SunriseTime;
        public string SunsetTime;

        public float SunlightDurationAsMinutes;
    }
}
