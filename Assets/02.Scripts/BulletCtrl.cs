using UnityEngine;
using System.Collections;

public class BulletCtrl : MonoBehaviour {
    public int damage = 20;
    public float speed = 1000.0f;
    private Rigidbody rbody;

	// Use this for initialization
	void Start () {
        rbody = GetComponent<Rigidbody>();
        rbody.AddForce(transform.forward * speed);
	}
    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag != "PLAYER" && coll.collider.tag != "BULLET")
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update () {
	
	}
}
