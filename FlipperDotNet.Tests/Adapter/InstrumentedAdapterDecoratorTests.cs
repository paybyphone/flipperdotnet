using System;
using FlipperDotNet.Adapter;
using FlipperDotNet.Instrumenter;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FlipperDotNet.Tests.Adapter
{
	public class InstrumentedAdapterDecoratorTests : AdapterTests.SharedAdapterTests
	{
		private MockInstrumenter Instrumenter { get; set; }

		[SetUp]
		public new void SetUp()
		{
			var decoratedAdapter = new MemoryAdapter();
			Instrumenter = new MockInstrumenter();
			Adapter = new InstrumentedAdapterDecorator(decoratedAdapter, Instrumenter);
		}

		[Test]
		public void ShouldRecordInstrumentationWhenGettingFeature()
		{
			var feature = Flipper.Feature("Stats");
			var result = Adapter.Get(feature);

			var expectedPayload = new InstrumentationPayload {
				Operation = "get",
				AdapterName = "memory",
				FeatureName = "Stats",
				Result = result,
			};

			Assert.That(Instrumenter.Events.First().Name, Is.EqualTo("adapter_operation.flipper"));
			Assert.That(Instrumenter.Events.First().Payload, Is.EqualTo(expectedPayload));
		}

		[Test]
		public void ShouldRecordInstrumentationWhenEnablingFeature()
		{
			var feature = Flipper.Feature("Stats");
			var gate = feature.PercentageOfActorsGate;
			var value = 22;
			Adapter.Enable(feature, gate, value);

			var expectedPayload = new InstrumentationPayload {
				Operation = "enable",
				AdapterName = "memory",
				FeatureName = "Stats",
				GateName = "percentage_of_actors",
			};

			Assert.That(Instrumenter.Events.First().Name, Is.EqualTo("adapter_operation.flipper"));
			Assert.That(Instrumenter.Events.First().Payload, Is.EqualTo(expectedPayload));
		}

		[Test]
		public void ShouldRecordInstrumentationWhenDisablingFeature()
		{
			var feature = Flipper.Feature("Stats");
			var gate = feature.PercentageOfActorsGate;
			var value = 22;
			Adapter.Disable(feature, gate, value);

			var expectedPayload = new InstrumentationPayload {
				Operation = "disable",
				AdapterName = "memory",
				FeatureName = "Stats",
				GateName = "percentage_of_actors",
			};

			Assert.That(Instrumenter.Events.First().Name, Is.EqualTo("adapter_operation.flipper"));
			Assert.That(Instrumenter.Events.First().Payload, Is.EqualTo(expectedPayload));
		}

		[Test]
		public void ShouldRecordInstrumentationWhenAddingFeature()
		{
			var feature = Flipper.Feature("Stats");
			Adapter.Add(feature);

			var expectedPayload = new InstrumentationPayload {
				Operation = "add",
				AdapterName = "memory",
				FeatureName = "Stats",
			};

			Assert.That(Instrumenter.Events.First().Name, Is.EqualTo("adapter_operation.flipper"));
			Assert.That(Instrumenter.Events.First().Payload, Is.EqualTo(expectedPayload));
		}

		[Test]
		public void ShouldRecordInstrumentationWhenRemovingFeature()
		{
			var feature = Flipper.Feature("Stats");
			Adapter.Remove(feature);

			var expectedPayload = new InstrumentationPayload {
				Operation = "remove",
				AdapterName = "memory",
				FeatureName = "Stats",
			};

			Assert.That(Instrumenter.Events.First().Name, Is.EqualTo("adapter_operation.flipper"));
			Assert.That(Instrumenter.Events.First().Payload, Is.EqualTo(expectedPayload));
		}

		[Test]
		public void ShouldRecordInstrumentationWhenClearingFeature()
		{
			var feature = Flipper.Feature("Stats");
			Adapter.Clear(feature);

			var expectedPayload = new InstrumentationPayload {
				Operation = "clear",
				AdapterName = "memory",
				FeatureName = "Stats",
			};

			Assert.That(Instrumenter.Events.First().Name, Is.EqualTo("adapter_operation.flipper"));
			Assert.That(Instrumenter.Events.First().Payload, Is.EqualTo(expectedPayload));
		}

		[Test]
		public void ShouldRecordInstrumentationWhenLoadingFeatures()
		{
			var feature = Flipper.Feature("Stats");
			var result = Adapter.Features;

			var expectedPayload = new InstrumentationPayload {
				Operation = "features",
				AdapterName = "memory",
				Result = result,
			};

			Assert.That(Instrumenter.Events.First().Name, Is.EqualTo("adapter_operation.flipper"));
			Assert.That(Instrumenter.Events.First().Payload, Is.EqualTo(expectedPayload));
		}
	}

	internal class MockInstrumenter : IInstrumenter
	{
		public struct Event
		{
			public string Name;
			public InstrumentationPayload Payload;

			public override string ToString()
			{
				return string.Format("<Name=\"{0}\", Payload=\"{1}\">", Name, Payload);
			}
		}

		public List<Event> Events = new List<Event>();

		public IInstrumentationToken Instrument(string name, InstrumentationPayload payload)
		{
			return new InstrumentationToken(this, name, payload);
		}

		public class InstrumentationToken : IInstrumentationToken
		{
			readonly MockInstrumenter _instrumenter;
			readonly string _name;
			InstrumentationPayload _payload;

			public InstrumentationToken(MockInstrumenter instrumenter, string name, InstrumentationPayload payload)
			{
				_instrumenter = instrumenter;
				_name = name;
				_payload = payload;
			}

			public void Dispose()
			{
				_instrumenter.Events.Add(new Event {
					Name = _name,
					Payload = _payload,
				});
			}
		}
	}
}

