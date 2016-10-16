using UnityEngine;
using System.Collections;

public class raycast_ctrl : MonoBehaviour
{
    public Camera c;
    public GameObject pointer;
    RaycastHit hit;
    public PhotonInit pi;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1") != 0)
        {
            Debug.DrawRay(GetComponent<Transform>().position, transform.forward, Color.yellow, 3.0f);
            //Debug.DrawLine(GetComponent<Transform>().position, c.transform.position, Color.yellow, 3.0f);

            if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
            {
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag.Equals("SINGLE"))
                {
                    //Application.LoadLevel("VR_Test02");
                    string _roomName = "";
                    string userId = "";
                    //룸 이름이 없거나 Null일 경우 룸 이름 지정
                    if (string.IsNullOrEmpty(_roomName))
                    {
                        _roomName = "ROOM_" + Random.Range(0, 999);
                    }

                    //로컬 플레이어의 이름을 설정
                    PhotonNetwork.player.name = userId;
                    //플레이어 이름 저장
                    PlayerPrefs.SetString("USER_ID", userId);

                    //생성할 룸의 조건 설정
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.isOpen = true;
                    roomOptions.isVisible = false;
                    roomOptions.maxPlayers = 1;

                    //지정한 조건에 맞는 룸 생성 함수
                    PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
                }
                else if (hit.collider.gameObject.tag == "MULTI")
                {
                    Application.LoadLevel("testMultiPlay");
                }
                else if (hit.collider.gameObject.tag.Equals("QUIT"))
                {
                    Application.Quit();
                }
                else if (hit.collider.gameObject.tag.Equals("MAKE_ROOM"))
                {
                    pi.OnClickCreateRoom();
                }
                else if (hit.collider.gameObject.tag.Equals("RANDOM_JOIN_ROOM"))
                {
                    pi.OnClickJoinRandomRoom();
                }
                else if (hit.collider.gameObject.tag.Equals("ROOM_ITEM"))
                {
                    pi.OnClickRoomItem(pi.rm);
                }
            }
        }
    }

}