using FlipperDotNet.Instrumenter;
using System.Collections.Generic;

namespace FlipperDotNet.Tests.Mock
{

	internal class MockInstrumenter : IInstrumenter
	{
		public struct Event
		{
			public InstrumentationType Type;
			public InstrumentationPayload Payload;

			public override string ToString()
			{
				return string.Format("<Type=\"{0}\", Payload=\"{1}\">", Type, Payload);
			}
		}

		public List<Event> Events = new List<Event>();

		public IInstrumentationToken InstrumentFeature(InstrumentationPayload payload)
		{
			return new InstrumentationToken(this, InstrumentationType.FeatureOperation, payload);
		}

		public IInstrumentationToken InstrumentAdapter(InstrumentationPayload payload)
		{
			return new InstrumentationToken(this, InstrumentationType.AdapterOperation, payload);
		}

		public IInstrumentationToken InstrumentGate(InstrumentationPayload payload)
		{
			return new InstrumentationToken(this, InstrumentationType.GateOperation, payload);
		}

		public class InstrumentationToken : IInstrumentationToken
		{
			readonly MockInstrumenter _instrumenter;
			readonly InstrumentationType _type;
			readonly InstrumentationPayload _payload;

			public InstrumentationToken(MockInstrumenter instrumenter, InstrumentationType type, InstrumentationPayload payload)
			{
				_instrumenter = instrumenter;
				_type = type;
				_payload = payload;
			}

			public void Dispose()
			{
				_instrumenter.Events.Add(new Event {
					Type = _type,
					Payload = _payload,
				});
			}
		}
	}
}
