/////////////////////////////////////////////////////////////////////////////
// <copyright file="VersionTests.cs" company="James John McGuire">
// Copyright © 2020 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.VersionUtilities.Test
{
	using System.Diagnostics;
	using DigitalZenWorks.Common.VersionUtilities;
	using NUnit.Framework;

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
			Assert.That(versionInfo, Is.Not.Null);
		}

		/// <summary>
		/// Tests the get version.
		/// </summary>
		[Test]
		public void TestGetVersion()
		{
			string version = VersionSupport.GetVersion();
			Assert.That(version, Is.Not.Null);

			Assert.That(version, Is.EqualTo("1.0.0.0"));
		}
	}
}
