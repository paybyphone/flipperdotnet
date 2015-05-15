using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlipperDotNet.Adapter;
using NUnit.Framework;

namespace FlipperDotNet.AdapterTests
{
    [TestFixture]
    public abstract class SharedAdapterTests
    {
        protected Flipper Flipper { get; private set; }
        protected IAdapter Adapter { get; set; }

        [SetUp]
        public void SetUp()
        {
            Flipper = new Flipper(Adapter);
        }

        [Test]
        public void ShouldSetDefaultGateValues()
        {
            var feature = Flipper.Feature("Stats");
            Assert.That(Adapter.Get(feature), Is.EqualTo(new FeatureResult()));
        }

        [Test]
        public void ShouldEnableABooleanGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.BooleanGate, true);
            Assert.That(Adapter.Get(feature).Boolean, Is.EqualTo(true));
        }

        [Test]
        public void ShouldDisableABooleanGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.BooleanGate, true);
            Adapter.Disable(feature, feature.BooleanGate, false);
            Assert.That(Adapter.Get(feature).Boolean, Is.Null);
        }
    }
}
