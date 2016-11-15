using UnityEngine;
using System.Collections;
using System.Collections.Generic;  
using System.Text;
using LitJson;
public class select : MonoBehaviour {
	skill A=new skill(); 
	skill B=new skill();
	int uid;
	string token;
	hero he=new hero();

	void Start () {
		uid=PlayerPrefs.GetInt("uid");
		token = PlayerPrefs.GetString ("token");
	}
	// Update is called once per frame
	void Update () {
	
	}
		
	public void OnClick(){
		List<hero> myheros=PlayerPrefsExtend.heros;
		hero mychoose = new hero ();
		mychoose = myheros [int.Parse(this.transform.parent.tag)];
		A.channelBefore = 1 / mychoose.attackSpeed;
		A.physicalDamage = mychoose.attackDamage;
		A.channel = A.channelBefore;
		A.skcode = 4321;
		A.skid = -1;
		mychoose.skills [4] = A;
		PlayerPrefsExtend.myone = mychoose;
		StartCoroutine (Sendmyandgethis());
		PlayerPrefs.SetString ("MyServant_id",this.transform.parent.tag);
		//Application.LoadLevel("Loading");
		DestroyObject(this.gameObject);
		DestroyObject(GameObject.Find ("left"));
		DestroyObject(GameObject.Find ("right"));
	}
	public IEnumerator Sendmyandgethis()
	{
		
		WWWForm form = new WWWForm();
		form.AddField("uid", uid);
		form.AddField("token", token);
		form.AddField("selectEmp",int.Parse(this.transform.parent.tag));
		WWW www = new WWW(" ", form);
		yield return www;
		if (www.error != null) {
			print (www.error);
		}
		else 
		{
			he = GetEnemyEmp (www.text);
			B.physicalDamage = he.attackDamage;
			B.channelBefore = 1 / he.attackSpeed;
			B.skcode = 4321;
			B.channel = B.channelBefore;
			B.skid = -2;
			he.skills [4] = B;
			PlayerPrefsExtend.hisone = he;
		}
	}



	public hero GetEnemyEmp(string text){
		JsonData whole = JsonMapper.ToObject (text);
		JsonData Hero = whole ["value"];
		hero ceo = new hero ();
		skill skill1 = new skill ();
		ceo = JsonMapper.ToObject<hero> (Hero.ToJson ());
		for (int j = 0; j < Hero  ["skills"].Count; j++) {
			skill1 = JsonMapper.ToObject<skill>(Hero  ["skills"][j].ToJson());
			if(Hero ["skills"].Keys.Contains("functions"))
			{
				if (Hero  ["skills"][j] ["functions"].Count >= 2) {
					for (int k = 0; k < Hero ["skills"][j] ["functions"].Count; k++) {
						skill1.functions [k] = JsonMapper.ToObject<Fun> (Hero ["skills"][j] ["functions"] [k].ToJson ());
					}
				} 
				else {
					skill1.functions [0] = JsonMapper.ToObject<Fun> (Hero  ["skills"][j]["functions"].ToJson ());
				}
			}
			ceo.skills [j] = skill1;
		}
		return ceo;
	}
}
