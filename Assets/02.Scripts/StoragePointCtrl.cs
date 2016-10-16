using UnityEngine;
using System.Collections;

public class StoragePointCtrl : MonoBehaviour {
	public bool isFull = true;
	public int monsterIdx = 100; // 초기상태
    public int monsterType; //  2 : dog , 3 : boss
	public GameObject[] monsters;
    public GameObject[] dMonsters;
    public GameObject[] zMonsters;
    public GameObject[] bMonsters;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//monters = GameObject.FindGameObjectsWithTag ("Monster");


        dMonsters = GameObject.FindGameObjectsWithTag("DOGMONSTER");
        bMonsters = GameObject.FindGameObjectsWithTag("BOSSMONSTER");
        zMonsters = GameObject.FindGameObjectsWithTag("ZOMBIEMONSTER");

        int dLen = dMonsters.Length;
        int bLen = bMonsters.Length;

        monsters = new GameObject[dLen+bLen];
        int idx = 0;
        for (int i = 0; i < dMonsters.Length; i++, idx++)
            monsters[idx] = dMonsters[i];
        for (int i = 0; i < bMonsters.Length; i++, idx++)
            monsters[idx] = bMonsters[i];

    }
}
