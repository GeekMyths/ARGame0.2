using UnityEngine;
using System.Collections;

public class invite_ok : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void OnClick(){
		//here send agree message
		Application.LoadLevel("SelectServant");
	}
}
