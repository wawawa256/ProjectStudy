using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class BackToSelectChart : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public UnityAction OnBackToSelectChart;
    bool isClicked;
    Image image;
    Color normalColor;
    Color pressedColor = Color.gray;

    public bool IsClicked { get => isClicked; set => isClicked = value; }
    void Awake()
    {
        image = GetComponent<Image>();
        normalColor = GetColor("#007AFF");
        Init();
    }
    private void Init()
    {
        image.color = normalColor;
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
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        image.color = pressedColor;
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        image.color = normalColor;
    }
}