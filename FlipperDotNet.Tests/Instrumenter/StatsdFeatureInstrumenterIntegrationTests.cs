using StatsdClient;
using NUnit.Framework;
using Rhino.Mocks;
using FlipperDotNet.Adapter;
using FlipperDotNet.Instrumenter;

namespace FlipperDotNet.Tests.Instrumenter
{
	[TestFixture]
	public class StatsdFeatureInstrumenterIntegrationTests
	{
		private IFlipperActor FlipperActor { get; set; }
		private IStatsd StatsdClient { get; set; }
		private StatsdInstrumenter Instrumenter { get; set; }
		private Feature Feature { get; set; }

		[SetUp]
		public void SetUp()
		{
			FlipperActor = MockActor("User:5");
			StatsdClient = MockRepository.GenerateStub<IStatsd>();
			Instrumenter = new StatsdInstrumenter(StatsdClient);
			Feature = new Feature("Name", new MemoryAdapter(), Instrumenter);
		}

		[Test]
		public void ShouldCreateAFeatureTimerMetricWhenEnablingFeature()
		{
			Feature.EnableActor(FlipperActor);

			StatsdClient.AssertWasCalled(x => x.LogTiming(Arg<string>.Is.Equal("flipper.feature_operation.enable"), Arg<long>.Is.Anything));
		}

		[Test]
		public void ShouldCreateAFeatureTimerMetricWhenDisablingFeature()
		{
			Feature.DisableActor(FlipperActor);

			StatsdClient.AssertWasCalled(x => x.LogTiming(Arg<string>.Is.Equal("flipper.feature_operation.disable"), Arg<long>.Is.Anything));
		}

		[Test]
		public void ShouldCreateFeatureTimerMetricAndCountMetricWhenTestingEnabledFeature()
		{
			const string counterName = "flipper.feature.Name.enabled";
			Feature.EnableActor(FlipperActor);

			TestFeatureMetricsForTestingFeature(counterName);
		}

		[Test]
		public void ShouldCreateFeatureTimerMetricAndCountMetricWhenTestingDisabledFeature()
		{
			const string counterName = "flipper.feature.Name.disabled";
			Feature.DisableActor(FlipperActor);

			TestFeatureMetricsForTestingFeature(counterName);
		}

		public void TestFeatureMetricsForTestingFeature(string counterName)
		{
			Feature.IsEnabledFor(FlipperActor);

			StatsdClient.AssertWasCalled(x => x.LogTiming(Arg<string>.Is.Equal("flipper.feature_operation.enabled"), Arg<long>.Is.Anything));
			StatsdClient.AssertWasCalled(x => x.LogCount(counterName));
		}

		private static IFlipperActor MockActor(string id)
		{
			var actor = MockRepository.GenerateStub<IFlipperActor>();
			actor.Stub(x => x.FlipperId).Return(id);
			return actor;
		}
	}
}

