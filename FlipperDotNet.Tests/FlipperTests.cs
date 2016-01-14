using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using FlipperDotNet.Adapter;
using FlipperDotNet.Instrumenter;

namespace FlipperDotNet.Tests
{
    [TestFixture]
    public class FlipperTests
    {
        [Test]
        public void ConstructorShouldSetAdapter()
        {
            var adapter = MockRepository.GenerateStub<IAdapter>();
            var flipper = new Flipper(adapter);
			Assert.That(flipper.Adapter, Is.Not.Null);
        }

		[Test]
		public void ConstructorSetsNoOpInstrumenterByDefault()
		{
			var flipper = new Flipper(MockRepository.GenerateStub<IAdapter>());
			Assert.That(flipper.Instrumenter, Is.InstanceOf<NoOpInstrumenter>());
		}

		[Test]
		public void ConstructorShouldThrowExceptionOnNullInstrumenter()
		{
			Assert.Throws<ArgumentNullException>(delegate {
				var flipper = new Flipper(MockRepository.GenerateStub<IAdapter>(), null);
			});
		}

		[Test]
		public void ConstructorShouldSetInstrumenter()
		{
			var instrumenter = MockRepository.GenerateStub<IInstrumenter>();
			var flipper = new Flipper(MockRepository.GenerateStub<IAdapter>(), instrumenter);
			Assert.That(flipper.Instrumenter, Is.EqualTo(instrumenter));
		}

        [Test]
        public void GetFeatureSetsNameOfFeature()
        {
            var adapter = MockRepository.GenerateStub<IAdapter>();
            var flipper = new Flipper(adapter);
            var feature = flipper.Feature("Test");
			Assert.That(feature.Name, Is.EqualTo("Test"));
        }

        [Test]
        public void GetFeatureSetsAdapterOnFeature()
        {
            var adapter = MockRepository.GenerateStub<IAdapter>();
            var flipper = new Flipper(adapter);
            var feature = flipper.Feature("Test");
			Assert.That(feature.Adapter, Is.EqualTo(flipper.Adapter));
        }

		[Test]
		public void GetFeatureSetsInstrumenterOnFeature()
		{
			var instrumenter = MockRepository.GenerateStub<IInstrumenter>();
			var flipper = new Flipper(MockRepository.GenerateStub<IAdapter>(), instrumenter);
			var feature = flipper.Feature("Test");
			Assert.That(feature.Instrumenter, Is.EqualTo(instrumenter));
		}

        [Test]
        public void GetFeatureMemoizesTheFeature()
        {
            var adapter = MockRepository.GenerateStub<IAdapter>();
            var flipper = new Flipper(adapter);
            var feature = flipper.Feature("Test");
            Assert.That(flipper.Feature("Test"), Is.SameAs(feature));
        }

        [Test]
        public void EnableEnablesTheFeature()
        {
            var flipper = new Flipper(new MemoryAdapter());
            flipper.Enable("Test");
            Assert.That(flipper.Feature("Test").BooleanValue, Is.True);
        }

        [Test]
        public void DisableDisablesTheFeature()
        {
            var flipper = new Flipper(new MemoryAdapter());
            flipper.Enable("Test");
            flipper.Disable("Test");
            Assert.That(flipper.Feature("Test").BooleanValue, Is.False);
        }

        [Test]
        public void FeaturesShouldDefaultToEmptySet()
        {
            var flipper = new Flipper(new MemoryAdapter());
            Assert.That(flipper.Features, Is.Empty);
        }

        [Test]
        public void FeaturesShouldReturnEnabledAndDisabledFeatures()
        {
            var flipper = new Flipper(new MemoryAdapter());
            flipper.Enable("Stats");
            flipper.Enable("Cache");
            flipper.Disable("Search");
            Assert.That(from feature in flipper.Features select feature.Name,
                        Is.EquivalentTo(new[] {"Stats", "Cache", "Search"}));
        }
    }
}
