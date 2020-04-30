using UnityEngine;

namespace UnityTools.Extensions
{
	public static class AudioExtensions
	{
		public static float LinearToDecibel(this float linear)
		{
			float dB;

			if (!Mathf.Approximately(linear, 0))
				dB = 20.0f * Mathf.Log10(linear);
			else
				dB = -80f;

			return dB;
		}

		public static float DecibelToLinear(this float dB)
		{
			return Mathf.Pow(10.0f, dB / 20.0f);
		}
	}
}