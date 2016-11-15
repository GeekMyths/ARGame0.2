using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;
public class login : MonoBehaviour {
	string url_login = "http://115.28.140.76:8080/Game1/s/user/login";
	string token;
	int state;
	int uid;
	int speed=2000;
	GameObject hint;
	GameObject Bt_login;
	GameObject Bt_regit;
	GameObject Ip_username;
	GameObject Ip_password;


	// Use this for initialization
	void Start() {
		hint = GameObject.Find ("hint");
		Bt_login = GameObject.Find ("Bt_login");
		Bt_regit = GameObject.Find ("Bt_regit");
		Ip_username = GameObject.Find ("Ip_username");
		Ip_password = GameObject.Find ("Ip_password");
	}

	// Update is called once per frame
	void Update() {

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
		PlayerPrefs.SetInt("uid", uid);
		PlayerPrefs.SetString ("token", token);
		print ("token:" + token+" uid:"+uid);
		Application.LoadLevel("Main");
	}

	IEnumerator hidehint(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		hint.GetComponent<CanvasRenderer>().SetAlpha(0);
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
				uid  = int.Parse (data ["value"]["uid"].ToString ());
				StartCoroutine (goLogin (0.5F));
			} 
			else {
				hint.GetComponent<CanvasRenderer>().SetAlpha(1);
				StartCoroutine(hidehint(1.5F));
			}
		}
	}
	public void Onclick(){
		StartCoroutine(Login ());
	}

}
