namespace UnityTools
{
	public static class MathEx
	{
		public static float GetPercentage(float min, float max, float input)
		{
			return (input - min) / (max - min);
		}
	}
}