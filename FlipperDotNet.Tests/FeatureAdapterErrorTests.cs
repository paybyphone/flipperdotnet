using System;
using FlipperDotNet.Adapter;
using NUnit.Framework;
using Rhino.Mocks;

namespace FlipperDotNet.Tests
{
	[TestFixture]
	class FeatureAdapterErrorTests
	{
		private IAdapter _adapter;
		private Feature  _feature;

		[SetUp]
		public void SetUp()
		{
			_adapter = MockRepository.GenerateStub<IAdapter>();
			_feature = new Feature("unobtanium", _adapter);
		}

		[Test]
		public void ShouldThrowExceptionWhenEnabling()
		{
			_adapter.Stub(x => x.Add(_feature)).Throw(new TestException());
			Assert.That(_feature.Enable, Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Failed to enable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenEnablingActor()
		{
			_adapter.Stub(x => x.Add(_feature)).Throw(new TestException());
			Assert.That(() => _feature.EnableActor(MockActor("User:5")), Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Failed to enable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenEnablingPercentageOfTime()
		{
			_adapter.Stub(x => x.Add(_feature)).Throw(new TestException());
			Assert.That(() => _feature.EnablePercentageOfTime(10), Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Failed to enable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenEnablingPercentageOfActors()
		{
			_adapter.Stub(x => x.Add(_feature)).Throw(new TestException());
			Assert.That(() => _feature.EnablePercentageOfActors(10), Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Failed to enable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenDisabling()
		{
			_adapter.Stub(x => x.Add(_feature)).Throw(new TestException());
			Assert.That(_feature.Disable, Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Failed to disable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenDisblingActor()
		{
			_adapter.Stub(x => x.Add(_feature)).Throw(new TestException());
			Assert.That(() => _feature.DisableActor(MockActor("User:5")), Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Failed to disable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenDisablingPercentageOfTime()
		{
			_adapter.Stub(x => x.Add(_feature)).Throw(new TestException());
			Assert.That(_feature.DisablePercentageOfTime, Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Failed to disable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenDisablingPercentageOfActors()
		{
			_adapter.Stub(x => x.Add(_feature)).Throw(new TestException());
			Assert.That(_feature.DisablePercentageOfActors, Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Failed to disable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenGettingFeatureState()
		{
			_adapter.Stub(x => x.Get(_feature)).Throw(new TestException());
			Assert.That(() => _feature.State, Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenTestingOnState()
		{
			_adapter.Stub(x => x.Get(_feature)).Throw(new TestException());
			Assert.That(() => _feature.IsOn, Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenTestingOffState()
		{
			_adapter.Stub(x => x.Get(_feature)).Throw(new TestException());
			Assert.That(() => _feature.IsOff, Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenTestingConditionalState()
		{
			_adapter.Stub(x => x.Get(_feature)).Throw(new TestException());
			Assert.That(() => _feature.IsConditional, Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenGettingGateValues()
		{
			_adapter.Stub(x => x.Get(_feature)).Throw(new TestException());
			Assert.That(() => _feature.GateValues, Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenListingEnabledGates()
		{
			_adapter.Stub(x => x.Get(_feature)).Throw(new TestException());
			Assert.That(() => _feature.EnabledGates, Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenListingDisabledGates()
		{
			_adapter.Stub(x => x.Get(_feature)).Throw(new TestException());
			Assert.That(() => _feature.DisabledGates, Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenTestingIfFeatureIsEnabled()
		{
			_adapter.Stub(x => x.Get(_feature)).Throw(new TestException());
			Assert.That(() => _feature.IsEnabled(), Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenTestingIfFeatureIsEnabledForActor()
		{
			_adapter.Stub(x => x.Get(_feature)).Throw(new TestException());
			Assert.That(() => _feature.IsEnabledFor(MockActor("User:5")), Throws.TypeOf<AdapterRequestException>()
				.With.InnerException.TypeOf<TestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		private static IFlipperActor MockActor(string id)
		{
			var actor = MockRepository.GenerateStub<IFlipperActor>();
			actor.Stub(x => x.FlipperId).Return(id);
			return actor;
		}

		private class TestException : Exception
		{
			public TestException():base()
			{ }
		}
	}
}

