using UnityEngine;
using System.Collections;

public class dialog_cancel : MonoBehaviour {
	public GameObject dialog;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnClick(){
		dialog.SetActive (false);
	}
}
