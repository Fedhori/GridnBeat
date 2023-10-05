using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameDirector : MonoBehaviour
{

    // 0 very easy 3 hard
    public Sprite[] BackGroundSprite = new Sprite[5];
    public GameObject BackGround;
    public GameObject PopUp;
    GameObject TimeText;
    GameObject ReadyText;
    public GameObject HighScoreText;
    public GameObject Difficulty_Text;

    public GameObject Pattern_Text;
    public GameObject Pattern_End_Text;

    public GameObject Origin;

    public GameObject Easier;
    public GameObject Harder;

    public GameObject Origin_Btn;

    public GameObject Option_Btn;

    GameObject TileDirector;
    TileDirector TD;

    public AudioSource[] AudioClips = new AudioSource[10];
    AudioSource cur_AudioClip = new AudioSource();

    public ParticleSystem Ready_Particle;

    // 0 is very easy, 3 is hard

    public bool isPopUp = false;
    public bool Died = false;
    public bool isStart = false;
    public bool isDev = false;
    public string cur_music;
    public bool alreadyOrdered = false; // there's some pattern's already showed
    public bool newOrder = false;
    public bool isStartButNotReally = false;
    float ReadyTime = 0;

    public float easiest_score = 0f;
    public float easy_score = 0f;
    public float normal_score = 0f;
    public float hard_score = 0f;

    // Use this for initialization
    void Awake()
    {

        TileDirector = GameObject.Find("TileDirector");
        TD = TileDirector.GetComponent<TileDirector>();

        TimeText = GameObject.Find("TimeText");
        TimeText.SetActive(false);

        ReadyText = GameObject.Find("Ready?");
        HighScoreText = GameObject.Find("HighScore");

        Pattern_Text = GameObject.Find("Pattern_Text");
        Pattern_Text.SetActive(false);
        Pattern_End_Text.SetActive(false);

        PopUp.SetActive(false);

        UpdateScore();

        // show different highscores depending on difficulty
        if (PlayerPrefs.GetInt("Difficulty", 2) == 0)
        {
            VeryEasy();
        }
        else if (PlayerPrefs.GetInt("Difficulty", 2) == 1)
        {
            Easy();
        }
        else if (PlayerPrefs.GetInt("Difficulty", 2) == 2)
        {
            Normal();
        }
        else if (PlayerPrefs.GetInt("Difficulty", 2) == 3)
        {
            Hard();
        }

        // 0 is sliding, 1 is using origin
        if (PlayerPrefs.GetInt("ControlType", 0) == 0)
        {
            Origin.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Died && isStartButNotReally)
        {
            ReadyTime += Time.deltaTime;
        }
        TimeText.GetComponent<Text>().text = (ReadyTime - 1f).ToString("F2");

        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject())
            {
                if (Died)
                {
                    StartCoroutine(Restart());
                }
                else if (!isStart)
                {
                    //determine whether UI touched or not

                    ReadyText.SetActive(false);
                    Easier.SetActive(false);
                    Harder.SetActive(false);
                    Origin_Btn.SetActive(false);
                    Option_Btn.SetActive(false);
                    isStartButNotReally = true;

                }
            }
        }

        if (isStartButNotReally && !isStart && ReadyTime >= 1f)
        {
            Ready_Particle.Stop();
            Ready_Particle.Clear();
            TimeText.SetActive(true);
            if (cur_music == "beato_140")
            {
                cur_AudioClip = AudioClips[0];

                TD.start_bpm = 140;
                TD.originalcycle = 60f / TD.start_bpm;
                TD.cycle = TD.originalcycle;
                TD.delay = TD.originalcycle - 1 / 5f;
                TD.time = TD.delay;
            }
            else if (cur_music == "beato_80")
            {
                cur_AudioClip = AudioClips[2];

                TD.start_bpm = 80;
                TD.originalcycle = 60f / TD.start_bpm;
                TD.cycle = TD.originalcycle;
                TD.delay = TD.originalcycle - 1 / 5f;
                TD.time = TD.delay;
            }
            else if (cur_music == "beato_120")
            {
                cur_AudioClip = AudioClips[1];

                TD.start_bpm = 120;
                TD.originalcycle = 60f / TD.start_bpm;
                TD.cycle = TD.originalcycle;
                TD.delay = TD.originalcycle - 1 / 5f;
                TD.time = TD.delay;
            }
            else if (cur_music == "beato3")
            {
                cur_AudioClip = AudioClips[2];

                TD.start_bpm = 160;
                TD.originalcycle = 60f / TD.start_bpm;
                TD.cycle = TD.originalcycle;
                TD.delay = TD.originalcycle - 1 / 5f;
                TD.time = TD.delay;
            }
            cur_AudioClip.Play();
            isStart = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ahh.. Hide it! Quick!
            if (isStart && !Died)
            {
                Application.Quit();
            }
            // let's show the menu
            else
            {
                ShowPopUp();
            }

        }
    }

    public void GameOver()
    {
        if (!isDev)
        {
            cur_AudioClip.Stop();
            Died = true;
        }
    }

    public void SongControl(float speed)
    {
        cur_AudioClip.pitch *= speed;
    }

    public void CycleControl(float speed)
    {
        Pattern_Text.SetActive(true);
        Pattern_Text.GetComponent<Text>().text = "FASTER";

        TD.time /= speed;
        TD.cycle /= speed;
        StartCoroutine(Pattern_TextHide());
    }

    public IEnumerator Pattern_TextHide()
    {
        if (alreadyOrdered)
        {
            newOrder = true;
        }
        alreadyOrdered = true;
        yield return new WaitForSeconds(2);
        alreadyOrdered = false;
        if (!newOrder)
        {
            Pattern_Text.SetActive(false);
        }
        else
        {
            newOrder = false;
        }
    }

    public IEnumerator Pattern_End_TextHide()
    {
        yield return new WaitForSeconds(2);
        Pattern_End_Text.SetActive(false);
    }

    public IEnumerator Restart()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameScene");
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void Hard()
    {
        cur_music = "beato3";
        PlayerPrefs.SetString("CurrentMusic", "beato3");
        Difficulty_Text.SetActive(true);
        Difficulty_Text.GetComponent<Text>().text = "HARD";
        BackGround.GetComponent<SpriteRenderer>().sprite = BackGroundSprite[3];

        HighScoreText.GetComponent<Text>().text =
            "HighScore: " + hard_score.ToString("F2");

        Harder.SetActive(false);
    }

    public void Normal()
    {
        cur_music = "beato_140";
        PlayerPrefs.SetString("CurrentMusic", "beato_140");
        Difficulty_Text.SetActive(true);
        Difficulty_Text.GetComponent<Text>().text = "NORMAL";
        BackGround.GetComponent<SpriteRenderer>().sprite = BackGroundSprite[2];

        HighScoreText.GetComponent<Text>().text =
            "HighScore: " + normal_score.ToString("F2");

        Harder.SetActive(true);
    }

    public void Easy()
    {
        cur_music = "beato_120";
        PlayerPrefs.SetString("CurrentMusic", "beato_120");
        Difficulty_Text.SetActive(true);
        Difficulty_Text.GetComponent<Text>().text = "EASY";
        BackGround.GetComponent<SpriteRenderer>().sprite = BackGroundSprite[1];

        HighScoreText.GetComponent<Text>().text =
            "HighScore: " + easy_score.ToString("F2");

        Easier.SetActive(true);
    }

    public void VeryEasy()
    {
        cur_music = "beato_80";
        PlayerPrefs.SetString("CurrentMusic", "beato_80");
        Difficulty_Text.SetActive(true);
        Difficulty_Text.GetComponent<Text>().text = "E-Z";
        BackGround.GetComponent<SpriteRenderer>().sprite = BackGroundSprite[0];

        HighScoreText.GetComponent<Text>().text =
            "HighScore: " + easiest_score.ToString("F2");

        Easier.SetActive(false);
    }

    public void ShowPopUp()
    {
        if (!isPopUp)
        {
            isPopUp = true;
            PopUp.SetActive(true);
        }
        else
        {
            isPopUp = false;
            PopUp.SetActive(false);
        }
    }

    public IEnumerator Update_Score()
    {
        yield return new WaitForSeconds(3f);
        if (PlayerPrefs.GetInt("Difficulty", 2) == 0)
        {
            VeryEasy();
        }
        else if (PlayerPrefs.GetInt("Difficulty", 2) == 1)
        {
            Easy();
        }
        else if (PlayerPrefs.GetInt("Difficulty", 2) == 2)
        {
            Normal();
        }
        else if (PlayerPrefs.GetInt("Difficulty", 2) == 3)
        {
            Hard();
        }
    }

    public void UpdateScore()
    {
        
    }

    class Data
    {
        public float easiest = 0f;
        public float easy = 0f;
        public float normal = 0f;
        public float hard = 0f;

        public Data(float easiest, float easy, float normal, float hard)
        {
            this.easiest = easiest;
            this.easy = easy;
            this.normal = normal;
            this.hard = hard;
        }

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["E-Z"] = easiest;
            result["Easy"] = easy;
            result["Normal"] = normal;
            result["Hard"] = hard;

            return result;
        }
    }

}
