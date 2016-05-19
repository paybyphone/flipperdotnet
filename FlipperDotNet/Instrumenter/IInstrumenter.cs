using System;

namespace FlipperDotNet.Instrumenter
{
	public interface IInstrumentationToken : IDisposable
	{
	}


	public interface IInstrumenter
	{
		IInstrumentationToken InstrumentFeature(InstrumentationPayload payload);
		IInstrumentationToken InstrumentAdapter(InstrumentationPayload payload);
	}
}

