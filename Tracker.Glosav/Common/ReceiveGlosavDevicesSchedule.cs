using MassTransit.Scheduling;

namespace Tracker.Glosav.Common
{
	internal sealed class ReceiveGlosavDevicesSchedule : DefaultRecurringSchedule
	{
		public ReceiveGlosavDevicesSchedule(string cronExpression)
		{
			CronExpression = cronExpression;
			Description = "Получение состояния устройств";
			MisfirePolicy = MissedEventPolicy.Skip;
		}
	}
}
