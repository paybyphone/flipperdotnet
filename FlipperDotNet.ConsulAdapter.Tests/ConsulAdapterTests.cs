using System.Text;
using NUnit.Framework;

namespace FlipperDotNet.ConsulAdapter.Tests
{
    internal abstract class ConsulAdapterTests : AdapterTests.SharedAdapterTests
    {
        public abstract string Namespace { get; }

        [SetUp]
        public new void SetUp()
        {
            Client = new Consul.Client();
            Adapter = new ConsulAdapter(Client, Namespace);
            Client.KV.DeleteTree("/");
        }

        protected Consul.Client Client { get; private set; }
    }

    [TestFixture]
    class NoNamespaceConsulAdapterTests : ConsulAdapterTests
    {
        public override string Namespace
        {
            get { return ""; }
        }
    }

    [TestFixture]
    class NamespacedConsulAdapterTests : ConsulAdapterTests
    {
        public override string Namespace
        {
            get { return "foo/bar"; }
        }

        [Test]
        public void ShouldStoreFeatureDataInNamespacedHierarchy()
        {
            var feature = new Feature("search", Adapter);
            Adapter.Add(feature);

            var result = Client.KV.Get(string.Join("/", Namespace, ConsulAdapter.FeaturesKey, "features", "search"));
            Assert.That(Encoding.UTF8.GetString(result.Response.Value), Is.EqualTo("1"));
        }

        [Test, Ignore]
        public void ShouldStoreKeyValueDataInNamespacedHierarchy()
        {
            var feature = new Feature("search", Adapter);
            Adapter.Enable(feature, feature.BooleanGate, true);

            var result = Client.KV.Get(string.Join("/", Namespace, "search", "boolean"));
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
            var client = new Consul.Client();
            var adapter = new ConsulAdapter(client, name);
            return adapter.Namespace;
        }
    }
}
