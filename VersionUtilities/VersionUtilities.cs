/////////////////////////////////////////////////////////////////////////////
// <copyright file="VersionUtilities.cs" company="James John McGuire">
// Copyright © 2020 - 2022 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DigitalZenWorks.Common.VersionUtilities
{
	/// <summary>
	/// Version Utilities.
	/// </summary>
	public static class VersionUtilities
	{
		/// <summary>
		/// Gets the build number.
		/// </summary>
		/// <returns>The build number.</returns>
		public static int GetBuildNumber()
		{
			int buildNumber;

			Assembly assembly = Assembly.GetCallingAssembly();

			AssemblyName name = assembly.GetName();
			Version version = name.Version;

			buildNumber = version.Revision;

			return buildNumber;
		}

		/// <summary>
		/// Gets the package version.
		/// </summary>
		/// <param name="packageId">The identifier for the package.</param>
		/// <returns>The package version.</returns>
		public static string GetPackageVersion(string packageId)
		{
			string version = string.Empty;

			if (!string.IsNullOrWhiteSpace(packageId))
			{
				// get the package file, based on current directory
				string contents = File.ReadAllText("packages.config");

				if (!string.IsNullOrWhiteSpace(contents))
				{
					int index = contents.IndexOf(
						packageId, StringComparison.OrdinalIgnoreCase) +
						packageId.Length + 1;
					string substring = contents[index..];
					version =
						Regex.Match(substring, "\"([^\"]*)\"").Groups[1].Value;
				}
			}

			return version;
		}

		/// <summary>
		/// Get the version.
		/// </summary>
		/// <returns>The version.</returns>
		public static string GetVersion()
		{
			string version;

			Assembly assembly = Assembly.GetCallingAssembly();

			AssemblyName name = assembly.GetName();
			Version versionNumber = name.Version;

			version = versionNumber.ToString();

			return version;
		}

		/// <summary>
		/// Updates the version tag inside the given file.
		/// </summary>
		/// <param name="fileName">The file containing the version tag.</param>
		/// <returns>The updated version build number.</returns>
		public static string VersionUpdate(string fileName)
		{
			string version = null;

			try
			{
				if (File.Exists(fileName))
				{
					string contents = File.ReadAllText(fileName);

					if (!string.IsNullOrWhiteSpace(contents))
					{
						VersionFileType fileType = GetFileType(fileName);

						switch (fileType)
						{
							case VersionFileType.AssemblyInfo:
								contents = AssemblyInfoUpdate(
									contents, out version);
								break;
							case VersionFileType.CsProj:
								contents = CsProjUpdate(
									contents, out version);
								break;
							case VersionFileType.Css:
								contents = CssUpdate(
									contents, out version);
								break;
							case VersionFileType.Php:
								contents = PhpUpdate(
									contents, out version);
								break;
							default:
								break;
						}

						File.WriteAllText(fileName, contents);
					}
				}
			}
			catch (Exception exception) when
				(exception is ArgumentException ||
				exception is ArgumentNullException ||
				exception is ArgumentOutOfRangeException ||
				exception is DirectoryNotFoundException ||
				exception is FileNotFoundException ||
				exception is FormatException ||
				exception is IOException ||
				exception is NotSupportedException ||
				exception is OverflowException ||
				exception is PathTooLongException ||
				exception is System.Security.SecurityException ||
				exception is UnauthorizedAccessException)
			{
			}

			return version;
		}

		private static string AssemblyInfoTagUpdate(
			string contents, string tag, out string version)
		{
			string pattern = tag + "\\(\"(?<major>\\d+)\\.(?<minor>\\d+)\\." +
				"(?<revision>\\d+)\\.(?<build>\\d+)\"\\)";
			string replacementFormat = tag + "(\"{0}.{1}.{2}.{3}\")";

			contents = VersionTagUpdate(
				contents, pattern, replacementFormat, out version);

			return contents;
		}

		private static string AssemblyInfoUpdate(
			string contents, out string version)
		{
			string tag = "AssemblyVersion";
			contents = AssemblyInfoTagUpdate(contents, tag, out version);

			tag = "AssemblyFileVersion";
			contents = AssemblyInfoTagUpdate(
				contents, tag, out string nextVersion);

			if (!string.IsNullOrWhiteSpace(nextVersion))
			{
				version = nextVersion;
			}

			return contents;
		}

		private static string CsProjTagUpdate(
			string contents, string tag, out string version)
		{
			string pattern = tag + "\\>(?<major>\\d+)\\.(?<minor>\\d+)\\." +
				"(?<revision>\\d+)\\.(?<build>\\d+)\\<";
			string replacementFormat = tag + ">{0}.{1}.{2}.{3}<";

			contents = VersionTagUpdate(
				contents, pattern, replacementFormat, out version);

			return contents;
		}

		private static string CsProjUpdate(
			string contents, out string version)
		{
			string tag = "AssemblyVersion";
			contents = CsProjTagUpdate(contents, tag, out version);

			tag = "AssemblyFileVersion";
			contents = CsProjTagUpdate(contents, tag, out string nextVersion);

			if (string.IsNullOrWhiteSpace(version))
			{
				version = nextVersion;
			}

			tag = "FileVersion";
			contents = CsProjTagUpdate(contents, tag, out nextVersion);

			if (string.IsNullOrWhiteSpace(version))
			{
				version = nextVersion;
			}

			tag = "Version";
			contents = CsProjTagUpdate(contents, tag, out nextVersion);

			if (string.IsNullOrWhiteSpace(version))
			{
				version = nextVersion;
			}

			return contents;
		}

		private static string CssUpdate(
			string contents, out string version)
		{
			string tag = "Version: ";

			string pattern = tag +
				"(?<major>\\d+)\\.(?<minor>\\d+)\\.(?<build>\\d+)";
			string replacementFormat = tag + "{0}.{1}.{2}";

			contents = VersionTagUpdate(
				contents, pattern, replacementFormat, out version);

			return contents;
		}

		private static VersionFileType GetFileType(string fileName)
		{
			VersionFileType fileType = VersionFileType.Generic;

			string extension = Path.GetExtension(fileName);

			switch (extension)
			{
				case ".cs":
					fileType = VersionFileType.AssemblyInfo;
					break;
				case ".csproj":
					fileType = VersionFileType.CsProj;
					break;
				case ".css":
					fileType = VersionFileType.Css;
					break;
				case ".php":
					fileType = VersionFileType.Php;
					break;
				default:
					break;
			}

			return fileType;
		}

		private static string PhpUpdate(
			string contents, out string version)
		{
			string tag = "', '";

			string pattern = tag +
				"(?<major>\\d+)\\.(?<minor>\\d+)\\.(?<build>\\d+)";
			string replacementFormat = tag + "{0}.{1}.{2}";

			contents = VersionTagUpdate(
				contents, pattern, replacementFormat, out version);

			if (string.IsNullOrWhiteSpace(version))
			{
				tag = "@version(?<whitespace>\\s+)";

				pattern = tag +
					"(?<major>\\d+)\\.(?<minor>\\d+)\\.(?<build>\\d+)";
				replacementFormat = tag + "{0}.{1}.{2}";

				contents = VersionTagUpdate(
					contents, pattern, replacementFormat, out version);
			}

			return contents;
		}

		private static string UpdateBuildNumber(
			string fileContents)
		{
			Match oldVersionMatch = Regex.Match(
				fileContents, "PRODUCT_VERSION_BUILD\\s*([0-9])*");

			string oldVersion = oldVersionMatch.ToString();
			Console.WriteLine("Old Version: " + oldVersion);

			Match versionMatchsub = Regex.Match(
				oldVersion, "[0-9]+"); // only one digit

			string versionNumber = versionMatchsub.ToString();
			int buildNumber = Convert.ToInt32(versionNumber);
			buildNumber++;

			string newVersion = "PRODUCT_VERSION_BUILD\t" + buildNumber.ToString();

			string newFileContents = Regex.Replace(
				fileContents,
				"PRODUCT_VERSION_BUILD\\s*([0-9])*",
				newVersion);
			Console.WriteLine("New Version: " + newVersion);

			return newFileContents;
		}

		private static string UpdateDateStamp(
			string fileContents)
		{
			_ = Regex.Match(
				fileContents,
				"PRODUCT_VERSION_DATE\\s*\"([0-9]{4,4}-?[0-1][0-9]-?[0-3][0-9])");

			DateTime today = DateTime.UtcNow;

			string newDate = "PRODUCT_VERSIONDATE\t\"" + today.ToString("yyyy-MM-dd");

			string newFileContents = Regex.Replace(
				fileContents,
				"PRODUCT_VERSION_DATE\\s\"([0-9]{4,4}-?[0-1][0-9]-?[0-3][0-9])",
				newDate);

			return newFileContents;
		}

		private static string UpdateTimeStamp(
			string fileContents)
		{
			_ = Regex.Match(
				fileContents,
				"PRODUCT_VERSION_TIME\\s*\"([0-9]*:[0-9]*:[0-9]*)");

			DateTime today = DateTime.UtcNow;

			string newTime =
				"PRODUCT_VERSION_TIME\t\"" + today.ToString("HH:mm:ss");

			string newFileContents = Regex.Replace(
				fileContents,
				"PRODUCT_VERSION_TIME\\s*\"([0-9]*:[0-9]*:[0-9]*)",
				newTime);

			return newFileContents;
		}

		private static string VersionFileUpdate(
			string contents, out string version)
		{
			version = null;

			contents = UpdateBuildNumber(contents);
			contents = UpdateDateStamp(contents);
			contents = UpdateTimeStamp(contents);

			return contents;
		}

		private static string VersionTagUpdate(
			string contents,
			string pattern,
			string replacementFormat,
			out string version)
		{
			version = null;
			Regex regex = new (pattern);
			MatchCollection matches = regex.Matches(contents);

			if (matches.Count > 0)
			{
				int build;
				string major = matches[0].Groups["major"].Value;
				string minor = matches[0].Groups["minor"].Value;
				string revision = matches[0].Groups["revision"].Value;

				build = Convert.ToInt32(matches[0].Groups["build"].Value);
				build++;

				version = build.ToString(CultureInfo.InvariantCulture);

				string replacement;
				if (string.IsNullOrWhiteSpace(revision))
				{
					string whitespace = matches[0].Groups["whitespace"].Value;

					replacementFormat = replacementFormat.Replace("(?<whitespace>\\s+)", whitespace);
					// Just 3 sections
					replacement = string.Format(
						CultureInfo.InvariantCulture,
						replacementFormat,
						major,
						minor,
						version);
				}
				else
				{
					// 4 sections
					replacement = string.Format(
						CultureInfo.InvariantCulture,
						replacementFormat,
						major,
						minor,
						revision,
						version);
				}

				contents = Regex.Replace(
					contents, pattern, replacement);
			}

			return contents;
		}
	}
}
