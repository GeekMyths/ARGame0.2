using UnityEngine;
using UnityEngine.UI;
using System.Collections; 
using System.Collections.Generic;  
using System.Text;
using LitJson;

public class listServant : MonoBehaviour {
	string url_getMyEmp = "http://220.184.61.5:8080/Game1/s/battle/getMyEmp";
	List<hero> emps=new List<hero>();
	string token;
	int uid;
	private Vector3 p_left;
	private Vector3 p_right;
	private Vector3 p_current;
	private Vector3 end2;
	private float distance;

	private int servant_count;
	private bool left_active = false;
	private bool right_active = false;
	private float process = 0f;

	public GameObject shade;
	public GameObject prefab;
	private GameObject parent;
	private List<GameObject> servants = new List<GameObject>();
	private GameObject servant;
	private int current = 0;
	private GameObject bt_left;
	private GameObject bt_right;
	// Use this for initialization
	void Start () {
		//get servants' count and id
		token = PlayerPrefs.GetString ("token");
		uid = PlayerPrefs.GetInt ("uid");
		StartCoroutine (GetMyEmp());
		//init button of UI and set shade's avtive
		bt_left = GameObject.Find ("left");
		bt_right = GameObject.Find ("right");
		bt_right.SetActive (false);
		shade.SetActive (false);
		//set move distance by parent's width
		parent = GameObject.Find ("ScrollServant");
		distance = parent.GetComponent<RectTransform> ().rect.width / 2.5f;
		//init the three position
		p_left = new Vector3 (parent.transform.position.x - distance,parent.transform.position.y,parent.transform.position.z);
		p_right = new Vector3 (parent.transform.position.x + distance, parent.transform.position.y, parent.transform.position.z);
		p_current = new Vector3 (parent.transform.position.x, parent.transform.position.y, parent.transform.position.z);
		//load card of servants;
		servant = GameObject.Instantiate (prefab) as GameObject;
		//for(int i = 0; i < servant_count; i++){
		for(int i = 0; i < 7; i++){
			servant = GameObject.Instantiate (prefab) as GameObject;
			servant.GetComponent<Transform> ().SetParent (parent.GetComponent<Transform> (), false);
			servant.name = "" + i;
			int id = (int)emps [i].empid;
			AddTag (id.ToString(),servant);
			//AddTag(""+i,servant);
			servant.transform.FindChild("picture").GetComponent<RawImage>().texture = Resources.Load<Texture> (""+i);
			//servant.transform.FindChild("picture").GetComponent<RawImage>().texture = Resources.Load<Texture> (id.ToString());
			servants.Add (servant);
			servant.SetActive (false);
		}
		//servants.Find (1).SetActive (true);
		servants [0].SetActive (true);
		//print (servants.Count);
		if(current == servants.Count-1){
			bt_left.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(left_active){
			process += 0.02f * 2;
			if (process < 1) {
				servants [current-1].GetComponent<Rigidbody> ().MovePosition (Vector3.Lerp(p_current,p_left,process));
				servants [current].GetComponent<Rigidbody> ().MovePosition (Vector3.Lerp(p_right,p_current,process));
			} else {
				servants [current-1].SetActive (false);
				left_active = false;
				shade.SetActive (false);
			}
		}
		if(right_active){
			process += 0.02f * 2;
			if (process < 1) {
				servants [current+1].GetComponent<Rigidbody> ().MovePosition (Vector3.Lerp(p_current,p_right,process));
				servants [current].GetComponent<Rigidbody> ().MovePosition (Vector3.Lerp(p_left,p_current,process));
			} else {
				servants [current+1].SetActive (false);
				right_active = false;
				shade.SetActive (false);
			}
		}

	}

	public void leftButton () {
		shade.SetActive (true);
		process = 0;
		if(current != servants.Count || current > servants.Count-1){
			move (true);
		}
		if(current == servants.Count-1){
			bt_left.SetActive (false);
		} else if(!bt_right.active){
			bt_right.SetActive (true);
		}
	}
	public void rightButton () {
		shade.SetActive (true);
		process = 0;
		if(current != 0||current > 0){
			move (false);
		}
		if(current == 0){
			bt_right.SetActive (false);
		} else if(!bt_left.active){
			bt_left.SetActive (true);
		}
	}

	void move (bool isLeft) {
		GameObject g1;
		GameObject g2;
		if (isLeft) {
			current++;
			left_active = true;
			g1 = servants [current-1];
			g2 = servants [current];
			g2.SetActive (true);
			g2.transform.position = p_right;
			changeAlpha (g1,g2);
		} else {
			current--;
			right_active = true;
			g1 = servants [current+1];
			g2 = servants [current];
			g2.SetActive (true);
			g2.transform.position = p_left;
			changeAlpha (g1,g2);
		}
	}
	void changeAlpha(GameObject g1,GameObject g2){
		//change component's Alpha value
		Component[] g_g2 = g2.GetComponentsInChildren<Component> ();
		foreach(Component c in g_g2){
			if (c is Graphic)
			{
				(c as Graphic).CrossFadeAlpha (0, 0, true);
				(c as Graphic).CrossFadeAlpha(1, 0.5f, true);
			}
		}
		Component[] comps = g1.GetComponentsInChildren<Component>();
		foreach (Component c in comps)
		{			
			if (c is Graphic)
			{
				(c as Graphic).CrossFadeAlpha(0, 0.5f, true);
			}
		}
	}

	private IEnumerator GetMyEmp()
	{
		Dictionary<string,string> header = new Dictionary<string,string> ();
		header.Add("token", token);
		header.Add("uid", ""+uid);
		print ("token:" + token+" uid:"+uid);
		WWW www = new WWW(url_getMyEmp,null,header);
		yield return www;
		if (www.error != null) 
		{
			print (www.error);
		} 
		else 
		{
			print ("get message");
			print (www.text);
			emps= GetMyEmps (www.text);
			PlayerPrefsExtend.heros = emps;
			servant_count = emps.Count;
			print (servant_count);
		} 

	}

	public List<hero> GetMyEmps(string text){
		JsonData whole = JsonMapper.ToObject (text);
		JsonData Hero = whole ["value"];
		hero ceo = new hero ();
		skill skill1 = new skill ();
		List<hero> heros=new List<hero>();
		for (int i = 0; i < Hero.Count; i++) 
		{
			ceo = JsonMapper.ToObject<hero> (Hero [i].ToJson ());
			for (int j = 0; j < Hero [i] ["skills"].Count; j++) {
				skill1 = JsonMapper.ToObject<skill>(Hero [i] ["skills"][j].ToJson());
				if(Hero [i] ["skills"][j].Keys.Contains("functions"))
				{
					if (Hero [i] ["skills"][j] ["functions"].Count >= 2) {
						for (int k = 0; k < Hero [i] ["skills"][j] ["functions"].Count; k++) {
							skill1.functions [k] = JsonMapper.ToObject<Fun> (Hero [i] ["skills"][j] ["functions"] [k].ToJson ());

						}
					} else {
						skill1.functions [0] = JsonMapper.ToObject<Fun> (Hero [i] ["skills"][j]["functions"].ToJson ());
					}
				}
				ceo.skills [j] = skill1;
			}
			heros.Add (ceo);
		}
		return heros;
	}
	void AddTag(string tag,GameObject obj)
	{
		if(!isHasTag(tag))
		{	
			#if UNITY_EDITOR
			UnityEditor.SerializedObject tagManager = new UnityEditor.SerializedObject(obj);//序列化物体
			UnityEditor.SerializedProperty it = tagManager.GetIterator();//序列化属性
			while (it.NextVisible(true))//下一属性的可见性
			{
				if(it.name == "m_TagString")
				{
					it.stringValue =tag;
					tagManager.ApplyModifiedProperties();
				}
			}
			#endif
		}
	}
	bool isHasTag(string tag)
	{
		#if UNITY_EDITOR
		for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++) {
			if (UnityEditorInternal.InternalEditorUtility.tags[i].Equals(tag))
				return true;
		}
		#endif
		return false;
	}
}
