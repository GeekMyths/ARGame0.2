using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;

public class LoginEvent : MonoBehaviour {
	string url_login = "http://115.28.140.76:8080/Game1/s/user/login";
	string token;
	int state;
	string uid;
	int speed=10000;
	GameObject hint;
	GameObject Bt_login;
	GameObject Bt_regit;
	GameObject Ip_username;
	GameObject Ip_password;
	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		hint = GameObject.Find ("hint");
		Bt_login = GameObject.Find ("Bt_login");
		Bt_regit = GameObject.Find ("Bt_regit");
		Ip_username = GameObject.Find ("Ip_username");
		Ip_password = GameObject.Find ("Ip_password");
		hint.GetComponent<CanvasRenderer>().SetAlpha(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoginOnclick(){
		StartCoroutine(Login ());
	}

	public void RegitOnClick() {
		GameObject Bt_login = GameObject.Find ("Bt_login");
		GameObject Bt_regit = GameObject.Find ("Bt_regit");
		GameObject Ip_username = GameObject.Find ("Ip_username");
		GameObject Ip_password = GameObject.Find ("Ip_password");
		Bt_login.GetComponent<Rigidbody> ().velocity = Bt_login.transform.up * speed;
		Bt_regit.GetComponent<Rigidbody> ().velocity = Bt_regit.transform.up * speed;
		Ip_username.GetComponent<Rigidbody> ().velocity = Ip_username.transform.up * speed;
		Ip_password.GetComponent<Rigidbody> ().velocity = Ip_password.transform.up * speed;
		StartCoroutine(WaitAndPrint(0.3F));
	}

	IEnumerator WaitAndPrint(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		Application.LoadLevel("Regit");
	}
	void fly(){
		Bt_login.GetComponent<Rigidbody> ().velocity = Bt_login.transform.up * speed;
		Bt_regit.GetComponent<Rigidbody> ().velocity = Bt_regit.transform.up * speed;
		Ip_username.GetComponent<Rigidbody> ().velocity = Ip_username.transform.up * speed;
		Ip_password.GetComponent<Rigidbody> ().velocity = Ip_password.transform.up * speed;
	}
	IEnumerator goLogin(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		PlayerPrefs.SetString("uid", uid);
		PlayerPrefs.SetString ("token", token);
		print ("token:" + token+" uid:"+uid);
		Application.LoadLevel("Main");
	}

	IEnumerator hidehint(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		hint.GetComponent<CanvasRenderer>().SetAlpha(0);
		hint.transform.FindChild ("Text").GetComponent<Text> ().text = "";
	}

	public IEnumerator Login()
	{
		JsonData data;
		string user = Ip_username.GetComponent<InputField>().text;
		string password = Ip_password.GetComponent<InputField>().text;
		WWWForm form = new WWWForm();
		form.AddField("account", user);
		form.AddField("pwd", password);
		WWW www = new WWW(url_login, form);
		yield return www;
		if (www.error != null)
		{
			print(www.error);
		}
		else
		{
			print (www.text);
			data = JsonMapper.ToObject (www.text);
			state = int.Parse (data ["state"].ToString ());
			if (state == 1) {
				fly ();
				token = data["value"]["token"].ToString ();
				uid  = data ["value"]["uid"].ToString ();
				StartCoroutine (goLogin (0.5F));
			} 
			else {
				hint.GetComponent<CanvasRenderer>().SetAlpha(1);
				hint.transform.FindChild ("Text").GetComponent<Text> ().text = data["message"].ToString ();
				StartCoroutine(hidehint(1.5F));
			}
		}
	}
}
