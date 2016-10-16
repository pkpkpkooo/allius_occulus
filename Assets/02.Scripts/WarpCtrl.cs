using UnityEngine;
using System.Collections;

public class WarpCtrl : MonoBehaviour {
    private Vector3 startPosition;
    private Vector3 goalPosition;

	// Use this for initialization
	void Start () {
	
	}
    void OnCollisionEnter(Collision coll)
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update () {
	
	}
}
