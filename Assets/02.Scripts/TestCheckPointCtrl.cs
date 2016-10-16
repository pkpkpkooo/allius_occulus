using UnityEngine;
using System.Collections;

public class TestCheckPointCtrl : MonoBehaviour
{
    public int spawn = 0;

    public GameObject[] monsters;
    public GameObject[] dMonsters;
    public GameObject[] zMonsters;
    public GameObject[] bMonsters;
    public GameObject[] spwans;
    void OnCollisionEnter(Collision coll)
    {
        Debug.Log("crash!");
        if (coll.collider.tag == "PLAYER")
        {
            //spawn++;
            int idx = 0;
            foreach (GameObject spwan in spwans)
            {
                foreach (GameObject bMonster in bMonsters)
                {
                    if (bMonster.GetComponent<BossMonsterCtrl>().isUsing == false)
                    {
                        spwan.GetComponent<TestSpawnPointCtrl>().moveMonster(idx, coll);
                        idx++;
                        break;
                    }
                }
            }

            Destroy(gameObject, 3f);
        }
    }
    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        bMonsters = GameObject.FindGameObjectsWithTag("BOSSMONSTER");
        spwans = GameObject.FindGameObjectsWithTag("SPAWN_POINT_TAG");
    }
}