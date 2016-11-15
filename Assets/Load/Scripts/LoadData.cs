using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadData : MonoBehaviour {
	private GameObject MyServantImg;
	private GameObject HeServantImg;
	string MyServnt_id;
	// Use this for initialization
	void Start () {
		MyServantImg = GameObject.Find ("MyServant");
		HeServantImg = GameObject.Find ("OtherServant");
		MyServnt_id = PlayerPrefs.GetString ("MyServant_id");
		print (MyServnt_id);
		MyServantImg.transform.FindChild ("Img").GetComponent<RawImage> ().texture = Resources.Load<Texture> (MyServnt_id);
		//HeServantImg.transform.FindChild ("Img").GetComponent<RawImage> ().texture = Resources.Load<Texture> (HeServantImg);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
