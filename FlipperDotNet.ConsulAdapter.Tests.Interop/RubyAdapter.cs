using NUnit.Framework;
using System;

namespace FlipperDotNet.ConsulAdapter.Tests.Interop
{

	public class RubyImpl
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
