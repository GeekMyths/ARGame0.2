using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Main.MessageEventArgs;
using Main.Messages;
using WebSocketSharp;

public class initMessage : MonoBehaviour
{
	List<GameObject> items = new List<GameObject> ();
	LinkedList<BaseMessage> messageStore = new LinkedList<BaseMessage> ();
	public GameObject item;
	GameObject myMessage;
	GameObject parent;
	Vector3 itemLocalPos;
	Vector2 contentSize;
	float itemHeight;
	private WebSocketBuilder webSocketBuilder;

	private EventHandler<BoxEventArgs> boxHandler;

	private event UnityAction okeyClick;
	private event UnityAction cancelClick;

	private bool update = false;

	void Start ()
	{
		webSocketBuilder = WebSocketBuilder.instance ();
		webSocketBuilder.BoxHandler += boxHandle;
		webSocketBuilder.connect ();
		parent = GameObject.Find ("Content");
		contentSize = parent.GetComponent<RectTransform> ().sizeDelta;
		itemHeight = item.GetComponent<RectTransform> ().rect.height;
		itemLocalPos = item.transform.localPosition;
//		okeyClick = new UnityAction (itemOkClick);
//		cancelClick = new UnityAction (itemCancelClick);
	}

	void Update ()
	{
		if (messageStore.Count == 0) {
			return;
		}
		BaseMessage message;
		lock (messageStore) {
			message = messageStore.First.Value;
			messageStore.RemoveFirst ();
		}
		AddItem (message);
	}

	public void boxHandle (object context, BoxEventArgs e)
	{
		try {
			MessageBox box = e.BOX;
			if (box == null || box.getMessages ().Count == 0)
				return;
			if (box.isInBox ()) {
				foreach (BaseMessage message in box.getMessages()) {
					lock (messageStore) {
						messageStore.AddLast (message);
					}
				}
			} else if (box.isOutBox ()) {

			}
		} catch (Exception ex) {
			print ("main_background boxHandle exception" + ex.Message);
		}
	}
	//	IEnumerator UpdateSliderUI () {
	//		var query = ParseObject.GetQuery ("Creatures");
	//		Task task = query.FindAsync ();
	//		while(!task.IsCompleted) yield return null;
	//		// Do all your continue with stuff here
	//	}
	//添加列表项
	public void AddItem (BaseMessage message)
	{
		GameObject a = Instantiate (item) as GameObject;
		a.transform.FindChild ("Text").GetComponent<Text> ().text = message.text;
		a.transform.FindChild ("okay").GetComponent<Button> ().name = message.mId.ToString ();
//		a.transform.FindChild ("okay").GetComponent<Button> ().onClick = itemOkClick;
//		UIEventListener.Get (a.transform.FindChild ("okay").gameObject).onClick = itemOkClick;
////		((Button)a.FindChild ("cancel"))
//		UIEventListener.Get (a.transform.FindChild ("cancel").gameObject).onClick = itemCancelClick;
		a.transform.FindChild ("cancel").GetComponent<Button> ().name = message.mId.ToString ();
		a.GetComponent<Transform> ().SetParent (parent.GetComponent<Transform> (), false);
		a.transform.localPosition = new Vector3 (itemLocalPos.x, itemLocalPos.y - items.Count * itemHeight, 0);
		items.Add (a);
		if (contentSize.y <= items.Count * itemHeight) {//增加内容的高度
			parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (contentSize.x, items.Count * itemHeight);
		}
	}

	//移除列表项
	public void RemoveItem (GameObject t)
	{
		int index = items.IndexOf (t);
		items.Remove (t);
		Destroy (t);

		for (int i = index; i < items.Count; i++) {//移除的列表项后的每一项都向前移动
			items [i].transform.localPosition += new Vector3 (0, itemHeight, 0);
		}

		if (contentSize.y <= items.Count * itemHeight)//调整内容的高度
			parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (contentSize.x, items.Count * itemHeight);
		else
			parent.GetComponent<RectTransform> ().sizeDelta = contentSize;
	}

	public void itemOkClick (GameObject button)
	{
		long mId = long.Parse (button.name);
		foreach (BaseMessage message in messageStore) {
			if (message.mId == mId) {
				StartCoroutine (accept (message));
			}
		}
	}

	public IEnumerator accept (BaseMessage message)
	{
		Dictionary<string,string> dic = new Dictionary<string,string> ();
		dic.Add ("uid", WebSocketSharp.WebSocketBuilder.UID + "");
		dic.Add ("token", WebSocketSharp.WebSocketBuilder.TOKEN);
		WWWForm form = new WWWForm ();
		form.AddField ("fromId", message.fromUid + "");
		form.AddField ("mId", message.mId + "");
		form.AddField ("state", BaseMessage.ACCEPT);
		WWW www = new WWW (API.Constant.handleMessage, Encoding.ASCII.GetBytes (form.ToString ()), dic);
		yield return www;
		if (www.error != null) {
			MonoBehaviour.print (www.error);
		} else {
			
		}
	}

	public void itemCancelClick (GameObject button)
	{

	}
}
