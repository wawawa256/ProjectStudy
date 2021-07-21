using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VarSetting : MonoBehaviour
{
    public VarCollection[] intVarArray = new VarCollection[128];
    public VarCollection[] floatVarArray = new VarCollection[128];
    public VarCollection[] stringVarArray = new VarCollection[128];
    public string[] nameArray = new string[256];
    public static string[] saveintvalueArray = new string[128];
    public static string[] saveintnameArray = new string[128];
    public static string[] savefloatvalueArray = new string[128];
    public static string[] savefloatnameArray = new string[128];
    public static string[] savestringvalueArray = new string[128];
    public static string[] savestringnameArray = new string[128];

    public InputField nameInputField;
    public InputField valueInputField;
    public Dropdown formatDropDown;
    public Text nameText;
    public Text valueText;
    public Text varList;
    public Text messageText;

    public static int varFormat;
    public static string varName;
    public static string varValue;

    public static string stringValue;
    public static int intValue;
    public static float floatValue;

    public static int intCount;
    public static int floatCount;
    public static int stringCount;
    public static int varCount;

    void Start()
    {
        //作った変数の数を数えてるやつを初期化
        intCount = 0;
        floatCount = 0;
        stringCount = 0;
        varCount = 0;

        //それぞれのUIのコンポーネントを取得
        nameInputField = nameInputField.GetComponent<InputField>();
        valueInputField = valueInputField.GetComponent<InputField>();
        nameText = nameText.GetComponent<Text>();
        valueText = valueText.GetComponent<Text>();
        varList = varList.GetComponent<Text>();
        messageText = messageText.GetComponent<Text>();

        //変数を入れる配列のインスタンスを作る
        makeInstance();
    }

    void makeInstance()
    {
        int i;
        for(i = 0;i < 128;i++)　intVarArray[i] = new VarCollection();
        for(i = 0;i < 128;i++)　floatVarArray[i] = new VarCollection();
        for(i = 0;i < 128;i++)　stringVarArray[i] = new VarCollection();
    }

    public void EnterButtonClicked()
    {
        //inputfieldの入力を確定
        ValueChanged();

        //それぞれ入力された値を代入
        varFormat = formatDropDown.value;
        varName = nameText.text.ToString();

        foreach(string a in nameArray)
        {
            if(varName == a)
            {
                messageText.text =
                    "同じ名前の変数は作ることができません";
                ResetInputField();
                return;
            }
        }
        foreach(char c in varName)
        {
            if(!checkLetter(c))
            {
                messageText.text =
                    "変数には半角英数字または_(アンダーバー)のみ使用できます";
                ResetInputField();
                return;
            }
        }
        char init;
        init = varName[0];
        if('0' <= init && init <= '9')
        {
            messageText.text =
                "変数の頭文字に数字は使用できません";
            ResetInputField();
            return;
        }
        varValue = valueText.text.ToString();

        //変数の作成
        if(MakeVar(varFormat,varValue))
        {
            nameArray[varCount] = varName;
            varCount++;
        }
        ResetInputField();
    }

    void ResetInputField()
    {
        //inputfieldの中身をリセット
        nameInputField.text = "";
        valueInputField.text = "";
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
        nameText.text = nameInputField.text;
        valueText.text = valueInputField.text;
    }

    bool MakeVar(int format,string value)
    {
        switch(format)
        {
            //int型を選択した場合
            case 0:
                //int型以外を入力された時の例外処理
                //+変換後の値をintValueに代入
                if(!int.TryParse(value,out intValue))
                {
                    messageText.text =
                        value + "は整数型には変換できません";
                    return false;
                }
                intVarArray[intCount].originalValue = value;
                intVarArray[intCount].intValue = intValue;
                intVarArray[intCount].varName = varName;
                saveintvalueArray[intCount] = value;
                saveintnameArray[intCount] = varName;
                intCount++;
                return true;

            //float型を選択した場合
            case 1:
                //float型以外を入力された時の例外処理
                //+変換後の値をfloatValueに代入
                if(!float.TryParse(value,out floatValue))
                {
                    messageText.text =
                        value + "は小数型には変換できません";
                    return false;
                }
                floatVarArray[floatCount].originalValue = value;
                floatVarArray[floatCount].floatValue = floatValue;
                floatVarArray[floatCount].varName = varName;
                savefloatvalueArray[floatCount] = value;
                savefloatnameArray[floatCount] = varName;
                floatCount++;
                return true;

            //char型を選択した場合
            case 2:
                stringValue = value;
                stringVarArray[stringCount].originalValue = value;
                stringVarArray[stringCount].stringValue = stringValue;
                stringVarArray[stringCount].varName = varName;
                savestringvalueArray[stringCount] = value;
                savestringnameArray[stringCount] = varName;
                stringCount++;
                return true;
            default:
                return false;
        }
    }

    public void saveVar()
    {
        //int配列をストレージに保存
        for(int i=0; i<intCount; i++)
        {
            Debug.Log(saveintvalueArray[i]);
            Debug.Log(intCount);
            PlayerPrefs.SetString("intvalue" + i, saveintvalueArray[i]);
            PlayerPrefs.SetString("intname" + i, saveintnameArray[i]);
        }

        //float配列をストレージに保存
        for (int i = 0; i < floatCount; i++)
        {
            PlayerPrefs.SetString("floatvalue" + i, savefloatvalueArray[i]);
            PlayerPrefs.SetString("floatname" + i, savefloatnameArray[i]);
        }

        //string配列をストレージに保存
        for (int i = 0; i < stringCount; i++)
        {
            PlayerPrefs.SetString("stringvalue" + i, savestringvalueArray[i]);
            PlayerPrefs.SetString("stringname" + i, savestringnameArray[i]);
        }

        //変数の数をストレージへ保存

        PlayerPrefs.SetInt("intCount", intCount);
        PlayerPrefs.SetInt("floatCount", floatCount);
        PlayerPrefs.SetInt("stringCount", stringCount);
        PlayerPrefs.Save();
    }

    public void loadVar()
    {
        intCount = PlayerPrefs.GetInt("intCount", 0);
        floatCount = PlayerPrefs.GetInt("floatCount", 0);
        stringCount = PlayerPrefs.GetInt("stringCount", 0);
        for (int i = 0;i < intCount; i++)
        {
            varFormat = 0;
            loadmakevar(i, varFormat, PlayerPrefs.GetString("intvalue" + i, null), PlayerPrefs.GetString("intname" + i, null));

        }
        for (int i = 0; i < floatCount; i++)
        {
            varFormat = 1;
            loadmakevar(i, varFormat, PlayerPrefs.GetString("floatvalue" + i, null), PlayerPrefs.GetString("floatname" + i, null));
        }
        for (int i = 0; i <stringCount; i++)
        {
            varFormat = 2;
            loadmakevar(i, varFormat, PlayerPrefs.GetString("stringvalue" + i, null), PlayerPrefs.GetString("stringname" + i, null));
        }
    }

    public void loadmakevar(int count,int format,string value,string name)
    {
        switch (format)
        {
            case 0:

                int.TryParse(value, out intValue);
                intVarArray[count].originalValue = value;
                intVarArray[count].intValue = intValue;
                intVarArray[count].varName = name;
                saveintvalueArray[count] = value;
                saveintnameArray[count] = name;

                break;

            case 1:
                float.TryParse(value, out floatValue);
                floatVarArray[count].originalValue = value;
                floatVarArray[count].floatValue = floatValue;
                floatVarArray[count].varName = name;
                savefloatvalueArray[count] = value;
                savefloatnameArray[count] = name;

                break;

            case 2:
                stringValue = value;
                stringVarArray[count].originalValue = value;
                stringVarArray[count].stringValue = stringValue;
                stringVarArray[count].varName = name;
                savestringvalueArray[count] = value;
                savestringnameArray[count] = name;

                break;

            default:
                break;
        }
        //Debug.Log("うひひ");
    }


    public void VarListButtonClicked()
    {
        int i;
        varList.text = "\n";
        for(i = 0;i < intCount;i++)
        {
            varList.text += "int " +
                intVarArray[i].varName + " = " +
                intVarArray[i].originalValue + "\n";
        }
        for(i = 0;i < floatCount;i++)
        {
            varList.text += "float " +
                floatVarArray[i].varName + " = " +
                floatVarArray[i].originalValue + "\n";
        }
        for(i = 0;i < stringCount;i++)
        {
            varList.text += "char[] " +
                stringVarArray[i].varName + " = " +
                stringVarArray[i].originalValue + "\n";
        }
    }

    //こっから
    public Dropdown VarDropdownPrintf;
    public Dropdown VarDropdownCalc1;
    public Dropdown VarDropdownCalc2;
    public Dropdown VarDropdownCalc3;
    public Dropdown VarDropdownIf1;
    public Dropdown VarDropdownIf2;


    public void UpdateVarDropdown(){ //vardropdownが呼び出されるたびに呼び出せばいいかなあ、、、の気持ち。 contentメニュー開くのと同時に呼び出す
        //削除が万が一追加されたときに備えて毎回変数リスト全読み込みしようかなあの気持ち
        VarDropdownPrintf.ClearOptions();
        VarDropdownIf1.ClearOptions();
        VarDropdownIf2.ClearOptions();
        VarDropdownCalc1.ClearOptions();
        VarDropdownCalc2.ClearOptions();
        VarDropdownCalc3.ClearOptions();
        List<string> list = new List<string>();
        list.Add("変数一覧");
        for(int i=0;i<intCount;i++){
            list.Add("int "+intVarArray[i].varName);
        }
        for(int i=0;i<floatCount;i++){
            list.Add("float "+floatVarArray[i].varName);
        }
        for(int i=0;i<stringCount;i++){
            list.Add("char[] "+stringVarArray[i].varName);
        }
        VarDropdownPrintf.AddOptions(list);
        VarDropdownPrintf.value = 0; //set default...
        VarDropdownIf1.AddOptions(list);
        VarDropdownIf1.value=0;
        VarDropdownIf2.AddOptions(list);
        VarDropdownIf2.value=0;
        VarDropdownCalc1.AddOptions(list);
        VarDropdownCalc1.value=0;
        VarDropdownCalc2.AddOptions(list);
        VarDropdownCalc2.value=0;
        VarDropdownCalc3.AddOptions(list);
        VarDropdownCalc3.value=0;
    }

    public string whatisthis(int xth){
        if(xth>intCount){
            xth -= intCount;
        }else{
            return "int "+intVarArray[xth-1].varName;
        }
        if(xth>floatCount){
            xth -= floatCount;
        }else{
            return "float "+floatVarArray[xth-1].varName;
        }
        return "char[] "+stringVarArray[xth-1].varName;
    }

    public string watchthis(int xth){
        if(xth>intCount){
            xth -= intCount;
        }else{
            return "int";
        }
        if(xth>floatCount){
            xth -= floatCount;
        }else{
            return "float";
        }
        return "char";
    }

    public string youshouldrun(int xth){
        if(xth>intCount){
            xth -= intCount;
        }else{
            return intVarArray[xth-1].varName;
        }
        if(xth>floatCount){
            xth -= floatCount;
        }else{
            return floatVarArray[xth-1].varName;
        }
        return stringVarArray[xth-1].varName;
    }
}

public class VarCollection
{
    //変数名
    public string varName;
    //変数の値
    public int intValue;
    public float floatValue;
    public string stringValue;
    //入力された文字列
    //変数の型に変換した後でstring型に戻すのは面倒
    public string originalValue;
}
