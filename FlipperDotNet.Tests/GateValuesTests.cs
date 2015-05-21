using FlipperDotNet.Adapter;
using FlipperDotNet.Gate;
using NUnit.Framework;

namespace FlipperDotNet.Tests
{
    [TestFixture]
    public class GateValuesTests
    {
        [TestCase(null, ExpectedResult = false)]
        [TestCase(false, ExpectedResult = false)]
        [TestCase(true, ExpectedResult = true)]
        public bool ShouldReturnBooleanValue(bool? input)
        {
            var adapterValues = new FeatureResult {Boolean = input};
            var gateValues = new GateValues(adapterValues);
            return gateValues.Boolean;
        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        public int ShouldReturnPercentageOfTimeValue(int input)
        {
            var adapterValues = new FeatureResult {PercentageOfTime = input};
            var gateValues = new GateValues(adapterValues);
            return gateValues.PercentageOfTime;
        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        public int ShouldReturnPercentageOfActorsValue(int input)
        {
            var adapterValues = new FeatureResult { PercentageOfActors = input };
            var gateValues = new GateValues(adapterValues);
            return gateValues.PercentageOfActors;
        }

        [Test]
        public void ShouldRetrieveTheBooleanValue()
        {
            var adapterValues = new FeatureResult {Boolean = true};
            var gateValues = new GateValues(adapterValues);
            Assert.That(gateValues[BooleanGate.KEY], Is.EqualTo(true));
        }

        [Test]
        public void ShouldRetrieveTheActorsValue()
        {
            var adapterValues = new FeatureResult();
            adapterValues.Actors.Add("1");
            adapterValues.Actors.Add("2");
            var gateValues = new GateValues(adapterValues);
            Assert.That(gateValues[ActorGate.KEY], Is.EquivalentTo(new[] {"1", "2"}));
        }

        [Test]
        public void ShouldRetrieveTheGroupsValue()
        {
            var adapterValues = new FeatureResult();
            adapterValues.Groups.Add("admins");
            var gateValues = new GateValues(adapterValues);
            Assert.That(gateValues[GroupGate.KEY], Is.EquivalentTo(new[] { "admins" }));
        }

        [Test]
        public void ShouldRetrieveThePercentageOfTimeValue()
        {
            var adapterValues = new FeatureResult {PercentageOfTime = 15};
            var gateValues = new GateValues(adapterValues);
            Assert.That(gateValues[PercentageOfTimeGate.KEY], Is.EqualTo(15));
        }

        [Test]
        public void ShouldRetrieveThePercentageOfActorsValue()
        {
            var adapterValues = new FeatureResult { PercentageOfActors = 25 };
            var gateValues = new GateValues(adapterValues);
            Assert.That(gateValues[PercentageOfActorsGate.KEY], Is.EqualTo(25));
        }

        [Test]
        public void ShouldReturnNullForNonExistantValue()
        {
            var adapterValues = new FeatureResult();
            var gateValues = new GateValues(adapterValues);
            Assert.That(gateValues["foo"], Is.Null);
        }
    }
}
