using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitJson;
public class input_ok : MonoBehaviour {
	public GameObject inputbox;
	string uid;
	string token;
	public GameObject tip;
	// Use this for initialization
	void Start () {
		uid = PlayerPrefs.GetString ("uid");
		token = PlayerPrefs.GetString ("token");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnClick(){
		StartCoroutine (invite ());


	
	}
	public IEnumerator invite()
	{
		JsonData data;
		string ss =	inputbox.GetComponent<InputField>().text;
		Dictionary<string,string> dic = new Dictionary<string,string>();
		dic.Add ("token", token);
		dic.Add ("uid",uid+"");
		WWWForm form = new WWWForm ();
		form.AddField ("toGamename",ss);
		var rawdata = form.data;
		var head = form.headers;
		head ["token"] = token;
		head ["uid"] = uid;
		print (ss);
		WWW www = new WWW (API.Constant.inviteEnemy,rawdata,head);
		yield return www;
		if (www.error != null) {
			print (www.error);
		} 
		else
		{
			data = JsonMapper.ToObject (www.text);
			print ("aaa");
			int state =int.Parse( data ["state"].ToString ());
			if (state == -1) {
				tip.SetActive (true);
				tip.transform.FindChild ("Text").GetComponent<Text> ().text = "对方不在线，请稍后再试~";
				yield return new WaitForSeconds (1.0F);
				tip.SetActive (false);
				tip.transform.FindChild ("Text").GetComponent<Text> ().text = "";
			}
		}
	
	}
}

