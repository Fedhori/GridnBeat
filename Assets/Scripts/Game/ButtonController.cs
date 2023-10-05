using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

    GameObject GameDirector;
    GameDirector GD;
    GameObject TileDirector;
    TileDirector TD;
    

	// Use this for initialization
	void Start () {
        GameDirector = GameObject.Find("GameDirector");
        GD = GameDirector.GetComponent<GameDirector>();
	}
	
	// Update is called once per frame
	void Update () {
		
	} 

    public void Score_Btn()
    {
        SceneManager.LoadScene("ScoreScene");
    }

    public void Menu_Btn()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Option_Btn()
    {
        GD.isPopUp = false;
        GD.PopUp.SetActive(false);
        // later
    }

    public void Exit_Btn()
    {
        Application.Quit();
    }

    public void Return_Btn()
    {
        GD.isPopUp = false;
        GD.PopUp.SetActive(false);
    }

    public void Origin_Btn()
    {
        if(!GD.isStartButNotReally || GD.Died)
        {
            if (PlayerPrefs.GetInt("ControlType") == 0)
            {
                GD.Origin.SetActive(true);
                PlayerPrefs.SetInt("ControlType", 1);
            }
            else
            {
                GD.Origin.SetActive(false);
                PlayerPrefs.SetInt("ControlType", 0);
            }
        }
    }

    public void Harder()
    {
        int Difficulty = PlayerPrefs.GetInt("Difficulty", 2);

        if (Difficulty == 0)
        {
            Difficulty++;
            PlayerPrefs.SetInt("Difficulty", Difficulty);
            GD.Easy();
        }
        else if (Difficulty == 1)
        {
            Difficulty++;
            PlayerPrefs.SetInt("Difficulty", Difficulty);
            GD.Normal();
        }
        else if (Difficulty == 2)
        {
            Difficulty++;
            PlayerPrefs.SetInt("Difficulty", Difficulty);
            GD.Hard();
        }
    }

    public void Easier()
    {
        int Difficulty = PlayerPrefs.GetInt("Difficulty", 2);

        if (Difficulty == 3)
        {
            Difficulty--;
            PlayerPrefs.SetInt("Difficulty", Difficulty);
            GD.Normal();
        }
        else if (Difficulty == 2)
        {
            Difficulty--;
            PlayerPrefs.SetInt("Difficulty", Difficulty);
            GD.Easy();
        }
        else if (Difficulty == 1)
        {
            Difficulty--;
            PlayerPrefs.SetInt("Difficulty", Difficulty);
            GD.VeryEasy();
        }
    }

    public void ShowPopUp()
    {
        GD.ShowPopUp();
    }
}
