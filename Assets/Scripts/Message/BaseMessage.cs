using System;

namespace Main.Messages
{
	public class BaseMessage
	{
		#region message state

		// 消息状态值
		public  const byte READY = (byte)0x01;
		public  const byte ARRIVED = (byte)0x02;
		// 邀请类被处理状态(接受，拒绝，被取消)，表示该消息为被处理过的
		public  const byte ABOUTINVITE = (byte)0x80;
		public  const byte CANCELED = (byte)0x81;
		public  const byte ACCEPT = (byte)0x82;
		public  const byte DENY = (byte)0x83;

		#endregion

		#region message type

		public  const byte INVITE = (byte)0x80;
		public  const byte BATTLE = (byte)0x81;
		public  const byte FRIEND = (byte)0x82;
		public  const byte CHAT = (byte)0x40;

		#endregion

		#region Public Fields

		public long mId;
		public byte type;
		public string text;
		public byte state;
		public long fromUid;
		public string fromName;
		public long toUid;
		public string toName;

		#endregion

		public BaseMessage (long mId,byte type, string text, byte state, long fromUid, string fromName, long toUid, string toName)
		{
			this.mId = mId;
			this.type = type;
			this.text = text;
			this.state = state;
			this.fromUid = fromUid;
			this.fromName = fromName;
			this.toUid = toUid;
			this.toName = toName;
		}

	}
}

