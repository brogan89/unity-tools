namespace UnityTools
{
	public class DebounceFloat : DebounceBehaviour<float>
	{
		protected override float Value => _value;
		private float _value;

		public void Ping(float value)
		{
			_value = value;
			base.Ping();
		}
	}
}