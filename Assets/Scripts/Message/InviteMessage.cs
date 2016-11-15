using System;

namespace Main.Messages
{
	public class InviteMessage :Main.Messages.BaseMessage
	{
		public bool cancel;
		public bool accept;

		public InviteMessage (long mId,byte type, string text, byte state, long fromUid, string fromName, long toUid, string toName, bool cancel, bool accept) : base (mId,type, text, state, fromUid, fromName, toUid, toName)
		{
			this.cancel = cancel;
			this.accept = accept;
		}

	}
}

