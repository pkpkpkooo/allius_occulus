using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestGameMgr : MonoBehaviour
{

    //플레이어 접속정보 Text UI 항목 연결 변수
    public Text txtConnect;
    //접속로그를 표시할 Text UI 항목 연결 변수
    public Text txtLogMsg;
    //RPC호출을 위한 PhotonView
    private PhotonView pv;

    public GameObject check_point;
    public GameObject[] spawn_points;
    public GameObject[] storagePoints;
    public GameObject[] players;
    public GameObject[] monsters;
    public GameObject[] dMonsters;
    public GameObject[] zMonsters;
    public GameObject[] bMonsters;

    void Awake()
    {
        //PhotonView 컴포넌트 할당
        pv = GetComponent<PhotonView>();
        pv.observed = this;
        //포톤 클라우드의 네트워크 메시지 수신 다시 연결
        PhotonNetwork.isMessageQueueRunning = true;



        check_point = GameObject.FindGameObjectWithTag("CHECK_POINT_TAG");
        spawn_points = GameObject.FindGameObjectsWithTag("SPAWN_POINT_TAG");
        storagePoints = GameObject.FindGameObjectsWithTag("StoragePoint");
        players = GameObject.FindGameObjectsWithTag("PLAYER");


        

        //플레이어 생성 코루틴 함수 호출
        StartCoroutine(this.CreatePlayer());
        StartCoroutine(this.CreateMonster());
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

        for (int i = 0; i < storagePoints.Length; i++)
        {
            dMonsters[i].GetComponent<Transform>().position = storagePoints[i].GetComponent<Transform>().position;
            dMonsters[i].GetComponent<DogCtrl>().idx = i;
            dMonsters[i].GetComponent<NavMeshAgent>().enabled = false;
            storagePoints[i].GetComponent<StoragePointCtrl>().isFull = true;
            storagePoints[i].GetComponent<StoragePointCtrl>().monsterIdx = i;
            int rNum = (int)Random.Range(0.0f, players.Length);
            Debug.Log(rNum);
            dMonsters[i].GetComponent<DogCtrl>().targetPtr = null;
        }

    }

    //탱크 생성 코루틴 함수
    IEnumerator CreatePlayer()
    {
        float pos = 0.0f;
        //float pos = Random.Range(-100.0f, 100.0f);
        GameObject player = PhotonNetwork.Instantiate("NEWPLAYER", new Vector3(-6.7f, 1.33f, -1.39f), Quaternion.identity, 0);


        yield return null;
    }

    IEnumerator CreateMonster()
    {
        players = GameObject.FindGameObjectsWithTag("PLAYER");
        if (players[0].GetComponent<PhotonView>().viewID / 1000 == 1)
        {
            foreach (GameObject storagePoint in storagePoints)
            {
                GameObject monster;
                switch (storagePoint.GetComponent<StoragePointCtrl>().monsterType)
                {
                    case 2:
                        monster = PhotonNetwork.Instantiate("Infernal_Dog", storagePoint.GetComponent<Transform>().position, Quaternion.identity, 0);
                        storagePoint.GetComponent<StoragePointCtrl>().monsterType = 2;
                        break;
                        //case 3:
                        //    monster = PhotonNetwork.Instantiate("BossMonster", storagePoint.GetComponent<Transform>().position, Quaternion.identity, 0);
                        //    storagePoint.GetComponent<StoragePointCtrl>().monsterType = 3;
                        //    break;
                }
            }
            GameObject zombieMonster;
            zombieMonster = PhotonNetwork.Instantiate("zombie", new Vector3(-9, 1, 15), Quaternion.identity, 0);
            zombieMonster = PhotonNetwork.Instantiate("zombie", new Vector3(-47, 1, 25), Quaternion.identity, 0);
            zombieMonster = PhotonNetwork.Instantiate("zombie", new Vector3(-67, 1, 16), Quaternion.identity, 0);
            GameObject bossMonster;
            bossMonster = PhotonNetwork.Instantiate("BossMonster", new Vector3(55, 0, -20), Quaternion.identity, 0);
            bossMonster.GetComponent<BossMonsterCtrl>().idx = 0;
            bossMonster.GetComponent<NavMeshAgent>().enabled = false;
            bossMonster = PhotonNetwork.Instantiate("BossMonster", new Vector3(55, 0, -20), Quaternion.identity, 0);
            bossMonster.GetComponent<BossMonsterCtrl>().idx = 1;
            bossMonster.GetComponent<NavMeshAgent>().enabled = false;
            bossMonster = PhotonNetwork.Instantiate("BossMonster", new Vector3(55, 0, -20), Quaternion.identity, 0);
            bossMonster.GetComponent<BossMonsterCtrl>().idx = 2;
            bossMonster.GetComponent<NavMeshAgent>().enabled = false;
        }
        yield return null;
    }

    //룸 접속자 정보 조회 함수
    void GetConnectPlayerCount()
    {
        //현재 입장한 룸 정보를 받아옴
        Room currRoom = PhotonNetwork.room;

        //현재 룸의 접속자수와 최대 접속가능수 문자열 구성 후 Text UI 항목에 출력
        // txtConnect.text = currRoom.playerCount.ToString() 
        ///                     + "/" 
        //                    + currRoom.maxPlayers.ToString();
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
            //         int currKillCount = player.GetComponent<PhotonView>().owner.GetScore();
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
        PhotonNetwork.LeaveRoom();         // 다이하면 이거 그냥 실행 시키면됨
    }

    //룸에서 접속종료되었을 때 호출되는 콜백함수
    void OnLeftRoom()
    {
        //로비 씬을 호출
        Application.LoadLevel("testMenu");
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("PLAYER");
        //monsters = GameObject.FindGameObjectsWithTag ("Monster");
        dMonsters = GameObject.FindGameObjectsWithTag("DOGMONSTER");
        bMonsters = GameObject.FindGameObjectsWithTag("BOSSMONSTER");
        zMonsters = GameObject.FindGameObjectsWithTag("ZOMBIEMONSTER");

        //if (players.Length != 1)
        //    for (int i = 0; i < storagePoints.Length; i++)
        //    {
        //        //int rNum = i % players.Length;
        //        int idx = 0;

        //        if (players[0].GetComponent<PhotonView>().viewID > players[1].GetComponent<PhotonView>().viewID) // 내가 늦게들어오면
        //            dMonsters[i].GetComponent<DogCtrl>().targetPtr = players[(idx + i) % 2].GetComponent<Transform>();
        //        else // 남이 늦으면
        //            dMonsters[i].GetComponent<DogCtrl>().targetPtr = players[(idx + i) % 2].GetComponent<Transform>();
        //    }

        if (players.Length != 1)
            for (int i = 0; i < dMonsters.Length; i++)
            {
                int rNum = players[i % players.Length].GetComponent<PhotonView>().viewID / 1000 - 1;
                dMonsters[i].GetComponent<DogCtrl>().targetPtr = players[rNum].GetComponent<Transform>();
            }
        else
            for (int i = 0; i < dMonsters.Length; i++)
            {
                int rNum = players[i % players.Length].GetComponent<PhotonView>().viewID / 1000 - 1;
                dMonsters[i].GetComponent<DogCtrl>().targetPtr = players[0].GetComponent<Transform>();
            }
    }


}