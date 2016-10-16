using UnityEngine;
using System.Collections;

public class LastDoorCtrl : MonoBehaviour
{
    public AudioClip doorSwishClip;                 // Clip to play when the doors open or close.
    public AudioClip accessDeniedClip;              // Clip to play when the player doesn't have the key for the door.
    public int lastDoorCnt;

    private Animator anim;                           // Reference to the animator component.
    private int cnt; // 1: open , 0:close


    public GameObject door;

    //void enableNavi()
    //{
    //    door.GetComponent<NavMeshObstacle>().enabled = false;
    //}

    //    StartCoroutine(this.EnableNavi());

    IEnumerator EnableNavi()
    {
        yield return new WaitForSeconds(0.08f);
        door.GetComponent<NavMeshObstacle>().enabled = false;
    }

    IEnumerator AbleNavi()
    {
        yield return new WaitForSeconds(0.08f);
        door.GetComponent<NavMeshObstacle>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PLAYER" && lastDoorCnt == 9)
        {
            // If the player doesn't have the key play the access denied audio clip.
            GetComponent<AudioSource>().clip = accessDeniedClip;
            GetComponent<AudioSource>().Play();
            StartCoroutine(this.EnableNavi());
            cnt++;
            Debug.Log(cnt);
            GetComponent<SphereCollider>().enabled = false;
        }

    }
    /*
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PLAYER")
        {
            // If the player doesn't have the key play the access denied audio clip.
            GetComponent<AudioSource>().clip = accessDeniedClip;
            GetComponent<AudioSource>().Play();
            StartCoroutine(this.AbleNavi());
            cnt--;
            Debug.Log(cnt);
        }

    }
    */
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        cnt = 0;
        lastDoorCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Open", cnt > 0);
    }
}
