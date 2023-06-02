using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToEditButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    bool isClicked;
    Text text;
    Color normalColor;
    Color pressedColor = Color.gray;

    public bool IsClicked { get => isClicked; set => isClicked = value; }
    void Start()
    {
        text = GetComponent<Text>();
        normalColor = GetColor("#007AFF");
        Init();
    }
    public void Init()
    {
        text.color = normalColor;
        IsClicked = false;
    }
    private Color GetColor(string colorCode)
    {
        Color color = default(Color);
        if (ColorUtility.TryParseHtmlString(colorCode, out color))
        {
            //Colorを生成できたらそれを返す
            return color;
        }
        else
        {
            //Colorの生成に失敗したら黒を返す
            return Color.black;
        }
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (isClicked)
        {
            return;
        }
        isClicked = true;
        SceneManager.LoadScene("EditScene");
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        text.color = pressedColor;
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        text.color = normalColor;
    }
}