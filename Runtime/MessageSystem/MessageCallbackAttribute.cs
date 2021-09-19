using System;

namespace UnityTools.MessageSystem
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class MessageCallbackAttribute : Attribute
	{
	}
}