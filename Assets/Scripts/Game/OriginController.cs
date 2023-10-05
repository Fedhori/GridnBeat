using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OriginController : EventTrigger
{

    GameObject GameDirector;
    GameDirector GD;
    RectTransform rectTransform;

    private bool dragging;

    void Start()
    {
        GameDirector = GameObject.Find("GameDirector");
        GD = GameDirector.GetComponent<GameDirector>();
        rectTransform = this.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(PlayerPrefs.GetFloat("Origin_x",540f), PlayerPrefs.GetFloat("Origin_y",256f));

        Image image = this.GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0.5f);

    }

    void Update()
    {
        if (dragging && (!GD.isStartButNotReally || GD.Died))
        {
            rectTransform.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); 
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!GD.isStartButNotReally || GD.Died)
        {
            Image image = this.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f, 1f);
            dragging = true;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!GD.isStartButNotReally || GD.Died)
        {
            Image image = this.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f, 0.5f);
            PlayerPrefs.SetFloat("Origin_x", Input.mousePosition.x);
            PlayerPrefs.SetFloat("Origin_y", Input.mousePosition.y);
            dragging = false;
        }   
    }
}

