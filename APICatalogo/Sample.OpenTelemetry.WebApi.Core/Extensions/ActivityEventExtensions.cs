using OpenTelemetry.Logs;
using OpenTelemetry;
using System.Diagnostics;

namespace Sample.OpenTelemetry.WebApi.Core.Extensions
{
	public class ActivityEventExtensions : BaseProcessor<LogRecord>
	{
		public override void OnEnd(LogRecord data)
		{
			base.OnEnd(data);
			var currentActivity = Activity.Current;
            var attributesAsString = data?.Attributes?
				.Select(kv => $"{kv.Key}: {kv.Value}")
				.Aggregate((current, next) => $"{current}, {next}") ?? string.Empty;

            currentActivity?.AddEvent(new ActivityEvent(attributesAsString));
        }
	}
}