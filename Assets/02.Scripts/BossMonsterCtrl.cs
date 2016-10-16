using UnityEngine;
using System.Collections;

public class BossMonsterCtrl : MonoBehaviour
{
    public enum MonsterState { idle, trace, attack, die, hit };
    public MonsterState monsterState = MonsterState.idle;
    public float traceDist = 1000.0f; // 임시로 늘려놨음
    public float attackDist = 2.0f;
    public int hp = 10;
    public int ap = 10;

    public bool isDie = false;
    private Transform tr;
    public Transform targetPtr = null;
    private Rigidbody rig;
    private NavMeshAgent nvAgent;
    private Animator animator;
    public int idx = 2;
    public bool isUsing = false;
    public GameObject[] storagePoints;
    public GameObject[] players;

    private PhotonView pv = null;

    private GameObject player;

    public AudioClip attack_snd;
    public AudioClip attack2_snd;
    public AudioClip trace_snd;
    public AudioSource audio;
    public Transform ptr;

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);
            if (isUsing)
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
                        animator.SetTrigger("isHit");
                        animator.SetBool("isAttack", false);
                        animator.SetBool("isTrace", false);
                        tr.Translate(Vector3.back * 0.15f);
                        break;
                }

            }
            yield return null;

        }
        animator.SetBool("isDie", true);
    }


    void attack3() { targetPtr.GetComponent<PlayerCtrl>().hit(ap * 2); }
    void attack2() { targetPtr.GetComponent<PlayerCtrl>().hit(ap / 2); }
    void attack() { targetPtr.GetComponent<PlayerCtrl>().hit(ap); }
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
    //여기 부분이 일단 필요 없겠지 보스는 안죽으니까
    void die()
    {
        animator.Stop();
        int idx = 0;

        for (idx = 0; idx < storagePoints.Length; idx++)
        {
            if (storagePoints[idx].GetComponent<StoragePointCtrl>().isFull == false)
            {
                storagePoints[idx].GetComponent<StoragePointCtrl>().isFull = true;
                storagePoints[idx].GetComponent<StoragePointCtrl>().monsterIdx = this.idx; // dog니까 2 이제 한종류만 관리하면 idx 필요없다
                GetComponent<Transform>().position = storagePoints[idx].GetComponent<Transform>().position;
                isUsing = false;
                isDie = false;
                break;
            }
        }
        nvAgent.enabled = false;
        animator.SetBool("isDie", false);
    }

    void hitEnd()
    {
        monsterState = MonsterState.idle;
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

       

        if(players.Length == 1)
            ptr = player.GetComponent<Transform>();
        else
        {
            if (idx == 0)
                ptr = players[0].GetComponent<Transform>();
            else
                ptr = players[1].GetComponent<Transform>();
        }

        tr.LookAt(ptr);
        animator.ResetTrigger("isHit");
        if (!isUsing)
            return;
        else if (targetPtr == null)
            return;
        else
            nvAgent.destination = targetPtr.position;
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
