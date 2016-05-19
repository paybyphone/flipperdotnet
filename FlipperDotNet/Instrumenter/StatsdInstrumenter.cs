using System;
using StatsdClient;
using FlipperDotNet.Util;

namespace FlipperDotNet.Instrumenter
{
	public class StatsdInstrumenter : IInstrumenter
	{
		private readonly IStatsd _statsdClient;
		private readonly IClock _clock;

		public StatsdInstrumenter(IStatsd statsdClient) : this(statsdClient, new SystemClock())
		{ }

		public StatsdInstrumenter(IStatsd statsdClient, IClock clock)
		{
			_statsdClient = statsdClient;
			_clock = clock;
		}

		public IInstrumentationToken InstrumentFeature(InstrumentationPayload payload)
		{
			return new FeatureInstrumentationToken(this, payload, _clock);
		}

		public IInstrumentationToken InstrumentAdapter(InstrumentationPayload payload)
		{
			return new AdapterInstrumentationToken(this, payload, _clock);
		}

		private void PublishFeatureMetrics(TimeSpan duration, InstrumentationPayload payload)
		{
			var operation = payload.Operation.TrimEnd('?');
			_statsdClient.LogTiming(string.Format("flipper.feature_operation.{0}", operation), (long)duration.TotalMilliseconds);

			if (payload.Operation == "enabled?")
			{
				var featureName = payload.FeatureName;
				string metricName;
				if ((bool)payload.Result)
				{
					metricName = string.Format("flipper.feature.{0}.enabled", featureName);
				} else
				{
					metricName = string.Format("flipper.feature.{0}.disabled", featureName);
				}
				_statsdClient.LogCount(metricName);
			}
		}

		private void PublishAdapterMetrics(TimeSpan duration, InstrumentationPayload payload)
		{
			var adapterName = payload.AdapterName;
			var operation = payload.Operation;
			_statsdClient.LogTiming(string.Format("flipper.adapter.{0}.{1}", adapterName, operation), (long)duration.TotalMilliseconds);
		}

		abstract class InstrumentationToken : IInstrumentationToken
		{
			protected readonly StatsdInstrumenter _instrumenter;
			protected readonly InstrumentationPayload _payload;
			protected readonly DateTime _startTime;
			readonly IClock _clock;

			public InstrumentationToken(StatsdInstrumenter instrumenter, InstrumentationPayload payload, IClock clock)
			{
				_instrumenter = instrumenter;
				_payload = payload;
				_clock = clock;
				_startTime = _clock.Now;
			}

			public void Dispose()
			{
				var endTime = _clock.Now;
				Publish(endTime - _startTime);
			}

			protected abstract void Publish(TimeSpan duration);
		}

		class FeatureInstrumentationToken : InstrumentationToken
		{
			public FeatureInstrumentationToken(StatsdInstrumenter instrumenter, InstrumentationPayload payload, IClock clock)
				: base(instrumenter, payload, clock)
			{}

			protected override void Publish(TimeSpan duration)
			{
				_instrumenter.PublishFeatureMetrics(duration, _payload);
			}
		}

		class AdapterInstrumentationToken : InstrumentationToken
		{
			public AdapterInstrumentationToken(StatsdInstrumenter instrumenter, InstrumentationPayload payload, IClock clock)
				: base(instrumenter, payload, clock)
			{}

			protected override void Publish(TimeSpan duration)
			{
				_instrumenter.PublishAdapterMetrics(duration, _payload);
			}
		}
	}
}

