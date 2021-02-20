using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTools.MessageSystem;
using UnityTools.MessageSystem.Tests;

namespace UnityTools.Tests
{
	// TODO: do more tests
	
	public class MessageTestRuntimeTests
	{
		// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
		// `yield return null;` to skip a frame.
		[UnityTest]
		public IEnumerator MessageTestRuntimeTestsWithEnumeratorPasses()
		{
			CreateGameObjects();
			new TestSub();
			
			yield return null;

			EventMessage.Publish(new TestMessage {StringMessage = "Hello World!"});
			EventMessage.Publish(new TestMessage2 {IntMessage = Time.frameCount});
		}

		private static void CreateGameObjects()
		{
			new GameObject(nameof(TestSubscriber), typeof(TestSubscriber));
			new GameObject(nameof(TestSubscriber2), typeof(TestSubscriber2));
		}
	}

	public class TestSub : ISubscriber<TestMessage>, ISubscriber<TestMessage2>
	{
		public TestSub()
		{
			EventMessage.Sub(this);
		}
		
		public void OnPublished(TestMessage message)
		{
			Debug.Log($"TestSub message received: {message.StringMessage}");
		}

		public void OnPublished(TestMessage2 message)
		{
			Debug.Log($"TestSub message 2 received: {message.IntMessage}");
		}
	}
}