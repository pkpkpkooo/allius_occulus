using UnityEngine;
using System.Collections;

public class MonsterCtrl : MonoBehaviour {
	public enum MonsterState{idle, trace, attack, die, hit};
	public MonsterState monsterState = MonsterState.idle;
	public float traceDist = 20.0f;
	public float attackDist = 2.0f;
	public int hp = 1;
	public int ap = 10;
	public AudioClip attack_snd;
	public AudioClip attack2_snd;
	public AudioClip trace_snd;
	public AudioSource audio;

	private bool isDie = false;
	private GameObject player;
	private Transform tr;
	private Transform ptr;
	private Rigidbody rig;
    private NavMeshAgent nvAgent;
	private Animator animator;

	IEnumerator CheckMonsterState(){
		while (!isDie) {
			yield return new WaitForSeconds (0.2f);
			float dist = Vector3.Distance (ptr.position, tr.position);
			//Debug.Log (dist);
			if (monsterState != MonsterState.hit) {
				if (dist <= attackDist) {
					nvAgent.Stop ();
					monsterState = MonsterState.attack;
				}
				else if (dist <= traceDist && dist > attackDist)
					monsterState = MonsterState.trace;
				else if (dist > traceDist)
					monsterState = MonsterState.idle;
			}
		}
	}

	IEnumerator MonsterAction(){
		while (!isDie) {
			switch (monsterState) {
			case MonsterState.idle:
				nvAgent.Stop ();
				animator.SetBool ("isTrace", false);
				break;
			case MonsterState.trace:
				animator.SetBool ("isTrace", true);
				animator.SetBool ("isAttack", false);
				nvAgent.Resume ();
				break;
			case MonsterState.attack:
				nvAgent.Stop ();
				animator.SetBool ("isAttack", true);
				animator.SetBool ("isTrace", false);
				//GetComponent<SphereCollider>().
				break;
			case MonsterState.hit:
				nvAgent.Stop ();
				animator.SetTrigger ("isHit");
				animator.SetBool ("isAttack", false);
				animator.SetBool ("isTrace", false);
				tr.Translate (Vector3.back * 0.02f);
				break;

			}
			yield return null;
		}
		animator.SetBool ("isDie", true);
	}
	void trace(){
		
	}
	//void idle(){nvAgent.Stop ();}
	void attack3(){	player.GetComponent<PlayerCtrl> ().hit (ap*2);}
	void attack2(){	player.GetComponent<PlayerCtrl> ().hit (ap/2);}
	void attack(){	player.GetComponent<PlayerCtrl> ().hit (ap);}
	void atkSnd(){
		audio.clip = attack_snd;
		audio.Play ();
	}
	void atkSnd2(){
		audio.clip = attack2_snd;
		audio.Play ();
	}
	void traceSnd(){
		audio.clip = trace_snd;
		audio.Play ();
	}
	void die(){
		animator.Stop ();
		Destroy (gameObject,1f);
	}
	void hitEnd(){
		animator.ResetTrigger ("isHit");
		monsterState = MonsterState.idle;
	}
	// Use this for initialization
	void OnCollisionEnter(Collision coll){
		if (coll.collider.tag == "BULLET") {
			monsterState = MonsterState.hit;
			nvAgent.Stop ();
			hp--;
			Destroy (coll.gameObject);
		}
		if (hp <= 0) {
			isDie = true;
			nvAgent.Stop ();
			GetComponent<CapsuleCollider> ().isTrigger = true;
		}
	}
	void Start () {
		tr = GetComponent<Transform> ();
		rig = GetComponent<Rigidbody> ();
		player = GameObject.FindGameObjectWithTag ("PLAYER");
		ptr = player.GetComponent<Transform> ();
        nvAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();
		StartCoroutine (CheckMonsterState ());
		StartCoroutine (MonsterAction ());
	}
	
	// Update is called once per frame
	void Update () {
		animator.ResetTrigger ("isHit");
		tr.LookAt (ptr);
		nvAgent.destination = ptr.position;

	}
}
