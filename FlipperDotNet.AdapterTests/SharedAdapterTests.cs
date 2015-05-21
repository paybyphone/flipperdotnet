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
        public void ShouldFullyDisableTheFeatureWhenBooleanGateIsDisabled()
        {
            var actor22 = new object();
            var group = new object();
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.BooleanGate, true);
            Adapter.Enable(feature, feature.GroupGate, group);
            Adapter.Enable(feature, feature.ActorGate, actor22);
            Adapter.Enable(feature, feature.PercentageOfActorsGate, 25);
            Adapter.Enable(feature, feature.PercentageOfTimeGate, 45);

            Adapter.Disable(feature, feature.BooleanGate, false);

            Assert.That(Adapter.Get(feature), Is.EqualTo(new FeatureResult()));
        }

        [Test,Ignore("No Group support yet")]
        public void ShouldEnableGroupsForGroupGate()
        {
        }

        [Test, Ignore("No Group support yet")]
        public void ShouldDisableGroupForGroupGateWhenThereAreMultipleGroups()
        {
        }

        [Test, Ignore("No Group support yet")]
        public void ShouldDisableGroupForGroupGateWhenItIsTheLastGroup()
        {
        }

        [Test]
        public void ShouldEnableActorsForActorGate()
        {
            var feature = Flipper.Feature("Stats");

            Adapter.Enable(feature, feature.ActorGate, "22");
            Adapter.Enable(feature, feature.ActorGate, "asdf");

            Assert.That(Adapter.Get(feature).Actors, Is.EquivalentTo(new[] {"22", "asdf"}));
        }

        [Test]
        public void ShouldDisableActorForActorGateWhenThereAreMultipleActors()
        {
            var feature = Flipper.Feature("Stats");

            Adapter.Enable(feature, feature.ActorGate, "22");
            Adapter.Enable(feature, feature.ActorGate, "asdf");

            Adapter.Disable(feature, feature.ActorGate, "22");

            Assert.That(Adapter.Get(feature).Actors, Is.EquivalentTo(new[] {"asdf"}));
        }

        [Test]
        public void ShouldDisableActorForActorGateWhenItIsTheLastActor()
        {
            var feature = Flipper.Feature("Stats");

            Adapter.Enable(feature, feature.ActorGate, "asdf");

            Adapter.Disable(feature, feature.ActorGate, "asdf");

            Assert.That(Adapter.Get(feature).Actors, Is.Empty);
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

        [Test]
        public void ShouldRemoveAllGateValuesWhenFeatureRemoved()
        {
            var actor22 = new object();
            var group = new object();
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.BooleanGate, true);
            Adapter.Enable(feature, feature.GroupGate, group);
            Adapter.Enable(feature, feature.ActorGate, actor22);
            Adapter.Enable(feature, feature.PercentageOfActorsGate, 25);
            Adapter.Enable(feature, feature.PercentageOfTimeGate, 45);

            Adapter.Remove(feature);

            Assert.That(Adapter.Get(feature), Is.EqualTo(new FeatureResult()));
        }

        [Test]
        public void ShouldRemoveAllGateValuesWhenFeatureCleared()
        {
            var actor22 = new object();
            var group = new object();
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.BooleanGate, true);
            Adapter.Enable(feature, feature.GroupGate, group);
            Adapter.Enable(feature, feature.ActorGate, actor22);
            Adapter.Enable(feature, feature.PercentageOfActorsGate, 25);
            Adapter.Enable(feature, feature.PercentageOfTimeGate, 45);

            Adapter.Clear(feature);

            Assert.That(Adapter.Get(feature), Is.EqualTo(new FeatureResult()));
        }
    }
}
