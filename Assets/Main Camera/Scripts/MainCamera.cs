using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	public GameObject dialog;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown (KeyCode.Escape))) {
			dialog.SetActive (true);
		}
	}
	public void IsQuit (bool quit)
	{
		if (quit) {
			Application.Quit ();
		}
	}
}
