using DigitalZenWorks.Common.VersionUtilities;
using System;
using System.IO;

namespace VersionUpdate
{
	/// <summary>
	/// Version update program.
	/// </summary>
	public static class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Version Update");

			if (CheckCommandLineParameters(args))
			{
				string output = Versioning.VersionUpdate(args[0]);
				Console.WriteLine(output);
			}
		}

		private static bool CheckCommandLineParameters(string[] parameters)
		{
			bool isValid = false;

			// Ensure we have a valid file name
			if (parameters.Length < 1)
			{
				Console.WriteLine("usage: VersionUpdate: <version file>");
			}
			else
			{
				if (File.Exists(parameters[0]))
				{
					isValid = true;
				}
			}

			return isValid;
		}
	}
}
