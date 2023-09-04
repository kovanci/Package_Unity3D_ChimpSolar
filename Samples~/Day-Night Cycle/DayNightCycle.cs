using Unity.Mathematics;

using UnityEngine;

using Chimp.Solar.Data;

namespace Chimp.Solar.Samples
{
    [DisallowMultipleComponent]
    public class DayNightCycle : MonoBehaviour
    {
        [Header("Usage")]
        [SerializeField]
        private DateTimeUsage _dateTimeUsage = DateTimeUsage.UseCurrent;

        [SerializeField]
        [Tooltip("Only active when 'DateTimeUsage.UseProvided' selected")]
        private float _timeScale = 2.0f;

        [Header("Cardinal Directions")]
        [SerializeField]
        [Range(0, 360), Tooltip("Default North Direction is -Z")]
        private float _northOffset = 0.0f;

        [SerializeField] private Transform _compassObject = default;

        [Header("Inputs")]
        [SerializeField] private SolarInputs _solarInputs = default;

        [Header("Outputs")]
        [SerializeField] private SolarPosition _solarPosition = default;

        private SolarCalculator _solarCalculator;

        // Avoid far away positions by dividing
        private const float EarthRadius = 6371000f / 100000.0f;

        private System.DateTime _startTime;

        private void Start()
        {
            _solarCalculator = new SolarCalculator(_solarInputs);
            _startTime = _solarInputs.GetDateTime();
        }

        private void Update()
        {
            UpdateDateTime();

            _solarCalculator.CalculateSolarData(ref _solarPosition);

            transform.SetPositionAndRotation(
                GetSolarPosition(),
                Quaternion.Euler(
                    _solarPosition.CorrectedElevation,
                    _solarPosition.AzimuthAngle + _northOffset,
                    0.0f
                )
            );

            if (_compassObject != null)
            {
                _compassObject.rotation = Quaternion.Euler(
                    _compassObject.rotation.eulerAngles.x,
                    90.0f + _northOffset,
                    _compassObject.rotation.eulerAngles.z
                );
            }
        }

        private void UpdateDateTime()
        {
            if (_dateTimeUsage == DateTimeUsage.UseCurrent)
            {
                _solarInputs.SetDateTime(System.DateTime.Now);
                return;
            }

            _solarInputs.SetDateTime(_startTime.AddSeconds(Time.realtimeSinceStartup * _timeScale));
        }

        private Vector3 GetSolarPosition()
        {
            float solarHourAngle = math.radians(180.0f - _solarPosition.AzimuthAngle);
            float solarElevationAngle = math.radians(_solarPosition.CorrectedElevation);

            float x = EarthRadius * math.cos(solarElevationAngle) * math.sin(solarHourAngle);
            float y = EarthRadius * math.sin(solarElevationAngle);
            float z = EarthRadius * math.cos(solarElevationAngle) * math.cos(solarHourAngle);

            return new Vector3(x, y, z);
        }
    }
}
