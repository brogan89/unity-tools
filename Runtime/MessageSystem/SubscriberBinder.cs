using UnityEngine;

namespace UnityTools.MessageSystem
{
	public class SubscriberBinder : MonoBehaviour
	{
		private MonoBehaviour _sub;

		private void OnEnable()
		{
			Sub();
		}

		private void OnDisable()
		{
			Unsub();
		}

		public void Bind(MonoBehaviour sub)
		{
			_sub = sub;
			Sub();
		}

		private void Sub()
		{
			if (_sub)
				EventMessage.Sub(_sub as ISubscriber);
		}

		private void Unsub()
		{
			EventMessage.Unsub(_sub as ISubscriber);
		}
	}
}