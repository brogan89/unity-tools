using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;
using UnityTools.MessageSystem;
using UnityTools.MessageSystem.Tests;

namespace UnityTools.Tests
{
	public class MessageTestRuntimeTests
	{
		// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
		// `yield return null;` to skip a frame.
		[UnityTest]
		public IEnumerator MessageTestRuntimeTestsWithEnumeratorPasses()
		{
			// ReSharper disable once ObjectCreationAsStatement
			var sub = new GameObject(nameof(TestSubscriber)).AddComponent<TestSubscriber>();
			
			// ReSharper disable once ObjectCreationAsStatement
			new TestSub();
			
			yield return null;

			Message.Publish(new TestMessage {StringMessage = "Hello World!"});

			// wait for test to done. If this isn't waited for then the coroutine message test will fail
			yield return new WaitUntil(() => sub.IsDone);
			
			Debug.Log("MessageTestRuntimeTests complete");
		}
	}

	public class TestSub
	{
		[MessageCallback]
		public void TestMessage(TestMessage message)
		{
			Debug.Log($"TestSub::TestMessage message received: {message.StringMessage}");
		}
		
		[MessageCallback]
		public async void TestMessageAsync(TestMessage message)
		{
			await Task.Delay(1000);
			Debug.Log($"TestSub::TestMessageAsync message received: {message.StringMessage}");
		}
	}
}