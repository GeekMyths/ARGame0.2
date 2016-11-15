using UnityEngine;
using System.Collections;

public class bt_return : MonoBehaviour {
	int speed=10000;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnClick()
    {
		GameObject telephone = GameObject.Find ("telephone");
		GameObject password = GameObject.Find ("password");
		GameObject confirm = GameObject.Find ("confirm");
		GameObject bt_commit = GameObject.Find ("bt_commit");
		GameObject bt_return = GameObject.Find ("bt_return");
		telephone.GetComponent<Rigidbody>().velocity = transform.up * speed;
		password.GetComponent<Rigidbody>().velocity = transform.up * speed;
		confirm.GetComponent<Rigidbody>().velocity = transform.up * speed;
		bt_commit.GetComponent<Rigidbody>().velocity = transform.up * speed;
		bt_return.GetComponent<Rigidbody>().velocity = transform.up * speed;
		StartCoroutine(gologin(0.5F));

    }
    IEnumerator gologin(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Application.LoadLevel("Login");
    }
}
