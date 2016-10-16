using UnityEngine;
using System.Collections;

public class TestSpawnPointCtrl : MonoBehaviour {
	public GameObject check_point;
	public GameObject[] storagePoints;
    public GameObject[] players;


    public GameObject[] monsters;
    public GameObject[] dMonsters;
    public GameObject[] zMonsters;
    public GameObject[] bMonsters;

    public void moveMonster(int idx, Collision coll)
    {
        bMonsters[idx].GetComponent<Transform>().position = GetComponent<Transform>().position;
        bMonsters[idx].GetComponent<NavMeshAgent>().enabled = true;
        bMonsters[idx].GetComponent<BossMonsterCtrl>().isUsing = true;
        int randomNum = Random.Range(0, players.Length);
        bMonsters[idx].GetComponent<BossMonsterCtrl>().targetPtr = players[randomNum].transform;

        //storagePoints [idx].GetComponent<StoragePointCtrl> ().isFull = false;
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
        bMonsters = GameObject.FindGameObjectsWithTag("BOSSMONSTER");
        check_point = GameObject.FindGameObjectWithTag ("CHECK_POINT_TAG");
		storagePoints = GameObject.FindGameObjectsWithTag ("StoragePoint");
        players = GameObject.FindGameObjectsWithTag("PLAYER");
	}

}
