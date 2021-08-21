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
    public GameObject AddFunctionPanel;
    public GameObject VarListPanel;
    public GameObject UIButtons;

    bool AddpanelActivity;
    bool SettingpanelActivity;
    bool CodepanelActivity;
    bool AddVarPanelActivity;
    bool AddFunctionPanelActivity;
    bool VarListPanelActivity;
    bool UIButtonsActivity;

    //if、for用
    public GameObject PrintfButton;
    public GameObject IfStartButton;
    public GameObject IfEndButton;
    public GameObject ForStartButton;
    public GameObject ForEndButton;
    public GameObject WhileStartButton;
    public GameObject WhileEndButton;
    public GameObject BreakButton;
    public GameObject SubroutineButton;
    public GameObject CalcButton;

    bool PrintButtonActivity;
    bool IfStartButtonActivity;
    bool IfEndButtonActivity;
    bool ForStartButtonActivity;
    bool ForEndButtonActivity;
    bool CalcButtonActivity;
    bool WhileMenuActivity;

    public GameObject IfMenu;
    public GameObject PrintfMenu;
    public GameObject ForMenu;
    public GameObject CalcMenu;
    public GameObject WhileMenu;

    public Text PrintfDisplay;
    public Text IfDisPlay;
    public Text ForDisplay;
    public Text CalcDisplay;
    public Text WhileDisplay;

    int ifFlag;
    int forFlag;
    int whileFlag;

    public void Start()
    {
        PanelClose();
        UIButtons.SetActive(false);

        ButtonUnlock();

        ifFlag = 0;
        forFlag = 0;
        whileFlag = 0;
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
        SettingpanelActivity = SettingPanel.activeInHierarchy;
        PanelClose();
        SettingPanel.SetActive(!SettingpanelActivity);
    }

    public void AddVarPanelChange()
    {
        AddVarPanelActivity = AddVarPanel.activeInHierarchy;
        PanelClose();
        AddVarPanel.SetActive(!AddVarPanelActivity);
    }

    public void AddFunctionPanelChange()
    {
        AddFunctionPanelActivity = AddFunctionPanel.activeInHierarchy;
        PanelClose();
        AddFunctionPanel.SetActive(!AddFunctionPanelActivity);
    }

    public void CodePanelChange()
    {
        CodepanelActivity = CodePanel.activeInHierarchy;
        PanelClose();
        CodePanel.SetActive(!CodepanelActivity);
    }

    public void VarListPanelChange()
    {
        VarListPanelActivity = VarListPanel.activeInHierarchy;
        PanelClose();
        VarListPanel.SetActive(!VarListPanelActivity);
    }

    public void PanelClose()
    {
        SettingPanel.SetActive(false);
        AddVarPanel.SetActive(false);
        AddFunctionPanel.SetActive(false);
        CodePanel.SetActive(false);
        VarListPanel.SetActive(false);
    }

    public void UICallButtonClicked()
    {
        UIButtonsActivity = UIButtons.activeInHierarchy;
        UIButtons.SetActive(!UIButtonsActivity);
    }

    public void IfButtonClicked()
    {
        switch(ifFlag)
        {
            case 0:
                ButtonLock();
                IfEndButton.SetActive(true);
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
        switch(forFlag)
        {
            case 0 :
                ButtonLock();
                ForEndButton.SetActive(true);
                forFlag = 1;
                break;
            case 1:
                ButtonUnlock();
                forFlag = 0;
                break;
        }
    }

    public void WhileButtonClicked()
    {
        switch(whileFlag)
        {
            case 0 :
                ButtonLock();
                WhileEndButton.SetActive(true);
                whileFlag = 1;
                break;
            case 1:
                ButtonUnlock();
                whileFlag = 0;
                break;
        }
    }

    void ButtonLock()
    {
        PrintfButton.SetActive(false);
        IfStartButton.SetActive(false);
        ForStartButton.SetActive(false);
        WhileStartButton.SetActive(false);
        CalcButton.SetActive(false);
        BreakButton.SetActive(false);
        SubroutineButton.SetActive(false);

        IfEndButton.SetActive(false);
        ForEndButton.SetActive(false);
        WhileEndButton.SetActive(false);
    }

    void ButtonUnlock()
    {
        PrintfButton.SetActive(true);
        IfStartButton.SetActive(true);
        ForStartButton.SetActive(true);
        WhileStartButton.SetActive(true);
        CalcButton.SetActive(true);
        BreakButton.SetActive(true);
        SubroutineButton.SetActive(true);

        IfEndButton.SetActive(false);
        ForEndButton.SetActive(false);
        WhileEndButton.SetActive(false);
    } 

    //中身メニューを開こう。メニュー開くボタンにアタッチ
    public void OpenContentMenu()
    {
        //text→textcomponent取得する。
        PrintfDisplay = PrintfDisplay.GetComponent<Text>();
        IfDisPlay = IfDisPlay.GetComponent<Text>();
        ForDisplay = ForDisplay.GetComponent<Text>();
        CalcDisplay = CalcDisplay.GetComponent<Text>();
        WhileDisplay = WhileDisplay.GetComponent<Text>();
        int imanani = ObjectCollection.ItemCheck2();
        bool IfMenuActivity = IfMenu.activeInHierarchy;
        bool PrintfMenuActivity = PrintfMenu.activeInHierarchy;
        bool ForMenuActivity = ForMenu.activeInHierarchy;
        bool CalcMenuActivity = CalcMenu.activeInHierarchy;
        bool WhileMenuActivity = WhileMenu.activeInHierarchy;
        string DataHere = 
            ObjectCollection.content[ObjectCollection.CurrentColumn,ObjectCollection.CurrentRow];

        if(imanani==2)
        {
            //更新処理。何もしないとさっき入力した時のdisplayがmenuないに表示されるからそれを今のデータに合わせて変更してあげなくちゃいけない。
            PrintfDisplay.text = DataHere;
            PrintfMenu.SetActive(!PrintfMenuActivity);
            ObjectCollection.touch_flag = 0;
        }
        else if(imanani==3)
        {
            IfDisPlay.text = DataHere;
            IfMenu.SetActive(!IfMenuActivity);
            ObjectCollection.touch_flag = 0;
        }
        else if(imanani==5)
        {
            ForDisplay.text=DataHere;
            ForMenu.SetActive(!ForMenuActivity);
            ObjectCollection.touch_flag = 0;
        }
        else if(imanani==7)
        {
            CalcDisplay.text=DataHere;
            CalcMenu.SetActive(!CalcMenuActivity);
            ObjectCollection.touch_flag = 0;
        }
        else if(imanani==8)
        {
            WhileDisplay.text=DataHere;
            WhileMenu.SetActive(!WhileMenuActivity);
            ObjectCollection.touch_flag=0;
        }
        else
        {
            ObjectCollection.touch_flag = 1;
        }
        //ObjectCollection.BeyondDimension();
    }

    public void touch_flagtateruyo(){
        ObjectCollection.touch_flag = 1;
    }
}
