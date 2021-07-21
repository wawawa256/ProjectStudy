using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameObject AddPanel;
    public GameObject SettingPanel;
    public GameObject CodePanel;
    public GameObject AddVarPanel;
    public GameObject VarListPanel;
    public GameObject UIButtons;
    bool AddpanelActivity;
    bool SettingpanelActivity;
    bool CodepanelActivity;
    bool AddVarPanelActivity;
    bool VarListPanelActivity;
    bool UIButtonsActivity;

    //if、for用
    public GameObject PrintfButton;
    public GameObject IfStartButton;
    public GameObject IfEndButton;
    public GameObject ForStartButton;
    public GameObject ForEndButton;
    public GameObject CalcButton;
    bool PrintButtonActivity;
    bool IfStartButtonActivity;
    bool IfEndButtonActivity;
    bool ForStartButtonActivity;
    bool ForEndButtonActivity;
    bool CalcButtonActivity;

    int ifFlag;
    int forFlag;

    public void Start()
    {
        //AddPanel.SetActive(false);
        SettingPanel.SetActive(false);
        AddVarPanel.SetActive(false);
        CodePanel.SetActive(false);
        VarListPanel.SetActive(false);
        UIButtons.SetActive(false);

        PrintfButton.SetActive(true);
        IfStartButton.SetActive(true);
        ForStartButton.SetActive(true);
        CalcButton.SetActive(true);
        IfEndButton.SetActive(false);
        ForEndButton.SetActive(false);

        ifFlag = 0;
        forFlag = 0;
    }

    public void PaizaButtonClicked()
    {
        Application.OpenURL("https://paiza.io/ja/projects/new");
    }

    public void OnClick()
    {
        AddpanelActivity = AddPanel.activeInHierarchy;
        AddPanel.SetActive(!AddpanelActivity);
    }
    
    public void OnClick2()
    {
        //AddPanel.SetActive(false);
        AddVarPanel.SetActive(false);
        CodePanel.SetActive(false);
        VarListPanel.SetActive(false);
        SettingpanelActivity = SettingPanel.activeInHierarchy;
        SettingPanel.SetActive(!SettingpanelActivity);
    }

    public void AddVarPanelChange()
    { 
        //AddPanel.SetActive(false);
        SettingPanel.SetActive(false);
        CodePanel.SetActive(false);
        VarListPanel.SetActive(false);
        UIButtons.SetActive(false);
        AddVarPanelActivity = AddVarPanel.activeInHierarchy;
        AddVarPanel.SetActive(!AddVarPanelActivity);
    }

    public void CodePanelChange()
    {
        //AddPanel.SetActive(false);
        SettingPanel.SetActive(false);
        AddVarPanel.SetActive(false);
        VarListPanel.SetActive(false);
        CodepanelActivity = CodePanel.activeInHierarchy;
        CodePanel.SetActive(!CodepanelActivity);
    }

    public void VarListPanelChange()
    { 
        //AddPanel.SetActive(false);
        SettingPanel.SetActive(false);
        AddVarPanel.SetActive(false);
        CodePanel.SetActive(false);
        VarListPanelActivity = VarListPanel.activeInHierarchy;
        VarListPanel.SetActive(!VarListPanelActivity);
    }

    public void UICallButtonClicked()
    {
        UIButtonsActivity = UIButtons.activeInHierarchy;
        UIButtons.SetActive(!UIButtonsActivity);
    }

    public void IfButtonClicked()
    {
        switch(ifFlag){
        case 0:
            ButtonLock();
            IfEndButton.SetActive(true);
            ForEndButton.SetActive(false);
            ifFlag = 1;
            break;
        case 1:
            ButtonUnlock();
            ifFlag = 0;
            break;
        }
    }

    public void ForButtonClicked()
    {
        switch(forFlag){
        case 0 :
            ButtonLock();
            IfEndButton.SetActive(false);
            ForEndButton.SetActive(true);
            forFlag = 1;
            break;
        case 1:
            ButtonUnlock();
            forFlag = 0;
            break;
        }
    }

    void ButtonLock()
    {
        PrintfButton.SetActive(false);
        IfStartButton.SetActive(false);
        ForStartButton.SetActive(false);
        CalcButton.SetActive(false);
    }

    void ButtonUnlock()
    {
        PrintfButton.SetActive(true);
        IfStartButton.SetActive(true);
        ForStartButton.SetActive(true);
        CalcButton.SetActive(true);
        IfEndButton.SetActive(false);
        ForEndButton.SetActive(false);
    }
}
