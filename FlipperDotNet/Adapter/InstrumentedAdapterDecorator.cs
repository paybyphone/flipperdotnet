using FlipperDotNet.Instrumenter;

namespace FlipperDotNet.Adapter
{
	public class InstrumentedAdapterDecorator : IAdapter
	{
		public string Name { get { return "instrumented"; } }

		public InstrumentedAdapterDecorator(IAdapter adapter, IInstrumenter instrumenter)
		{
			Adapter = adapter;
			Instrumenter = instrumenter;
		}

		public IAdapter Adapter { get; private set; }

		public IInstrumenter Instrumenter { get; private set; }

		public System.Collections.Generic.IDictionary<string, object> Get(Feature feature)
		{
			var instrumentationPayload = new InstrumentationPayload {
				Operation = "get",
				AdapterName = Adapter.Name,
				FeatureName = feature.Name,
			};
			using (Instrumenter.InstrumentAdapter(instrumentationPayload))
			{
				var result = Adapter.Get(feature);
				instrumentationPayload.Result = result;
				return result;
			}
		}

		public void Enable(Feature feature, FlipperDotNet.Gate.IGate gate, object thing)
		{
			var instrumentationPayload = new InstrumentationPayload {
				Operation = "enable",
				AdapterName = Adapter.Name,
				FeatureName = feature.Name,
				GateName = gate.Name,
			};
			using (Instrumenter.InstrumentAdapter(instrumentationPayload))
			{
				Adapter.Enable(feature, gate, thing);
			}
		}

		public void Disable(Feature feature, FlipperDotNet.Gate.IGate gate, object thing)
		{
			var instrumentationPayload = new InstrumentationPayload {
				Operation = "disable",
				AdapterName = Adapter.Name,
				FeatureName = feature.Name,
				GateName = gate.Name,
			};
			using (Instrumenter.InstrumentAdapter(instrumentationPayload))
			{
				Adapter.Disable(feature, gate, thing);
			}
		}

		public void Add(Feature feature)
		{
			var instrumentationPayload = new InstrumentationPayload {
				Operation = "add",
				AdapterName = Adapter.Name,
				FeatureName = feature.Name,
			};
			using (Instrumenter.InstrumentAdapter(instrumentationPayload))
			{
				Adapter.Add(feature);
			}
		}

		public void Remove(Feature feature)
		{
			var instrumentationPayload = new InstrumentationPayload {
				Operation = "remove",
				AdapterName = Adapter.Name,
				FeatureName = feature.Name,
			};
			using (Instrumenter.InstrumentAdapter(instrumentationPayload))
			{
				Adapter.Remove(feature);
			}
		}

		public void Clear(Feature feature)
		{
			var instrumentationPayload = new InstrumentationPayload {
				Operation = "clear",
				AdapterName = Adapter.Name,
				FeatureName = feature.Name,
			};
			using (Instrumenter.InstrumentAdapter(instrumentationPayload))
			{
				Adapter.Clear(feature);
			}
		}

		public System.Collections.Generic.ISet<string> Features {
			get {
				var instrumentationPayload = new InstrumentationPayload {
					Operation = "features",
					AdapterName = Adapter.Name,
				};
				using (Instrumenter.InstrumentAdapter(instrumentationPayload))
				{
					var result = Adapter.Features;
					instrumentationPayload.Result = result;
					return result;
				}
			}
		}
	}
}

