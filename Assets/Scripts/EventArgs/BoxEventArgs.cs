using System;
using System.Text;
using WebSocketSharp;
using LitJson;
using UnityEngine;
using Main.Messages;
using System.Collections.Generic;

namespace Main.MessageEventArgs
{
	public class BoxEventArgs : EventArgs
	{
		#region Private Fields

		private MessageBox box;

		public MessageBox BOX {
			get{ return box; }	
		}

		private string data_text;
		private byte[] data_byte;

		private void jsonToBox (JsonData jsonData)
		{
			if (!jsonData ["messages"].IsArray)
				return;
			try {
				byte boxtype = jsonData.Keys.Contains ("type") ? byte.Parse (jsonData ["type"].ToString ()) : (byte)0x0;
				List<BaseMessage> messages = new List<BaseMessage> ();
				int i;
				for (i = 0; i < jsonData ["messages"].Count; i++) {
					JsonData message = jsonData ["messages"] [i];
					byte messagetype = message.Keys.Contains ("type") ? (byte)Math.Abs (int.Parse (message ["type"].ToString ())) : (byte)0x0;
					MonoBehaviour.print (messagetype);
					BaseMessage baseMessage;
					long mId = long.Parse (message ["mId"].ToString ());
					string text = message ["text"].ToString ();
					byte state = (byte)Math.Abs (int.Parse (message ["state"].ToString ()));
					long fromUid = long.Parse (message ["fromUid"].ToString ());
					string fromName = message ["fromName"].ToString ();
					long toUid = long.Parse (message ["toUid"].ToString ());
					string toName = message ["toName"].ToString ();
					if ((messagetype & BaseMessage.INVITE) == BaseMessage.INVITE) {
						bool cancel = bool.Parse (message ["cancel"].ToString ());
						bool accept = bool.Parse (message ["accept"].ToString ());
						baseMessage = new InviteMessage (mId, messagetype, text, state, fromUid, fromName, toUid, toName, cancel, accept);
					} else if ((messagetype & BaseMessage.CHAT) == BaseMessage.CHAT) {
						baseMessage = new BaseMessage (mId, messagetype, text, state, fromUid, fromName, toUid, toName);
					} else {
						baseMessage = new BaseMessage (mId, messagetype, text, state, fromUid, fromName, toUid, toName);
					}
					messages.Add (baseMessage);
				}
				box = MessageBox.newABox (boxtype, messages);

			} catch (Exception e) {
				MonoBehaviour.print ("BoxEventArgs Message Box Json Wrong:" + e.Message);
			}
		}

		#endregion

		#region public fields

		public string TEXT {
			get{ return data_text; }
		}

		public byte[] DATA {
			get{ return data_text.Length > 0 ? Encoding.UTF8.GetBytes (data_text) : null; }
		}

		#endregion

		internal BoxEventArgs (JsonData jsonData)
		{	
			data_text = jsonData.ToJson ();
			jsonToBox (jsonData);
		}

	}
}

