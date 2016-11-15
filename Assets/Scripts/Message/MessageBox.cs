using System;
using System.Collections.Generic;

namespace Main.Messages
{
	public class MessageBox
	{
		public const byte INBOX = 0x1;
		public const byte OUTBOX = 0x2;
		private byte type;
		private List<BaseMessage> messages;

		protected MessageBox (byte type, List<BaseMessage> messages)
		{
			this.type = type;
			this.messages = messages;
		}
		public bool isInBox(){
			return this.type == INBOX;
		}
		public bool isOutBox(){
			return this.type == OUTBOX;
		}
		public List<BaseMessage> getMessages ()
		{
			return messages;
		}

		public static MessageBox newABox (byte type, List<BaseMessage> messages)
		{
			return new MessageBox (type, messages);
		}
	}
}

