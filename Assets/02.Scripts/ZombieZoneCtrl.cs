using UnityEngine;
using System.Collections;

public class ZombieZoneCtrl : MonoBehaviour
{

    public GameObject[] patrollPoint;
    public GameObject zombie;
    public GameObject[] tmps;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "ZOMBIEMONSTER")
        {
            zombie = coll.gameObject;
            zombie.GetComponent<ZombieCtrl>().tmps = patrollPoint;
        }
        if (coll.tag == "PLAYER")
        {
            zombie.GetComponent<ZombieCtrl>().isPatroll = false;
            zombie.GetComponent<ZombieCtrl>().targetPtr = coll.gameObject.transform;
            //zombie.transform.LookAt(zombie.GetComponent<ZombieCtrl>().targetPtr);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "PLAYER")
        {
            zombie.GetComponent<ZombieCtrl>().isPatroll = true;
            zombie.GetComponent<ZombieCtrl>().targetPtr = null;
            //zombie.GetComponent<ZombieCtrl>().nvAgent.destination = tmps[0].transform.position;
            //zombie.transform.LookAt(tmps[0].transform);
            zombie.GetComponent<ZombieCtrl>().nvAgent.destination = patrollPoint[0].transform.position;
            zombie.transform.LookAt(patrollPoint[0].transform);
        }
    }

    // Use this for initialization
    void Start()
    {
        //tmps = GameObject.FindGameObjectsWithTag("TMP");
        for(int idx=0; idx<transform.GetChildCount(); idx++)
        {
            GameObject tmp = transform.GetChild(idx).gameObject;
            patrollPoint[idx] = tmp;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}