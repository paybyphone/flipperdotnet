using StatsdClient;
using NUnit.Framework;
using Rhino.Mocks;
using FlipperDotNet.Adapter;
using FlipperDotNet.Instrumenter;

namespace FlipperDotNet.Tests.Instrumenter
{
	[TestFixture]
	public class StatsdGateInstrumenterIntegrationTests
	{
		private IFlipperActor FlipperActor { get; set; }
		private IStatsd StatsdClient { get; set; }
		private StatsdInstrumenter Instrumenter { get; set; }
		private Flipper Flipper { get; set; }

		[SetUp]
		public void SetUp()
		{
			FlipperActor = MockActor("User:5");
			StatsdClient = MockRepository.GenerateStub<IStatsd>();
			Instrumenter = new StatsdInstrumenter(StatsdClient);
			Flipper = new Flipper(new MemoryAdapter(), Instrumenter);
		}

		[Test]
		public void ShouldGenerateGateOperationTimerMetricWhenTestingFeature()
		{
			Flipper.Feature("Stats").EnableActor(FlipperActor);
			Flipper.Feature("Stats").IsEnabledFor(FlipperActor);

			StatsdClient.AssertWasCalled(x => x.LogTiming(Arg<string>.Is.Equal("flipper.gate_operation.boolean.open"), Arg<long>.Is.Anything));
		}

		[Test]
		public void ShouldGenerateFeatureSpecificGateOperationTimerMetricWhenTestingFeature()
		{
			Flipper.Feature("Stats").EnableActor(FlipperActor);
			Flipper.Feature("Stats").IsEnabledFor(FlipperActor);

			StatsdClient.AssertWasCalled(x => x.LogTiming(Arg<string>.Is.Equal("flipper.feature.Stats.gate_operation.boolean.open"), Arg<long>.Is.Anything));
		}

		[TestCase("actor","open")]
		[TestCase("boolean","closed")]
		public void ShouldGenerateFeatureSpecificGateOperationTimerMetricWhenTestingFeature(string gateName, string state)
		{
			Flipper.Feature("Stats").EnableActor(FlipperActor);
			Flipper.Feature("Stats").IsEnabledFor(FlipperActor);

			var expectedStatName = string.Format("flipper.feature.Stats.gate.{0}.{1}", gateName, state);

			StatsdClient.AssertWasCalled(x => x.LogCount(expectedStatName));
		}

		private static IFlipperActor MockActor(string id)
		{
			var actor = MockRepository.GenerateStub<IFlipperActor>();
			actor.Stub(x => x.FlipperId).Return(id);
			return actor;
		}
	}
}

