using System.Threading;
using System;
using UnityEngine;
using LitJson;
using Constant.Message;
using Main.MessageEventArgs;
using Main.Messages;
using System.Collections.Generic;
namespace WebSocketSharp
{
	public class WebSocketBuilder
	{
		#region private static variable

		private static WebSocketBuilder webSocketBuilder;
		private static int uid;
		private static string token;

		#endregion

		#region public static variable

		public static int UID {
			set {
				uid = value;
			}
			get { 
				return uid;
			}
		}

		public static string TOKEN {
			set {
				token = value;
			}
			get { 
				return token;
			}
		}

		#endregion

		#region private variable

		private event EventHandler onOpen;
		private event EventHandler<MessageEventArgs> onMessage;
		private event EventHandler<CloseEventArgs> onClose;
		private event EventHandler<ErrorEventArgs> onError;

		private event EventHandler<BoxEventArgs> boxHandler;

		#endregion

		#region public variable

		public WebSocket websocket;
		public Thread onlineThread;

		public EventHandler<BoxEventArgs> BoxHandler {
			set { boxHandler = value; }
			get{ return boxHandler; }
		}

		#endregion

		#region constructor private

		private WebSocketBuilder ()
		{
			onOpen = new EventHandler (onopen);
			onMessage = new EventHandler<MessageEventArgs> (onmessage);
			onError = new EventHandler<ErrorEventArgs> (onerror);
			onClose = new EventHandler<CloseEventArgs> (onclose);
			boxHandler = new EventHandler<BoxEventArgs> (onbox);
			websocket = new WebSocket (API.Constant.wsonline, onOpen, onMessage, onError, onClose, new string[] {
				"chat",
				"surperchat"
			});
			websocket.AddHeader ("uid", uid + "");
			websocket.AddHeader ("token", token);
		}

		#endregion

		public static WebSocketBuilder instance ()
		{
			if (webSocketBuilder == null) {
				webSocketBuilder = new WebSocketBuilder ();
			}
			return webSocketBuilder;
		}

		#region public fun

		public void addHeader (string key, string value)
		{
			websocket.AddHeader (key, value);
		}

		public void connect ()
		{
			websocket.Connect ();
		}

		#endregion

		#region event handler fun

		private void onbox (object sender, BoxEventArgs e)
		{
			print ("onbox");
		}

		private void onopen (object sender, EventArgs e)
		{
			print ("onOpen");
		}

		private void onmessage (object sender, MessageEventArgs e)
		{
			print (e.Data);
			if (boxHandler == null)
				return;
			try {
				JsonData json = JsonMapper.ToObject (e.Data);
				switch (int.Parse (json ["state"].ToString ())) {
				case 1:
					if ((byte.Parse (json ["code"].ToString ()) & ValueCode.MESSAGEBOX) != ValueCode.MESSAGEBOX)
						break;
					if (json ["value"].IsArray) {
						int i, count;
						count = json ["value"].Count;
						for (i = 0; i < count; i++) {
							print (json ["value"] [i].ToString ());
							BoxEventArgs boxArgs = new BoxEventArgs (json ["value"] [i]);
							boxHandler.Emit(this, boxArgs);
						}
					} else {
						print(json["value"].ToJson());
						BoxEventArgs boxArgs = new BoxEventArgs(json["value"]);
						boxHandler.Emit(this, boxArgs);
					}
					break;
				case -1:
					print (json ["message"].ToString ());
					break;
				}
			} catch (Exception exception) {
				print ("WebSocketBuilder onmessage exception :" + exception.Message);
			}
		}

		private void onclose (object sender, CloseEventArgs e)
		{
			print ("onclose");
			print (e.Reason);
		}

		private void onerror (object sender, ErrorEventArgs e)
		{
			print ("onerror");
			print (e.Message);
		}

		#endregion

		private void print (string text)
		{
			MonoBehaviour.print (text);
		}


	}
}