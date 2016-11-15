using UnityEngine;
using System.Collections;

public class show : MonoBehaviour {
    public float duration = 5;
    // Use this for initialization
    void Start () {    
		float proportion = Time.time / duration;
		GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0, 1, proportion));
	}
	
	// Update is called once per frame
	void Update () {
        
    }
}
