using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDirector : MonoBehaviour {

    public Text Ez;
    public Text Easy;
    public Text Normal;
    public Text Hard;

    public float easiest_score = 0f;
    public float easy_score = 0f;
    public float normal_score = 0f;
    public float hard_score = 0f;

    // Use this for initialization
    void Start () {
        StartCoroutine(Update_Score());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator Update_Score()
    {
        yield return new WaitForSeconds(3f);

        Ez.text = "E-Z : " + easiest_score.ToString("F2");
        Easy.text = "Easy : " + easy_score.ToString("F2");
        Normal.text = "Normal : " + normal_score.ToString("F2");
        Hard.text = "Hard : " + hard_score.ToString("F2");
    }
}
