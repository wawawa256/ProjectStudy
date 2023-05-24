using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentButtonUI : MonoBehaviour
{
    public Text PrintfDisplay;
    public Text IfDisPlay;
    public Text ForDisplay;
    public Text CalcDisplay;
    public Text WhileDisplay;

    [SerializeField] GameObject ifMenu;
    [SerializeField] GameObject printfMenu;
    [SerializeField] GameObject forMenu;
    [SerializeField] GameObject calcMenu;
    [SerializeField] GameObject whileMenu;

    [SerializeField] ObjectCollection objectCollection;

    //中身メニューを開こう。メニュー開くボタンにアタッチ
    public void OpenContentMenu()
    {
        //text→textcomponent取得する。
        PrintfDisplay = PrintfDisplay.GetComponent<Text>();
        IfDisPlay = IfDisPlay.GetComponent<Text>();
        ForDisplay = ForDisplay.GetComponent<Text>();
        WhileDisplay = WhileDisplay.GetComponent<Text>();
        int imanani = objectCollection.ItemCheck2();
        bool IfMenuActivity = ifMenu.activeInHierarchy;
        bool PrintfMenuActivity = printfMenu.activeInHierarchy;
        bool ForMenuActivity = forMenu.activeInHierarchy;
        bool WhileMenuActivity = whileMenu.activeInHierarchy;
        string DataHere =
            objectCollection.content[objectCollection.CurrentColumn, objectCollection.CurrentRow];

        if (imanani == 2)
        {
            //更新処理。何もしないとさっき入力した時のdisplayがmenuないに表示されるからそれを今のデータに合わせて変更してあげなくちゃいけない。
            PrintfDisplay.text = DataHere;
            printfMenu.SetActive(!PrintfMenuActivity);
            objectCollection.touch_flag = 0;
        }
        else if (imanani == 3)
        {
            IfDisPlay.text = DataHere;
            ifMenu.SetActive(!IfMenuActivity);
            objectCollection.touch_flag = 0;
        }
        else if (imanani == 5)
        {
            ForDisplay.text = DataHere;
            forMenu.SetActive(!ForMenuActivity);
            objectCollection.touch_flag = 0;
        }
        else if (imanani == 8)
        {
            WhileDisplay.text = DataHere;
            whileMenu.SetActive(!WhileMenuActivity);
            objectCollection.touch_flag = 0;
        }
        else
        {
            objectCollection.touch_flag = 1;
        }
    }
}
