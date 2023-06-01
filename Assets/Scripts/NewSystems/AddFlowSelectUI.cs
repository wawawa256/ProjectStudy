using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class AddFlowSelectUI : MonoBehaviour
{
    public UnityAction SkillButtonOnClick;
    List<TextButton> buttons = new List<TextButton>();
    private void Start()
    {
        buttons.AddRange(GetComponentsInChildren<TextButton>());
        buttons.ForEach(button => button.OnClick += ButtonClicked);
        buttons[0].OnClick += SkillButtonClicked;   // Skill
        buttons[1].OnClick += IfButtonClicked;      // If
        buttons[2].OnClick += ForButtonClicked;     // For
        buttons[3].OnClick += WhileButtonClicked;   // While
        Close();
    }

    private void ButtonClicked()
    {
        buttons.ForEach(button => button.IsClicked = true);
    }
    private void SkillButtonClicked()
    {
        SkillButtonOnClick?.Invoke();
    }
    private void IfButtonClicked()
    {

    }
    private void ForButtonClicked()
    {

    }
    private void WhileButtonClicked()
    {

    }


    public void Open()
    {
        gameObject.SetActive(true);
        buttons.ForEach(button => button.Init());
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
