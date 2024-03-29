﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit;

namespace UselessMachineLightSourceFinder.UnitTests
{
	/// <summary>
	/// Summary description for LightSourceLocatorTests
	/// </summary>
	[TestClass]
	public class LightSourceLocatorTests
	{
		[TestMethod]
		public void TestLightSourceLocating()
		{
			var sr = new SensorReading(new List<int> { 600, 500, 400, 300, 200, 100 });
			var result = LightSourceLocator.FindLightSourceLocationGivenSensorReadings(sr, new double[] { 74.00, 62.00, 77.00, 42.00, 59.00, 69.00 });
			Console.WriteLine(result.ToString());
		}

		[TestMethod]
		public void TestArrayAddition()
		{
			double[] arr1 = { 1, -2, 3 };
			double[] arr2 = { 2, 4, 8 };

			var result = LightSourceLocator.ArrayAddition(arr1, arr2);
			Assert.AreEqual(3, result[0]);
			Assert.AreEqual(2, result[1]);
			Assert.AreEqual(11, result[2]);
		}

		[TestMethod]
		public void TestArraySubstraction()
		{
			double[] arr1 = { 1, -2, 3 };
			double[] arr2 = { 2, 4, 8 };

			var result = LightSourceLocator.ArraySubstraction(arr2, arr1);
			Assert.AreEqual(1, result[0]);
			Assert.AreEqual(6, result[1]);
			Assert.AreEqual(5, result[2]);
		}

		[TestMethod]
		public void TestArrayMultiplication_array()
		{
			double[] arr1 = { 1, -2, 3 };
			double[] arr2 = { 2, 4, 8 };

			var result = LightSourceLocator.ArrayMultiplication(arr1, arr2);
			Assert.AreEqual(2, result[0]);
			Assert.AreEqual(-8, result[1]);
			Assert.AreEqual(24, result[2]);
		}

		[TestMethod]
		public void TestArrayMultiplication_factor()
		{
			double[] arr1 = { 1, -2, 3 };
			double factor = 2.0;

			var result = LightSourceLocator.ArrayMultiplication(arr1, factor);
			Assert.AreEqual(2, result[0]);
			Assert.AreEqual(-4, result[1]);
			Assert.AreEqual(6, result[2]);
		}

		[TestMethod]
		public void TestArrayPower_1()
		{
			double[] arr1 = { 1, 2, 3 };
			var result = LightSourceLocator.ArrayPower(arr1, 2);
			Assert.AreEqual(1, result[0]);
			Assert.AreEqual(4, result[1]);
			Assert.AreEqual(9, result[2]);
		}

		[TestMethod]
		public void TestArrayPower_2()
		{
			double[] arr1 = { 1, -2, 3 };
			var result = LightSourceLocator.ArrayPower(arr1, 0.33);
			Assert.IsTrue(result[0] > 0);
			Assert.IsTrue(result[1] < 0);
			Assert.IsTrue(result[2] > 0);
		}
	}
}
