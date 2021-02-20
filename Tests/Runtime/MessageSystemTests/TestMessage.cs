namespace UnityTools.MessageSystem.Tests
{
	public class TestMessage : IMessage
	{
		public string StringMessage { get; set; }
	}
	
	public class TestMessage2 : IMessage
	{
		public int IntMessage { get; set; }
	}
}