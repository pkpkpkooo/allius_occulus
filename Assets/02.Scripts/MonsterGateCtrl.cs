using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterGateCtrl : MonoBehaviour {

    public List<SwitchCtrl> targetSwitchs = new List<SwitchCtrl>();
    public GameObject[] dMonsters;
    public GameObject[] storagePoints;
    public int monsterCount = 0;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        dMonsters = GameObject.FindGameObjectsWithTag("DOGMONSTER");
        storagePoints = GameObject.FindGameObjectsWithTag("StoragePoint");
        if (targetSwitchs == null) // 계산할 시간을 주자
            return;
        for(int sIdx=0; sIdx<targetSwitchs.Count; sIdx++)
        {
            if (!targetSwitchs[sIdx].closeGate && targetSwitchs[sIdx].isActivate) // 활성화되고 소환하지 않았다면 !
            {
                //몬스터 이동 고고
                for (int idx = 0; idx < dMonsters.Length; idx++)
                {
                    if (monsterCount == 2)
                    {
                        monsterCount = 0;
                        targetSwitchs[sIdx].closeGate = true;
                        break;
                    }
                    if (!dMonsters[idx].GetComponent<DogCtrl>().isUsing) // 사용 중이지 않으면 이동시킨다 / 도그로 바꿔야된다 스크립트
                    {
                        dMonsters[idx].transform.position = new Vector3(transform.position.x + (idx%2 * 2), transform.position.y, transform.position.z);
                        dMonsters[idx].GetComponent<NavMeshAgent>().enabled = true;
                        dMonsters[idx].GetComponent<DogCtrl>().isUsing = true; // 도그로 바꿔야 된다 스크립다
                        storagePoints[idx].GetComponent<StoragePointCtrl>().isFull = false;
                        monsterCount++;
                    }
                }
            }
        }
    }
}
