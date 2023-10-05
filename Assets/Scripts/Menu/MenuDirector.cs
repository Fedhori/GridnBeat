using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDirector : MonoBehaviour {

    public Camera camera;

    float cycle = 5/6f;
    float time = 5/6f-1/5f;
    

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time > cycle)
        {
            time = 0;
        }
        else if (time > cycle*0.9f)
        {
            camera.orthographicSize = 1200f * time / cycle;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
