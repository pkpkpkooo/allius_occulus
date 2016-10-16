using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomData : MonoBehaviour{

	//외부 접근을 위해 public으로 선언 했지만 Inspector에 누출시키지 않음
	[HideInInspector]
	public string roomName = "";
	[HideInInspector]
	public int connectPlayer = 0;
	[HideInInspector]
	public int maxPlayers = 0;

	//룸 이름 표시할 Text UI 항목
	public Text textRoomName;
	//룸 접속자수와 최대접속자수 표시할 Text UI 항목
	public Text textConnectInfo;

	//룸정보 전달 후 Text UI 항목에 표시하는 함수
	public void DispRoomData()
	{
		textRoomName.text = roomName;
		textConnectInfo.text = "(" + connectPlayer.ToString() + "/" + maxPlayers.ToString() + ")";
	}
}
