using UnityEngine;
using System.Collections;

public class ZombieCtrl : MonoBehaviour
{
    public enum MonsterState { idle, trace, attack, die, hit };
    public MonsterState monsterState = MonsterState.idle;
    public float traceDist = 20.0f;
    public float attackDist = 2.0f;
    public int hp = 1;
    public int ap = 10;

    public bool isDie = false;
    private Transform tr;
    public Transform targetPtr = null;
    private Rigidbody rig;
    public NavMeshAgent nvAgent;
    private Animator animator;
    public int idx = 100;
    public bool isUsing = true;
    public GameObject[] storagePoints;
    public GameObject[] players;

    private PhotonView pv = null;

    private GameObject player;

    public AudioClip attack_snd;
    public AudioClip attack2_snd;
    public AudioClip trace_snd;
    public AudioSource audio;

    public Vector3[] patrollPoint = new Vector3[2];
    private Vector3 tempV;
    public bool isPatroll = true;

    public GameObject[] tmps;
    int tmpJ = 0;
    public GameObject tmpPos;

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);
            if (isUsing)
            {
                if (!isPatroll)
                {
                    if (targetPtr == null)
                    {
                        float dist = Vector3.Distance(new Vector3(1, 0.04f, 310), tr.position);
                    }
                    else
                    {
                        float dist = Vector3.Distance(targetPtr.position, tr.position);
                        if (monsterState != MonsterState.hit)
                        {

                            if (dist <= attackDist)
                                monsterState = MonsterState.attack;

                            else if (dist <= traceDist)
                                monsterState = MonsterState.trace;
                            else
                                monsterState = MonsterState.idle;
                        }
                    }
                }
                else
                {
                    if(tmpJ == 0) // 처음 지역 지정
                    {
                        nvAgent.destination = tmps[0].transform.position; 
                        tempV = tmps[0].transform.position;
                        tr.LookAt(tmps[0].transform);
                        tmpJ++;
                    }
                    monsterState = MonsterState.trace;
                    // zombieZone 하위 오브젝트 받아와서 패트롤 시켜놓는 코드
                    if (Vector3.Distance(tmps[0].transform.position, tr.position) <= 4)
                    {
                        nvAgent.destination = tmps[1].transform.position;
                        tempV = tmps[1].transform.position;
                        tr.LookAt(tmps[1].transform);
                    }
                    else if (Vector3.Distance(tmps[1].transform.position, tr.position) <= 4)
                    {
                        nvAgent.destination = tmps[0].transform.position;
                        tempV = tmps[0].transform.position;
                        tr.LookAt(tmps[0].transform);
                    }
                }
            }
        }
    }

    IEnumerator MonsterAction()
    {

        while (!isDie)
        {
            if (isUsing)
            {
                switch (monsterState)
                {
                    case MonsterState.idle:
                        nvAgent.Stop();
                        animator.SetBool("isTrace", false);
                        break;
                    case MonsterState.trace:
                        nvAgent.Resume();
                        animator.SetBool("isTrace", true);
                        animator.SetBool("isAttack", false);
                        break;
                    case MonsterState.attack:
                        nvAgent.Stop();
                        animator.SetBool("isAttack", true);
                        animator.SetBool("isTrace", false);
                        break;
                    case MonsterState.hit:
                        nvAgent.Stop();
                        //animator.Stop ();
                        animator.SetTrigger("isHit");
                        animator.SetBool("isAttack", false);
                        animator.SetBool("isTrace", false);
                        tr.Translate(Vector3.back * 0.15f);
                        break;

                }

            }
            yield return null;

        }
    }


    void attack3() { targetPtr.GetComponent<PlayerCtrl>().hit(ap * 2); }
    void attack2() { targetPtr.GetComponent<PlayerCtrl>().hit(ap / 2); }
    void attack() {
        if(targetPtr != null)
            targetPtr.GetComponent<PlayerCtrl>().hit(ap);
    }
    void atkSnd()
    {
        audio.clip = attack_snd;
        audio.Play();
    }
    void atkSnd2()
    {
        audio.clip = attack2_snd;
        audio.Play();
    }
    void traceSnd()
    {
        audio.clip = trace_snd;
        audio.Play();
    }
    void die()
    {
        animator.Stop();
        int idx = 0;
        //      foreach (GameObject storagePoint in storagePoints) {
        //         if (storagePoint.GetComponent<StoragePointCtrl> ().isFull == false) {
        //            storagePoint.GetComponent<StoragePointCtrl> ().isFull = true;
        //            storagePoint.GetComponent<StoragePointCtrl> ().monsterIdx = this.idx;
        //            GetComponent<Transform> ().position = storagePoint.GetComponent<Transform> ().position;
        //            isUsing = false;
        //            isDie = false;
        //            break;
        //         }
        
        // 밑에하면서 주석한거(염꺼고치는중)
        //for (idx = 0; idx < storagePoints.Length; idx++)
        //{
        //    if (storagePoints[idx].GetComponent<StoragePointCtrl>().isFull == false)
        //    {
        //        storagePoints[idx].GetComponent<StoragePointCtrl>().isFull = true;
        //        storagePoints[idx].GetComponent<StoragePointCtrl>().monsterIdx = this.idx;
        //        GetComponent<Transform>().position = storagePoints[idx].GetComponent<Transform>().position;
        //        isUsing = false;
        //        isDie = false;
        //        break;
        //    }
        //}
        nvAgent.enabled = false;
        animator.SetBool("isDie", false);
    }

    void hitEnd()
    {
        monsterState = MonsterState.idle; // 이거바꿈 원래 idle
        //nvAgent.
    }
    // Use this for initialization
    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "BULLET")
        {
            monsterState = MonsterState.hit;
            nvAgent.Stop();
            hp--;
            Destroy(coll.gameObject);
        }
        if (hp <= 0)
        {
            isDie = true;
            nvAgent.Stop();
            GetComponent<CapsuleCollider>().isTrigger = true;
        }
    }
    void Start()
    {
        tr = GetComponent<Transform>();
        rig = GetComponent<Rigidbody>();
        nvAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        pv = GetComponent<PhotonView>();
        //PhotonView Observed 속성에 TankMove 스크립트를 연결
        pv.observed = this;
        
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());


    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("PLAYER");
        storagePoints = GameObject.FindGameObjectsWithTag("StoragePoint");

        player = GameObject.FindGameObjectWithTag("PLAYER");
        //tmps = GameObject.FindGameObjectsWithTag("TMP");
        
        animator.ResetTrigger("isHit");

        if (!isUsing)
            return;
        else if (targetPtr == null)
            return;
        //nvAgent.destination = tmpPos.transform.position;
        else
        {
            tr.LookAt(targetPtr);
            nvAgent.destination = targetPtr.position;
        }
    }

    //photonVIew 없어도 될거같아.. 이거는 무슨값을 서로 전달받을떄 있는거같으니까...
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //로컬 플레이어의 위치 정보 송신
        if (stream.isWriting)
        {

        }
        else //원격 플레이어의 위치 정보 수신
        {


        }
    }
}