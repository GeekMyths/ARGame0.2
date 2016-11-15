using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class addtext : MonoBehaviour {
	bool hasMessage = true;
	string newMessage = "asd";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(hasMessage){
			GameObject.Find ("message").transform.FindChild("Text").GetComponent<Text> ().text = newMessage;
			hasMessage = false;

		}
	}
}
