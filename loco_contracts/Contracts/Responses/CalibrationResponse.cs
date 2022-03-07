using System.Collections.Generic;

namespace Contracts.Responses
{
    public class CalibrationResponse
    {
        public List<CalibratedFuelLevelContract> CalibratedFuelLevels { get; set; }
    }
}
