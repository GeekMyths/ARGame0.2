using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;

public class RegitEvent : MonoBehaviour {
	GameObject hint2;
	GameObject text;
	int state;
	string message;
	GameObject telephone;
	GameObject password;
	GameObject confirm;
	GameObject bt_commit;
	GameObject bt_return;
	float duration = 5;
	int speed=10000;
	// Use this for initialization
	void Start () {
		float proportion = Time.time / duration;
		text=GameObject.Find("Text");
		hint2=GameObject.Find("hint2");
		telephone = GameObject.Find ("telephone");
		password = GameObject.Find ("password");
		confirm = GameObject.Find ("confirm");
		bt_commit = GameObject.Find ("bt_commit");
		bt_return = GameObject.Find ("bt_return");
		hint2.GetComponent<CanvasRenderer>().SetAlpha(0);
		show (proportion);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void CommitOnClick(){
		StartCoroutine (Regit());
	}
	public void GoBackOnClock(){
		telephone.GetComponent<Rigidbody>().velocity = transform.up * speed;
		password.GetComponent<Rigidbody>().velocity = transform.up * speed;
		confirm.GetComponent<Rigidbody>().velocity = transform.up * speed;
		bt_commit.GetComponent<Rigidbody>().velocity = transform.up * speed;
		bt_return.GetComponent<Rigidbody>().velocity = transform.up * speed;
		StartCoroutine(gologin(0.3F));
	}

	IEnumerator gologin(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		Application.LoadLevel("Login");
	}
	void show(float proportion){
		telephone.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0, 1, proportion));
		password.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0, 1, proportion));
		confirm.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0, 1, proportion));
		bt_commit.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0, 1, proportion));
		bt_return.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0, 1, proportion));
	}
	IEnumerator hidehint2(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		hint2.transform.FindChild("Text").GetComponent<Text>().text = "";
		hint2.GetComponent<CanvasRenderer>().SetAlpha(0);
	}
	public IEnumerator Regit()
	{
		JsonData data;
		string account = telephone.GetComponent<InputField>().text;
		string pwd = password.GetComponent<InputField>().text;
		string con = confirm.GetComponent<InputField>().text;
		if (pwd.Equals (con)) {
			WWWForm form = new WWWForm();
			form.AddField("account", account);
			form.AddField("pwd", pwd);
			WWW www = new WWW("http://115.28.140.76:8080/Game1/s/user/register", form);
			yield return www;
			if (www.error != null) {
				print (www.error);
			} 
			else {
				data = JsonMapper.ToObject (www.text);
				state = int.Parse (data ["state"].ToString ());
				if (state == 1) {
					hint2.SetActive (true);
					hint2.transform.FindChild("Text").GetComponent<Text>().text = "注册成功，请登录";
					StartCoroutine (hidehint2 (1.5F));
					StartCoroutine (gologin (1.5F));
				} else {
					message = data ["message"].ToString();
					hint2.GetComponent<CanvasRenderer>().SetAlpha(1);
					hint2.transform.FindChild("Text").GetComponent<Text>().text = message;
					StartCoroutine (hidehint2 (1.5F));

				}
			}
		} 
		else {
			hint2.SetActive (true);
			hint2.transform.FindChild("Text").GetComponent<Text>().text = "两次密码输入不一致";
			StartCoroutine(hidehint2(1.5F));
		}
	}
}
