namespace UnityTools
{
	public class DebounceString : DebounceBehaviour<string>
	{
		private string _value;
		protected override string Value => _value;

		public void Ping(string value)
		{
			_value = value;
			base.Ping();
		}
	}
}