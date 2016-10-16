using UnityEngine;
using System.Collections;

public class MiniMapCamCtrl : MonoBehaviour
{

    private GameObject miniMapCam;
    private GameObject CardboardMain;
    private GameObject stereoRender;
    private GameObject preRender;
    private GameObject postRender;
    private GameObject PlayerModel;
    private GameObject player_root;
    private GameObject Bip001;
    private GameObject Bip001_Spine;
    private GameObject mainCamera;

    private GameObject[] playerEyes;
    private Transform _transform;
    private PhotonView pv = null;
    public float f3 = 0.0f;

    // Use this for initialization
    void Awake()
    {

        pv = GetComponent<PhotonView>();
        _transform = GetComponent<Transform>();
        CardboardMain = _transform.GetChild(1).gameObject;
        stereoRender = CardboardMain.transform.GetChild(1).gameObject;
        preRender = stereoRender.transform.GetChild(0).gameObject;
        postRender = stereoRender.transform.GetChild(1).gameObject;

        miniMapCam = _transform.GetChild(4).gameObject;
        miniMapCam.GetComponent<FollowCam>().targetTr = _transform;
        miniMapCam.GetComponent<Camera>().enabled = false;

        if (pv.isMine)
        {
        }

        else
        {
            PlayerModel = _transform.GetChild(3).gameObject;
            player_root = PlayerModel.transform.GetChild(2).gameObject;
            Bip001 = player_root.transform.GetChild(0).gameObject;
            Bip001_Spine = Bip001.transform.GetChild(3).gameObject;
            mainCamera = Bip001_Spine.transform.GetChild(0).gameObject;

            mainCamera.GetComponent<Camera>().enabled = false;
            preRender.GetComponent<Camera>().enabled = false;
            postRender.GetComponent<Camera>().enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        playerEyes = GameObject.FindGameObjectsWithTag("PlayerEye");
        if (pv.isMine)
        {
            f3 = Input.GetAxis("Fire3");
            if (f3 >= 1.0f)
            {
                miniMapCam.GetComponent<Camera>().enabled = true;

                preRender.GetComponent<Camera>().enabled = false;
                postRender.GetComponent<Camera>().enabled = false;
                foreach (GameObject playerEye in playerEyes)
                {
                    playerEye.GetComponent<Camera>().enabled = false;
                }
            }
            else
            {
                preRender.GetComponent<Camera>().enabled = true;
                postRender.GetComponent<Camera>().enabled = true;


                PlayerModel = _transform.GetChild(3).gameObject;
                player_root = PlayerModel.transform.GetChild(2).gameObject;
                Bip001 = player_root.transform.GetChild(0).gameObject;
                Bip001_Spine = Bip001.transform.GetChild(3).gameObject;
                mainCamera = Bip001_Spine.transform.GetChild(0).gameObject;

                mainCamera.GetComponent<Camera>().enabled = true;

                miniMapCam.GetComponent<Camera>().enabled = false;
            }
        }
    }
}
