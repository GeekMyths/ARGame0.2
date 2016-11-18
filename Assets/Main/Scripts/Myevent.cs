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

public class Myevent : MonoBehaviour {
	string username;

	bool isRight;
	bool click;
	Rigidbody game;
	float distance;
	Vector3 endPoi;
	float process;
	RectTransform group_friend;

	public GameObject dialog;
	public GameObject dialog_input;
	public GameObject item;
	public GameObject messageBox;
	public GameObject inviteMessage;

	List<GameObject> items = new List<GameObject> ();
	LinkedList<BaseMessage> messageStore = new LinkedList<BaseMessage> ();
	static LinkedList<BaseMessage> messages = new LinkedList<BaseMessage> ();
	GameObject parent;
	Vector3 itemLocalPos;
	Vector2 contentSize;
	float itemHeight;
	private WebSocketBuilder webSocketBuilder;

	private EventHandler<BoxEventArgs> boxHandler;


	// Use this for initialization
	void Start () {
		webSocketBuilder = WebSocketBuilder.instance ();
		webSocketBuilder.BoxHandler += boxHandle;
		webSocketBuilder.connect ();
		username = PlayerPrefs.GetString ("uid");

		isRight = false;
		click = false;
		game = GameObject.Find ("friend").GetComponent<Rigidbody>();
		group_friend = GameObject.Find ("friend").GetComponent<RectTransform>();
		distance = group_friend.rect.width / 12;
		endPoi = new Vector3 (game.transform.position.x - distance, game.transform.position.y, game.transform.position.z);
		game.position = endPoi;

		SetId ();
		SetEValue ();
		InviteMessage ();
	}
	
	// Update is called once per frame
	void Update () {
		if(click){
			process += 0.02f * 2;
			if (process < 1) {
				game.MovePosition(Vector3.Lerp (game.transform.position, endPoi, process));
			} else {
				click = false;
			}
		}
		if (messageStore.Count == 0) {
			return;
		}
		BaseMessage message;
		lock (messageStore) {
			message = messageStore.First.Value;
			messageStore.RemoveFirst ();
		}
		//多条消息弹出效果
		//inviteMessage.SetActive (true);
		inviteMessage.name = message.mId.ToString ();
		messages.AddLast (message);
	}
	void SetId(){
		GameObject.Find("name").GetComponent<Text>().text = username;
		GameObject.Find("lv").GetComponent<Text>().text = "lv.123";
	}
	void SetEValue(){
		GameObject.Find ("my_message").GetComponent<Scrollbar> ().value = 0.5f;
	}
	public void Onclick () {
		process = 0;
		click = true;
		isRight = !isRight;
		if (isRight) {
			endPoi = new Vector3 (game.transform.position.x + distance, game.transform.position.y, game.transform.position.z);
		} else {
			endPoi = new Vector3 (game.transform.position.x - distance, game.transform.position.y, game.transform.position.z);
		}
	}
	public void ExitOnClick(){
		dialog.SetActive (true);
	}
	public void PVPOnClick(){
		dialog_input.SetActive (true);
	}
	void MessageOnClick(){
		messageBox.SetActive (true);
		parent = GameObject.Find ("Content");
		messageBox.transform.FindChild ("bg").GetComponent<Button> ().onClick.AddListener (
			delegate() {
				messageBox.SetActive(false);
				for(int i = 0; i < parent.transform.childCount; i++)
				{
					GameObject go = parent.transform.GetChild(i).gameObject;
					RemoveItem(go);
				}
			}
		);
		contentSize = parent.GetComponent<RectTransform> ().sizeDelta;
		itemHeight = item.GetComponent<RectTransform> ().rect.height;
		itemLocalPos = item.transform.localPosition;

		foreach(BaseMessage message in messages){
			AddItem (message);
		}
	}
	public void InviteMessage(){
		inviteMessage.transform.FindChild ("dialog").transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (
			delegate() {
				long mId = long.Parse (inviteMessage.name);
				foreach (BaseMessage message in messages) {
					if (message.mId == mId) {
						StartCoroutine (accept (message));
					}
				}
				Application.LoadLevel("SelectServant");
			}
		);
		inviteMessage.transform.FindChild ("dialog").transform.FindChild ("cancel").GetComponent<Button> ().onClick.AddListener (
			delegate() {
				inviteMessage.SetActive (false);
			}
		);
	}
	public void boxHandle (object context, BoxEventArgs e)
	{
		try {
			MessageBox box = e.BOX;
			if (box == null || box.getMessages ().Count == 0)
				return;
			if (box.isInBox ()) {
				foreach (BaseMessage message in box.getMessages()) {
					print(message.text);
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
		a.name = message.mId.ToString ();
		a.transform.FindChild ("Text").GetComponent<Text> ().text = message.text;
		a.transform.FindChild ("okay").GetComponent<Button> ().onClick.AddListener (
			delegate() {
				itemOkClick(a);
			}
		);
		a.transform.FindChild ("cancel").GetComponent<Button> ().onClick.AddListener (
			delegate() {
				itemCancelClick(a,message);
			}
		);
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

	public void itemOkClick (GameObject a)
	{
		long mId = long.Parse (a.name);
		foreach (BaseMessage message in messages) {
			if (message.mId == mId) {
				StartCoroutine (accept (message));
			}
		}
		RemoveItem (item);
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

	public void itemCancelClick (GameObject item,BaseMessage message)
	{
		RemoveItem (item);
		messages.Remove (message);
	}
}
