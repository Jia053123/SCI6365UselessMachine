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
		// be sure to avoid 0 as it effectively disables a sensor
		private static double[] sensor_x_coordinates = { 6, 1, -10, -1, -9, 9 }; 
		private static double[] sensor_y_coordinates = { -7, -7, -4, -1, 5, 5 }; 
		private static double[] sensor_z_coordinates = { 1, 1, 4, 1, 2, 1 };

		private static double[] sensor_x_weight = { 0.15, 0.1, 0.07, 0.1, 0.1, 0.15 };
		private static double[] sensor_y_weight = { 0.1, 0.1, 0.07, 0.1, 0.15, 0.15 };
		private static double[] sensor_z_weight = { -0.9, -0.9, -0.9, -0.9, -0.9, -0.9 }; // must be negative
		private static double sensor_z_coordinates_weight = 2.0; // must be positive

		private static double z_adjustment = 20;

		private static double power = 0.33;

		/// <summary>
		/// To reduce the effect of environmental lighting. 
		/// Measure the value of each sensor when no light is shine on them, or enter 0s to consider environmental light as light source 
		/// If the light sensors rotates, enter the average value of all sensors
		/// </summary>
		private static double[] baselines;
		public static LightSourceLocation FindLightSourceLocationGivenSensorReadings(SensorReading sensorReading, double[] sensor_baselineValues)
		{
			baselines = sensor_baselineValues;
			PrintArray(sensorReading.ReadingOfEachSensor);
			//Console.WriteLine("Average: {0:0.00}", sensorReading.ReadingOfEachSensor.Sum() / sensorReading.NumOfSensors); // print average for convenience

			double[] readingsMinusBaselines = ArraySubstraction(sensorReading.ReadingOfEachSensor, baselines); // this removes the effect of environemtal light
			double[] readingsToPower = ArrayPower(readingsMinusBaselines, power); // this makes the reading values linear to its distance to the light source

			double[] readingsWeightedForX = ArrayMultiplication(readingsToPower, sensor_x_weight);
			double xPredict = ArrayMultiplication(sensor_x_coordinates, readingsWeightedForX).Sum();

			double[] readingsWeightedForY = ArrayMultiplication(readingsToPower, sensor_y_weight);
			double yPredict = ArrayMultiplication(sensor_y_coordinates, readingsWeightedForY).Sum();

			double[] readingsWeightedForZ = ArrayMultiplication(readingsToPower, sensor_z_weight);
			double[] z_coordinates_weighted = ArrayMultiplication(sensor_z_coordinates, sensor_z_coordinates_weight);
			double zPredict = ArrayAddition(readingsWeightedForZ, z_coordinates_weighted).Sum() + z_adjustment;

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

		public static double[] ArrayMultiplication(double[] arr1, double factor)
		{
			double[] result = new double[arr1.Length];
			for (int i = 0; i < arr1.Length; i++)
			{
				result[i] = arr1[i] * factor;
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
