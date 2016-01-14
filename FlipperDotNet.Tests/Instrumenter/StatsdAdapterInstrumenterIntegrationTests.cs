using StatsdClient;
using NUnit.Framework;
using Rhino.Mocks;
using FlipperDotNet.Adapter;
using FlipperDotNet.Instrumenter;

namespace FlipperDotNet.Tests.Instrumenter
{
	[TestFixture]
	public class StatsdAdapterInstrumenterIntegrationTests
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
		public void ShouldCreateAFeatureTimerMetricWhenEnablingFeature()
		{
			Flipper.Feature("Name").EnableActor(FlipperActor);

			StatsdClient.AssertWasCalled(x => x.LogTiming(Arg<string>.Is.Equal("flipper.adapter.memory.enable"), Arg<long>.Is.Anything));
		}

		[Test]
		public void ShouldCreateAFeatureTimerMetricWhenDisablingFeature()
		{
			Flipper.Feature("Name").DisableActor(FlipperActor);

			StatsdClient.AssertWasCalled(x => x.LogTiming(Arg<string>.Is.Equal("flipper.adapter.memory.disable"), Arg<long>.Is.Anything));
		}

		[Test]
		public void ShouldCreateFeatureTimerMetricAndCountMetricWhenTestingEnabledFeature()
		{
			Flipper.Feature("Name").EnableActor(FlipperActor);

			TestAdapterMetricsForTestingFeature();
		}

		[Test]
		public void ShouldCreateFeatureTimerMetricAndCountMetricWhenTestingDisabledFeature()
		{
			Flipper.Feature("Name").DisableActor(FlipperActor);

			TestAdapterMetricsForTestingFeature();
		}

		public void TestAdapterMetricsForTestingFeature()
		{
			Flipper.Feature("Name").IsEnabledFor(FlipperActor);

			StatsdClient.AssertWasCalled(x => x.LogTiming(Arg<string>.Is.Equal("flipper.adapter.memory.get"), Arg<long>.Is.Anything));
		}

		private static IFlipperActor MockActor(string id)
		{
			var actor = MockRepository.GenerateStub<IFlipperActor>();
			actor.Stub(x => x.FlipperId).Return(id);
			return actor;
		}
	}
}

