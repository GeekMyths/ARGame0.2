using UnityEngine;
using System.Collections;

public class regit_onclick : MonoBehaviour {
	int speed=2000;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    public void OnClick() {
		GameObject Bt_login = GameObject.Find ("Bt_login");
		GameObject Bt_regit = GameObject.Find ("Bt_regit");
		GameObject Ip_username = GameObject.Find ("Ip_username");
		GameObject Ip_password = GameObject.Find ("Ip_password");
		Bt_login.GetComponent<Rigidbody> ().velocity = Bt_login.transform.up * speed;
		Bt_regit.GetComponent<Rigidbody> ().velocity = Bt_regit.transform.up * speed;
		Ip_username.GetComponent<Rigidbody> ().velocity = Ip_username.transform.up * speed;
		Ip_password.GetComponent<Rigidbody> ().velocity = Ip_password.transform.up * speed;
        StartCoroutine(WaitAndPrint(0.5F));
        
    }
    IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Application.LoadLevel("Regit");
    }
}
