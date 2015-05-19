using FlipperDotNet.Gate;
using NUnit.Framework;

namespace FlipperDotNet.Tests.Gate
{
    [TestFixture]
    class PercentageOfActorsGateTests
    {
        [TestCase(0, ExpectedResult = false)]
        [TestCase(1, ExpectedResult = true)]
        public bool IsEnabled(int value)
        {
            var gate = new PercentageOfActorsGate();
            return gate.IsEnabled(value);
        }
    }
}
