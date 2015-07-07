using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FlipperDotNet.ConsulAdapter.Tests.Interop
{

	public class RubyAdapter
	{
		public void Enable(string key)
		{
			const string command = @"
require 'flipper-consul'
client = Diplomat::Kv.new
adapter = Flipper::Adapters::Consul.new(client)
flipper = Flipper.new(adapter)
flipper.enable('{0}')";
			Run(String.Format(command, key));
		}

		public void EnableActor(string key, string actor)
		{
			const string command = @"
require 'flipper-consul'
client = Diplomat::Kv.new
adapter = Flipper::Adapters::Consul.new(client)
flipper = Flipper.new(adapter)
Actor = Struct.new(:flipper_id)
actor = Actor.new('{1}')
flipper.enable('{0}', actor)";
			Run(String.Format(command, key, actor));
		}

		public void EnablePercentageOfActors(string key, int percentage)
		{
			const string command = @"
require 'flipper-consul'
client = Diplomat::Kv.new
adapter = Flipper::Adapters::Consul.new(client)
flipper = Flipper.new(adapter)
feature = flipper.feature '{0}'
feature.enable_percentage_of_actors {1}";
			Run(String.Format(command, key, percentage));
		}

		public void EnablePercentageOfTime(string key, int percentage)
		{
			const string command = @"
require 'flipper-consul'
client = Diplomat::Kv.new
adapter = Flipper::Adapters::Consul.new(client)
flipper = Flipper.new(adapter)
feature = flipper.feature '{0}'
feature.enable_percentage_of_time {1}";
			Run(String.Format(command, key, percentage));
					}

		public bool IsEnabled(string key)
		{
			const string command = @"
require 'flipper-consul'
client = Diplomat::Kv.new
adapter = Flipper::Adapters::Consul.new(client)
flipper = Flipper.new(adapter)
p flipper.enabled?('{0}')";
			var output = Run(String.Format(command, key));

			return output.TrimEnd( Environment.NewLine.ToCharArray()) == "true";
		}

		public ISet<string> ActorsValue(string key)
		{
			const string command = @"
require 'flipper-consul'
client = Diplomat::Kv.new
adapter = Flipper::Adapters::Consul.new(client)
flipper = Flipper.new(adapter)
feature = flipper.feature '{0}'
print feature.actors_value.to_a.join(',')";
			var output = Run(String.Format(command, key));

			return new HashSet<string>(output.Split(','));
		}

		public int PercentageOfActorsValue(string key)
		{
			const string command = @"
require 'flipper-consul'
client = Diplomat::Kv.new
adapter = Flipper::Adapters::Consul.new(client)
flipper = Flipper.new(adapter)
feature = flipper.feature '{0}'
print feature.percentage_of_actors_value";
			var output = Run(String.Format(command, key));

			return Int32.Parse(output);
		}

		public object PercentageOfTimeValue(string key)
		{
			const string command = @"
require 'flipper-consul'
client = Diplomat::Kv.new
adapter = Flipper::Adapters::Consul.new(client)
flipper = Flipper.new(adapter)
feature = flipper.feature '{0}'
print feature.percentage_of_time_value";
			var output = Run(String.Format(command, key));

			return Int32.Parse(output);
		}

		private string Run(string script)
		{
			var process = new System.Diagnostics.Process ();
			process.StartInfo.FileName = "ruby";
			process.StartInfo.Arguments = String.Format("-e \"{0}\"", script);
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.UseShellExecute = false;
			process.Start ();
			string tool_output = process.StandardOutput.ReadToEnd ();
			process.WaitForExit (1000);
			int exit_code = process.ExitCode;
			if (exit_code != 0) {
				Assert.Fail ("ruby code failed");
			}
			return tool_output;
		}
	}
}
