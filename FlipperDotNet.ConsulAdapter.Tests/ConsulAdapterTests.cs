using System;
using System.Text;
using Consul;
using NUnit.Framework;
using Rhino.Mocks;

namespace FlipperDotNet.ConsulAdapter.Tests
{
    internal abstract class ConsulAdapterTests : AdapterTests.SharedAdapterTests
    {
        protected abstract string Namespace { get; }

        [SetUp]
        public new void SetUp()
        {
            Client = new ConsulClient();
            Adapter = new ConsulAdapter(Client, Namespace);
            var pairs =  Client.KV.Keys("/","/").Result;
            foreach (var key in pairs.Response)
            {
                 Client.KV.DeleteTree(key).Wait();
            }
        }

        protected IConsulClient Client { get; private set; }
    }

    [TestFixture]
    class NoNamespaceConsulAdapterTests : ConsulAdapterTests
    {
        protected override string Namespace
        {
            get { return ""; }
        }
    }

    [TestFixture]
    class NamespacedConsulAdapterTests : ConsulAdapterTests
    {
        protected override string Namespace
        {
            get { return "foo/bar"; }
        }

        [Test]
        public void ShouldStoreFeatureDataInNamespacedHierarchy()
        {
            var feature = new Feature("search", Adapter);
            Adapter.Add(feature);

            var result = Client.KV.Get(string.Join("/", Namespace, ConsulAdapter.FeaturesKey, "features", "search")).Result;
            Assert.That(Encoding.UTF8.GetString(result.Response.Value), Is.EqualTo("1"));
        }

        [Test]
        public void ShouldStoreKeyValueDataInNamespacedHierarchy()
        {
            var feature = new Feature("search", Adapter);
            Adapter.Enable(feature, feature.BooleanGate, true);

            var result = Client.KV.Get(string.Join("/", Namespace, "search", "boolean")).Result;
            Assert.That(Encoding.UTF8.GetString(result.Response.Value), Is.EqualTo("true"));
        }
    }

    [TestFixture]
    class AdapterNamespaceTests
    {
        [TestCase("", ExpectedResult = "")]
        [TestCase("foo", ExpectedResult = "foo")]
        [TestCase("/foo", ExpectedResult = "foo")]
        public string TestNamespace(string name)
        {
            var client = new ConsulClient();
            var adapter = new ConsulAdapter(client, name);
            return adapter.Namespace;
        }
    }

	[TestFixture]
	class ConsulErrorTests
	{
		private Feature _feature;

		[SetUp]
		public void SetUp()
		{
		    var clientConfig = new ConsulClientConfiguration {Address = new Uri("http://127.0.0.1:9500")};
		    var client = new ConsulClient(clientConfig);
			var adapter = new ConsulAdapter(client);
			var flipper = new Flipper(adapter);
			_feature = flipper.Feature("unobtanium");
		}

		[Test]
		public void ShouldThrowExceptionWhenEnabling()
		{
			Assert.That(_feature.Enable, Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Failed to enable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenEnablingActor()
		{
			Assert.That(() => _feature.EnableActor(MockActor("User:5")), Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Failed to enable feature unobtanium"));
		}
		
		[Test]
		public void ShouldThrowExceptionWhenEnablingPercentageOfTime()
		{
			Assert.That(() => _feature.EnablePercentageOfTime(10), Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Failed to enable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenEnablingPercentageOfActors()
		{
			Assert.That(() => _feature.EnablePercentageOfActors(10), Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Failed to enable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenDisabling()
		{
			Assert.That(_feature.Disable, Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Failed to disable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenDisblingActor()
		{
			Assert.That(() => _feature.DisableActor(MockActor("User:5")), Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Failed to disable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenDisablingPercentageOfTime()
		{
			Assert.That(_feature.DisablePercentageOfTime, Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Failed to disable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenDisablingPercentageOfActors()
		{
			Assert.That(_feature.DisablePercentageOfActors, Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Failed to disable feature unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenGettingFeatureState()
		{
			Assert.That(() => _feature.State, Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenTestingOnState()
		{
			Assert.That(() => _feature.IsOn, Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenTestingOffState()
		{
			Assert.That(() => _feature.IsOff, Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenTestingConditionalState()
		{
			Assert.That(() => _feature.IsConditional, Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenGettingGateValues()
		{
			Assert.That(() => _feature.GateValues, Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenListingEnabledGates()
		{
			Assert.That(() => _feature.EnabledGates, Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenListingDisabledGates()
		{
			Assert.That(() => _feature.DisabledGates, Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenTestingIfFeatureIsEnabled()
		{
			Assert.That(() => _feature.IsEnabled(), Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		[Test]
		public void ShouldThrowExceptionWhenTestingIfFeatureIsEnabledForActor()
		{
			Assert.That(() => _feature.IsEnabledFor(MockActor("User:5")), Throws.TypeOf<AdapterRequestException>()
				.With.Property("Message").EqualTo("Unable to retrieve feature values for unobtanium"));
		}

		private static IFlipperActor MockActor(string id)
		{
			var actor = MockRepository.GenerateStub<IFlipperActor>();
			actor.Stub(x => x.FlipperId).Return(id);
			return actor;
		}
	}
}
