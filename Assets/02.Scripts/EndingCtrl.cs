using UnityEngine;
using System.Collections;

public class EndingCtrl : MonoBehaviour {
    
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PLAYER")
        {
            coll.gameObject.transform.Translate(-75, 15, 90);
        }
    }
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
