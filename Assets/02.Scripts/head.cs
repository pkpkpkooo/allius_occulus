using UnityEngine;
using System.Collections;

public class head : MonoBehaviour {
    public GameObject tmpHead;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        tmpHead.transform.localPosition = new Vector3(0, 0, 0);

    }
}
