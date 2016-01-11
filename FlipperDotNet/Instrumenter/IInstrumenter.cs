using System;

namespace FlipperDotNet.Instrumenter
{
	public interface IInstrumentationToken : IDisposable
	{
	}


	public interface IInstrumenter
	{
		IInstrumentationToken Instrument(string name, InstrumentationPayload payload);
	}
}

