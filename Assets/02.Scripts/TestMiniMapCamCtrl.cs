using UnityEngine;
using System.Collections;

public class TestMiniMapCamCtrl : MonoBehaviour
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
	private GameObject mainCameraLeft;
	private GameObject mainCameraRight;
	private GameObject stateQuad;
	private GameObject main_player_lorez;

    private GameObject[] playerEyes;
    private Transform _transform;
    private PhotonView pv = null;
	public float f3 = 0.0f;
	private string[] layers = new string[6]{"Default","TransparentFX", "Ignore Raycast", "Water", "UI", "miniMap" };

    // Use this for initialization
    void Awake()
    {

        pv = GetComponent<PhotonView>();
        _transform = GetComponent<Transform>();
        CardboardMain = _transform.GetChild(1).gameObject;
        stereoRender = CardboardMain.transform.GetChild(1).gameObject;
        preRender = stereoRender.transform.GetChild(0).gameObject;
        postRender = stereoRender.transform.GetChild(1).gameObject;
		stateQuad = _transform.GetChild(6).gameObject;
		stateQuad.SetActive (false); // 일단 안보이게 설정

        miniMapCam = _transform.GetChild(4).gameObject;
        miniMapCam.GetComponent<FollowCam>().targetTr = _transform;
        miniMapCam.GetComponent<Camera>().enabled = false;



		if (pv.isMine){
			PlayerModel = _transform.GetChild(3).gameObject;
			player_root = PlayerModel.transform.GetChild(2).gameObject;
			Bip001 = player_root.transform.GetChild(0).gameObject;
			Bip001_Spine = Bip001.transform.GetChild(3).gameObject;
			mainCamera = Bip001_Spine.transform.GetChild(0).gameObject;
			mainCameraLeft = mainCamera.transform.GetChild (0).gameObject;
			mainCameraRight = mainCamera.transform.GetChild (1).gameObject;

			mainCamera.GetComponent<Camera> ().cullingMask = LayerMask.GetMask (layers); 
			mainCameraLeft.GetComponent<Camera> ().cullingMask = LayerMask.GetMask (layers); 
			mainCameraRight.GetComponent<Camera> ().cullingMask = LayerMask.GetMask (layers); 

			main_player_lorez = PlayerModel.transform.GetChild (1).gameObject;
			main_player_lorez.layer = LayerMask.NameToLayer("invisible");
        }

        else
        {
            PlayerModel = _transform.GetChild(3).gameObject;
            player_root = PlayerModel.transform.GetChild(2).gameObject;
            Bip001 = player_root.transform.GetChild(0).gameObject;
            Bip001_Spine = Bip001.transform.GetChild(3).gameObject;
            mainCamera = Bip001_Spine.transform.GetChild(0).gameObject;
            CardboardMain.GetComponent<Cardboard>().enabled = false;
            CardboardMain.SetActive (false);
			mainCamera.SetActive (false);
            mainCamera.GetComponent<Camera>().enabled = false;
            preRender.GetComponent<Camera>().enabled = false;
            postRender.GetComponent<Camera>().enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (pv.isMine)
        {
            f3 = Input.GetAxis("Fire3");
            if (f3 >= 1.0f)
            {
                miniMapCam.GetComponent<Camera>().enabled = true;
				stateQuad.SetActive (true);


            }
            else
            {
				miniMapCam.GetComponent<Camera>().enabled = false;
				stateQuad.SetActive (false);
            }
        }
    }
}

