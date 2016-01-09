using FlipperDotNet.AdapterTests.Interop;

namespace FlipperDotNet.ConsulAdapter.Tests.Interop
{
	public class ConsulRubyAdapter : RubyAdapter
	{
		protected override string BuildScript(string command)
		{
			return @"
require 'flipper-consul'
client = Diplomat::Kv.new
adapter = Flipper::Adapters::Consul.new(client)
flipper = Flipper.new(adapter)" + "\n" +
			command;
		}
	}
}
