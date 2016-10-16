using UnityEngine;
using System.Collections;

public class CheckPointCtrl : MonoBehaviour {
	public int spawn = 0;
	void OnCollisionEnter(Collision coll){
		Debug.Log ("crash!");
		if (coll.collider.tag == "PLAYER") {
			spawn++;
            this.transform.Translate(200.0f, -20.0f, 10.0f);
        }
    }
	// Use this for initialization
	void Start () {	
	}
	// Update is called once per frame
	void Update () {	
	}
}