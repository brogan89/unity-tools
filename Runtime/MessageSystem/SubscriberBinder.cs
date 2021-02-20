using UnityEngine;

namespace UnityTools.MessageSystem
{
	public class SubscriberBinder : MonoBehaviour
	{
		private ISubscriber _sub;

		private void OnEnable()
		{
			Sub();
		}

		private void OnDisable()
		{
			Unsub();
		}

		public void Bind(ISubscriber sub)
		{
			_sub = sub;
			Sub();
		}

		private void Sub()
		{
			if (_sub != null)
				EventMessage.Sub(_sub);
		}

		private void Unsub()
		{
			if (_sub != null)
				EventMessage.Unsub(_sub);
		}
	}
}