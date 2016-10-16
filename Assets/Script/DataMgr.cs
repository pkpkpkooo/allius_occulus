using UnityEngine;
using System.Collections;
using SimpleJSON;

public class DataMgr : MonoBehaviour {
    //MySQL DB사용을 위한 독자여러분에게 부여된 고유번호
    private const string seqNo = "3201411232";
    //점수저장 PHP 주소
    private string urlSave = "http://www.Unity3dStudy.com/Tankwar/save_score.php";
    //랭킹정보 요청 PHP 주소
    private string urlScoreList = "http://www.Unity3dStudy.com/Tankwar/get_score_list.php";

    //점수저장 코루틴 함수
    public IEnumerator SaveScore( string user_name, int killCount )
    {
        //POST 방식으로 인자를 전달하기 위한 FORM 선언
        WWWForm form = new WWWForm();
        form.AddField("user_name", user_name);
        form.AddField("kill_count", killCount);
        form.AddField("seq_no", seqNo);

        //URL 호출
        var www = new WWW (urlSave, form);
        //완료시점까지 대기
        yield return www;

        if ( string.IsNullOrEmpty(www.error) ){
            Debug.Log (www.text);
        }else{
            Debug.Log ("Error : " + www.error );
        }

        //점수 저장 후 랭킹정보 요청 코루틴 함수 호출
        StartCoroutine(this.GetScoreList());
    }

    //랭킹정보를 요청하는 코루틴 함수
    public IEnumerator GetScoreList()
    {
        //POST 방식으로 인자를 전달하기 위한 FORM 선언
        WWWForm form = new WWWForm();
        form.AddField("seq_no", seqNo);

        //URL 호출
        var www = new WWW (urlScoreList, form);
        //완료시점까지 대기
        yield return www;

        if ( string.IsNullOrEmpty(www.error) ){
            Debug.Log (www.text);
            //점수 표시 함수호출
            DispScoreList(www.text);
        }else{
            Debug.Log ("Error : " + www.error );
        }
    }

    //JSON 파일 파싱 후 점수 표시하는 함수
    void DispScoreList(string strJsonData)
    {
        //JSON 파일 파싱
        var N = JSON.Parse(strJsonData);

        //JSON 오브젝트 배열 만큼 순환
        for(int i=0; i< N.Count; i++)
        {
            int ranking     = N[i]["ranking"].AsInt;
            string userName = N[i]["user_name"].ToString ();
            int killCount   = N[i]["kill_count"].AsInt;
            //결과값 Console 뷰에 표시
            Debug.Log (ranking.ToString() + userName + killCount.ToString());
        }
    }
}
