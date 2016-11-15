using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class myTime : MonoBehaviour {
	private int x = 0;
	private int speed = 6;
	private int seconds = 30;
	private float lastTime = 0;
	bool time_out = false;
	GameObject text;
	// Use this for initialization
	void Start () {
		GameObject timeScroll = this.gameObject;
		float width = timeScroll.transform.GetComponent<RectTransform> ().rect.width;
		timeScroll.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (width,width);

		text = GameObject.Find ("myTime");
		float t_width = timeScroll.transform.GetComponent<RectTransform> ().rect.width;
		text.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (t_width,t_width);
		lastTime = Time.time;
		text.GetComponent<Text> ().text = seconds.ToString();
	}

	// Update is called once per frame
	void Update () {
		speed--;
		if(speed == 0){
			TimeScroll();
			speed = 6;
		}
		if(Time.time-lastTime >= 1f && !time_out){
			seconds--;
			text.GetComponent<Text> ().text = seconds.ToString();
			lastTime = Time.time;
		}
		if(seconds == 0){
			time_out = true;
		}
	}
	void TimeScroll(){
		x -= 10;
		this.transform.Rotate (new Vector3(0,0,x),-18f);
	}
}
