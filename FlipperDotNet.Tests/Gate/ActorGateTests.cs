using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlipperDotNet.Gate;
using NUnit.Framework;
using Rhino.Mocks;

namespace FlipperDotNet.Tests.Gate
{
    [TestFixture]
    public class ActorGateTests
    {
        [Test]
        public void IsEnabledReturnsFalseForEmptySet()
        {
            var gate = new ActorGate();
            Assert.That(gate.IsEnabled(new HashSet<string>()), Is.False);
        }

        [Test]
        public void IsEnabledReturnsTrueForNonEmptySet()
        {
            var gate = new ActorGate();
            Assert.That(gate.IsEnabled(new HashSet<string>(new[] {"foo"})), Is.True);
        }

        [Test]
        public void IsOpenReturnsFalseForNullActor()
        {
            var gate = new ActorGate();

            Assert.That(gate.IsOpen(null, new HashSet<string>(new[] {"5"}), "feature"), Is.False);
        }

        [Test]
        public void IsOpenReturnsTrueWhenActorIsInSet()
        {
            var actor = MockRepository.GenerateStub<IFlipperActor>();
            actor.Stub(x => x.FlipperId).Return("5");
            var gate = new ActorGate();

            Assert.That(gate.IsOpen(actor, new HashSet<string>(new[] {"5"}), "feature"), Is.True);
        }

        [Test]
        public void IsOpenReturnsFalseWhenActorNotInSet()
        {
            var actor = MockRepository.GenerateStub<IFlipperActor>();
            actor.Stub(x => x.FlipperId).Return("5");
            var gate = new ActorGate();

            Assert.That(gate.IsOpen(actor, new HashSet<string>(new[] {"25"}), "feature"), Is.False);
        }

        [Test]
        public void IsOpenReturnsFalseWhenActorNotAnIFlipperActor()
        {
            var gate = new ActorGate();

            Assert.That(gate.IsOpen(new object(), new HashSet<string>(new[] {"5"}), "feature"), Is.False);
        }
    }
}
