using UnityEngine;
using System.Collections;
using LitJson;
public class invite_ok : MonoBehaviour {
	string uid;
	string token;
	// Use this for initialization
	void Start () {
		uid = PlayerPrefs.GetString ("uid");
		token = PlayerPrefs.GetString ("token");
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	public void OnClick(){		
		
		//Application.LoadLevel("SelectServant");
	}

	public IEnumerator responseinvite()
	{
		JsonData data;
		string fromId="";
		string mId="";
		int state=1;	
		WWWForm form = new WWWForm ();
		form.AddField ("fromId",fromId);
		form.AddField ("mId",mId);
		form.AddField ("state",state);
		form.AddField ("token",token);
		form.AddField ("uid",uid);
		WWW www = new WWW ("http://115.28.140.76:8080/Game1/s/battle/handleMessage",form);
		yield return www;
		if (www.error != null) {
			print (www.error);
		} 
		else 
		{
			data = JsonMapper.ToObject (www.text);
			int sstate=int.Parse(data["state"].ToString());
			if (sstate == -1) {
				string message = data ["message"].ToString ();
				//error 里面放errormessage	
			
			}
		}
	
	}

}
