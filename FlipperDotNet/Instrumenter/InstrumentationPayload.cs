using System;

namespace FlipperDotNet.Instrumenter
{
	public sealed class InstrumentationPayload : IEquatable<InstrumentationPayload>
	{
		public string Operation;
		public string AdapterName;
		public string FeatureName;
		public string GateName;
		public object Thing;
		public object Result;

		public bool Equals(InstrumentationPayload other)
		{
			return Operation == other.Operation &&
				AdapterName == other.AdapterName &&
				FeatureName == other.FeatureName &&
				GateName == other.GateName &&
				((Thing == null && other.Thing == null) || (Thing != null && Thing.Equals(other.Thing))) &&
				((Result == null && other.Result == null) || (Result != null && Result.Equals(other.Result)));
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
			return string.Format("InstrumentationPayload: \"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\"",
				Operation, AdapterName, FeatureName, GateName, Thing, Result);
		}
	}
}
