using System;
using System.Collections.Generic;
using FlipperDotNet.Adapter;
using FlipperDotNet.Gate;
using NUnit.Framework;
using Rhino.Mocks;

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
            Assert.That(Adapter.Get(feature), Is.EquivalentTo(EmptyResult()));
        }

        [Test]
        public void ShouldEnableABooleanGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.BooleanGate, true);
            Assert.That(Adapter.Get(feature)[BooleanGate.KEY], Is.EqualTo("true"));
        }

        [Test]
        public void ShouldDisableABooleanGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.BooleanGate, true);
            Adapter.Disable(feature, feature.BooleanGate, false);
            Assert.That(Adapter.Get(feature)[BooleanGate.KEY], Is.Null);
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

            Assert.That(Adapter.Get(feature), Is.EqualTo(EmptyResult()));
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

            Assert.That(Adapter.Get(feature)[ActorGate.KEY], Is.EquivalentTo(new[] {"22", "asdf"}));
        }

        [Test]
        public void ShouldDisableActorForActorGateWhenThereAreMultipleActors()
        {
            var feature = Flipper.Feature("Stats");

            Adapter.Enable(feature, feature.ActorGate, "22");
            Adapter.Enable(feature, feature.ActorGate, "asdf");

            Adapter.Disable(feature, feature.ActorGate, "22");

            Assert.That(Adapter.Get(feature)[ActorGate.KEY], Is.EquivalentTo(new[] { "asdf" }));
        }

        [Test]
        public void ShouldDisableActorForActorGateWhenItIsTheLastActor()
        {
            var feature = Flipper.Feature("Stats");

            Adapter.Enable(feature, feature.ActorGate, "asdf");

            Adapter.Disable(feature, feature.ActorGate, "asdf");

            Assert.That(Adapter.Get(feature)[ActorGate.KEY], Is.Empty);
        }

        [Test]
        public void ShouldEnableAPercentageOfActorsGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.PercentageOfActorsGate, 15);
            Assert.That(Adapter.Get(feature)[PercentageOfActorsGate.KEY], Is.EqualTo("15"));
        }

        [Test]
        public void ShouldDisableAPercentageOfActorsGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.PercentageOfActorsGate, 15);
            Adapter.Disable(feature, feature.PercentageOfActorsGate, 0);
            Assert.That(Adapter.Get(feature)[PercentageOfActorsGate.KEY], Is.EqualTo("0"));
        }

        [Test]
        public void ShouldEnableAPercentageOfTimeGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.PercentageOfTimeGate, 10);
            Assert.That(Adapter.Get(feature)[PercentageOfTimeGate.KEY], Is.EqualTo("10"));
        }

        [Test]
        public void ShouldDisableAPercentageOfTimeGate()
        {
            var feature = Flipper.Feature("Stats");
            Adapter.Enable(feature, feature.PercentageOfTimeGate, 10);
            Adapter.Disable(feature, feature.PercentageOfTimeGate, 0);
            Assert.That(Adapter.Get(feature)[PercentageOfTimeGate.KEY], Is.EqualTo("0"));
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
			Assert.That(Adapter.Features, Is.EquivalentTo(new[]{ "Stats" }));
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

            Assert.That(Adapter.Get(feature), Is.EqualTo(EmptyResult()));
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

            Assert.That(Adapter.Get(feature), Is.EqualTo(EmptyResult()));
        }

		[Test]
		public void ShouldNotRemoveFeatureWhenFeatureCleared ()
		{
			var actor22 = new object();
			var group = new object();
			var feature = Flipper.Feature("Stats");
			Adapter.Add(feature);
			Adapter.Enable(feature, feature.BooleanGate, true);
			Adapter.Enable(feature, feature.GroupGate, group);
			Adapter.Enable(feature, feature.ActorGate, actor22);
			Adapter.Enable(feature, feature.PercentageOfActorsGate, 25);
			Adapter.Enable(feature, feature.PercentageOfTimeGate, 45);

			Adapter.Clear(feature);

			Assert.That(Adapter.Features, Is.EquivalentTo(new[] { "Stats" }));
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException), ExpectedMessage="TEST is not supported by this adapter yet")]
		public void ShouldFailWhenUnsupportedGateTypeIsEnabled()
		{
			var feature = Flipper.Feature("Stats");
			var gate = MockRepository.GenerateStub<IGate>();
			gate.Stub(x => x.DataType).Return(typeof(string));
			gate.Stub(x => x.Name).Return("TEST");

			Adapter.Enable(feature, gate, "foo");
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException), ExpectedMessage="TEST is not supported by this adapter yet")]
		public void ShouldFailWhenUnsupportedGateTypeIsDisabled()
		{
			var feature = Flipper.Feature("Stats");
			var gate = MockRepository.GenerateStub<IGate>();
			gate.Stub(x => x.DataType).Return(typeof(string));
			gate.Stub(x => x.Name).Return("TEST");

			Adapter.Disable(feature, gate, "foo");
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException), ExpectedMessage="TEST is not supported by this adapter yet")]
		public void ShouldFailWhenUnsupportedGateTypeIsRetrieved()
		{
			var feature = Flipper.Feature("Stats");
			var gate = MockRepository.GenerateStub<IGate>();
			gate.Stub(x => x.DataType).Return(typeof(string));
			gate.Stub(x => x.Name).Return("TEST");
			feature.Gates.Add(gate);

			Adapter.Get(feature);
		}

        private static Dictionary<string, object> EmptyResult()
        {
            return new Dictionary<string, object>
                {
                    {BooleanGate.KEY, null},
                    {GroupGate.KEY, new HashSet<string>()},
                    {ActorGate.KEY, new HashSet<string>()},
                    {PercentageOfActorsGate.KEY, null},
                    {PercentageOfTimeGate.KEY, null}
                };
        }
    }
}
