using System;
using System.Collections.Generic;
using System.Linq;
using FlipperDotNet.Adapter;
using FlipperDotNet.Instrumenter;

namespace FlipperDotNet
{
    public class Flipper
    {
		public const string InstrumentationNamespace = "flipper";

        private readonly Dictionary<string, Feature> _features = new Dictionary<string, Feature>();

		public Flipper(IAdapter adapter) : this(adapter, new NoOpInstrumenter())
		{ }

		public Flipper(IAdapter adapter, IInstrumenter instrumenter)
		{
			if (instrumenter == null)
			{
				throw new ArgumentNullException("instrumenter");
			}
			Instrumenter = instrumenter;
			Adapter = new InstrumentedAdapterDecorator(adapter, Instrumenter);
		}

        public IAdapter Adapter { get; private set; }

		public IInstrumenter Instrumenter { get; private set; }

        public ISet<Feature> Features
        {
            get
            {
                return new HashSet<Feature>(from featureName in Adapter.Features
                                            select Feature(featureName));
            }
        }

        public Feature Feature(string name)
        {
            Feature feature;
            if (!_features.TryGetValue(name, out feature))
            {
				feature = new Feature(name, Adapter, Instrumenter);
                _features.Add(name, feature);
            }
            return feature;
        }

        public void Enable(string name)
        {
            Feature(name).Enable();
        }

        public void Disable(string name)
        {
            Feature(name).Disable();
        }
    }
}
