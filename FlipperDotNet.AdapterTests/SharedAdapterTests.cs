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

        [Test]
        public void ShouldEnableAPercentageOfActorsGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.PercentageOfActorsGate, 15);
            Assert.That(Adapter.Get(feature).PercentageOfActors, Is.EqualTo(15));
        }

        [Test]
        public void ShouldDisableAPercentageOfActorsGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.PercentageOfActorsGate, 15);
            Adapter.Disable(feature, feature.PercentageOfActorsGate, 0);
            Assert.That(Adapter.Get(feature).PercentageOfActors, Is.EqualTo(0));
        }

        [Test]
        public void ShouldEnableAPercentageOfTimeGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.PercentageOfTimeGate, 10);
            Assert.That(Adapter.Get(feature).PercentageOfTime, Is.EqualTo(10));
        }

        [Test]
        public void ShouldDisableAPercentageOfTimeGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.PercentageOfTimeGate, 10);
            Adapter.Disable(feature, feature.PercentageOfTimeGate, 0);
            Assert.That(Adapter.Get(feature).PercentageOfTime, Is.EqualTo(0));
        }

        [Test]
        public void ShouldDefaultToEmptySetOfFeatures()
        {
            Assert.That(Adapter.Features, Is.Empty);
        }

        [Test]
        public void ShouldAddFeature()
        {
            Adapter.Add(Flipper.Feature("Stats"));
            Assert.That(Adapter.Features,Is.EquivalentTo(new[]{"Stats"}));
        }

        [Test]
        public void ShouldAddMultipleFeatures()
        {
            Adapter.Add(Flipper.Feature("Stats"));
            Adapter.Add(Flipper.Feature("Search"));
            Assert.That(Adapter.Features, Is.EquivalentTo(new[] {"Stats", "Search"}));
        }

        [Test]
        public void ShouldRemoveFeature()
        {
            Adapter.Add(Flipper.Feature("Stats"));
            Adapter.Add(Flipper.Feature("Search"));
            Adapter.Remove(Flipper.Feature("Stats"));
            Assert.That(Adapter.Features, Is.EquivalentTo(new[] {"Search"}));
        }
    }
}
