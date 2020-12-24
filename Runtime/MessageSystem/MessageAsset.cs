using UnityEngine;
using UnityEngine.Events;

namespace UnityTools.MessageSystem
{
	/// <summary>
	/// An class for easily creating a simple message as an asset rather than a class
	/// </summary>
	[CreateAssetMenu(fileName = "New Message Asset", menuName = "Message System/New Message Asset", order = 0)]
	public class MessageAsset : ScriptableObject
	{
		public UnityEvent onPublish;
		public UnityEvent<int> onPublishInt;
		public UnityEvent<float> onPublishFloat;
		public UnityEvent<bool> onPublishBool;
		public UnityEvent<string> onPublishString;
		public UnityEvent<Object> onPublishObject;
 		
		public void Publish()
		{
			onPublish?.Invoke();
		}

		public void Publish(int arg)
		{
			onPublishInt?.Invoke(arg);
		}
		
		public void Publish(float arg)
		{
			onPublishFloat?.Invoke(arg);
		}

		public void Publish(bool arg)
		{
			onPublishBool?.Invoke(arg);
		}
		
		public void Publish(string arg)
		{
			onPublishString?.Invoke(arg);
		}
		
		public void Publish(Object arg)
		{
			onPublishObject?.Invoke(arg);
		}
	}
}