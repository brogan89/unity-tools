using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityTools
{
	[Serializable] public class IntEvent : UnityEvent<int> { }
	[Serializable] public class FloatEvent : UnityEvent<float> { }
	[Serializable] public class BoolEvent : UnityEvent<bool> { }
	[Serializable] public class StringEvent : UnityEvent<string> { }
	[Serializable] public class GameObjectEvent : UnityEvent<GameObject> { }
	[Serializable] public class TransformEvent : UnityEvent<Transform> { }
	[Serializable] public class RectTransformEvent : UnityEvent<RectTransform> { }
	
	// ... add more events as you need them
}