using UnityEngine;

namespace UnityTools.MessageSystem.Tests
{
	public class TestSubscriber2 : MonoBehaviour, ISubscriber<TestMessage2>
	{
		public int frameCount;
		
		private void OnEnable()
		{
			EventMessage.Sub(this);
		}

		private void OnDisable()
		{
			EventMessage.Unsub(this);
		}		
		
		public void OnPublished(TestMessage2 message)
		{
			frameCount = message.IntMessage;
		}
	}
}