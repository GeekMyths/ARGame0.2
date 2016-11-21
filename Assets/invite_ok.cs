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
		Application.LoadLevel("SelectServant");
	}
}
