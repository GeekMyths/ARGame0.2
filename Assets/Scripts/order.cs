using UnityEngine;
using System.Collections;

public class order : MonoBehaviour {
	bool fighting=true;
	double fightstart;
	double fighttime;
	int i=0;
	GameObject arthus;
	string[] orders = new string[5];
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (fighting) {

			fighttime = Time.time;
			if(fighttime - fightstart <100) {

				if(Input.GetMouseButtonDown(0))
				{
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit))
					{
						orders[i] = hit.transform.name;
						print (orders[i]);
						i++;
						arthus = GameObject.Find ("self");
						arthus.GetComponent<Animation> ().Play ();
					}
				}


			}
			if (fighttime - fightstart > 100) {
				fighting = false;
				i = 0;
			
			}
		}



	}
	public void Onclick(){
		fighting = true;
		fightstart = Time.time;

		i = 0;
	}
}
