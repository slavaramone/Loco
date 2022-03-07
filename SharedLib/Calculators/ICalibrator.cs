using System.Collections.Generic;

namespace SharedLib.Calculators
{
    public interface ICalibrator
    {
        public CalibrationResult Calibrate(double value, Dictionary<double, double> nodes);
    }
}
