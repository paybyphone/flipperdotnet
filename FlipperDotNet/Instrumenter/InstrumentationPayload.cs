using System;

namespace FlipperDotNet.Instrumenter
{
	public sealed class InstrumentationPayload : IEquatable<InstrumentationPayload>
	{
		public string Operation;
		public string AdapterName;
		public string FeatureName;
		public string GateName;
		public object Result;

		public bool Equals(InstrumentationPayload other)
		{
			return Operation == other.Operation &&
				   AdapterName == other.AdapterName &&
				   FeatureName == other.FeatureName &&
				   GateName == other.GateName &&
				   Result == other.Result;
		}
		
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var payload = obj as InstrumentationPayload;
			return payload != null && Equals(payload);
		}

		public override string ToString()
		{
			return string.Format("InstrumentationPayload: \"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\"",
				Operation, AdapterName, FeatureName, GateName, Result);
		}
	}
}
