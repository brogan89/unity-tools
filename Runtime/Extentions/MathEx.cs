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
		
		/// <summary>
		/// i = Returns index based on progress length,
		/// t = Returns a value 0-1 between two given indexes based on total progress.
		/// </summary>
		/// <param name="progress"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static (int i, float t) GetIndexAndLerpValue(float progress, int length)
		{
			// index
			var p = (length - 1) * progress;
			var i = Mathf.CeilToInt(p);
			i = Mathf.Clamp(i, 0, length - 1);
		
			// lerp value
			var t = progress * (length - 1) % 1f;
			if (i == length - 1 && progress >= 1f)
				t = 1;
		
			return (i, t);
		}
	
		/// <summary>
		/// Returns index based on progress length
		/// </summary>
		/// <param name="progress"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static int GetIndex(float progress, int length)
		{
			return GetIndexAndLerpValue(progress, length).i;
		}

		/// <summary>
		/// Returns a value 0-1 between two given indexes based on total progress.
		/// </summary>
		/// <param name="progress"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static float GetLerpValue(float progress, int length)
		{
			return GetIndexAndLerpValue(progress, length).t;
		}
	}
}