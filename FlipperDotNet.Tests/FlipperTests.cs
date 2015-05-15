using NUnit.Framework;
using Rhino.Mocks;
using FlipperDotNet.Adapter;

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
            Assert.That(flipper.Adapter,Is.Not.Null);
        }

        [Test]
        public void GetFeatureSetsNameOfFeature()
        {
            var adapter = MockRepository.GenerateStub<IAdapter>();
            var flipper = new Flipper(adapter);
            var feature = flipper.Feature("Test");
            Assert.That(feature.Name,Is.EqualTo("Test"));
        }

        [Test]
        public void GetFeatureSetsAdapterOnFeature()
        {
            var adapter = MockRepository.GenerateStub<IAdapter>();
            var flipper = new Flipper(adapter);
            var feature = flipper.Feature("Test");
            Assert.That(feature.Adapter, Is.EqualTo(adapter));
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
    }
}
