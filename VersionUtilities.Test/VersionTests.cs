/////////////////////////////////////////////////////////////////////////////
// <copyright file="VersionTests.cs" company="James John McGuire">
// Copyright © 2020 - 2023 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System.Diagnostics;
using DigitalZenWorks.Common.VersionUtilities;

namespace DigitalZenWorks.Common.VersionUtilities.Test
{
	/// <summary>
	/// Version tests class.
	/// </summary>
	public class VersionTests
	{
		/// <summary>
		/// Setups this instance.
		/// </summary>
		[SetUp]
		public void Setup()
		{
		}

		/// <summary>
		/// Tests the get assembly information.
		/// </summary>
		[Test]
		public void TestGetAssemblyInformation()
		{
			FileVersionInfo versionInfo =
				VersionSupport.GetAssemblyInformation();
			Assert.NotNull(versionInfo);
		}

		/// <summary>
		/// Tests the get version.
		/// </summary>
		[Test]
		public void TestGetVersion()
		{
			string version = VersionSupport.GetVersion();
			Assert.NotNull(version);

			Assert.That(version, Is.EqualTo("1.0.1.0 Digital Zen Works"));
		}
	}
}