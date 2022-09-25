﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UselessMachineLightSourceFinder
{
	/// <summary>
	/// Assumption: baseline value is set in the testing environment; 
	/// Observation: after substracting by baseline, the reading increases dramatically as the light source (iphone) gets closer, 
	/// but decrease slowly as it gets further away; 
	/// </summary>
	class LightSourceLocator
	{
		private static int[] sensor_x_coordinates = { 4, 15, 5, -15, -18, 0 };
		private static int[] sensor_y_coordinates = { -15, -8, 0, 5, -5, 10 };
		private static int[] sensor_z_coordinates = { 0, 0, 15, 10, 0, 3 };

		private static double[] sensor_x_weight = { 1, 1, 1, 1, 1, 1 };
		private static double[] sensor_x_adjustment = { 0, 0, 0, 0, 0, 0 };
		private static double[] sensor_y_weight => sensor_x_weight;
		private static double[] sensor_y_adjustment => sensor_x_adjustment;

		private static double[] sensor_z_weight = { 1, 1, 1, 1, 1, 1 };
		private static double[] sensor_z_adjustment = { 0, 0, 0, 0, 0, 0 };

		private static double powerValueForLinearTransform = 0.33;

		/// <summary>
		/// To reduce the effect of environmental lighting. 
		/// Measure the value of each sensor when no light is shine on them, or enter 0s to consider environmental light as light source 
		/// </summary>
		private static double[] sensor_baselineValue = { 328, 239, 297, 204, 251, 279 }; 
		public static LightSourceLocation FindLightSourceLocationGivenSensorReadings(SensorReading sensorReading)
		{
			double[] readingsMinusBaselines = ArraySubstraction(sensorReading.ReadingOfEachSensor, sensor_baselineValue); // this removes the effect of environemtal light
			double[] readingsToNegativePower = ArrayPower(readingsMinusBaselines, powerValueForLinearTransform); // this makes the reading values linear to its distance to the light source
			

			PrintArray(readingsToNegativePower);

			return new LightSourceLocation(0,0,0);
		}

		private static void PrintArray(double[] arr)
		{
			Console.WriteLine("");
			for (int i = 0; i < arr.Length; i++)
			{
				Console.Write("{0:0.00}/", arr[i]);
			}
			Console.WriteLine("");
		}

		private static double[] ArraySubstraction(double[] arr1, double[] arr2)
		{
			if (arr1.Length != arr2.Length)
			{
				Debug.Fail("FAILURE: arrays have different length");
			}
			double[] result = new double[arr1.Length];
			for (int i = 0; i < arr1.Length; i++)
			{
				result[i] = arr1[i] - arr2[i];
			}
			return result;
		}

		/// <summary>
		/// raise each value in the array to power specified; 
		/// If the value is negative, instead of outputing NAN, output the negative of its abs to power
		/// </summary>
		private static double[] ArrayPower(double[] arr, double power)
		{
			double[] result = new double[arr.Length];
			for (int i = 0; i < arr.Length; i++)
			{
				if (arr[i] >= 0)
				{
					result[i] = Math.Pow(arr[i], power);
				}
				else
				{
					result[i] = -1 * Math.Pow(-1 * arr[i], power);
				}
			}
			return result;
		}
	}
}