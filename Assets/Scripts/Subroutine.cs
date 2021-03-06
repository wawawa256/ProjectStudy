using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subroutine : MonoBehaviour
{
    public string[] nameArray = new string[256];
    public int[] formatArray = new int[256];

    public InputField NameInputField;
    public Dropdown FormatDropdown;
    public Dropdown FunctionDropdown;
    public Text nameText;
    public Text messageText;

    int functionFormat;
    string functionName;
    int functionCount;

    List<string> reservedWord = new List<string>();
    List<string> optionsList = new List<string>();

    void Start()
    {
        //作った変数の数を数えてるやつを初期化
        functionFormat = 0;
        functionName = "";
        functionCount = 1;

        //それぞれのUIのコンポーネントを取得
        NameInputField = NameInputField.GetComponent<InputField>();
        FunctionDropdown = FunctionDropdown.GetComponent<Dropdown>();
        nameText = nameText.GetComponent<Text>();
        messageText = messageText.GetComponent<Text>();

        reservedList();

        nameArray[0] = "main";
        formatArray[0] = 0;
        optionsList.Add("main");
        RefreshOptions();
    }

    void reservedList()
    {
        reservedWord.Add("auto");
        reservedWord.Add("break");
        reservedWord.Add("case");
        reservedWord.Add("char");
        reservedWord.Add("const");
        reservedWord.Add("continue");
        reservedWord.Add("default");
        reservedWord.Add("do");
        reservedWord.Add("double");
        reservedWord.Add("else");
        reservedWord.Add("enum");
        reservedWord.Add("exturn");
        reservedWord.Add("float");
        reservedWord.Add("for");
        reservedWord.Add("goto");
        reservedWord.Add("if");
        reservedWord.Add("int");
        reservedWord.Add("long");
        reservedWord.Add("resister");
        reservedWord.Add("return");
        reservedWord.Add("signed");
        reservedWord.Add("sizeof");
        reservedWord.Add("short");
        reservedWord.Add("static");
        reservedWord.Add("struct");
        reservedWord.Add("switch");
        reservedWord.Add("typedef");
        reservedWord.Add("union");
        reservedWord.Add("unsigned");
        reservedWord.Add("void");
        reservedWord.Add("volatile");
        reservedWord.Add("while");
    }

    public void EnterButtonClicked()
    {
        //inputfieldの入力を確定
        ValueChanged();

        //それぞれ入力された値を代入
        functionFormat = FormatDropdown.value;
        functionName = nameText.text.ToString();

        foreach(string a in nameArray)
        {
            if(functionName == a)
            {
                messageText.text =
                    "同じ名前の関数は作ることができません";
                ResetInputField();
                return;
            }
        }
        foreach(char c in functionName)
        {
            if(!checkLetter(c))
            {
                messageText.text =
                    "関数名には半角英数字または_(アンダーバー)のみ使用できます";
                ResetInputField();
                return;
            }
        }
        char init;
        init = functionName[0];
        if('0' <= init && init <= '9')
        {
            messageText.text =
                "関数名の頭文字に数字は使用できません";
            ResetInputField();
            return;
        }
        foreach(string r in reservedWord)
        {
            if(functionName==r)
            {
                messageText.text =
                    "関数名に予約語を用いることはできません";
                ResetInputField();
                return;
            }
        }

        nameArray[functionCount] = functionName;
        formatArray[functionCount] = functionFormat;
        optionsList.Add(functionName);

        messageText.text =
            "関数" + functionName + "が追加されました";

        functionCount++;
        ResetInputField();
        RefreshOptions();
        return;
    }

    void RefreshOptions()
    {
        FunctionDropdown.ClearOptions();
        FunctionDropdown.AddOptions(optionsList);
    }

    void ResetInputField()
    {
        //inputfieldの中身をリセット
        NameInputField.text = "";
    }

    bool checkLetter(char c)
    {
        if('A' <= c && c <= 'Z')return true;
        else if('a' <= c && c <= 'z')return true;
        else if('0' <= c && c <= '9')return true;
        else if(c == '_') return true;
        else return false;
    }

    public void ValueChanged()
    {
        //inputfieldの入力を反映
        nameText.text = NameInputField.text;
    }
}
