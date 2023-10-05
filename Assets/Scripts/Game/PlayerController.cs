using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Vector3 mouseDownPos;
    Vector3 mouseUpPos;

    GameObject TileDirector;
    TileDirector TD;
    GameObject GameDirector;
    GameDirector GD;

    public GameObject Player;

    public Sprite[] Player_Color = new Sprite[10];

    float StartPos_x;
    float StartPos_y;

    float Pos_x;
    float Pos_y;

    int dir;
    int[] dx = { 1, 0, -1, 0 };
    int[] dy = { 0, -1, 0, 1 };

    int next_x;
    int next_y;

    bool MoveFlag = false;

    float cycle = 0.08f;
    float time = 0f;

    // Use this for initialization
    void Start () {
        GameDirector = GameObject.Find("GameDirector");
        GD = GameDirector.GetComponent<GameDirector>();
        TileDirector = GameObject.Find("TileDirector");
        TD = TileDirector.GetComponent<TileDirector>();
        Player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {

        if (MoveFlag && time<cycle && !GD.Died)
        {
            time += Time.deltaTime;
            Pos_x = StartPos_x + dx[dir] * time/cycle * 192f;
            Pos_y = StartPos_y + dy[dir] * time/cycle * 192f;
            transform.position = new Vector3(Pos_x, Pos_y, 0);
        }
        else if(time > cycle && !GD.Died)
        {
            transform.position = transform.position = 
                new Vector3(next_x * 192f, next_y * 192f, 0);
            MoveFlag = false;
            time = 0f;
        }

        if (GD.isStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseDownPos = Input.mousePosition;
                if(PlayerPrefs.GetInt("ControlType") == 1)
                {
                    StartPos_x = transform.position.x;
                    StartPos_y = transform.position.y;

                    float x = Input.mousePosition.x - PlayerPrefs.GetFloat("Origin_x");
                    float y = Input.mousePosition.y - PlayerPrefs.GetFloat("Origin_y");
                    if (x > y)
                    {
                        // right side
                        if(x >= -y)
                        {
                            dir = 0;
                        }
                        // down side
                        else
                        {
                            dir = 1;
                        }
                    }
                    else
                    {
                        // left side
                        if(x <= -y)
                        {
                            dir = 2;
                        }
                        // upper side
                        else
                        {
                            dir = 3;
                        }
                    }

                    if (TD.isflip)
                    {
                        dir = (dir + 2) % 4;
                    }

                    if (TD.isTile[next_x + 1 + dx[dir], next_y + 1 + dy[dir]])
                    {
                        next_x += dx[dir];
                        next_y += dy[dir];
                        MoveFlag = true;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if ((Input.mousePosition - mouseDownPos).magnitude >= 64f && 
                    !MoveFlag && !GD.Died && PlayerPrefs.GetInt("ControlType") == 0)
                {
                    StartPos_x = transform.position.x;
                    StartPos_y = transform.position.y;

                    if (Mathf.Abs((Input.mousePosition - mouseDownPos).x) >=
                        Mathf.Abs((Input.mousePosition - mouseDownPos).y))
                    {
                        if ((Input.mousePosition - mouseDownPos).x > 0)
                        {
                            dir = 0;
                        }
                        else
                        {
                            dir = 2;
                        }
                    }
                    else
                    {
                        if ((Input.mousePosition - mouseDownPos).y > 0)
                        {
                            dir = 3;
                        }
                        else
                        {
                            dir = 1;
                        }
                    }

                    if (TD.isflip)
                    {
                        dir = (dir + 2) % 4;
                    }

                    if (TD.isTile[next_x + 1 +dx[dir], next_y +1 +dy[dir]])
                    {
                        next_x += dx[dir];
                        next_y += dy[dir];
                        MoveFlag = true;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "box")
        {
            GetComponent<SpriteRenderer>().sprite = Player_Color[1];
            GD.GameOver();
        }
    }
}
