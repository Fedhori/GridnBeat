using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Start_Btn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Score_Btn()
    {
        SceneManager.LoadScene("ScoreScene");
    }

    public void BackToMenu_Btn()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
