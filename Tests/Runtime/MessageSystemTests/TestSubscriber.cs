using UnityEngine;

namespace UnityTools.MessageSystem.Tests
{
	public class TestSubscriber : MonoBehaviour, ISubscriber<TestMessage>, ISubscriber<TestMessage2>
	{
		private void OnEnable()
		{
			EventMessage.Sub(this);
		}

		private void OnDisable()
		{
			EventMessage.Unsub(this);
		}

		public void OnPublished(TestMessage message)
		{
			Debug.Log($"Message 1 Received: {message.StringMessage}", this);
		}
		
		public void OnPublished(TestMessage2 message)
		{
			Debug.Log($"Message 2 Received: {message.IntMessage}", this);
		}
	}
}