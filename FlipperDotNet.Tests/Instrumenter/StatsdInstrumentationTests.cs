using System;
using NUnit.Framework;
using StatsdClient;
using Rhino.Mocks;
using FlipperDotNet.Instrumenter;
using FlipperDotNet.Util;

namespace FlipperDotNet.Tests.Instrumenter
{
	[TestFixtureAttribute]
	public class StatsdInstrumentationTests
	{
		[Test]
		public void ShouldPublishFeatureTimingMetricForAllOperations()
		{
			var operationName = "Operation";
			long milliseconds = 15;
			var clock = MockClock(milliseconds);
			var StatsdClient = MockRepository.GenerateStub<IStatsd>();
			var payload = new InstrumentationPayload {
				Operation = operationName,
			};

			var Instrumenter = new StatsdInstrumenter(StatsdClient, clock);
			var token = Instrumenter.InstrumentFeature(payload);
			token.Dispose();

			StatsdClient.AssertWasCalled(x => x.LogTiming(string.Format("flipper.feature_operation.{0}", operationName), milliseconds));
		}

		[Test]
		public void ShouldStripQuestionMarkFromOperationName()
		{
			var operationName = "Operation?";
			var expectedOperationName = "Operation";
			var StatsdClient = MockRepository.GenerateStub<IStatsd>();
			var payload = new InstrumentationPayload {
				Operation = operationName,
			};

			var Instrumenter = new StatsdInstrumenter(StatsdClient);
			var token = Instrumenter.InstrumentFeature(payload);
			token.Dispose();

			StatsdClient.AssertWasCalled(x => x.LogTiming(
				Arg<string>.Is.Equal(string.Format("flipper.feature_operation.{0}", expectedOperationName)),
				Arg<long>.Is.Anything));
		}

		[TestCase(true, "enabled")]
		[TestCase(false, "disabled")]
		public void ShouldPublishFeatureCountMetricForIsEnabledOperation(bool isEnabled, string status)
		{
			var featureName = "Feature";
			var operationName = "enabled?";
			var StatsdClient = MockRepository.GenerateStub<IStatsd>();
			var payload = new InstrumentationPayload {
				FeatureName = featureName,
				Operation = operationName,
				Result = isEnabled,
			};

			var Instrumenter = new StatsdInstrumenter(StatsdClient);
			var token = Instrumenter.InstrumentFeature(payload);
			token.Dispose();

			StatsdClient.AssertWasCalled(x => x.LogCount(string.Format("flipper.feature.{0}.{1}", featureName, status)));
		}

		[Test]
		public void ShouldPublishAdapterTimingMetricForAllOperations()
		{
			var adapterName = "Adapter";
			var operationName = "Operation";
			long milliseconds = 15;
			var clock = MockClock(milliseconds);
			var StatsdClient = MockRepository.GenerateStub<IStatsd>();
			var payload = new InstrumentationPayload {
				AdapterName = adapterName,
				Operation = operationName,
			};

			var Instrumenter = new StatsdInstrumenter(StatsdClient, clock);
			var token = Instrumenter.InstrumentAdapter(payload);
			token.Dispose();

			StatsdClient.AssertWasCalled(x => x.LogTiming(string.Format("flipper.adapter.{0}.{1}", adapterName, operationName), milliseconds));
		}

		[Test]
		public void ShouldPublishGateTimingMetricForAllOperations()
		{
			var gateName = "Gate";
			var operationName = "Operation";
			long milliseconds = 15;
			var clock = MockClock(milliseconds);
			var StatsdClient = MockRepository.GenerateStub<IStatsd>();
			var payload = new InstrumentationPayload {
				GateName = gateName,
				Operation = operationName,
			};

			var Instrumenter = new StatsdInstrumenter(StatsdClient, clock);
			var token = Instrumenter.InstrumentGate(payload);
			token.Dispose();

			StatsdClient.AssertWasCalled(x => x.LogTiming(string.Format("flipper.gate_operation.{0}.{1}", gateName, operationName), milliseconds));
		}

		[Test]
		public void ShouldPublishFeatureSpecificGateTimingMetricForAllOperations()
		{
			var featureName = "Feature";
			var gateName = "Gate";
			var operationName = "Operation";
			long milliseconds = 15;
			var clock = MockClock(milliseconds);
			var StatsdClient = MockRepository.GenerateStub<IStatsd>();
			var payload = new InstrumentationPayload {
				FeatureName = featureName,
				GateName = gateName,
				Operation = operationName,
			};

			var Instrumenter = new StatsdInstrumenter(StatsdClient, clock);
			var token = Instrumenter.InstrumentGate(payload);
			token.Dispose();

			StatsdClient.AssertWasCalled(x => x.LogTiming(string.Format("flipper.feature.{0}.gate_operation.{1}.{2}", featureName, gateName, operationName), milliseconds));
		}

		[TestCase(true, "open")]
		[TestCase(false, "closed")]
		public void ShouldPublishGateCountMetricForIsOpenOperation(bool isEnabled, string status)
		{
			var featureName = "Feature";
			var gateName = "Gate";
			var operationName = "open?";
			var StatsdClient = MockRepository.GenerateStub<IStatsd>();
			var payload = new InstrumentationPayload {
				FeatureName = featureName,
				GateName = gateName,
				Operation = operationName,
				Result = isEnabled,
			};

			var Instrumenter = new StatsdInstrumenter(StatsdClient);
			var token = Instrumenter.InstrumentGate(payload);
			token.Dispose();

			StatsdClient.AssertWasCalled(x => x.LogCount(string.Format("flipper.feature.{0}.gate.{1}.{2}", featureName, gateName, status)));
		}
			
		static IClock MockClock(long milliseconds)
		{
			var clock = MockRepository.GenerateStub<IClock>();
			var time = DateTime.Now;
			clock.Stub(x => x.Now).Return(time).Repeat.Once();
			clock.Stub(x => x.Now).Return(time.AddMilliseconds(milliseconds)).Repeat.Once();
			return clock;
		}
	}
}

