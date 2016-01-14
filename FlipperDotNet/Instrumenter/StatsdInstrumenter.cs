using System;
using System.Linq;
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

		public IInstrumentationToken Instrument(InstrumentationType type, InstrumentationPayload payload)
		{
			return new InstrumentationToken(this, type, payload, _clock);
		}

		private void Publish(InstrumentationType type, DateTime startTime, DateTime endTime, InstrumentationPayload payload)
		{
			var duration = endTime - startTime;
			switch(type){
				case InstrumentationType.FeatureOperation:
					PublishFeatureMetrics(payload, duration);
					break;
				case InstrumentationType.AdapterOperation:
					PublishAdapterMetrics(payload, duration);
					break;
				case InstrumentationType.GateOperation:
					PublishGateMetrics(payload, duration);
					break;
			}
		}

		private void PublishFeatureMetrics(InstrumentationPayload payload, TimeSpan duration)
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

		private void PublishAdapterMetrics(InstrumentationPayload payload, TimeSpan duration)
		{
			var adapterName = payload.AdapterName;
			var operation = payload.Operation;
			_statsdClient.LogTiming(string.Format("flipper.adapter.{0}.{1}", adapterName, operation), (long)duration.TotalMilliseconds);
		}

		private void PublishGateMetrics(InstrumentationPayload payload, TimeSpan duration)
		{
			var featureName = payload.FeatureName;
			var gateName = payload.GateName;
			var operation = payload.Operation.TrimEnd('?');
			var durationMilliseconds = (long)duration.TotalMilliseconds;

			_statsdClient.LogTiming(string.Format("flipper.gate_operation.{0}.{1}", gateName, operation), durationMilliseconds);
			_statsdClient.LogTiming(string.Format("flipper.feature.{0}.gate_operation.{1}.{2}", featureName, gateName, operation), durationMilliseconds);

			if (payload.Operation == "open?")
			{
				string metricName;
				if ((bool)payload.Result)
				{
					metricName = string.Format("flipper.feature.{0}.gate.{1}.open", featureName, gateName);
				} else
				{
					metricName = string.Format("flipper.feature.{0}.gate.{1}.closed", featureName, gateName);
				}
				_statsdClient.LogCount(metricName);
			}
		}

		class InstrumentationToken : IInstrumentationToken
		{
			readonly StatsdInstrumenter _instrumenter;
			readonly InstrumentationType _type;
			readonly InstrumentationPayload _payload;
			readonly IClock _clock;
			readonly DateTime _startTime;

			public InstrumentationToken(StatsdInstrumenter instrumenter, InstrumentationType type, InstrumentationPayload payload, IClock clock)
			{
				_instrumenter = instrumenter;
				_type = type;
				_payload = payload;
				_clock = clock;
				_startTime = _clock.Now;
			}

			public void Dispose()
			{
				var endTime = _clock.Now;

				_instrumenter.Publish(_type, _startTime, endTime, _payload);
			}
		}

		//class FeatureInstrumentationToken
	}
}

