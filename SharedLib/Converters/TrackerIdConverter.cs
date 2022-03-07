using System;
using System.Text.RegularExpressions;

namespace SharedLib.Converters
{
	public sealed class TrackerIdConverter
	{
		private static readonly Regex TrackerIdRegex = new Regex(@"^Glosav:\(OperatorId:(.+)\)_\(DeviceId:(.+)\)$", RegexOptions.Compiled);
		private static readonly Regex FuelTrackerIdRegex = new Regex(@"^Glosav:\(OperatorId:(.+)\)_\(DeviceId:(.+)\)_tank_(\d)_sensor_(\d)$", RegexOptions.Compiled);

		public static (int OperatorId, int DeviceId) TrackerIdToGlosavIdentifier(string trackerId)
		{
			var match = TrackerIdRegex.Match(trackerId);
			if (!match.Success || match.Groups.Count < 3) throw new FormatException(nameof(trackerId));

			return (OperatorId: int.Parse(match.Groups[1].Value), DeviceId: int.Parse(match.Groups[2].Value));
		}

		public static (int OperatorId, int DeviceId) FuelTrackerIdToGlosavIdentifier(string trackerId)
		{
			var match = FuelTrackerIdRegex.Match(trackerId);
			if (!match.Success || match.Groups.Count < 3) throw new FormatException(nameof(trackerId));

			return (OperatorId: int.Parse(match.Groups[1].Value), DeviceId: int.Parse(match.Groups[2].Value));
		}

		public static string GlosavIdentifierToTrackerId(int operatorId, int deviceId)
		{
			return $"Glosav:(OperatorId:{operatorId})_(DeviceId:{deviceId})";
		}

		public static string GlosavIdentifierToFuelTrackerId(int operatorId, int deviceId, int tank, int sensor)
		{
			return $"Glosav:(OperatorId:{operatorId})_(DeviceId:{deviceId})_tank_{tank}_sensor_{sensor}";
		}

		public static bool IsTrackerIdMatchGlosavTemplate(string trackerId)
		{
			return TrackerIdRegex.IsMatch(trackerId);
		}

		public static bool IsTrackerIdMatchGlosavFuelTemplate(string trackerId)
		{
			return FuelTrackerIdRegex.IsMatch(trackerId);
		}	
	}
}
