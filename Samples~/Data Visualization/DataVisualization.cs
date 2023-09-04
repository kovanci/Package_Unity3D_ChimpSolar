using UnityEngine;

using Chimp.Solar.Data;

using TMPro;

namespace Chimp.Solar.Samples
{
    [RequireComponent(typeof(LineRenderer))]
    public class DataVisualization : MonoBehaviour
    {
        [Header("Inputs")]
        public int StartMonth = 5;
        public int EndMonth = 8;
        public SolarInputs SolarInputs;
        public TextMeshProUGUI InfoText;

        [Header("Outputs")]
        [SerializeField] private float[] _durations;
        [SerializeField] private float _maxDuration = 0.0f;
        [SerializeField] private float _minDuration = Mathf.Infinity;

        private int _maxIndex;
        private int _minIndex;

        private System.DateTime _maxDay;
        private System.DateTime _minDay;

        private LineRenderer _lineRenderer;

        private SolarCalculator _solarCalculator;
        private SolarTime _solarTime = new();
        private int _numberOfDays;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            _solarCalculator = new SolarCalculator(SolarInputs);

            for (int month = StartMonth; month <= EndMonth; month++)
            {
                int days = System.DateTime.DaysInMonth(SolarInputs.Year, month);
                _numberOfDays += days;
            }

            _durations = new float[_numberOfDays];

            GenerateData();
            VisualizeData();
        }

        private void GenerateData()
        {
            int i = 0;

            for (int month = StartMonth; month <= EndMonth; month++)
            {
                SolarInputs.Month = month;

                int days = System.DateTime.DaysInMonth(SolarInputs.Year, month);

                for (int day = 1;  day <= days; ++day)
                {
                    SolarInputs.Day = day;

                    _solarCalculator.CalculateSolarData(ref _solarTime);
                    float duration = _solarTime.SunlightDurationAsMinutes / 60.0f;
                    if (duration > _maxDuration)
                    {
                        _maxDuration = duration;
                        _maxIndex = i;
                        _maxDay = SolarInputs.GetDateTime();
                    }

                    if (duration < _minDuration)
                    {
                        _minDuration = duration;
                        _minIndex = i;
                        _minDay = SolarInputs.GetDateTime();
                    }

                    _durations[i] = _solarTime.SunlightDurationAsMinutes / 60.0f;

                    i++;
                }
            }
        }

        private void VisualizeData()
        {
            string maxSunlightTime = System.TimeSpan.FromHours(_durations[_maxIndex]).ToString();
            string minSunlightTime = System.TimeSpan.FromHours(_durations[_minIndex]).ToString();

            InfoText.text = $"Max Day: {_maxDay.ToShortDateString()}\n";
            InfoText.text += $"Duration: {maxSunlightTime} hours\n\n";
            InfoText.text += $"Min Day: {_minDay.ToShortDateString()}\n";
            InfoText.text += $"Duration: {minSunlightTime} hours";

            _lineRenderer.positionCount = _numberOfDays;

            for (int i = 0; i < _numberOfDays; ++i)
            {
                float normalizedX = (float)i / (_numberOfDays - 1) * Screen.width;
                float normalizedY = (_durations[i] - _minDuration) / (_maxDuration - _minDuration) * Screen.height;

                var position = new Vector3(normalizedX, normalizedY, Camera.main.transform.position.z + 30.0f);
                _lineRenderer.SetPosition(i, Camera.main.ScreenToWorldPoint(position));
            }
        }
    }
}
