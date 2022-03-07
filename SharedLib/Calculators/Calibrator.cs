using System.Collections.Generic;
using System.Linq;

namespace SharedLib.Calculators
{
    /// <summary>
    /// Класс для получения калиброванного значения на основе калибровочной таблицы
    /// </summary>
    public class Calibrator : ICalibrator
    {
        private const double MaxRawValue = 4095;

        /// <summary>
        /// Калибровка значения
        /// </summary>
        /// <param name="rawValue">"сырое значение"</param>
        /// <param name="nodes">ноды калибровчной таблицы (сырое значение - откалиброванное значение)</param>
        /// <returns></returns>
        public CalibrationResult Calibrate(double rawValue, Dictionary<double, double> nodes)
        {
            double calibratedValue = rawValue;
            double maxValue = MaxRawValue;
            if (nodes.Any())
            {
                maxValue = nodes.Max(x => x.Value);
                if (nodes.ContainsKey(rawValue))
                {
                    calibratedValue = nodes[rawValue];
                }
                else
                {
                    var lessThanValues = nodes.Keys.Where(x => rawValue > x).ToList();
                    var x0 = lessThanValues.Count > 0 ? lessThanValues.Max() : 0;

                    var moreThanValues = nodes.Keys.Where(x => rawValue < x).ToList();
                    var x1 = moreThanValues.Count > 0 ? moreThanValues.Min() : MaxRawValue;

                    double fx0 = x0 != 0 ? nodes[x0] : 0;
                    double fx1 = nodes[x1];
                    calibratedValue = fx0 + (rawValue - x0) * (fx1 - fx0) / (x1 - x0);
                }
            }
            return new CalibrationResult
            {
                CalibratedValue = calibratedValue,
                MaxValue = maxValue
            };
        }
    }
}
