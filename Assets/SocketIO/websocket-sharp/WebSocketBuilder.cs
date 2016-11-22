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
		private static int uid = int.Parse(PlayerPrefs.GetString("uid"));
		private static string token = PlayerPrefs.GetString("token");

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

		public EventHandler<MessageEventArgs> OnMessage {
			set { onMessage = value; }
			get{ return onMessage; }
		}

		public EventHandler<CloseEventArgs> OnClose {
			set { onClose = value; }
			get{ return onClose; }
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
			print ("onmessage");
			print (e.Data);
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