using System.Collections.Generic;

namespace Contracts.Requests
{
    public class CalibrationRequest
    {
        public List<FuelLevelContract> FuelLevels { get; set; }
    }
}
