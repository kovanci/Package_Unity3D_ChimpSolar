using NUnit.Framework;

using Chimp.Solar;
using Chimp.Solar.Data;

using UnityEngine;

namespace Chimp.Sonar.Editor.Tests
{
    [TestFixture]
    public class SolarCalculatorTests
    {
        public class SolarTestScenario
        {
            public SolarInputs Input;
            public SolarPosition ExpectedPosition;
            public SolarTime ExpectedTime;
        };

        /// <summary>
        ///     The percentage error difference between the expected value and the actual value
        ///     should be less than tolerance
        /// </summary>
        private const float SolarPositionErrorTolerance = 0.001f;
        private const float SolarTimeErrorTolerance = 0.003f;

        /// <summary>
        ///     Output Data
        /// </summary>
        private SolarPosition _actualSolarPosition;
        private SolarTime _actualSolarTime;

        /// <summary>
        ///     Test Inputs & Outputs
        ///     From: https://gml.noaa.gov/grad/solcalc/calcdetails.html (Daily and Yearly tables)
        /// </summary>
        private static readonly SolarTestScenario[] s_correctTestData = new SolarTestScenario[]
        {
            // yearly data
            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -7,
                    Year = 2010,
                    Month = 1,
                    Day = 1,
                    Hour = 12,
                    Minute = 0,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 179.0474507f,
                    Elevation = 27.032624f,
                    CorrectedElevation = 27.06410813f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "12:03:41",
                    SunriseTime = "7:21:57",
                    SunsetTime = "16:45:25",
                    SunlightDurationAsMinutes = 563.4681652f
                }
            },

            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -7,
                    Year = 2010,
                    Month = 6,
                    Day = 6,
                    Hour = 12,
                    Minute = 0,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 180.9989928f,
                    Elevation = 72.69710281f,
                    CorrectedElevation = 72.70212982f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "11:58:43",
                    SunriseTime = "4:31:28",
                    SunsetTime = "19:25:58",
                    SunlightDurationAsMinutes = 894.4956883f
                }
            },

            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -7,
                    Year = 2010,
                    Month = 12,
                    Day = 12,
                    Hour = 12,
                    Minute = 0,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 181.5909104f,
                    Elevation = 26.87532953f,
                    CorrectedElevation = 26.90702632f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "11:53:50",
                    SunriseTime = "7:12:42",
                    SunsetTime = "16:34:57",
                    SunlightDurationAsMinutes = 562.2439904f
                }
            },

            // daily data
            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -6,
                    Year = 2010,
                    Month = 6,
                    Day = 21,
                    Hour = 0,
                    Minute = 6,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 345.8691023f,
                    Elevation = -25.24571849f,
                    CorrectedElevation = -25.23348197f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "13:01:42",
                    SunriseTime = "5:31:16",
                    SunsetTime = "20:32:09",
                    SunlightDurationAsMinutes = 900.882771f
                }
            },

            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -6,
                    Year = 2010,
                    Month = 6,
                    Day = 21,
                    Hour = 6,
                    Minute = 30,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 66.97512151f,
                    Elevation = 9.124360432f,
                    CorrectedElevation = 9.220375537f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "13:01:46",
                    SunriseTime = "5:31:19",
                    SunsetTime = "20:32:12",
                    SunlightDurationAsMinutes = 900.8842304f
                }
            },

            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -6,
                    Year = 2010,
                    Month = 6,
                    Day = 21,
                    Hour = 13,
                    Minute = 0,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 178.53323f,
                    Elevation = 73.43367897f,
                    CorrectedElevation = 73.43847934f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "13:01:49",
                    SunriseTime = "5:31:23",
                    SunsetTime = "20:32:16",
                    SunlightDurationAsMinutes = 900.8813415f
                }
            },

            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -6,
                    Year = 2010,
                    Month = 6,
                    Day = 21,
                    Hour = 19,
                    Minute = 0,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 288.0992729f,
                    Elevation = 15.15457648f,
                    CorrectedElevation = 15.21320184f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "13:01:53",
                    SunriseTime = "5:31:26",
                    SunsetTime = "20:32:19",
                    SunlightDurationAsMinutes = 900.8747658f
                }
            }
        };

        private static readonly SolarTestScenario[] s_incorrectTestData = new SolarTestScenario[]
        {
            // yearly data
            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -7,
                    Year = 2010,
                    Month = 1,
                    Day = 1,
                    Hour = 12,
                    Minute = 0,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 178.0474507f,
                    Elevation = 26.032624f,
                    CorrectedElevation = 26.06410813f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "12:04:41",
                    SunriseTime = "7:22:57",
                    SunsetTime = "16:46:25",
                    SunlightDurationAsMinutes = 564.4681652f
                }
            },

            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -7,
                    Year = 2010,
                    Month = 6,
                    Day = 6,
                    Hour = 12,
                    Minute = 0,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 179.9989928f,
                    Elevation = 71.69710281f,
                    CorrectedElevation = 71.70212982f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "11:59:43",
                    SunriseTime = "4:32:28",
                    SunsetTime = "19:26:58",
                    SunlightDurationAsMinutes = 895.4956883f
                }
            },

            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -7,
                    Year = 2010,
                    Month = 12,
                    Day = 12,
                    Hour = 12,
                    Minute = 0,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 180.5909104f,
                    Elevation = 25.87532953f,
                    CorrectedElevation = 25.90702632f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "11:54:50",
                    SunriseTime = "7:13:42",
                    SunsetTime = "16:35:57",
                    SunlightDurationAsMinutes = 563.2439904f
                }
            },

            // daily data
            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -6,
                    Year = 2010,
                    Month = 6,
                    Day = 21,
                    Hour = 0,
                    Minute = 6,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 344.8691023f,
                    Elevation = -24.24571849f,
                    CorrectedElevation = -24.23348197f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "13:02:42",
                    SunriseTime = "5:32:16",
                    SunsetTime = "20:33:09",
                    SunlightDurationAsMinutes = 901.882771f
                }
            },

            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -6,
                    Year = 2010,
                    Month = 6,
                    Day = 21,
                    Hour = 6,
                    Minute = 30,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 65.97512151f,
                    Elevation = 8.124360432f,
                    CorrectedElevation = 8.220375537f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "13:02:46",
                    SunriseTime = "5:32:19",
                    SunsetTime = "20:33:12",
                    SunlightDurationAsMinutes = 901.8842304f
                }
            },

            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -6,
                    Year = 2010,
                    Month = 6,
                    Day = 21,
                    Hour = 13,
                    Minute = 0,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 177.53323f,
                    Elevation = 72.43367897f,
                    CorrectedElevation = 72.43847934f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "13:02:49",
                    SunriseTime = "5:32:23",
                    SunsetTime = "20:33:16",
                    SunlightDurationAsMinutes = 901.8813415f
                }
            },

            new()
            {
                Input = new SolarInputs
                {
                    Latitude = 40.0f,
                    Longitude = -105.0f,
                    TimeZone = -6,
                    Year = 2010,
                    Month = 6,
                    Day = 21,
                    Hour = 19,
                    Minute = 0,
                    Second = 0
                },
                ExpectedPosition = new SolarPosition
                {
                    AzimuthAngle = 287.0992729f,
                    Elevation = 14.15457648f,
                    CorrectedElevation = 14.21320184f
                },
                ExpectedTime = new SolarTime
                {
                    SolarNoon = "13:02:53",
                    SunriseTime = "5:32:26",
                    SunsetTime = "20:33:19",
                    SunlightDurationAsMinutes = 901.8747658f
                }
            }
        };

        [SetUp]
        public void SetUp()
        {
            _actualSolarPosition = new SolarPosition();
            _actualSolarTime = new SolarTime();
        }

        [Test, TestCaseSource(nameof(s_correctTestData))]
        public void SolarCalculator_Position_ShouldProduceCorrectValues(SolarTestScenario testScenario)
        {
            var calculator = new SolarCalculator(testScenario.Input);
            calculator.CalculateSolarData(ref _actualSolarPosition);

            AssertSolarPosition(testScenario.ExpectedPosition, _actualSolarPosition, Assert.LessOrEqual);
        }

        [Test, TestCaseSource(nameof(s_incorrectTestData))]
        public void SolarCalculator_Position_ShouldProduceIncorrectValues(SolarTestScenario testScenario)
        {
            var calculator = new SolarCalculator(testScenario.Input);
            calculator.CalculateSolarData(ref _actualSolarPosition);

            AssertSolarPosition(testScenario.ExpectedPosition, _actualSolarPosition, Assert.Greater);
        }

        [Test, TestCaseSource(nameof(s_correctTestData))]
        public void SolarCalculator_Time_ShouldProduceCorrectValues(SolarTestScenario testScenario)
        {
            var calculator = new SolarCalculator(testScenario.Input);
            calculator.CalculateSolarData(ref _actualSolarTime);

            AssertSolarTime(testScenario.ExpectedTime, _actualSolarTime, Assert.LessOrEqual);
        }

        [Test, TestCaseSource(nameof(s_incorrectTestData))]
        public void SolarCalculator_Time_ShouldProduceIncorrectValues(SolarTestScenario testScenario)
        {
            var calculator = new SolarCalculator(testScenario.Input);
            calculator.CalculateSolarData(ref _actualSolarTime);

            AssertSolarTime(testScenario.ExpectedTime, _actualSolarTime, Assert.Greater);
        }

        private void AssertSolarPosition(SolarPosition expected, SolarPosition actual, System.Action<float, float> assertMethod)
        {
            assertMethod(
                CalculateErrorPercentage(expected.AzimuthAngle, actual.AzimuthAngle),
                SolarPositionErrorTolerance
            );

            assertMethod(
                CalculateErrorPercentage(expected.Elevation, actual.Elevation),
                SolarPositionErrorTolerance
            );

            assertMethod(
                CalculateErrorPercentage(expected.CorrectedElevation, actual.CorrectedElevation),
                SolarPositionErrorTolerance
            );
        }

        private void AssertSolarTime(SolarTime expected, SolarTime actual, System.Action<float, float> assertMethod)
        {
            assertMethod(
                CalculateErrorPercentage(
                    (float)System.TimeSpan.Parse(expected.SolarNoon).TotalSeconds,
                    (float)System.TimeSpan.Parse(actual.SolarNoon).TotalSeconds
                ),
                SolarTimeErrorTolerance
            );

            assertMethod(
                CalculateErrorPercentage(
                    (float)System.TimeSpan.Parse(expected.SunriseTime).TotalSeconds,
                    (float)System.TimeSpan.Parse(actual.SunriseTime).TotalSeconds
                ),
                SolarTimeErrorTolerance
            );

            assertMethod(
                CalculateErrorPercentage(
                    (float)System.TimeSpan.Parse(expected.SunsetTime).TotalSeconds,
                    (float)System.TimeSpan.Parse(actual.SunsetTime).TotalSeconds
                ),
                SolarTimeErrorTolerance
            );

            assertMethod(
                CalculateErrorPercentage(
                    expected.SunlightDurationAsMinutes,
                    actual.SunlightDurationAsMinutes
                ), 
                SolarTimeErrorTolerance
            );
        }

        private static float CalculateErrorPercentage(float expectedValue, float actualValue)
        {
            return Mathf.Abs((actualValue - expectedValue) / expectedValue) * 100.0f;
        }
    }
}
