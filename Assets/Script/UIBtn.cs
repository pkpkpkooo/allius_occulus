using UnityEngine;
using System.Collections;

public class UIBtn : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}

	public void multiBtn()
	{
		Application.LoadLevel("testMultiPlay");
	}

	public void singleBtn()
	{
		Application.LoadLevel("VR_Test02");
	}

	public void exitBtn()
	{
		Application.Quit();
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
