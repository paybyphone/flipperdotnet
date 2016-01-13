using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using FlipperDotNet.Adapter;
using FlipperDotNet.Instrumenter;
using FlipperDotNet.Tests.Adapter;

namespace FlipperDotNet.Tests
{
	[TestFixture]
	public class FeatureInstrumentationTests
	{
		[Test]
		public void ShouldRecordInstrumentationForEnable()
		{
			var instrumenter = new MockInstrumenter();
			var feature = new Feature("Name", new MemoryAdapter(), instrumenter);

			var flipperActor = MockActor("User:5");
			feature.EnableActor(flipperActor);

			var expectedPayload = new InstrumentationPayload {
				FeatureName = "Name",
				GateName = feature.ActorGate.Name,
				Operation = "enable",
				Thing = flipperActor,
			};

			Assert.That(instrumenter.Events.First().Type, Is.EqualTo(InstrumentationType.FeatureOperation));
			Assert.That(instrumenter.Events.First().Payload, Is.EqualTo(expectedPayload));
		}

		[Test]
		public void ShouldRecordInstrumentationForDisable()
		{
			var instrumenter = new MockInstrumenter();
			var feature = new Feature("Name", new MemoryAdapter(), instrumenter);

			feature.Disable();

			var expectedPayload = new InstrumentationPayload {
				FeatureName = "Name",
				GateName = feature.BooleanGate.Name,
				Operation = "disable",
				Thing = false,
			};

			Assert.That(instrumenter.Events.First().Type, Is.EqualTo(InstrumentationType.FeatureOperation));
			Assert.That(instrumenter.Events.First().Payload, Is.EqualTo(expectedPayload));
		}

		[Test]
		public void ShouldRecordInstrumentationForIsEnabled([Values(true,false)] Boolean enabled)
		{
			var instrumenter = new MockInstrumenter();
			var feature = new Feature("Name", new MemoryAdapter(), instrumenter);

			if (enabled)
			{
				feature.Enable();
			} else
			{
				feature.Disable();
			}
			instrumenter.Events.Clear();

			feature.IsEnabled();

			var expectedPayload = new InstrumentationPayload {
				FeatureName = "Name",
				Operation = "enabled?",
				Thing = null,
				Result = enabled
			};
			if (enabled)
			{
				expectedPayload.GateName = feature.BooleanGate.Name;
			}
			var expectedEvent = new MockInstrumenter.Event {
				Type = InstrumentationType.FeatureOperation,
				Payload = expectedPayload,
			};

			Assert.That(instrumenter.Events, Has.Member(expectedEvent));
		}

		[Test]
		public void ShouldRecordGateInstrumentationForIsEnabled([Values(true,false)] Boolean enabled)
		{
			var instrumenter = new MockInstrumenter();
			var feature = new Feature("Name", new MemoryAdapter(), instrumenter);

			if (enabled)
			{
				feature.Enable();
			} else
			{
				feature.Disable();
			}
			instrumenter.Events.Clear();

			feature.IsEnabled();

			var expectedPayload = new InstrumentationPayload {
				FeatureName = "Name",
				GateName = "boolean",
				Operation = "open?",
				Thing = null,
			};
			if (enabled)
			{
				expectedPayload.GateName = feature.BooleanGate.Name;
			}
			var expectedEvent = new MockInstrumenter.Event {
				Type = InstrumentationType.GateOperation,
				Payload = expectedPayload,
			};

			Assert.That(instrumenter.Events, Has.Member(expectedEvent));
		}

		[Test]
		public void ShouldRecordInstrumentationForIsEnabledFor([Values(true,false)] Boolean enabled)
		{
			var instrumenter = new MockInstrumenter();
			var feature = new Feature("Name", new MemoryAdapter(), instrumenter);

			var flipperActor = MockActor("User:5");
			if (enabled)
			{
				feature.EnableActor(flipperActor);
			} else
			{
				feature.DisableActor(flipperActor);
			}
			instrumenter.Events.Clear();

			feature.IsEnabledFor(flipperActor);

			var expectedPayload = new InstrumentationPayload {
				FeatureName = "Name",
				Operation = "enabled?",
				Thing = flipperActor,
				Result = enabled
			};
			if (enabled)
			{
				expectedPayload.GateName = feature.ActorGate.Name;
			}
			var expectedEvent = new MockInstrumenter.Event {
				Type = InstrumentationType.FeatureOperation,
				Payload = expectedPayload,
			};

			Assert.That(instrumenter.Events, Has.Member(expectedEvent));
		}

		[Test]
		public void ShouldRecordGateInstrumentationForIsEnabledFor([Values(true,false)] Boolean enabled)
		{
			var instrumenter = new MockInstrumenter();
			var feature = new Feature("Name", new MemoryAdapter(), instrumenter);

			var flipperActor = MockActor("User:5");
			if (enabled)
			{
				feature.EnableActor(flipperActor);
			} else
			{
				feature.DisableActor(flipperActor);
			}
			instrumenter.Events.Clear();

			feature.IsEnabledFor(flipperActor);

			var expectedPayload = new InstrumentationPayload {
				FeatureName = "Name",
				GateName = "actor",
				Operation = "open?",
				Thing = flipperActor,
			};
			if (enabled)
			{
				expectedPayload.GateName = feature.ActorGate.Name;
			}
			var expectedEvent = new MockInstrumenter.Event {
				Type = InstrumentationType.GateOperation,
				Payload = expectedPayload,
			};

			Assert.That(instrumenter.Events, Has.Member(expectedEvent));
		}

		private static IFlipperActor MockActor(string id)
		{
			var actor = MockRepository.GenerateStub<IFlipperActor>();
			actor.Stub(x => x.FlipperId).Return(id);
			return actor;
		}
	}
}

