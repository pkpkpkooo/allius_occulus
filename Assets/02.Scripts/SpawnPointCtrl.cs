using UnityEngine;
using System.Collections;

public class SpawnPointCtrl : MonoBehaviour {
	public GameObject monster;
	public GameObject check_point;
	public void make_monster(){
		GameObject mon = PhotonNetwork.Instantiate("monster", GetComponent<Transform> ().position, Quaternion.identity, 0);
		Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		check_point = GameObject.FindGameObjectWithTag ("CHECK_POINT_TAG");
	}
	
	// Update is called once per frame
	void Update () {
	}

	IEnumerator CreateMonster ()
	{
		float pos = 0.0f;
		//float pos = Random.Range(-100.0f, 100.0f);
		GameObject mon = PhotonNetwork.Instantiate("monster", new Vector3( 10.0f, 10.0f, 10.0f ), Quaternion.identity, 0);
		mon.transform.position = GetComponent<Transform> ().position;
		Destroy (gameObject);
		yield return null;
	}
}
