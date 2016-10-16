using UnityEngine;
using System.Collections;

public class WallCtrl : MonoBehaviour {
    public GameObject sparkEffect;

    void OnCollisionEnter(Collision coll) {
        if(coll.collider.tag == "BULLET")
        {
            GameObject spark = (GameObject)Instantiate(sparkEffect,coll.transform.position,Quaternion.identity);
            Destroy(spark, spark.GetComponent<ParticleSystem>().duration + 1.5f);
            Destroy(coll.gameObject);
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
