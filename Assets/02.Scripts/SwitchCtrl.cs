using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchCtrl : MonoBehaviour {

    public GameObject myScreen;
    public Texture screen;
    public GameObject miniMapQuad;
    public Texture miniMapImage;
    public GameObject targetMonsterGate;
    public GameObject[] monsterGates;
    public bool isActivate = false;
    public bool closeGate = false;
    public int minIdx = 0;
    public int testNum = 0;
    public GameObject lastDoor;
    public GameObject lastSwitchDoor;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PLAYER")
        {
            Debug.Log("enter coll");
            myScreen.GetComponent<MeshRenderer>().material.mainTexture = screen;
            miniMapQuad.GetComponent<MeshRenderer>().material.mainTexture = miniMapImage;
            miniMapQuad.GetComponent<MeshRenderer>().material.mainTexture = miniMapImage;
            lastDoor.GetComponent<LastDoorCtrl>().lastDoorCnt++;
            lastSwitchDoor.GetComponent<LastSwitchCtrl>().lastSwitchCnt++;
            GetComponent<AudioSource>().Play();
            GetComponent<SphereCollider>().enabled = false;
            isActivate = true;
        }
    }
	// Use this for initialization
	void Start () {
        StartCoroutine(SelectMonsterGate());
    }

    IEnumerator SelectMonsterGate()
    {
        yield return new WaitForSeconds(1f);
        float currDist = 0;
        float minDist = Vector3.Distance(monsterGates[0].transform.position, transform.position);
        for (int idx = 0; idx < monsterGates.Length; idx++)
        {
            currDist = Vector3.Distance(monsterGates[idx].transform.position, transform.position);
            if (minDist >= currDist)
            {
                minDist = currDist;
                minIdx = idx;
            }
        }
        targetMonsterGate = monsterGates[minIdx];
        targetMonsterGate.GetComponent<MonsterGateCtrl>().targetSwitchs.Add(this);
    }

    // Update is called once per frame
    void Update () {
        monsterGates = GameObject.FindGameObjectsWithTag("MONSTERGATE");
        if(testNum == 10)
        {

        }
    }
}
