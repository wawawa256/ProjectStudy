using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TextButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    bool isClicked;
    Text text;
    Color normalColor = Color.black;
    Color pressedColor = Color.gray;

    public bool IsClicked { get => isClicked; set => isClicked = value; }

    public event UnityAction OnClick;
    public event UnityAction OnTouch;
    public event UnityAction OnRelease;

    public void Init()
    {
        text = GetComponent<Text>();
        text.color = normalColor;
        IsClicked = false;
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if(IsClicked)
        {
            return;
        }
        IsClicked = true;
        OnClick?.Invoke();
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        text.color = pressedColor;
        OnTouch?.Invoke();
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        text.color = normalColor;
        OnRelease?.Invoke();
    }
}
