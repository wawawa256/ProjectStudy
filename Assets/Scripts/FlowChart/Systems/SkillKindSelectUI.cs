using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SkillKindSelectUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityAction SkillButtonOnClick;
    List<TextButton> buttons = new List<TextButton>();
    public UnityAction OnTouch;
    public UnityAction OnRelease;
    private void Start()
    {
        buttons.AddRange(GetComponentsInChildren<TextButton>());
        buttons.ForEach(button => button.OnClick += ButtonClicked);
        buttons.ForEach(button => button.OnTouch += () => OnTouch?.Invoke());
        buttons.ForEach(button => button.OnRelease += () => OnRelease?.Invoke());
        buttons[0].OnClick += AggressiveButtonClicked;
        buttons[1].OnClick += DefensiveButtonClicked;
        buttons[2].OnClick += HealingButtonClicked;
        buttons[3].OnClick += BuffButtonClicked;
        Close();
    }

    private void ButtonClicked()
    {
        buttons.ForEach(button => button.IsClicked = true);
    }
    private void AggressiveButtonClicked()
    {

    }
    private void DefensiveButtonClicked()
    {

    }
    private void HealingButtonClicked()
    {

    }
    private void BuffButtonClicked()
    {

    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnTouch?.Invoke();
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        OnRelease?.Invoke();
    }


    public void Open()
    {
        //Debug.Log("SkillKindSelectUI Open");
        gameObject.SetActive(true);
        buttons.ForEach(button => button.IsClicked = false);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
