using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMgr : MonoBehaviour
{

    //플레이어 접속정보 Text UI 항목 연결 변수
    public Text txtConnect;
    //접속로그를 표시할 Text UI 항목 연결 변수
    public Text txtLogMsg;
    //RPC호출을 위한 PhotonView
    private PhotonView pv;

    public GameObject check_point;
    public GameObject[] spawn_points;
    public GameObject[] playerss;

    void Awake()
    {
        //PhotonView 컴포넌트 할당
        pv = GetComponent<PhotonView>();

        //포톤 클라우드의 네트워크 메시지 수신 다시 연결
        PhotonNetwork.isMessageQueueRunning = true;

        //플레이어 생성 코루틴 함수 호출
        StartCoroutine(this.CreatePlayer());
        //룸에 입장 후 기존 접속자 정보를 출력
        GetConnectPlayerCount();
    }

    IEnumerator Start()
    {
        //룸에 있는 네트워크 객체간의 통신이 완료 될때까지 잠시 대기
        yield return new WaitForSeconds(1.0f);
        //모든 탱크의 스코어 UI에 점수표시하는 함수 호출
        SetConnectPlayerScore();

        //로그 메시지에 출력할 문자열 생성
        string msg = "\n<color=#00ff00>[" + PhotonNetwork.player.name + "] Connected</color>";
        //RPC 함수 호출
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);

        check_point = GameObject.FindGameObjectWithTag("CHECK_POINT_TAG");
        spawn_points = GameObject.FindGameObjectsWithTag("SPAWN_POINT_TAG");
    }

    //탱크 생성 코루틴 함수
    IEnumerator CreatePlayer()
    {
        //float pos = Random.Range(-100.0f, 100.0f);
        GameObject player = PhotonNetwork.Instantiate("PLAYER", new Vector3(-4.0f, 2.0f, 5.0f), Quaternion.identity, 0);
        //GameObject player = PhotonNetwork.Instantiate("PLAYER", new Vector3(40.0f, 5.0f, 6.0f), Quaternion.identity, 0);

        yield return null;
    }

    //룸 접속자 정보 조회 함수
    void GetConnectPlayerCount()
    {
        //현재 입장한 룸 정보를 받아옴
        Room currRoom = PhotonNetwork.room;

        //현재 룸의 접속자수와 최대 접속가능수 문자열 구성 후 Text UI 항목에 출력
        // txtConnect.text = currRoom.playerCount.ToString() 
        //                      + "/" 
        //                     + currRoom.maxPlayers.ToString();
    }

    //모든 탱크의 스코어 UI에 점수표시하는 함수 호출
    void SetConnectPlayerScore()
    {
        //현재 입장한 룸에 접속한 모든 네트워크 플레이어 정보를 저장
        PhotonPlayer[] players = PhotonNetwork.playerList;
        foreach (PhotonPlayer _player in players)
        {
            Debug.Log("[" + _player.ID + "]" + _player.name + " " + _player.GetScore() + " kill");
        }

        //모든 Tank 프리팹을 배열에 저장
        GameObject[] player2 = GameObject.FindGameObjectsWithTag("PLAYER"); // 이거 이ㅁ바꾸자 캐릭터건 뭐ㄴ !!!!

        foreach (GameObject player in player2)
        {
            //각 Tank별 스코어를 조회
            int currKillCount = player.GetComponent<PhotonView>().owner.GetScore();
            //해당 Tank의 UI에 스코어 표시
            //  tank.GetComponent<TankDamage>().txtKillCount.text = currKillCount.ToString(); 이거에 대미지 입었으을 때 로직 넣어야되 
        }
    }

    //네트워크 플레이어가 접속했을 때 호출되는 함수
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log(newPlayer.ToStringFull());
        GetConnectPlayerCount();
    }

    //네트워크 플레이어가 룸을 나가거나 접속이 끊어졌을 때 호출되는 함수
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer)
    {
        GetConnectPlayerCount();
    }

    [PunRPC]
    void LogMsg(string msg)
    {
        //로그 메시지 Text UI에 텍스트를 누적시켜 표시
        //        txtLogMsg.text = txtLogMsg.text + msg;
    }


    //룸 나가기 버튼 클릭 이벤트에 연결될 함수
    public void OnClickExitRoom()
    {
        //로그 메시지에 출력할 문자열 생성
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.player.name + "] Disconnected</color>";
        //RPC 함수 호출
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);

        //현재 룸을 빠져나가며 생성한 모든 네트워크 오브젝트를 삭제
        PhotonNetwork.LeaveRoom();
    }

    //룸에서 접속종료되었을 때 호출되는 콜백함수
    void OnLeftRoom()
    {
        //로비 씬을 호출
        Application.LoadLevel("testMultiplay");
    }

    void Update()
    {
        foreach (GameObject spawn_point in spawn_points)
        {
            if(check_point != null)
            {
                if (check_point.GetComponent<CheckPointCtrl>().spawn == 1)
                {
                    if (spawn_point != null)
                        spawn_point.GetComponent<SpawnPointCtrl>().make_monster();
                }
            }
        }

        playerss = GameObject.FindGameObjectsWithTag("PLAYER");
    }

}
