using UnityEngine;
using System.Collections;

[System.Serializable]
public class Anim
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runRight;
    public AnimationClip runLeft;
}

public class PlayerCtrl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private float x = 0.0f;
    private float xx;
    private float yy;
    private float zz;
    private bool move = true;
    public int player_hp = 105;
    public GameObject hit_view;
    public float moveSpeed = 0.1f;
    public float rotSpeed = 100.0f;
    public Transform cp;
    public Anim anim;
    public Animation _animatioin;
    public GameObject hit_light;

    private PhotonView pv = null;
    private Transform _transform;
    private Rigidbody _rigidbody;
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;
    private Quaternion r = Quaternion.identity;
    private GameObject miniMapCam;
    public GameObject[] players;

    public GameObject Bip001_Spine;
    private Color c;
    private float oPlayerH;
    private float oPlayerV;
    private float oPlayerX;
    private Quaternion oPlayerR;
    public GameObject gameOver;
    public GameObject head;
    public GameObject gun;

    // Use this for initialization
    void Start()
    {
        c = hit_view.GetComponent<MeshRenderer>().sharedMaterial.color;
        hit_view.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(c.r, c.g, c.b, 0);
        _transform = GetComponent<Transform>();
        //Player 하위의 PlayerModel의 컴포넌트에 접근해야하기때문에 GetComponentInChildren
        _animatioin = GetComponentInChildren<Animation>();
        _animatioin.clip = anim.idle;
        _animatioin.Play();
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();

        miniMapCam = _transform.GetChild(4).gameObject;
        GameObject PlayerModel = _transform.GetChild(3).gameObject;
        GameObject player_root = PlayerModel.GetComponent<Transform>().GetChild(2).gameObject;
        GameObject bip001 = player_root.GetComponent<Transform>().GetChild(0).gameObject;
        Bip001_Spine = bip001.GetComponent<Transform>().GetChild(3).gameObject;

        //PhotonView 컴포넌트 할당
        pv = GetComponent<PhotonView>();
        //PhotonView Observed 속성에 TankMove 스크립트를 연결
        pv.observed = this;
        //PhotonView 가 자신의 탱크일 경우
        if (pv.isMine)
        {
            //플레이어의 중심 설정
            _rigidbody.centerOfMass = new Vector3(0.0f, -0.5f, 0.0f);
        }
        else
        {
            _rigidbody.isKinematic = true;
            //PlayerModel.GetComponent<Animation>().enabled = false;
        }

        //원격 탱크의 위치 및 회전 값을 처리 할 변수의 초깃값 설정
        currPos = _transform.position;
        currRot = _transform.rotation;
        //oPlayerR = Bip001_Spine.GetComponent<Transform>().rotation;
        //oPlayerR = head.GetComponent<CardboardHead>().sendRot;
        oPlayerH = h;
        oPlayerV = v;
        oPlayerX = x;
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("PLAYER");
        if (pv.isMine)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            MoveObject();
        }
        else //원격 플레이어일 때 수행
        {
            _transform.position = Vector3.Lerp(_transform.position, currPos, Time.deltaTime);
            showAnimation(oPlayerV, oPlayerH);
            Bip001_Spine.transform.rotation = Quaternion.Euler(zz, yy - 90, -xx -90);
        }

    }

    void MoveObject()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        x = Input.GetAxis("Mouse X");
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        // Time.deltaTime 1초동안 이동으로바뀜 프레임이높으면 더 부드럽게 움직임, Space.Self 로컬좌표이동
        _transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);
        //_transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed * x);
        showAnimation(v, h);
    }

    void showAnimation(float v, float h)
    {
        if (v >= 0.1f)
        {
            _animatioin.CrossFade(anim.runForward.name, 0.3f);
        }
        else if (v <= -0.1f)
        {
            _animatioin.CrossFade(anim.runBackward.name, 0.3f);
        }
        else if (h >= 0.1f)
        {
            _animatioin.CrossFade(anim.runRight.name, 0.3f);
        }
        else if (h <= -0.1f)
        {
            _animatioin.CrossFade(anim.runLeft.name, 0.3f);
        }
        else
        {
            _animatioin.CrossFade(anim.idle.name, 0.3f);
        }
    }


    public void hit(int dmg)
    {
        c = hit_view.GetComponent<MeshRenderer>().sharedMaterial.color;
        hit_view.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(c.r, c.g, c.b, 1.0f);
        hit_light.GetComponent<Light>().intensity = 1;
        player_hp -= dmg;
        StartCoroutine(recover_view());
        if (player_hp < 0)
        {
            for(int idx=0; idx<players.Length; idx++)
            {

                players[idx].transform.Translate(new Vector3(-37f, 2f, -6f));
                players[idx].GetComponent<PlayerCtrl>().gameOver.SetActive(true);
                gun.SetActive(true);
                this.enabled = false;
                StartCoroutine(player_dead());
            }
        }
    }

    IEnumerator recover_view()
    {
        while (hit_view.GetComponent<Renderer>().material.color.a > 0)
        {
            hit_view.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(c.r, c.g, c.b, hit_view.GetComponent<MeshRenderer>().sharedMaterial.color.a - 0.1f);
            hit_light.GetComponent<Light>().intensity -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator player_dead()
    {
        yield return new WaitForSeconds(10f);
        pv.RPC("leaveRoom", PhotonTargets.AllBuffered);

        //PhotonNetwork.LeaveRoom();
        gameOver.SetActive(false);
    }

    [PunRPC]
    void leaveRoom()
    {
        Debug.Log("들어왔어용");
        PhotonNetwork.LeaveRoom();
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //로컬 플레이어의 위치 정보 송신
        if (stream.isWriting)
        {
            stream.SendNext(_transform.position);
            stream.SendNext(_transform.rotation);
            stream.SendNext(h);
            stream.SendNext(v);
            stream.SendNext(head.GetComponent<CardboardHead>().sendRot.eulerAngles.x);
            stream.SendNext(head.GetComponent<CardboardHead>().sendRot.eulerAngles.y);
            stream.SendNext(head.GetComponent<CardboardHead>().sendRot.eulerAngles.z);
            stream.SendNext(player_hp);
        }
        else //원격 플레이어의 위치 정보 수신
        {

            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            oPlayerH = (float)stream.ReceiveNext();
            oPlayerV = (float)stream.ReceiveNext();
            xx = (float)stream.ReceiveNext();
            yy = (float)stream.ReceiveNext();
            zz = (float)stream.ReceiveNext();
            player_hp = (int)stream.ReceiveNext();
            //oPlayerR = (Quaternion)stream.ReceiveNext();
            //Debug.Log ("get " + oPlayerR);

        }
    }
}