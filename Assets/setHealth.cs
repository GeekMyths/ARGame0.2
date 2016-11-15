using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class setHealth : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject health = GameObject.Find ("health");
		Scrollbar red_bar = health.transform.FindChild ("red_bar").GetComponent<Scrollbar> ();
		red_bar.value = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
