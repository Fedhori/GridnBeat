using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileDirector : MonoBehaviour {

    public Camera camera;

    bool isinvisible = false;
    bool isfake = false;
    public bool isflip = false;
    bool isEnd = false; // determine pattern is endtime or not.
    bool already = false;
    bool alreadyPatternAppeared = false;

    public int TileSize_x = 2;
    public int TileSize_y = 2;

    // 0 is 3*3
    // 1 is cross
    int Pattern_Code = 0;

    // 패턴 위치
    int Ptn_x = 1;
    int Ptn_y = 1;

    int BeforeTurn = 0;
    int turn = 0;

    int Cross_seed;

    int invisible_length;
    int invisible_prob = 8; // 8/64

    int fake_length;
    int fake_prob = 4;

    int flip_length;
    int flip_prob = 8; // 8/64

    public int Rotate_seed = 1;

    int[] dx = { 1, 0, -1, 0, 0 };
    int[] dy = { 0, -1, 0, 1, 0 };

    /*
    public float cycle = 2979f/3500f;
    float time = 1579f/7000f;
    */

    public int start_bpm;
    int speed_stack = 0;

    public float originalcycle;
    public float cycle;
    public float time;
    public float delay;

    GameObject GameDirector;
    GameDirector GD;
    GameObject Player;
    PlayerController PC;

    public GameObject Flip_icon;
    public GameObject SquareTile_White;
    public GameObject SquareTile_Arrow;

    public Sprite[] Tile_Color = new Sprite[10];

    public GameObject[,] Tiles = new GameObject[10, 10];
    public bool[,] isTile = new bool[10, 10];
    GameObject[] Arrows = new GameObject[20];
     
	// Use this for initialization
	void Start () {
        GameDirector = GameObject.Find("GameDirector");
        GD = GameDirector.GetComponent<GameDirector>();
        Player = GameObject.Find("Player");
        PC = Player.GetComponent<PlayerController>();

        // 재활용할거기에 아껴둔다.
        MakeArrows();
        MakeSquareTiles();
    }
	
	// Update is called once per frame
	void Update () {
        if (GD.isStart)
        {
            if (time%cycle > cycle * 0.8f)
            {
                camera.orthographicSize = 1200f * (time % cycle)/cycle;
            }
            camera.transform.rotation = Quaternion.Euler(0, 0, time % cycle * 10f * Rotate_seed);

            if (turn % 32 == 0 && turn <= 128 && !GD.Died && !already && turn!=0)
            {
                if (time % cycle >= delay)
                {
                    already = true;
                    GD.SongControl((60f / cycle + 3f / originalcycle) / (60f / cycle));
                }
            }

            if ((int)(time/cycle) > BeforeTurn && !GD.Died)
            {
                already = false;
                alreadyPatternAppeared = false;
                Rotate_seed *= -1;

                turn++;

                if(turn % 32 == 1 && turn!=1 && turn <= 129)
                {
                    GD.CycleControl((60f/cycle+ 3f / originalcycle) /(60f / cycle));
                }

                if (Pattern_Code == 0)
                {
                    Pattern_Square3x3();
                }
                Pattern_fake();
                Pattern_invisible();
                Pattern_flip();
            }

            if (!GD.Died)
            {
                BeforeTurn = (int)(time / cycle);
                time += Time.deltaTime;
                // Debug.Log(time - GD.AudioClips[0].time);
            }
        }
	}

    void MakeArrows()
    {
        for(int i = 0; i < 20; i++)
        {
            Arrows[i] = Instantiate(SquareTile_Arrow) as GameObject;
        }
    }

    void MakeSquareTiles()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Tiles[i, j] = Instantiate(SquareTile_White) as GameObject;
                // 차후 타일 크기 변경에 따른 수정 필요 (i-1, j-1의 1값이 바뀌어야함.)
                Tiles[i, j].transform.position = new Vector3(192 * (i - 1), 192 * (j - 1), 0);
            }
        }
    }

    void Pattern_Square3x3()
    {
        if (turn % 4 == 2)
        {
            int k;
            k = Random.Range(0, 5);
            while (!(Ptn_x + dx[k] >= 0 && Ptn_x + dx[k] <= TileSize_x && Ptn_y + dy[k] >= 0 && Ptn_y + dy[k] <= TileSize_y))
            {
                k = Random.Range(0, 5);
            }
            Ptn_x += dx[k];
            Ptn_y += dy[k];

            int cnt = 0;
            for (int i = 0; i <= TileSize_x; i++)
            {
                if (i != Ptn_x)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Arrows[cnt].transform.position = new Vector3(192 * (i - 1), 384, 0);
                        Arrows[cnt++].transform.rotation = Quaternion.Euler(0, 0, 270);
                    }
                    else
                    {
                        Arrows[cnt].transform.position = new Vector3(192 * (i - 1), -384, 0);
                        Arrows[cnt++].transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                }
            }
            for (int i = 0; i <= TileSize_y; i++)
            {
                if (i != Ptn_y)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Arrows[cnt].transform.position = new Vector3(384, 192 * (i - 1), 0);
                        Arrows[cnt++].transform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                    else
                    {
                        Arrows[cnt].transform.position = new Vector3(-384, 192 * (i - 1), 0);
                        Arrows[cnt++].transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
            }
            for (int i = 0; i <= TileSize_x; i++)
            {
                for (int j = 0; j <= TileSize_y; j++)
                {
                    isTile[i, j] = true;

                    Tiles[i, j].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
                    Tiles[i, j].GetComponent<BoxCollider2D>().isTrigger = false;
                }
            }
        }
        else if(turn % 4 == 0)
        {
            for (int i = 0; i < 20; i++)
            {
                Arrows[i].transform.position = new Vector3(-10000, -10000, 0);
            }

            for (int i = 0; i <= TileSize_x; i++)
            {
                for (int j = 0; j <= TileSize_y; j++)
                {
                    isTile[i, j] = true;

                    if (i != Ptn_x || j != Ptn_y)
                    {
                        Tiles[i, j].GetComponent<SpriteRenderer>().sprite = Tile_Color[1];
                        Tiles[i, j].GetComponent<BoxCollider2D>().isTrigger = true;
                    }
                    else
                    {
                        Tiles[i, j].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
                        Tiles[i, j].GetComponent<BoxCollider2D>().isTrigger = false;
                    }
                }
            }
        }
    }

    void Pattern_invisible()
    {
        if(turn % 4 == 0 && !isinvisible && !alreadyPatternAppeared)
        {
            int invisible_seed = Random.Range(0, 64);
            if(invisible_seed < invisible_prob)
            {
                isinvisible = true;
                alreadyPatternAppeared = true;
                invisible_length = Random.Range(2, 5) * 4;
                Player.GetComponent<SpriteRenderer>().sprite = PC.Player_Color[2];

                GD.Pattern_Text.SetActive(true);
                GD.Pattern_Text.GetComponent<Text>().text = "INVISIBLE";
                StartCoroutine(GD.Pattern_TextHide());
            }
        }

        if (isinvisible)
        {
            if(invisible_length != 0)
            {
                invisible_length--;
                if(turn % 4 == 0)
                {
                    Tiles[Ptn_x, Ptn_y].GetComponent<SpriteRenderer>().sprite = Tile_Color[1];
                }
                else if(turn % 4 == 2)
                {
                    Tiles[Ptn_x, Ptn_y].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
                }
            }
            else
            {
                Player.GetComponent<SpriteRenderer>().sprite = PC.Player_Color[0];
                isinvisible = false;
            }
        }
    }

    void Pattern_fake()
    {
        if (turn % 64 == 60 && !isfake && !alreadyPatternAppeared)
        {
            isfake = true;
            alreadyPatternAppeared = true;
            fake_length = 4;

            GD.Pattern_Text.SetActive(true);
            GD.Pattern_Text.GetComponent<Text>().text = "FAKE";
            StartCoroutine(GD.Pattern_TextHide());
            /*
            int fake_seed = Random.Range(0, 64);
            if (fake_seed < fake_prob)
            {
                isfake = true;
                alreadyPatternAppeared = true;
                fake_length = 4;

                GD.Pattern_Text.SetActive(true);
                GD.Pattern_Text.GetComponent<Text>().text = "FAKE";
                StartCoroutine(GD.Pattern_TextHide());
            }
            */
        }

        if (isfake)
        {
            if (fake_length != 0)
            {
                fake_length--;
                if (turn % 4 == 2)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Arrows[10].transform.position = new Vector3(192 * (Ptn_x - 1), 384, 0);
                        if(Random.Range(0, 3) == 0)
                        {
                            Arrows[10].transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (Random.Range(0, 3) == 1)
                        {
                            Arrows[10].transform.rotation = Quaternion.Euler(0, 0, 90);
                        }
                        else
                        {
                            Arrows[10].transform.rotation = Quaternion.Euler(0, 0, 180);
                        }
                    }
                    else
                    {
                        Arrows[10].transform.position = new Vector3(192 * (Ptn_x - 1), -384, 0);
                        if (Random.Range(0, 3) == 0)
                        {
                            Arrows[10].transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (Random.Range(0, 3) == 1)
                        {
                            Arrows[10].transform.rotation = Quaternion.Euler(0, 0, 180);
                        }
                        else
                        {
                            Arrows[10].transform.rotation = Quaternion.Euler(0, 0, 270);
                        }
                    }

                    if (Random.Range(0, 2) == 0)
                    {
                        Arrows[11].transform.position = new Vector3(384, 192 * (Ptn_y - 1), 0);
                        if (Random.Range(0, 3) == 0)
                        {
                            Arrows[11].transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (Random.Range(0, 3) == 1)
                        {
                            Arrows[11].transform.rotation = Quaternion.Euler(0, 0, 90);
                        }
                        else
                        {
                            Arrows[11].transform.rotation = Quaternion.Euler(0, 0, 270);
                        }
                    }
                    else
                    {
                        Arrows[11].transform.position = new Vector3(-384, 192 * (Ptn_y - 1), 0);
                        if (Random.Range(0, 3) == 0)
                        {
                            Arrows[11].transform.rotation = Quaternion.Euler(0, 0, 90);
                        }
                        else if (Random.Range(0, 3) == 1)
                        {
                            Arrows[11].transform.rotation = Quaternion.Euler(0, 0, 180);
                        }
                        else
                        {
                            Arrows[11].transform.rotation = Quaternion.Euler(0, 0, 270);
                        }
                    }
                }
            }
            else
            {
                isfake = false;
            }
        }
    }

    void Pattern_flip()
    {
        if (turn % 4 == 0 && !isflip && !alreadyPatternAppeared)
        {
            int flip_seed = Random.Range(0, 64);
            if (flip_seed < flip_prob)
            {
                isflip = true;
                alreadyPatternAppeared = true;
                flip_length = Random.Range(3, 5) * 4;

                GD.Pattern_Text.SetActive(true);
                GD.Pattern_Text.GetComponent<Text>().text = "FLIP";
                StartCoroutine(GD.Pattern_TextHide());
                Flip_icon.SetActive(true);
            }
        }

        if (isflip)
        {
            if (flip_length != 0)
            {
                flip_length--;
            }
            else
            {
                GD.Pattern_End_Text.SetActive(true);
                GD.Pattern_End_Text.GetComponent<Text>().text = "FLIP END";
                StartCoroutine(GD.Pattern_End_TextHide());
                Flip_icon.SetActive(false);
                isflip = false;
            }
        }
    }

    /*
    void EndPattern_Square3x3()
    {

        if (turn % 2 == 1)
        {
            for (int i = 0; i <= TileSize_x; i++)
            {
                for (int j = 0; j <= TileSize_y; j++)
                {
                    isTile[i, j] = true;

                    Tiles[i, j].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
                    Tiles[i, j].GetComponent<BoxCollider2D>().isTrigger = false;
                }
            }

            Tiles[0, 0].GetComponent<SpriteRenderer>().sprite = Tile_Color[2];
            Tiles[2, 0].GetComponent<SpriteRenderer>().sprite = Tile_Color[2];
            Tiles[0, 2].GetComponent<SpriteRenderer>().sprite = Tile_Color[2];
            Tiles[2, 2].GetComponent<SpriteRenderer>().sprite = Tile_Color[2];
        }
        else
        {
            isTile[0, 0] = false;
            isTile[2, 0] = false;
            isTile[0, 2] = false;
            isTile[2, 2] = false;

            Tiles[0, 0].GetComponent<SpriteRenderer>().sprite = Tile_Color[3];
            Tiles[2, 0].GetComponent<SpriteRenderer>().sprite = Tile_Color[3];
            Tiles[0, 2].GetComponent<SpriteRenderer>().sprite = Tile_Color[3];
            Tiles[2, 2].GetComponent<SpriteRenderer>().sprite = Tile_Color[3];

            Tiles[0, 0].GetComponent<BoxCollider2D>().isTrigger = true;
            Tiles[2, 0].GetComponent<BoxCollider2D>().isTrigger = true;
            Tiles[0, 2].GetComponent<BoxCollider2D>().isTrigger = true;
            Tiles[2, 2].GetComponent<BoxCollider2D>().isTrigger = true;

            isEnd = false;
            Pattern_Code = 1;
            cycle = 70f / 140f;
            GD.AudioClips[0].pitch = 12/7f;
        }
    }

    void Pattern_Cross()
    {

        if (turn % 2 == 1)
        {
            Tiles[1, 2].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
            Tiles[1, 2].GetComponent<BoxCollider2D>().isTrigger = false;
            Tiles[0, 1].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
            Tiles[0, 1].GetComponent<BoxCollider2D>().isTrigger = false;
            Tiles[1, 1].GetComponent<SpriteRenderer>().sprite = Tile_Color[2];
            Tiles[1, 1].GetComponent<BoxCollider2D>().isTrigger = false;
            Tiles[2, 1].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
            Tiles[2, 1].GetComponent<BoxCollider2D>().isTrigger = false;
            Tiles[1, 0].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
            Tiles[1, 0].GetComponent<BoxCollider2D>().isTrigger = false;

            PC.Player.transform.position = new Vector3(0, 0, 0);

            for (int i = 0; i < 4; i++)
            {
                if (i != Cross_seed)
                {
                    Arrows[i].transform.position = new Vector3(384 * dx[i], 384 * dy[i], 0);
                    Arrows[i].transform.rotation = Quaternion.Euler(0, 0, 180 - 90 * i);
                }
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                Arrows[i].transform.position = new Vector3(-1000, 384 -1000, 0);
            }

            Tiles[1, 2].GetComponent<SpriteRenderer>().sprite = Tile_Color[1];
            Tiles[1, 2].GetComponent<BoxCollider2D>().isTrigger = true;
            Tiles[0, 1].GetComponent<SpriteRenderer>().sprite = Tile_Color[1];
            Tiles[0, 1].GetComponent<BoxCollider2D>().isTrigger = true;
            Tiles[1, 1].GetComponent<SpriteRenderer>().sprite = Tile_Color[3];
            Tiles[1, 1].GetComponent<BoxCollider2D>().isTrigger = true;
            isTile[1, 1] = false;
            Tiles[2, 1].GetComponent<SpriteRenderer>().sprite = Tile_Color[1];
            Tiles[2, 1].GetComponent<BoxCollider2D>().isTrigger = true;
            Tiles[1, 0].GetComponent<SpriteRenderer>().sprite = Tile_Color[1];
            Tiles[1, 0].GetComponent<BoxCollider2D>().isTrigger = true;

            if (Cross_seed == 0)
            {
                Tiles[2, 1].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
                Tiles[2, 1].GetComponent<BoxCollider2D>().isTrigger = false;
            }
            else if (Cross_seed == 1)
            {
                Tiles[1, 0].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
                Tiles[1, 0].GetComponent<BoxCollider2D>().isTrigger = false;
            }
            else if (Cross_seed == 2)
            {
                Tiles[0, 1].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
                Tiles[0, 1].GetComponent<BoxCollider2D>().isTrigger = false;
            }
            else
            {
                Tiles[1, 2].GetComponent<SpriteRenderer>().sprite = Tile_Color[0];
                Tiles[1, 2].GetComponent<BoxCollider2D>().isTrigger = false;
            }

            if (Random.Range(0, 2) == 0)
            {
                Cross_seed += 1;
                Cross_seed %= 4;
            }
            else
            {
                Cross_seed -= 1;
                if(Cross_seed < 0)
                {
                    Cross_seed = (Cross_seed + 4) % 4;
                }
            }
        }
    }
    */
}
