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

    public GameObject IfMenu;
    public GameObject PrintfMenu;
    public GameObject ForMenu;
    public GameObject CalcMenu;

    public Text PrintfDisplay;
    public Text IfDisPlay;
    public Text ForDisplay;
    public Text CalcDisplay;

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

    public void touch_flagtateruyo(){
        ObjectCollection.touch_flag = 1;
    }
    public void touch_flagkorosuyo(){
        ObjectCollection.touch_flag = 0;
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

    //中身メニューを開こう。メニュー開くボタンにアタッチ
    public void OpenContentMenu()
        {
            //text→textcomponent取得する。
            PrintfDisplay = PrintfDisplay.GetComponent<Text>();
            IfDisPlay = IfDisPlay.GetComponent<Text>();
            ForDisplay = ForDisplay.GetComponent<Text>();
            int imanani = ObjectCollection.ItemCheck2();
            bool IfMenuActivity = IfMenu.activeInHierarchy;
            bool PrintfMenuActivity = PrintfMenu.activeInHierarchy;
            bool ForMenuActivity = ForMenu.activeInHierarchy;
            bool CalcMenuActivity = CalcMenu.activeInHierarchy;
            string DataHere = 
                ObjectCollection.content[ObjectCollection.CurrentColumn,ObjectCollection.CurrentRow];

            if(imanani==2){
                //更新処理。何もしないとさっき入力した時のdisplayがmenuないに表示されるからそれを今のデータに合わせて変更してあげなくちゃいけない。
                PrintfDisplay.text = DataHere;
                PrintfMenu.SetActive(!PrintfMenuActivity);
            }else if(imanani==3){

                IfDisPlay.text = DataHere;
                IfMenu.SetActive(!IfMenuActivity);
            }else if(imanani==5||imanani==6){
                ForMenu.SetActive(!ForMenuActivity);
            }else if(imanani==7){
                CalcDisplay.text=DataHere;
                CalcMenu.SetActive(!CalcMenuActivity);
            }
        }
}
