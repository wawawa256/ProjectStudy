using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SkillKindSelectUI : MonoBehaviour
{
    public UnityAction SkillButtonOnClick;
    List<TextButton> buttons = new List<TextButton>();
    private void Start()
    {
        buttons.AddRange(GetComponentsInChildren<TextButton>());
        buttons.ForEach(button => button.OnClick += ButtonClicked);
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


    public void Open()
    {
        gameObject.SetActive(true);
        buttons.ForEach(button => button.IsClicked = false);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
