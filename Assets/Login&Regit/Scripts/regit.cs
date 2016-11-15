using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;
public class regit : MonoBehaviour {
	GameObject hint2;
	GameObject text;
	int state;
	string message;
	GameObject telephone;
	GameObject password;
	GameObject a;
	GameObject bt_commit;
	GameObject bt_return;
	// Use this for initialization
	void Start () {
		text=GameObject.Find("Text");
		hint2=GameObject.Find("hint2");

		telephone = GameObject.Find ("telephone");
		password = GameObject.Find ("password");
		a = GameObject.Find ("confirm");
		bt_commit = GameObject.Find ("bt_commit");
		bt_return = GameObject.Find ("bt_return");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator goback(float waitTime)
	{
		text.GetComponent<Text> ();
		int speed = 10000;
		yield return new WaitForSeconds(waitTime);
		telephone.GetComponent<Rigidbody>().velocity = transform.up * speed;
		password.GetComponent<Rigidbody>().velocity = transform.up * speed;
		a.GetComponent<Rigidbody>().velocity = transform.up * speed;
		bt_commit.GetComponent<Rigidbody>().velocity = transform.up * speed;
		bt_return.GetComponent<Rigidbody>().velocity = transform.up * speed;
		Application.LoadLevel("Game");
	}

	IEnumerator hidehint2(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		hint2.GetComponent<CanvasRenderer>().SetAlpha(0);
	}
	public IEnumerator Login()
	{
		JsonData data;
		string account = telephone.GetComponent<InputField>().text;
		string pwd = password.GetComponent<InputField>().text;
		string confirm = a.GetComponent<InputField>().text;
		if (pwd.Equals (confirm)) {
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
					text.GetComponent<Text> ().text = "注册成功，请登录";
					StartCoroutine (hidehint2 (1.5F));
					StartCoroutine (goback (1.5F));
				} else {
					message = data ["value"]["message"].ToString();
					hint2.GetComponent<CanvasRenderer>().SetAlpha(1);
					text.GetComponent<Text> ().text =message;
					StartCoroutine (hidehint2 (1.5F));
				
				}
			}
		} 
		else {
			hint2.SetActive (true);
			text.GetComponent<Text>().text="两次密码输入不一致";
			StartCoroutine(hidehint2(1.5F));
		}
	}
	public void OnClick(){
		StartCoroutine (Login());
	}
}
