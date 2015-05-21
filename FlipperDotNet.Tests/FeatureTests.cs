using FlipperDotNet.Adapter;
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
    public class FeatureGroupsValueTests { }

    [TestFixture, Ignore]
    public class FeatureActorsValueTests { }

    [TestFixture]
    public class FeaturePercentageOfTimeValueTests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
        }

        [Test]
        public void ShouldDefaultToZero()
        {
            Assert.That(_feature.PercentageOfTimeValue, Is.EqualTo(0));
        }

        [Test]
        public void ShouldReturnValueWhenEnabled()
        {
            _feature.EnablePercentageOfTime(5);
            Assert.That(_feature.PercentageOfTimeValue, Is.EqualTo(5));
        }

        [Test]
        public void ShouldReturnZeroWhenDisabled()
        {
            _feature.Disable();
            Assert.That(_feature.PercentageOfTimeValue, Is.EqualTo(0));
        }
    }

    [TestFixture]
    public class FeaturePercentageOfActorsValueTests
    {
        private Feature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = new Feature("Test", new MemoryAdapter());
        }

        [Test]
        public void ShouldDefaultToZero()
        {
            Assert.That(_feature.PercentageOfActorsValue, Is.EqualTo(0));
        }

        [Test]
        public void ShouldReturnValueWhenEnabled()
        {
            _feature.EnablePercentageOfActors(5);
            Assert.That(_feature.PercentageOfActorsValue, Is.EqualTo(5));
        }

        [Test]
        public void ShouldReturnZeroWhenDisabled()
        {
            _feature.Disable();
            Assert.That(_feature.PercentageOfActorsValue, Is.EqualTo(0));
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
            Assert.That(_feature.GateValues, Is.EqualTo(new GateValues(new FeatureResult())));
        }

        [Test, Ignore("No Actors or Groups support yet")]
        public void ShouldReturnValuesSetInAdapter()
        {
            _feature.Enable();
            //_feature.EnableActor(5);
            //_feature.EnableGroup("admins");
            _feature.EnablePercentageOfTime(50);
            _feature.EnablePercentageOfActors(25);

            var gateValues = _feature.GateValues;

            Assert.That(gateValues.Boolean, Is.True);
            //Assert.That(gateValues.Actors, Is.EquivalentTo(new[] {"5"}));
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

    [TestFixture, Ignore]
    public class EnabledGroupsTests{}

    [TestFixture, Ignore]
    public class DisabledGroupsTests { }
}
