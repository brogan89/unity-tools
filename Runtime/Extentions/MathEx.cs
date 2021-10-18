using UnityEngine;

namespace UnityTools.Extensions
{
	public static class MathEx
	{
		public static float GetPercentage(float min, float max, float input)
		{
			return (input - min) / (max - min);
		}

		/// <summary>
		/// Normalizes any number to an arbitrary range by assuming the range wraps around when going below min or above max.
		/// src: https://stackoverflow.com/questions/1628386/normalise-orientation-between-0-and-360
		/// </summary>
		/// <param name="value"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public static float Normalize(float value, float start = 0f, float end = 360f)
		{
			float width = end - start;
			float offsetValue = value - start; // value relative to 0

			return offsetValue - Mathf.Floor(offsetValue / width) * width + start;
			// + start to reset back to start of original range
		}
	}
}