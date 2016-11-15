using System;
using LitJson;

namespace Main.MessageEventArgs
{
	public class OutboxEventArgs
	{
		#region Private Fields

		private Main.Messages.BaseMessage[] messages;

		#endregion

		public OutboxEventArgs ()
		{
		}

		internal OutboxEventArgs (JsonData jsonData)
		{
			int i = 0;
			messages = new Main.Messages.BaseMessage[jsonData.Count];
			foreach (JsonData json in jsonData) {
				long mId = json.Keys.Contains ("mId") ? long.Parse (json ["mId"].ToString ()) : 0;
				byte	type = json.Keys.Contains ("type") ? byte.Parse (json ["type"].ToString ()) : (byte)0x0;
				string text = json.Keys.Contains ("text") ? json ["text"].ToString () : "0";
				byte state = json.Keys.Contains ("state") ? byte.Parse (json ["state"].ToString ()) : (byte)0x0;
				long fromUid = json.Keys.Contains ("fromUID") ? long.Parse (json ["fromUid"].ToString ()) : 0;
				string fromName = json.Keys.Contains ("fromName") ? json ["fromName"].ToString () : "0";
				long	toUid = json.Keys.Contains ("toUid") ? long.Parse (json ["toUid"].ToString ()) : 0;
				string toName = json.Keys.Contains ("toName") ? json ["toName"].ToString () : "0";
				messages [i++] = new Main.Messages.BaseMessage (mId,type, text, state, fromUid, fromName, toUid, toName);
			}
		}

	}
}

