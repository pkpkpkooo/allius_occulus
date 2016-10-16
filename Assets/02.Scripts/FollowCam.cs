using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	public Transform targetTr;
	public float dist = 10.0f;
	public float height = 3.0f;
	public float dampTrace = 10.0f;
	private Transform tr;

	// Use this for initialization
	void Start () {
		tr = GetComponent<Transform> ();
	}
	
	// 해당 씬의 다른모든 오브젝트의 업데이트 후 실행
	void LateUpdate () {
		// Lerp 이동시 보간법사용 회전시는 Slerp

		tr.position = Vector3.Lerp (tr.position, targetTr.position - (targetTr.forward * dist) + (Vector3.up * height), Time.deltaTime * dampTrace);
		tr.LookAt (targetTr.position);
	}
}
