using MassTransit.Scheduling;

namespace Tracker.Glosav.Common
{
	internal sealed class ReceiveGlosavFuelSchedule : DefaultRecurringSchedule
	{
		public ReceiveGlosavFuelSchedule(string cronExpression)
		{
			CronExpression = cronExpression;
			Description = "Получение уровня топлива";
			MisfirePolicy = MissedEventPolicy.Skip;
		}
	}
}
