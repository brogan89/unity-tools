using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityTools.MessageSystem.Tests
{
	public class TestSubscriber : MonoBehaviour
	{
		private bool[] doneArray = new bool[3];

		public bool IsDone
		{
			get
			{
				foreach (var b in doneArray)
					if (!b) return false;
				return true;
			}
		}
		
		[MessageCallback]
		public void TestMethod(TestMessage message)
		{
			Debug.Log($"TestSubscriber::TestMethod Received: {message.StringMessage}", this);
			doneArray[0] = true;
		}
		
		[MessageCallback]
		public IEnumerator TestMethodCoroutine(TestMessage message)
		{
			yield return null;
			Debug.Log($"TestSubscriber::TestMethodCoroutine Received: {message.StringMessage}", this);
			doneArray[1] = true;
		}
		
		[MessageCallback]
		public async void TestMethodAsync(TestMessage message)
		{
			await Task.Delay(1000);
			Debug.Log($"TestSubscriber::TestMethodAsync Received: {message.StringMessage}", this);
			doneArray[2] = true;
		}
	}
}