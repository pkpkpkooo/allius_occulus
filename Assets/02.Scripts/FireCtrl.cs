using UnityEngine;
using System.Collections;
//필요한 컴포넌트가 없는경우에는 실행시 문제발생을 알린다
[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;
    public AudioClip fireSfx;
    private AudioSource source = null;
    public MeshRenderer muzzleFlash;
    public float delay = 0.00f;
    private float tick;
    private PhotonView pv = null;

    float fire = 0.0f;
    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        muzzleFlash.enabled = false;
        pv = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        fire = Input.GetAxis("Fire1");
        //if (pv.isMine && Input.GetMouseButtonDown(0))
        if (pv.isMine && fire>=1.0f && Time.time > tick)
        {
            tick = Time.time + delay;
            Fire();
            pv.RPC("Fire", PhotonTargets.Others, null);
        }
    }
    [PunRPC]
    void Fire()
    {
        StartCoroutine(this.CreateBullet());
        source.PlayOneShot(fireSfx, 0.9f);
        StartCoroutine(this.ShowMuzzleFlash());
    }

    IEnumerator CreateBullet()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(bullet, new Vector3(firePos.position.x + Random.Range(-1.0f, 1.0f), firePos.position.y + Random.Range(-1.0f, 1.0f), firePos.position.z), firePos.rotation);
        }
        yield return null;
    }
    IEnumerator ShowMuzzleFlash()
    {
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;
        Quaternion rot = Quaternion.Euler(-50, -50, Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;
        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        muzzleFlash.enabled = false;
    }
}
