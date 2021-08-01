﻿using DigitalZenWorks.Common.Utilities;
using System;
using System.IO;

namespace VersionUpdate
{
	public static class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			if (CheckCommandLineParameters(args))
			{
				string output = VersionUtilities.VersionUpdate(args[0]);
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