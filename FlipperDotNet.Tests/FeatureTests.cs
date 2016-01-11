using System;
using System.Collections.Generic;
using System.Linq;
using FlipperDotNet.Adapter;
using FlipperDotNet.Gate;
using FlipperDotNet.Instrumenter;
using NUnit.Framework;
using Rhino.Mocks;

namespace FlipperDotNet.Tests
{
    [TestFixture]
    public class FeatureTests
    {
        [Test]
        public void ConstructorShouldSetName()
        {
            const string name = "Test";
            var feature = new Feature(name, null);
            Assert.That(feature.Name, Is.EqualTo(name));
        }

        [Test]
        public void ConstructorShouldSetAdapter()
        {
            var adapter = MockRepository.GenerateStub<IAdapter>();
            var feature = new Feature("Name", adapter);
            Assert.That(feature.Adapter, Is.EqualTo(adapter));
        }

		[Test]
		public void ConstructorSetsNoOpInstrumenterByDefault()
		{
			var feature = new Feature("Name", MockRepository.GenerateStub<IAdapter>());
			Assert.That(feature.Instrumenter, Is.InstanceOf<NoOpInstrumenter>());
		}

		[Test]
		public void ConstructorShouldThrowExceptionOnNullInstrumenter()
		{
			Assert.Throws<ArgumentNullException>(delegate {
				var feature = new Feature("Name", MockRepository.GenerateStub<IAdapter>(), null);
			});
		}

		[Test]
		public void ConstructorShouldSetInstrumenter()
		{
			var instrumenter = MockRepository.GenerateStub<IInstrumenter>();
			var feature = new Feature("Name", MockRepository.GenerateStub<IAdapter>(), instrumenter);
			Assert.That(feature.Instrumenter, Is.EqualTo(instrumenter));
		}
    }

    [TestFixture]
    public class FullyOnFeatureTests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
            _feature.Enable();
        }

        [Test]
        public void ShouldReturnStateOfOn()
        {
            Assert.That(_feature.State, Is.EqualTo(FeatureState.On));
        }

        [Test]
        public void ShouldReturnTrueForIsOn()
        {
            Assert.That(_feature.IsOn, Is.True);
        }

        [Test]
        public void ShouldReturnFalseForIsOff()
        {
            Assert.That(_feature.IsOff, Is.False);
        }

        [Test]
        public void ShouldReturnFalseForIsConditional()
        {
            Assert.That(_feature.IsConditional, Is.False);
        }
    }

    [TestFixture]
    public class PercentageOfTime100Tests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
            _feature.EnablePercentageOfTime(100);
        }

        [Test]
        public void ShouldReturnStateOfOn()
        {
            Assert.That(_feature.State, Is.EqualTo(FeatureState.On));
        }

        [Test]
        public void ShouldReturnTrueForIsOn()
        {
            Assert.That(_feature.IsOn, Is.True);
        }

        [Test]
        public void ShouldReturnFalseForIsOff()
        {
            Assert.That(_feature.IsOff, Is.False);
        }

        [Test]
        public void ShouldReturnFalseForIsConditional()
        {
            Assert.That(_feature.IsConditional, Is.False);
        }
    }

    [TestFixture]
    public class PercentageOfActors100Tests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
            _feature.EnablePercentageOfActors(100);
        }

        [Test]
        public void ShouldReturnStateOfOn()
        {
            Assert.That(_feature.State, Is.EqualTo(FeatureState.On));
        }

        [Test]
        public void ShouldReturnTrueForIsOn()
        {
            Assert.That(_feature.IsOn, Is.True);
        }

        [Test]
        public void ShouldReturnFalseForIsOff()
        {
            Assert.That(_feature.IsOff, Is.False);
        }

        [Test]
        public void ShouldReturnFalseForIsConditional()
        {
            Assert.That(_feature.IsConditional, Is.False);
        }
    }

    [TestFixture]
    public class FullyOffFeatureTests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
            _feature.Disable();
        }

        [Test]
        public void ShouldReturnStateOfOff()
        {
            Assert.That(_feature.State, Is.EqualTo(FeatureState.Off));
        }

        [Test]
        public void ShouldReturnFalseForIsOn()
        {
            Assert.That(_feature.IsOn, Is.False);
        }

        [Test]
        public void ShouldReturnTrueForIsOff()
        {
            Assert.That(_feature.IsOff, Is.True);
        }

        [Test]
        public void ShouldReturnFalseForIsConditional()
        {
            Assert.That(_feature.IsConditional, Is.False);
        }
    }

    [TestFixture]
    public class PartiallyOnFeatureTests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
            _feature.EnablePercentageOfTime(5);
        }

        [Test]
        public void ShouldReturnStateOfConditional()
        {
            Assert.That(_feature.State, Is.EqualTo(FeatureState.Conditional));
        }

        [Test]
        public void ShouldReturnFalseForIsOn()
        {
            Assert.That(_feature.IsOn, Is.False);
        }

        [Test]
        public void ShouldReturnFalseForIsOff()
        {
            Assert.That(_feature.IsOff, Is.False);
        }

        [Test]
        public void ShouldReturnTrueForIsConditional()
        {
            Assert.That(_feature.IsConditional, Is.True);
        }
    }

    [TestFixture]
    public class FeatureBooleanValueTests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
        }

        [Test]
        public void ShouldDefaultToFalse()
        {
            Assert.That(_feature.BooleanValue, Is.False);
        }

        [Test]
        public void ShouldReturnTrueWhenEnabled()
        {
            _feature.Enable();
            Assert.That(_feature.BooleanValue, Is.True);
        }

        [Test]
        public void ShouldReturnFalseWhenDisabled()
        {
            _feature.Disable();
            Assert.That(_feature.BooleanValue, Is.False);
        }
    }

    [TestFixture, Ignore]
    public class FeatureGroupsValueTests
    {
    }

    [TestFixture]
    public class FeatureActorsValueTests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
        }

        [Test]
        public void ShouldDefaultToEmpty()
        {
            Assert.That(_feature.ActorsValue, Is.Empty);
        }

        [Test]
        public void ShouldReturnActorIdsWhenSet()
        {
            _feature.EnableActor(MockActor("User:5"));
            _feature.EnableActor(MockActor("User:22"));

            Assert.That(_feature.ActorsValue, Is.EquivalentTo(new[] {"User:5", "User:22"}));
        }

        [Test]
        public void ShouldDisableActor()
        {
            _feature.EnableActor(MockActor("5"));

            _feature.DisableActor(MockActor("5"));

            Assert.That(_feature.ActorsValue, Is.Empty);
        }

        private static IFlipperActor MockActor(string id)
        {
            var actor = MockRepository.GenerateStub<IFlipperActor>();
            actor.Stub(x => x.FlipperId).Return(id);
            return actor;
        }
    }

	public abstract class FeaturePercentageValueTests
	{
		protected Feature _feature;
		
		protected abstract int PercentageValue { get; }
		protected abstract void EnablePercentage(int percentage);
		protected abstract void DisablePercentage();

		[SetUp]
		public void SetUp()
		{
			_feature = new Feature("Test", new MemoryAdapter());
		}

		[Test]
		public void ShouldDefaultToZero()
		{
			Assert.That(PercentageValue, Is.EqualTo(0));
		}

		[Test]
		public void ShouldReturnValueWhenEnabled()
		{
			EnablePercentage(5);
			Assert.That(PercentageValue, Is.EqualTo(5));
		}

		[Test]
		public void ShouldReturnZeroWhenFullyDisabled()
		{
			_feature.Disable();
			Assert.That(PercentageValue, Is.EqualTo(0));
		}

		[Test]
		public void ShouldReturnZeroWhenDisabled()
		{
			DisablePercentage();
			Assert.That(PercentageValue, Is.EqualTo(0));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage="Value must be a positive number less than or equal to 100, but was -1")]
		public void ShouldThrowExceptionWhenValueSetToNegativeNumber()
		{
			EnablePercentage(-1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage="Value must be a positive number less than or equal to 100, but was 101")]
		public void ShouldThrowExceptionWhenValueSetToNumberOver100()
		{
			EnablePercentage(101);
		}
	}

    [TestFixture]
	public class FeaturePercentageOfTimeValueTests : FeaturePercentageValueTests
    {
		protected override int PercentageValue {
			get {
				return _feature.PercentageOfTimeValue;
			}
		}

		protected override void EnablePercentage(int percentage)
		{
			_feature.EnablePercentageOfTime(percentage);
		}

		protected override void DisablePercentage()
		{
			_feature.DisablePercentageOfTime();
		}
    }

    [TestFixture]
	public class FeaturePercentageOfActorsValueTests : FeaturePercentageValueTests
    {
		protected override int PercentageValue {
			get {
				return _feature.PercentageOfActorsValue;
			}
		}

		protected override void EnablePercentage(int percentage)
		{
			_feature.EnablePercentageOfActors(percentage);
		}

		protected override void DisablePercentage()
		{
			_feature.DisablePercentageOfActors();
		}
    }

    [TestFixture]
    public class FeatureGateValuesTests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
        }

        [Test]
        public void ShouldDefaultToEmpty()
        {
            Assert.That(_feature.GateValues, Is.EqualTo(new GateValues(new Dictionary<string, object>())));
        }

        [Test, Ignore("No Groups support yet")]
        public void ShouldReturnValuesSetInAdapter()
        {
            var actor = MockRepository.GenerateStub<IFlipperActor>();
            actor.Stub(x => x.FlipperId).Return("5");

            _feature.Enable();
            _feature.EnableActor(actor);
            //_feature.EnableGroup("admins");
            _feature.EnablePercentageOfTime(50);
            _feature.EnablePercentageOfActors(25);

            var gateValues = _feature.GateValues;

            Assert.That(gateValues.Boolean, Is.True);
            Assert.That(gateValues.Actors, Is.EquivalentTo(new[] {"5"}));
            //Assert.That(gateValues.Groups, Is.EquivalentTo(new[] {"admins"}));
            Assert.That(gateValues.PercentageOfTime, Is.EqualTo(50));
            Assert.That(gateValues.PercentageOfActors, Is.EqualTo(25));
        }
    }

    [TestFixture]
    public class GateTests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
        }

        [Test]
        public void ShouldRetrieveGateByName()
        {
            Assert.That(_feature.Gate("boolean").Key, Is.EqualTo("boolean"));
        }

        [Test]
        public void ShouldReturnNullForNonExistantGate()
        {
            Assert.That(_feature.Gate("foo"), Is.Null);
        }
    }

    [TestFixture, Ignore("No Groups support yet")]
    public class EnabledGroupsTests
    {
    }

    [TestFixture, Ignore("No Groups support yet")]
    public class DisabledGroupsTests
    {
    }

    [TestFixture]
    public class EnabledAndDisabledGatesTests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
            _feature.EnablePercentageOfTime(5);
            _feature.EnablePercentageOfActors(15);
        }

        [Test]
        public void ShouldReturnEnabledGates()
        {
            var gates = _feature.EnabledGates;
            var names = from gate in gates
                        select gate.Name;

            Assert.That(names, Is.EquivalentTo(new[] {PercentageOfTimeGate.NAME, PercentageOfActorsGate.NAME}));
        }

        [Test]
        public void ShouldReturnDisabledGates()
        {
            var gates = _feature.DisabledGates;
            var names = from gate in gates
                        select gate.Name;

            Assert.That(names, Is.EquivalentTo(new[] {BooleanGate.NAME, GroupGate.NAME, ActorGate.NAME}));
        }
    }

    [TestFixture]
    public class IsEnabledTests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
        }

        [Test]
        public void ShouldReturnTrueWhenFullyEnabled()
        {
            _feature.Enable();
            Assert.That(_feature.IsEnabled, Is.True);
        }

        [Test]
        public void ForActorShouldReturnTrueWhenFullyEnabled()
        {
            var actor = MockRepository.GenerateStub<IFlipperActor>();
            actor.Stub(x => x.FlipperId).Return("22");
            _feature.Enable();
            Assert.That(_feature.IsEnabledFor(actor), Is.True);
        }

        [Test, Ignore("No Group support yet")]
        public void ForGroupShouldReturnTrueWhenFullyEnabled()
        {
            //var group = new object();
            //_feature.Enable();
            //Assert.That(_feature.IsEnabledFor(group), Is.True);
            throw new NotImplementedException();
        }

        [Test]
        public void ShouldReturnFalseWhenFullyDisabled()
        {
            _feature.Disable();
            Assert.That(_feature.IsEnabled, Is.False);
        }

        [TestCase(1, 50, ExpectedResult = true)]
        [TestCase(1, 20, ExpectedResult = false)]
        public bool TestIsEnabledWhenPercentageOfTimeSet(int seed, int percentage)
        {
            HackNewPercentageOfTimeGateIntoPlace(seed);

            _feature.EnablePercentageOfTime(percentage);

            return _feature.IsEnabled;
        }

        private void HackNewPercentageOfTimeGateIntoPlace(int seed)
        {
            var hackyIndex = _feature.Gates.IndexOf(_feature.PercentageOfTimeGate);
            var newPoTGate = new PercentageOfTimeGate(new Random(seed));
            _feature.Gates[hackyIndex] = newPoTGate;
        }

        [Test]
        public void ForActorShouldReturnFalseWhenFullyDisabled()
        {
            var actor = MockRepository.GenerateStub<IFlipperActor>();
            actor.Stub(x => x.FlipperId).Return("22");
            _feature.Disable();
            Assert.That(_feature.IsEnabledFor(actor), Is.False);
        }

        [Test, Ignore("No Group support yet")]
        public void ForGroupShouldReturnFalseWhenFullyDisabled()
        {
            //var group = new object();
            //_feature.Disable();
            //Assert.That(_feature.IsEnabledFor(group), Is.False);
            throw new NotImplementedException();
        }

        [Test]
        public void ForActorShouldReturnTrueWhenActorIsEnabled()
        {
            var actor = MockRepository.GenerateStub<IFlipperActor>();
            actor.Stub(x => x.FlipperId).Return("22");
            _feature.EnableActor(actor);
            Assert.That(_feature.IsEnabledFor(actor), Is.True);
        }

        [Test]
        public void ForActorShouldReturnFalseWhenActorIsDisabled()
        {
            var actor = MockRepository.GenerateStub<IFlipperActor>();
            actor.Stub(x => x.FlipperId).Return("22");
            _feature.DisableActor(actor);
            Assert.That(_feature.IsEnabledFor(actor), Is.False);
        }
    }
}
