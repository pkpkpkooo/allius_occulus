using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhotonInit : MonoBehaviour
{

    //App의 버전정보
    public string version = "v1.0";

    //플레이어 이름 입력필드 UI 항목 연결
    public InputField userId;
    //룸 이름 입력필드 UI 항목 연결
    public InputField roomName;
    //RoomItem이 차일드로 생성될 Parent 객체
    public GameObject scrollContents;
    //룸 목록만큼 생성될 RoomItem 프리팹
    public GameObject roomItem;
    public string rm;
    void Awake()
    {
        if (!PhotonNetwork.connected)
        {
            //포톤 클라우드에 접속
            PhotonNetwork.ConnectUsingSettings(version);
        }

        //사용자 이름 설정
        userId.text = GetUserId();

        //룸 이름 무작위 명칭으로 설정
        roomName.text = "ROOM_" + Random.Range(0, 999).ToString();
    }

    //포톤 클라우드에 정상 접속 후 로비에 입장하면 호출되는 콜백함수
    void OnJoinedLobby()
    {
        Debug.Log("Entered Lobby !");
        userId.text = GetUserId();

        //삭제하거나 주석처리
        //PhotonNetwork.JoinRandomRoom ();
    }

    //로컬에 저장된 플레이어 이름을 반환하거나 생성하는 함수
    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID");

        if (string.IsNullOrEmpty(userId))
        {
            userId = "USER_" + Random.Range(0, 999);
        }
        return userId;
    }


    //무작위 룸 접속에 실패한 경우 호출되는 콜백함수
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("No rooms !");
        //룸 생성
        RoomOptions ro = new RoomOptions() { isVisible = true, maxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom(roomName.text, ro, TypedLobby.Default);
        //PhotonNetwork.CreateRoom("MyRoom", true, true, 20);
    }

    //룸에 입장하면 호출되는 콜백함수
    void OnJoinedRoom()
    {
        Debug.Log("Enter Room");
        //룸 씬으로 이동하는 코루틴 실행
        StartCoroutine(this.LoadBattleField());
    }

    //룸 씬으로 이동하는 코루틴 함수
    IEnumerator LoadBattleField()
    {
        //씬을 이동하는 동안 포톤 클라우드 서버로부터 네트워크 메시지 수신 중단
        PhotonNetwork.isMessageQueueRunning = false;
        //씬 로딩
        Application.LoadLevel("StealthMap");
        yield return null;
    }

    //Join Random Room 버튼 클릭시 호출되는 함수
    public void OnClickJoinRandomRoom()
    {
        //로컬 플레이어의 이름을 설정
        PhotonNetwork.player.name = userId.text;
        //플레이어 이름 저장
        PlayerPrefs.SetString("USER_ID", userId.text);

        //무작위 추출된 룸으로 입장
        PhotonNetwork.JoinRandomRoom();
    }

    //Make Room 버튼 클릭시 호출될 함수
    public void OnClickCreateRoom()
    {
        string _roomName = roomName.text;
        //룸 이름이 없거나 Null일 경우 룸 이름 지정
        if (string.IsNullOrEmpty(roomName.text))
        {
            _roomName = "ROOM_" + Random.Range(0, 999);
        }

        //로컬 플레이어의 이름을 설정
        PhotonNetwork.player.name = userId.text;
        //플레이어 이름 저장
        PlayerPrefs.SetString("USER_ID", userId.text);

        //생성할 룸의 조건 설정
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.isOpen = true;
        roomOptions.isVisible = true;
        roomOptions.maxPlayers = 2;

        //지정한 조건에 맞는 룸 생성 함수
        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    //룸 생성 실패할때 호출되는 콜백함수
    void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Create Room Failed = " + codeAndMsg[1]);
    }

    //생성된 룸 목록의 변경시 호출되는 콜백함수
    public void OnReceivedRoomListUpdate()
    {
        //룸 목록을 다시 받았을 때 갱신하기위해 기존에 생성된 RoomItem을 삭제
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM_ITEM"))
        {
            Destroy(obj);
        }

        //Grid Layout Group 컴포넌트의 Constraint Count 값을 증가시킬 변수 
        int rowCount = 0;
        //스크롤영역 초기화
        scrollContents.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        //수신받은 룸 목록의 정보로 RoomItem을 생성
        foreach (RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            Debug.Log(_room.name);
            //RoomItem 프리팹을 동적으로 생성
            GameObject room = (GameObject)Instantiate(roomItem);
            room.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 9.0f);
            room.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.7f);
            //room.GetComponent<RectTransform> ().sizeDelta = new Vector2 (300.0f, 70.0f);
            room.transform.SetParent(scrollContents.transform, false);
            //생성한 RoomItem에 표시하기 위한 텍스트 정보 전달
            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.name;
            roomData.connectPlayer = _room.playerCount;
            roomData.maxPlayers = _room.maxPlayers;
            //텍스트 정보를 표시
            roomData.DispRoomData();
            //RoomItem의 Button 컴포넌트에 클릭이벤트를 동적으로 연결
            rm = _room.name;
            roomData.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { OnClickRoomItem(_room.name); });

            //Grid Layout Group 컴포넌트의 Constraint Count 값을 증가시킴
            scrollContents.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;
            //스크롤영역의 높이를 증가시킴
            scrollContents.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 200);
        }
    }

    //RoomItem이 클릭되면 호출될 이벤트 연결함수
    public void OnClickRoomItem(string roomName)
    {
        //로컬 플레이어의 이름을 설정
        PhotonNetwork.player.name = userId.text;
        //플레이어 이름 저장
        PlayerPrefs.SetString("USER_ID", userId.text);

        //인자로 전달된 룸 이름의 룸으로 입장
        PhotonNetwork.JoinRoom(roomName);
    }

    void OnGUI()
    {
        //화면 좌측상단에 접속과정 로그를 출력
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
