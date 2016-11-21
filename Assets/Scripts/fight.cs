using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;
public class fight : MonoBehaviour {
	bool fighting=false;
	double roundstart=0;
	double fightstart;
	double fighttime;
	double facttime;
	int truetime;
	string uid;
	string token;
	int i=0;
	int xxx=0;
	int round=1;
	string order="";
	long [] skidd=new long[3];
	static hero me=new hero();
	static hero bad= new hero();
	public GameObject clock;
	int myblood=0;
	int mymano=0;
	int enemyblood=0;
	int enemymano=0;
	public GameObject self;
	public GameObject enemy;
	public GameObject enblood;
	public GameObject seblood;
	public GameObject enmano;
	public GameObject semano;
	public GameObject hp1;
	public GameObject hp2;
	public GameObject mp1;
	public GameObject mp2;

	buff[] mybuffs = new buff[10];
	buff[] hisbuffs = new buff[10];
	cool[] mycool=new cool[4];
	cool[] hiscool = new cool[4];
	cool eo=new cool();

	void Start () {
		eo.ncool = 0;
		eo.skid = -1;
			for (int iu = 0; iu < 4; iu++) {
			mycool [iu] = eo;
			hiscool [iu] = eo;
		}

//		me = (hero)PlayerPrefsExtend.myone; 
//		bad = (hero)PlayerPrefsExtend.hisone;
		try{
		 myblood=(int)me.health;
		 mymano=(int)me.mana;
		 enemyblood=(int)bad.health;
		enemymano=(int)bad.mana;}
		catch(MissingReferenceException e){
			print (e);
		}
//		clock = GameObject.Find ("clock");
	
//	 enblood= GameObject.Find ("enblood");
//		seblood= GameObject.Find ("seblood");
		uid = PlayerPrefs.GetString ("uid");
		token=PlayerPrefs.GetString("token");

	}

	void Update () {
		facttime = Time.time-roundstart;
		truetime = (int)(5-facttime);
		print ("asdww");
		if (facttime < 5) {
			clock.GetComponent<TextMesh> ().text = truetime.ToString ();
			print ("asd");
		}
		if (facttime >= 5 && fighting==false )readtimebegin();
		if (fighting) 
		{
			fighttime = Time.time - fightstart;
			if (fighttime < 3) {
				if (Input.GetMouseButtonDown (0)) {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit) && i < 3) {
						order = order + hit.transform.name;
						++xxx;
						print ("hit ");
						print (hit.transform.name);
						if (xxx==4) {
							for(int j=0;j<5;j++)
							{
								if(order.Equals(me.skills[j].skcode.ToString()))
								{
									skidd [i] = (long)me.skills [j].skid;
									i++;
									xxx = 0;
								}	
							}
						}
					}
				}
			} 
			else if (fighttime >= 3) 
			{
				fighting=false;
				for (int iu = 0; iu < 4; iu++) {
					if (mycool[iu].ncool!= 0) {
						mycool [iu].ncool--;
					}
					if (hiscool[iu].ncool != 0) {
						hiscool [iu].ncool--;
					}
				}
				myblood += (int)me.healthRegen;
				mymano += (int)me.manaRegen;
				enemyblood += (int)bad.healthRegen;
				enemymano += (int)bad.manaRegen;
				seblood.GetComponent<TextMesh> ().text = myblood.ToString ();
				hp1.GetComponent<Scrollbar> ().value =(float)( myblood / me.health);
				semano.GetComponent<TextMesh> ().text = mymano.ToString ();
				mp1.GetComponent<Scrollbar> ().value =(float)( mymano/ me.mana);
				enblood.GetComponent<TextMesh> ().text = enemyblood.ToString ();
				hp2.GetComponent<Scrollbar> ().value =(float) (enemyblood / bad.health);
				enmano.GetComponent<TextMesh> ().text = enemymano.ToString ();
				mp2.GetComponent<Scrollbar> ().value = (float)(enemymano/ bad.mana);
				StartCoroutine (loadandcalculate ());
			}

		}

	}
	void readtimebegin (){
		print ("asdeee");
		fightstart = 0;
		fighting=true;
		i=0;
		order="";
		clock.GetComponent<TextMesh> ().text = "round " + round+" begin";
		round ++;
	}
	public IEnumerator loadandcalculate()
	{
		if (i == 0) 
		{
			long[] blank = new long[0];
			skidd = blank;
		}
//		JsonData jsq = new JsonData ();
//		for (int ob = 0; ob <= i; ob++) {
//			JsonData temp = new JsonData ();
//			temp = skidd [ob];
//			jsq.Add (temp);
//		}
//		string po = jsq.ToJson();
		WWWForm form = new WWWForm();
		form.AddField("myskiils",""); //   need skidd
		var head = form.headers;
		head ["token"] = token;
		head ["uid"] = uid;
		var dat = form.data;
		WWW www = new WWW("http://220.184.61.5:8080/game1/s/register", dat,head);
		yield return www;
		if (www.error != null) {
			print (www.error);
		}
		else 
		{
			hero me =(hero)PlayerPrefsExtend.myone;
			hero bad=(hero)PlayerPrefsExtend.hisone;
			JsonData big = JsonMapper.ToObject (www.text);
			JsonData skts = big ["value"];
			int count = skts.Count;
			skill myskt=new skill();
			int nmyskt=0;
			int nhisskt=0;
			double myskttime=0;
			skill hisskt=new skill();
			double hisskttime=0;
			double myadhurt=0;
			double hisadhurt=0;
			double myaphurt = 0;
			double hisaphurt = 0;

			for (int hu = 0; mybuffs [hu].effectLastsTime != -1; hu++) {
				if (mybuffs [hu].effectLastsTime > 0) {
					if (((long)mybuffs [hu].bufftype & 0x08000400) == 0x08000400) {
						myblood = (int)( myblood - (int)mybuffs [hu].number * 100 / (me.magicResist + 100));
						seblood.GetComponent<TextMesh> ().text = myblood.ToString ();
						hp1.GetComponent<Scrollbar> ().value =(float) (myblood / me.health);
						mybuffs [hu].effectLastsTime--;
						if (myblood <= 0) {
							Application.LoadLevel("Main");
						}
					}

					if (((long)mybuffs [hu].bufftype & 0x08000800) == 0x08000800) {
						mymano = mymano - (int)mybuffs [hu].number;
						semano.GetComponent<TextMesh> ().text = mymano.ToString ();
						mp1.GetComponent<Scrollbar> ().value =(float)( mymano/ me.mana);
						mybuffs [hu].effectLastsTime--;
						mybuffs [hu].effectLastsTime--;
					}
					if (((long)mybuffs [hu].bufftype & 0x01000800) == 0x01000800) {
						bad.magicResist = bad.magicResist/2;/////to be continue
						mybuffs [hu].effectLastsTime--;
					}
				}		
			}


			for (int li = 0; hisbuffs [li].effectLastsTime != -1; li++) {
				if (hisbuffs [li].effectLastsTime > 0) {
					if (((long)hisbuffs [li].bufftype & 0x08000400) == 0x08000400) {
						enemyblood = (int)( enemyblood - (int)hisbuffs [li].number * 100 / (bad.magicResist + 100));
						enblood.GetComponent<TextMesh> ().text = enemyblood.ToString ();
						hp2.GetComponent<Scrollbar> ().value =(float) (enemyblood / bad.health);
						hisbuffs [li].effectLastsTime--;
						if (enemyblood <= 0) {
							Application.LoadLevel("Main");
						}
					}

					if (((long)hisbuffs [li].bufftype & 0x08000800) == 0x08000800) {
						enemymano = enemymano - (int)hisbuffs [li].number;
						enmano.GetComponent<TextMesh> ().text = enemymano.ToString ();
						mp2.GetComponent<Scrollbar> ().value = (float)(enemymano/ bad.mana);
						hisbuffs [li].effectLastsTime--;
					}
					if (((long)hisbuffs [li].bufftype & 0x01000800) == 0x01000800) {
						me.magicResist = me.magicResist/2;/////to be continue
						hisbuffs [li].effectLastsTime--;
					}
				}		
			}





			while (count > 0 || i > 0) {

				myskt = whichsk (me, skidd[nmyskt]);
				hisskt = whichsk (bad,int.Parse(skts [nhisskt]["skid"].ToString()));
				cool mysktcool = whichcool (myskt.skid, mycool);
				cool hissktcool = whichcool (hisskt.skid, hiscool);
				int we;
				we = whichonecool (myskt.skid, mycool);
				int they;
				they = whichonecool (hisskt.skid, hiscool);
				if (myskttime == 0)
					myskttime =myskt.channelBefore;
				if (hisskttime == 0)
					hisskttime =hisskt.channelBefore;
		
				if (myskttime < hisskttime || (i > 0 && count == 0)) {
					if (mymano - myskt.manaCost <= 0 || mysktcool.ncool!=0) {
						myskttime += myskt.channel - myskt.channelBefore;
						skill skillrecord = new skill ();
						--i;
						if (i != 0) {
							++nmyskt;

							skillrecord = whichsk (me, skidd[nmyskt]);

							myskttime += skillrecord.channelBefore;
						}
					} 
					else{
					mymano = (int)(mymano - myskt.manaCost);
					int mengbi = 0;
					int justbuff = 0;

						if (((long)myskt.skillType & 0x10000000) == 0x10000000) {
							if (( (long)myskt.skillType & 0x10000001) == 0x10000001) {
							me.attackDamage += (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x10000002) == 0x10000002) {
							me.abilityPower += (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x10000004) == 0x10000004) {
							me.physicalResist += (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x10000008) == 0x10000008) {
							me.magicResist += (int)myskt.functions [mengbi].number;
							mengbi++;
						}	
							if (((long)myskt.skillType & 0x10000010) == 0x10000010) {
							myblood += (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x10000020) == 0x10000020) {
							me.health += (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x10000040) == 0x10000040) {
							mymano += (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x10000080) == 0x10000080) {
							me.attackDamage = me.attackDamage + me.attackDamage * (int)myskt.functions [mengbi].number;
							mengbi++;
						}
						//增加额外攻击次数 不太懂	if (myskt.skillType & 0x10000100) {+= myskt.functions [mengbi].number;mengbi++;}
							if (((long)myskt.skillType & 0x10000200) == 0x10000200) {
							me.skills [4].realDamage += (int)myskt.functions[mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x10000400) == 0x10000400) {
							myskt.physicalDamage = (int)myskt.physicalDamage + myskt.physicalDamage * (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x10000800) == 0x10000800) {
							myskt.magicDamage = (int)myskt.magicDamage + myskt.magicDamage * (int)myskt.functions [mengbi].number;
							mengbi++;
						}
					}
						if (((long)myskt.skillType & 0x08000000) == 0x08000000) {
						
							if (((long)myskt.skillType & 0x08000001) == 0x08000001) {
							bad.attackDamage = (int)bad.attackDamage - (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08000002) == 0x08000002) {
							bad.abilityPower = (int)bad.abilityPower - (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08000004) == 0x08000004) {
							bad.physicalResist = (int)bad.physicalResist - (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08000008) == 0x08000008) {
							bad.magicResist = (int)bad.magicResist - (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08000010) == 0x08000010) {
							myadhurt = (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08000020) == 0x08000020) {
							myaphurt = (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08000040) == 0x08000040) {
							enemyblood = (int)enemyblood - (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08000080) == 0x08000080) {
							bad.health = (int)bad.health - (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08000100) == 0x08000100) {
							enemyblood = (int)enemyblood - (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08000200) == 0x08000200) {
							enemymano = (int)enemymano - (int)myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08000400) == 0x08000400) {
							myaphurt += (int)myskt.functions [mengbi].number;
								hisbuffs [justbuff].bufftype = 0x08000400;
								hisbuffs [justbuff].effectLastsTimeType = 1;
								hisbuffs [justbuff].effectLastsTime = myskt.functions [mengbi].effectLastsTime-1;
								hisbuffs [justbuff].number = myskt.functions [mengbi].number;
							mengbi++;
							justbuff++;
						}
							if (((long)myskt.skillType & 0x08000800) == 0x08000800) {
							enemymano = enemymano - (int)myskt.functions [mengbi].number;
							hisbuffs [justbuff].bufftype = 0x08000800;
							hisbuffs [justbuff].effectLastsTimeType = 1;
							hisbuffs [justbuff].effectLastsTime = myskt.functions [mengbi].effectLastsTime;
							hisbuffs [justbuff].number = myskt.functions [mengbi].number;
							justbuff++;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08002000) == 0x08002000) {
							hisskt.channelBefore = (int)hisskt.channelBefore * 100 / myskt.functions [mengbi].number;
							hisskt.channel = hisskt.channel * 100 / myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08001000) == 0x08001000) {
							bad.skills [4].channelBefore = (int)bad.skills [4].channelBefore * 100 / myskt.functions [mengbi].number;
							mengbi++;
						}
							if (((long)myskt.skillType & 0x08004000) == 0x08004000) {
							hisbuffs [justbuff].bufftype = 0x08004000;
							hisbuffs [justbuff].effectLastsTimeType = 1;
							hisbuffs [justbuff].effectLastsTime = myskt.functions [mengbi].effectLastsTime-1;
							hisbuffs [justbuff].number = myskt.functions [mengbi].number;
							justbuff++;
								mengbi++;
						}
					}
						self.GetComponent<Animation> ().Play();
					myadhurt = myskt.physicalDamage * 100 / (bad.physicalResist + 100);
					myaphurt = myskt.magicDamage * 100 / (bad.magicResist + 100);
						enemyblood =  (int)(enemyblood - myadhurt);
						enemyblood = (int) (enemyblood - myaphurt);
						enemyblood = (int)( enemyblood - myskt.realDamage);
					enblood.GetComponent<TextMesh> ().text = enemyblood.ToString ();
					enmano.GetComponent<TextMesh> ().text = enemymano.ToString ();
					if (enemyblood <= 0) {
							Application.LoadLevel("Main");
					}
					myskttime += myskt.channel - myskt.channelBefore;
					skill skillrecord = new skill ();
					--i;
						mycool [we].ncool = (int)myskt.cooldown;
					if (i != 0) {
						++nmyskt;

							skillrecord = whichsk (me, skidd[nmyskt]);

						myskttime += skillrecord.channelBefore;
					}
				}
				}


				if (hisskttime < myskttime || (count > 0 && i == 0)) {
					if (bad.mana - hisskt.manaCost <= 0 || hissktcool.ncool!=0) {
						hisskttime += hisskt.channel - hisskt.channelBefore;
						skill skillrecord = new skill ();
						--count;
						if (count != 0) {
							++nhisskt;
							skillrecord = whichsk (bad, int.Parse (skts [nhisskt]["skid"].ToString ()));
							hisskttime += skillrecord.channelBefore;
						}



					} else {
						enemymano = (int)(enemymano - hisskt.manaCost);
						int mengbi = 0;
						int justbuff = 0;

				
						if (((long)hisskt.skillType & 0x10000000) == 0x10000000) {
							if (((long)hisskt.skillType & 0x10000001) == 0x10000001) {
								bad.attackDamage += (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x10000002) == 0x10000002) {
								bad.abilityPower += (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x10000004) == 0x10000004) {
								bad.physicalResist += (int)hisskt.functions [mengbi].number;
								mengbi++;	
							}
							if (((long)hisskt.skillType & 0x10000008) == 0x10000008) {
								bad.magicResist += (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x10000010) == 0x10000010) {
								enemyblood += (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x10000020) == 0x10000020) {
								bad.health += (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x10000040) == 0x10000040) {
								enemymano += (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x10000080) == 0x10000080) {
								bad.attackDamage = bad.attackDamage + bad.attackDamage * (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x10000200) == 0x10000200) {
								bad.skills [4].realDamage += (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x10000400) == 0x10000400) {
								hisskt.physicalDamage = (int)hisskt.physicalDamage + (int)hisskt.physicalDamage * (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (( (long)hisskt.skillType & 0x10000800) == 0x10000800) {
								hisskt.magicDamage = (int)hisskt.magicDamage + (int)hisskt.magicDamage * (int)hisskt.functions [mengbi].number;
								mengbi++;
							}

						}
						if (((long)hisskt.skillType & 0x08000000) == 0x08000000) {
											
							if (((long)hisskt.skillType & 0x08000001) == 0x08000001) {
								me.attackDamage = (int)me.attackDamage - (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08000002) == 0x08000002) {
								me.abilityPower = (int)me.abilityPower - (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08000004) == 0x08000004) {
								me.physicalResist = (int)me.physicalResist - (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08000008) == 0x08000008) {
								me.magicResist = (int)me.magicResist - (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08000010) == 0x08000010) {
								hisadhurt = (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08000020) == 0x08000020) {
								hisaphurt = (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08000040) == 0x08000040) {
								myblood = (int)myblood - (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08000080) == 0x08000080) {
								me.health = (int)me.health - (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08000100) == 0x08000100) {
								myblood = (int)myblood - (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08000200) == 0x08000200) {
								mymano = (int)mymano - (int)hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08000400)==0x08000400) { 
								hisaphurt += (int)hisskt.functions [mengbi].number;
								mybuffs [justbuff].bufftype = 0x08000400;
								mybuffs [justbuff].effectLastsTimeType = 1;
								mybuffs [justbuff].effectLastsTime = hisskt.functions [mengbi].effectLastsTime-1;
								mybuffs [justbuff].number = hisskt.functions [mengbi].number;
								mengbi++;
								justbuff++;
							}
							if (((long)hisskt.skillType & 0x08000800)==0x08000800) {
								mymano = mymano - (int)hisskt.functions [mengbi].number;
								hisbuffs [justbuff].bufftype = 0x08000800;
								hisbuffs [justbuff].effectLastsTimeType = 1;
								hisbuffs [justbuff].effectLastsTime = hisskt.functions [mengbi].effectLastsTime;
								hisbuffs [justbuff].number = hisskt.functions [mengbi].number;
								justbuff++;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08002000) == 0x08002000) {
								myskt.channelBefore = (int)myskt.channelBefore * 100 / hisskt.functions [mengbi].number;
								myskt.channel = myskt.channel * 100 / hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08001000) == 0x08001000) {
								me.skills [4].channelBefore = (int)me.skills [4].channelBefore * 100 / hisskt.functions [mengbi].number;
								mengbi++;
							}
							if (((long)hisskt.skillType & 0x08004000)==0x08004000) {
								mybuffs [justbuff].bufftype = 0x08004000;
								mybuffs [justbuff].effectLastsTimeType = 1;
								mybuffs [justbuff].effectLastsTime = hisskt.functions [mengbi].effectLastsTime-1;
								mybuffs [justbuff].number = hisskt.functions [mengbi].number;
								mengbi++;
								justbuff++; 
							}
						}
						enemy.GetComponent<Animation> ().Play();
						hisadhurt = hisskt.physicalDamage * 100 / (me.physicalResist + 100);
						hisaphurt = hisskt.magicDamage * 100 / (me.magicResist + 100);
						myblood = (int)( myblood - hisadhurt);
						myblood = (int)( myblood - hisaphurt);
						myblood = (int)( myblood - hisskt.realDamage);
						seblood.GetComponent<TextMesh> ().text = myblood.ToString ();
						semano.GetComponent<TextMesh> ().text = mymano.ToString ();
						if (myblood <= 0) {
							Application.LoadLevel("Main");
						}
						hisskttime += hisskt.channel - hisskt.channelBefore;
						skill skillrecord = new skill ();
						--count;
						hiscool [they].ncool =(int) hisskt.cooldown;
						if (count != 0) {
							++nhisskt;

							skillrecord = whichsk (bad, int.Parse (skts [nhisskt]["skid"].ToString ()));

							hisskttime += skillrecord.channelBefore;
						}
					}
				}}
			roundstart = Time.time;
		}
	}

	public skill whichsk(hero ob ,long skid)
	{
		for(int j=0;j<5;j++)
		{
			if(skid.Equals(ob.skills[j].skid.ToString()))
			{
				return ob.skills[j];
			}	
		}
		return null;
	}
	public cool whichcool(double id,cool[] eo ){
		for (int uu = 0; uu < 4;uu++) {
			if (eo [uu].skid == id)
				return eo [uu];
		}
		return null;
	}
	public int whichonecool(double id,cool[] eo ){
		for (int uu = 0; uu < 4; uu++) {
			if (eo [uu].skid == id)
				return  uu;
		}
		return -1;
	}
	public double baoji(int hurt,int probability,int baojidamage ){
		int pro = (int )Random.value*100;
		if (pro < probability) {
			return hurt * baojidamage / 100;
		} else
			return hurt;
	
		}

}
