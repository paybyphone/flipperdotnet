using System;

namespace FlipperDotNet.Instrumenter
{
	public interface IInstrumentationToken : IDisposable
	{
	}


	public interface IInstrumenter
	{
		IInstrumentationToken Instrument(InstrumentationType type, InstrumentationPayload payload);
	}
}

