using Unity.Mathematics;

using Chimp.Solar.Data;
using Chimp.Solar.Extensions;

namespace Chimp.Solar
{
    public class SolarCalculator
    {
        private class SolarData
        {
            public readonly float LatitudeAsRadians;
            public readonly float Longitude;
            public readonly float TimeZone;

            public readonly System.DateTime DateTime;

            public readonly double JulianDay;
            public readonly double JulianCentury;

            public readonly double GeomMeanLongSunAsDegrees;
            public readonly double GeomMeanLongSunAsRadians;
            public readonly double GeomMeanAnomSunAsDegrees;
            public readonly double GeomMeanAnomSunAsRadians;
            public readonly double EccentEarthOrbit;
            public readonly double SunEqOfCtr;
            public readonly double SunTrueLongAsDegrees;
            public readonly double SunAppLongAsDegrees;
            public readonly double SunAppLongAsRadians;
            public readonly double MeanObliqEclipticAsDegrees;
            public readonly double ObliqCorrAsDegrees;
            public readonly double ObliqCorrAsRadians;
            public readonly double SunDeclinAsRadians;
            public readonly double VarY;
            public readonly double EqOfTimeAsMinutes;
            public readonly double HASunriseAsDegrees;

            public SolarData(float latitude, float longitude, float timeZone, System.DateTime dateTime)
            {
                LatitudeAsRadians = math.radians(latitude);
                Longitude = longitude;
                TimeZone = timeZone;

                DateTime = dateTime;

                JulianDay = dateTime.ToJulianDay(timeZone);
                JulianCentury = (JulianDay - 2451545.0) / 36525.0;

                GeomMeanLongSunAsDegrees = GetGeomMeanLongSunAsDegrees();
                GeomMeanLongSunAsRadians = math.radians(GeomMeanLongSunAsDegrees);
                GeomMeanAnomSunAsDegrees = GetGeomMeanAnomSunDegAsDegrees();
                GeomMeanAnomSunAsRadians = math.radians(GeomMeanAnomSunAsDegrees);
                EccentEarthOrbit = GetEccentEarthOrbit();
                SunEqOfCtr = GetSunEqOfCtr();
                SunTrueLongAsDegrees = GetSunTrueLongAsDegrees();
                SunAppLongAsDegrees = GetSunAppLongAsDegrees();
                SunAppLongAsRadians = math.radians(SunAppLongAsDegrees);
                MeanObliqEclipticAsDegrees = GetMeanObliqEclipticAsDegrees();
                ObliqCorrAsDegrees = GetObliqCorrAsDegrees();
                ObliqCorrAsRadians = math.radians(ObliqCorrAsDegrees);
                SunDeclinAsRadians = GetSunDeclinAsRadians();
                VarY = GetVarY();
                EqOfTimeAsMinutes = GetEqOfTimeAsMinutes();
                HASunriseAsDegrees = GetHASunriseAsDegrees();
            }

            // mean longitude of the sun
            private double GetGeomMeanLongSunAsDegrees()
            {
                return (280.46646 + JulianCentury * (36000.76983 + JulianCentury * 0.0003032)) % 360.0;
            }

            // mean anomaly of the sun
            private double GetGeomMeanAnomSunDegAsDegrees()
            {
                return 357.52911 + JulianCentury * (35999.05029 - 0.0001537 * JulianCentury);
            }

            // orbital eccentricity of the earth
            private double GetEccentEarthOrbit()
            {
                return 0.016708634 - JulianCentury * (0.000042037 + 0.0000001267 * JulianCentury);
            }

            // the sun's equation of the center
            private double GetSunEqOfCtr()
            {
                return math.sin(GeomMeanAnomSunAsRadians) * (1.914602 - JulianCentury * (0.004817 + 0.000014 * JulianCentury))
                    + math.sin(2.0 * GeomMeanAnomSunAsRadians) * (0.019993 - 0.000101 * JulianCentury)
                    + math.sin(3.0 * GeomMeanAnomSunAsRadians) * 0.000289;
            }

            // the sun's true longitude
            private double GetSunTrueLongAsDegrees()
            {
                return GeomMeanLongSunAsDegrees + SunEqOfCtr;
            }

            // the sun's apparent longitude
            private double GetSunAppLongAsDegrees()
            {
                return SunTrueLongAsDegrees - 0.00569 - 0.00478 * math.sin(math.radians(125.04 - 1934.136 * JulianCentury));
            }

            // the earth's mean obliquity of the ecliptic
            private double GetMeanObliqEclipticAsDegrees()
            {
                return 23.0 + (26.0 + ((21.448 - JulianCentury * (46.815 + JulianCentury * (0.00059 - JulianCentury * 0.001813)))) / 60.0) / 60.0;
            }

            // oblique correction
            private double GetObliqCorrAsDegrees()
            {
                return MeanObliqEclipticAsDegrees + 0.00256 * math.cos(math.radians(125.04 - 1934.136 * JulianCentury));
            }

            // the sun's declination
            private double GetSunDeclinAsRadians()
            {
                return math.asin(math.sin(ObliqCorrAsRadians) * math.sin(SunAppLongAsRadians));
            }

            private double GetVarY()
            {
                return math.pow(math.tan(ObliqCorrAsRadians / 2.0), 2.0);
            }

            // equation of time
            private double GetEqOfTimeAsMinutes()
            {
                return 4.0 * math.degrees(
                    VarY * math.sin(2.0 * GeomMeanLongSunAsRadians)
                    - 2.0 * EccentEarthOrbit * math.sin(GeomMeanAnomSunAsRadians)
                    + 4.0 * EccentEarthOrbit * VarY * math.sin(GeomMeanAnomSunAsRadians) * math.cos(2.0 * GeomMeanLongSunAsRadians)
                    - 0.5 * VarY * VarY * math.sin(4.0 * GeomMeanLongSunAsRadians)
                    - 1.25 * EccentEarthOrbit * EccentEarthOrbit * math.sin(2.0 * GeomMeanAnomSunAsRadians)
                );
            }

            // hour angle of the sunrise
            private double GetHASunriseAsDegrees()
            {
                return math.degrees(
                    math.acos(
                        math.cos(math.radians(90.833)) / (math.cos(LatitudeAsRadians) * math.cos(SunDeclinAsRadians))
                        - math.tan(LatitudeAsRadians) * math.tan(SunDeclinAsRadians)
                    )
                );
            }

            // local time of solar noon
            public double GetSolarNoon()
            {
                return (720.0 - 4.0 * Longitude - EqOfTimeAsMinutes + TimeZone * 60.0) / 1440.0;
            }

            // local time of the sunrise
            public double GetSunriseTime()
            {
                return GetSolarNoon() - HASunriseAsDegrees * 4.0 / 1440.0;
            }

            // local time of the sunset
            public double GetSunsetTime()
            {
                return GetSolarNoon() + HASunriseAsDegrees * 4.0 / 1440.0;
            }

            public double GetSunlightDurationAsMinutes()
            {
                return HASunriseAsDegrees * 8.0f;
            }

            // true solar time
            private double GetTrueSolarTimeAsMinutes()
            {
                return (DateTime.TimeOfDay.TotalMinutes + EqOfTimeAsMinutes + 4.0 * Longitude - 60.0 * TimeZone) % 1440.0;
            }

            // hour angle of current time
            private double GetHourAngleAsDegrees()
            {
                double trueSolarTimeAsMinutes = GetTrueSolarTimeAsMinutes();
                return trueSolarTimeAsMinutes < 0 ? trueSolarTimeAsMinutes / 4.0 + 180.0 : trueSolarTimeAsMinutes / 4.0 - 180.0;
            }

            // solar zenith angle
            public double GetSolarZenithAngleAsRadians()
            {
                double hourAngleAsRadians = math.radians(GetHourAngleAsDegrees());

                return math.acos(
                    math.sin(LatitudeAsRadians) * math.sin(SunDeclinAsRadians)
                    + math.cos(LatitudeAsRadians) * math.cos(SunDeclinAsRadians) * math.cos(hourAngleAsRadians)
                );
            }

            public double GetSolarElevationAngleAsDegrees()
            {
                return 90.0 - math.degrees(GetSolarZenithAngleAsRadians());
            }

            // approximated atmospheric refraction
            public double GetApproxAtmosphericRefractionAsDegrees()
            {
                double solarElevationAngleAsDegrees = GetSolarElevationAngleAsDegrees();

                if (solarElevationAngleAsDegrees > 85.0)
                {
                    return 0.0;
                }

                double tanOfSolarElevationAngle = math.tan(math.radians(solarElevationAngleAsDegrees));
                double approxAtmosphericRefractionDeg;
                
                if (solarElevationAngleAsDegrees > 5.0)
                {
                    approxAtmosphericRefractionDeg = (58.1 / tanOfSolarElevationAngle) 
                        - (0.07 / math.pow(tanOfSolarElevationAngle, 3))
                        + 0.000086 / math.pow(tanOfSolarElevationAngle, 5);
                }
                else if (solarElevationAngleAsDegrees > -0.575)
                {
                    approxAtmosphericRefractionDeg = 1735.0 + solarElevationAngleAsDegrees 
                        * (-518.2 + solarElevationAngleAsDegrees 
                            * (103.4 + solarElevationAngleAsDegrees 
                                * (-12.79 + solarElevationAngleAsDegrees * 0.711)));
                }
                else
                {
                    approxAtmosphericRefractionDeg = -20.772 / tanOfSolarElevationAngle;
                }

                approxAtmosphericRefractionDeg /= 3600.0;

                return approxAtmosphericRefractionDeg;
            }

            public double GetSolarAzimuthAngleDegCWFromN()
            {
                double solarZenithAngleAsRadians = GetSolarZenithAngleAsRadians();

                double azimuth = math.degrees(
                    math.acos(
                        ((math.sin(LatitudeAsRadians) * math.cos(solarZenithAngleAsRadians))
                        - math.sin(SunDeclinAsRadians)) / (math.cos(LatitudeAsRadians) * math.sin(solarZenithAngleAsRadians))
                    )
                );

                return GetHourAngleAsDegrees() > 0.0 ? (azimuth + 180.0) % 360.0 : (540.0 - azimuth) % 360.0;
            }
        };

        private readonly SolarInputs _inputs;

        public SolarCalculator(SolarInputs inputs)
        {
            _inputs = inputs;
        }

        public void CalculateSolarData(ref SolarPosition solarPositionData)
        {
            var solarData = new SolarData(_inputs.Latitude, _inputs.Longitude, _inputs.TimeZone, _inputs.GetDateTime());

            double solarElevationAngleAsDegrees = solarData.GetSolarElevationAngleAsDegrees();
            double approxAtmosphericRefractionAsDegrees = solarData.GetApproxAtmosphericRefractionAsDegrees();
            double solarElevationCorrectedforATMRefractionAsDegrees = solarElevationAngleAsDegrees + approxAtmosphericRefractionAsDegrees;
            double solarAzimuthAngleDegCWfromN = solarData.GetSolarAzimuthAngleDegCWFromN();

            // populate position data
            solarPositionData.Elevation = (float)solarElevationAngleAsDegrees;
            solarPositionData.CorrectedElevation = (float)solarElevationCorrectedforATMRefractionAsDegrees;
            solarPositionData.AzimuthAngle = (float)solarAzimuthAngleDegCWfromN;
        }

        public void CalculateSolarData(ref SolarTime solarTimeData)
        {
            var solarData = new SolarData(_inputs.Latitude, _inputs.Longitude, _inputs.TimeZone, _inputs.GetDateTime());

            // populate time data
            solarTimeData.SolarNoon = System.TimeSpan.FromDays(solarData.GetSolarNoon()).ToString();
            solarTimeData.SunriseTime = System.TimeSpan.FromDays(solarData.GetSunriseTime()).ToString();
            solarTimeData.SunsetTime = System.TimeSpan.FromDays(solarData.GetSunsetTime()).ToString();
            solarTimeData.SunlightDurationAsMinutes = (float)solarData.GetSunlightDurationAsMinutes();
        }
    }
}
