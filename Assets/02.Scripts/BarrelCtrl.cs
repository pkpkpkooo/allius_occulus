using UnityEngine;
using System.Collections;

public class BarrelCtrl : MonoBehaviour {
    public GameObject sparkEffect;
    public GameObject expEffect;
    private Transform tr;
    private int hitCount = 0;
  

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();

	}

    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.tag == "BULLET")
        {
            GameObject spark = (GameObject)Instantiate(sparkEffect, coll.transform.position, Quaternion.identity);
            Destroy(spark, spark.GetComponent<ParticleSystem>().duration + 1.5f);
            Destroy(coll.gameObject);
            if(++hitCount >= 3)
            {
                ExpBarrel();
            }

        }
    }

    void ExpBarrel()
    {
        Instantiate(expEffect, tr.transform.position, Quaternion.identity);
        //tr위치 반경 10.0f 내의 모든 충돌체를 저장
        Collider[] colls = Physics.OverlapSphere(tr.position, 10.0f);

        foreach(Collider coll in colls)
        {
            Rigidbody rbody = coll.GetComponent<Rigidbody>();
            if(rbody != null)
            {
                //해당 충돌체의 질량을 1.0f로 바꾼 후 tr -> 충돌체의 위치 방향으로 1000f 만큼의 힘을 가한다. 300f 는 위로 향하는 힘
                rbody.mass = 1.0f;
                rbody.AddExplosionForce(1000.0f, tr.position, 10.0f, 300.0f);
            }
        }
        Destroy(gameObject, 5.0f);
       // Destroy(spark, spark.GetComponent<ParticleSystem>().duration + 1.5f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
