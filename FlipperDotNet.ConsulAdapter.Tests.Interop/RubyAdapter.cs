using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FlipperDotNet.ConsulAdapter.Tests.Interop
{

	public class RubyAdapter
	{
		public void Enable(string key)
		{
			Run(BuildScript(String.Format("flipper.enable('{0}')", key)));	
		}

		public void EnableActor(string key, string actor)
		{
			Run(BuildScript(String.Format("flipper.enable('{0}', Struct.new(:flipper_id).new('{1}'))", key, actor)));
		}

		public void EnablePercentageOfActors(string key, int percentage)
		{
			Run(BuildScript(String.Format("flipper.feature('{0}').enable_percentage_of_actors {1}", key, percentage)));
		}

		public void EnablePercentageOfTime(string key, int percentage)
		{
			Run(BuildScript(String.Format("flipper.feature('{0}').enable_percentage_of_time {1}", key, percentage)));
		}

		public bool IsEnabled(string key)
		{
			var output = Run(BuildScript(String.Format("print flipper.enabled?('{0}')", key)));

			return output.TrimEnd( Environment.NewLine.ToCharArray()) == "true";
		}

		public ISet<string> ActorsValue(string key)
		{
			var output = Run(BuildScript(String.Format("print flipper.feature('{0}').actors_value.to_a.join(',')", key)));

			return new HashSet<string>(output.Split(','));
		}

		public int PercentageOfActorsValue(string key)
		{
			var output = Run(BuildScript(String.Format(@"print flipper.feature('{0}').percentage_of_actors_value", key)));

			return Int32.Parse(output);
		}

		public object PercentageOfTimeValue(string key)
		{
			var output = Run(BuildScript(String.Format(@"print flipper.feature('{0}').percentage_of_time_value", key)));

			return Int32.Parse(output);
		}

		private string BuildScript(string command)
		{
			return @"
require 'flipper-consul'
client = Diplomat::Kv.new
adapter = Flipper::Adapters::Consul.new(client)
flipper = Flipper.new(adapter)" + "\n" +
			command;
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
