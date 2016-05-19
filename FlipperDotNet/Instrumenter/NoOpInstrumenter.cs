using FlipperDotNet.Instrumenter;

namespace FlipperDotNet.Instrumenter
{
	public class NoOpInstrumenter : IInstrumenter
	{
		public IInstrumentationToken InstrumentFeature(InstrumentationPayload payload)
		{
			return new InstrumentationToken();
		}

		public IInstrumentationToken InstrumentAdapter(InstrumentationPayload payload)
		{
			return new InstrumentationToken();
		}

		internal class InstrumentationToken : IInstrumentationToken
		{
			public void Dispose()
			{ }
		}
	}
}

