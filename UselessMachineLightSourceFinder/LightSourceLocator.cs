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
	public class LightSourceLocator
	{
		private static double[] sensor_x_coordinates = { 4, 15, 5, -15, -18, 0 };
		private static double[] sensor_y_coordinates = { -15, -8, 0, 5, -5, 10 };
		private static double[] sensor_z_coordinates = { 2, 0, 10, 10, 0, 3 };
		private static double[] sensor_z_coordinates_adjustment = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };

		private static double[] sensor_x_weight = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };
		private static double[] sensor_y_weight = { 0.1, 0.1, 0.05, 0.05, 0.1, 0.05 };
		private static double[] sensor_z_weight = { -0.15, -0.15, -0.15, -0.15, -0.15, -0.15 };
		


		private static double power = 0.33;

		/// <summary>
		/// To reduce the effect of environmental lighting. 
		/// Measure the value of each sensor when no light is shine on them, or enter 0s to consider environmental light as light source 
		/// </summary>
		private static double[] sensor_baselineValue = { 198, 206, 167, 191, 192, 187 }; //TODO: do this automatically at the beginning
		public static LightSourceLocation FindLightSourceLocationGivenSensorReadings(SensorReading sensorReading)
		{
			PrintArray(sensorReading.ReadingOfEachSensor);

			double[] readingsMinusBaselines = ArraySubstraction(sensorReading.ReadingOfEachSensor, sensor_baselineValue); // this removes the effect of environemtal light
			double[] readingsToNegativePower = ArrayPower(readingsMinusBaselines, power); // this makes the reading values linear to its distance to the light source

			double[] readingsWeightedForX = ArrayMultiplication(readingsToNegativePower, sensor_x_weight);
			double xPredict = ArrayMultiplication(sensor_x_coordinates, readingsWeightedForX).Sum();

			double[] readingsWeightedForY = ArrayMultiplication(readingsToNegativePower, sensor_y_weight);
			double yPredict = ArrayMultiplication(sensor_y_coordinates, readingsWeightedForY).Sum();

			double[] readingsWeightedForZ = ArrayMultiplication(readingsToNegativePower, sensor_z_weight);
			double[] coordinatesAdjustedForZ = ArrayAddition(sensor_z_coordinates, sensor_z_coordinates_adjustment);
			double zPredict = ArrayMultiplication(coordinatesAdjustedForZ, readingsWeightedForZ).Sum();

			return new LightSourceLocation(xPredict,yPredict,zPredict);
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

		public static double[] ArrayAddition(double[] arr1, double[] arr2)
		{
			if (arr1.Length != arr2.Length)
			{
				Debug.Fail("FAILURE: arrays have different length");
			}
			double[] result = new double[arr1.Length];
			for (int i = 0; i < arr1.Length; i++)
			{
				result[i] = arr1[i] + arr2[i];
			}
			return result;
		}

		public static double[] ArraySubstraction(double[] arr1, double[] arr2)
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

		public static double[] ArrayMultiplication(double[] arr1, double[] arr2)
		{
			if (arr1.Length != arr2.Length)
			{
				Debug.Fail("FAILURE: arrays have different length");
			}
			double[] result = new double[arr1.Length];
			for (int i = 0; i < arr1.Length; i++)
			{
				result[i] = arr1[i] * arr2[i];
			}
			return result;
		}

		/// <summary>
		/// raise each value in the array to power specified; 
		/// If the value is negative, instead of outputing NAN, output the negative of its abs to power
		/// </summary>
		public static double[] ArrayPower(double[] arr, double power)
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
