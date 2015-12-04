using System;

namespace FlipperDotNet
{
	public class AdapterRequestException : Exception
	{
		public AdapterRequestException() { }
		public AdapterRequestException(String message) : base(message) { }
		public AdapterRequestException(String message, Exception innerException) : base(message, innerException) { }
	}
}

