using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VarSetting : MonoBehaviour
{
    //グローバル変数
    public static VarCollection[] intVarArray = new VarCollection[128];
    public static VarCollection[] floatVarArray = new VarCollection[128];
    public static VarCollection[] stringVarArray = new VarCollection[128];
    public static string[] nameArray = new string[256];
    public static int intCount;
    public static int floatCount;
    public static int stringCount;
    public static int varCount;

    //グローバル変数のセーブ
    public static string[] saveintvalueArray = new string[128];
    public static string[] saveintnameArray = new string[128];
    public static string[] savefloatvalueArray = new string[128];
    public static string[] savefloatnameArray = new string[128];
    public static string[] savestringvalueArray = new string[128];
    public static string[] savestringnameArray = new string[128];

    //ローカル変数
    public static VarCollection[,] intVarArrayPlus = new VarCollection[64,32];
    public static VarCollection[,] floatVarArrayPlus = new VarCollection[64,32];
    public static VarCollection[,] stringVarArrayPlus = new VarCollection[64,32];
    public static string[,] nameArrayPlus = new string[64,128];
    public static int[] intCountPlus = new int[64];
    public static int[] floatCountPlus = new int[64];
    public static int[] stringCountPlus = new int[64];
    public static int[] varCountPlus = new int[64];

    //引数
    public static int[,] argsFormatArray = new int [64,128];
    public static string[,] argsNameArray = new string[64,128];
    public static int[] argsCount = new int[64];


    public InputField nameInputField;
    public InputField valueInputField;
    public Dropdown formatDropdown;
    public Text nameText;
    public Text valueText;

    public InputField argsNameInputField;
    public Dropdown argsFormatDropdown;
    public Text argsNameText;
    public Text argsNumberText;
    
    public Text varList;
    public Text messageText;
    public Text SwitchText;

    public static int varFormat;
    public static string varName;
    public static string varValue;

    public static string stringValue;
    public static int intValue;
    public static float floatValue;

    static int func;

    bool GlobalOrLocal;

    List<string> reservedWord = new List<string>();

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

        argsNameInputField = argsNameInputField.GetComponent<InputField>();
        argsNameText = argsNameText.GetComponent<Text>();
        argsNumberText = argsNumberText.GetComponent<Text>();

        varList = varList.GetComponent<Text>();
        messageText = messageText.GetComponent<Text>();
        SwitchText = SwitchText.GetComponent<Text>();

        GlobalOrLocal = true;
        SwitchText.text = "Global";

        ResetArgsNumber();
        ResetInputField();
        ResetArgsInputField();

        //変数を入れる配列のインスタンスを作る
        makeInstance();
        reservedList();

        //でばっぐ
        varName = "a";
        MakeVar(0, "3");
        varName = "b";
        MakeVar(1, "1.2");

    }

    void makeInstance()
    {
        int i,j;
        for(i=0;i<128;i++)　intVarArray[i] = new VarCollection();
        for(i=0;i<128;i++)　floatVarArray[i] = new VarCollection();
        for(i=0;i<128;i++)　stringVarArray[i] = new VarCollection();
        for(i=0;i<64;i++)
        {
            for(j=0;j<32;j++)
            {
                intVarArrayPlus[i,j] = new VarCollection();
                floatVarArrayPlus[i,j] = new VarCollection();
                stringVarArrayPlus[i,j] = new VarCollection();
            }
        }
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

    public void SwitchButtonClicked()
    {
        //true:Global
        //false:Local

        GlobalOrLocal = !GlobalOrLocal;
        if(GlobalOrLocal)
        {
            SwitchText.text = "Global";
        }
        else
        {
            SwitchText.text = "Local";
        }
    }

    public void EnterButtonClicked()
    {
        int i;
        func = ObjectCollection.CurrentFunction;
        //inputfieldの入力を確定
        ValueChanged();

        //それぞれ入力された値を代入
        varFormat = formatDropdown.value;
        varName = nameText.text.ToString();

        //実際のところグローバル変数とローカル変数に同じ名前をつけることはできる
        //ただ衝突するとめっちゃわかりにくいので例外処理する
        foreach(string a in nameArray)
        {
            if(varName == a)
            {
                if(GlobalOrLocal)
                {
                    messageText.text =
                        "同じ名前のグローバル変数は作ることができません";
                }
                else
                {
                    messageText.text =
                        "グローバル変数と同じ名前のローカル変数は作ることができません";
                }
                ResetInputField();
                return;
            }
        }
        for(i=0;i<varCountPlus[func];i++)
        {
            if(varName == nameArrayPlus[func,i])
            {
                messageText.text =
                    "同一の関数において同じ名前のローカル変数を複数作ることができません";
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
        foreach(string r in reservedWord)
        {
            if(varName==r)
            {
                messageText.text =
                    "変数に予約語を用いることはできません";
                ResetInputField();
                return;
            }
        }

        varValue = valueText.text.ToString();

        //変数の作成
        if(MakeVar(varFormat,varValue))
        {
            string type;
            if(GlobalOrLocal) type = "グローバル変数";
            else type = "ローカル変数";
            messageText.text =
                type + varName + "が追加されました";
        }
        ResetInputField();
        return;
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
                if(GlobalOrLocal)
                {
                    intVarArray[intCount].originalValue = value;
                    intVarArray[intCount].intValue = intValue;
                    intVarArray[intCount].varName = varName;
                    saveintvalueArray[intCount] = value;
                    saveintnameArray[intCount] = varName;
                    intCount++;

                    nameArray[varCount] = varName;
                    varCount++;
                }
                else
                {
                    intVarArrayPlus[func,intCountPlus[func]].originalValue = value;
                    intVarArrayPlus[func,intCountPlus[func]].intValue = intValue;
                    intVarArrayPlus[func,intCountPlus[func]].varName = varName;
                    intCountPlus[func]++;

                    nameArrayPlus[func,varCountPlus[func]] = varName;
                    varCountPlus[func]++;
                }
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
                if(GlobalOrLocal)
                {
                    floatVarArray[floatCount].originalValue = value;
                    floatVarArray[floatCount].floatValue = floatValue;
                    floatVarArray[floatCount].varName = varName;
                    savefloatvalueArray[floatCount] = value;
                    savefloatnameArray[floatCount] = varName;
                    floatCount++;

                    nameArray[varCount] = varName;
                    varCount++;
                }
                else
                {
                    floatVarArrayPlus[func,floatCountPlus[func]].originalValue = value;
                    floatVarArrayPlus[func,floatCountPlus[func]].floatValue = floatValue;
                    floatVarArrayPlus[func,floatCountPlus[func]].varName = varName;
                    floatCountPlus[func]++;

                    nameArrayPlus[func,varCountPlus[func]] = varName;
                    varCountPlus[func]++;
                }
                return true;

            
            //char型を選択した場合
            case 2:
                stringValue = value;
                if(GlobalOrLocal)
                {
                    stringVarArray[stringCount].originalValue = value;
                    stringVarArray[stringCount].stringValue = stringValue;
                    stringVarArray[stringCount].varName = varName;
                    savestringvalueArray[stringCount] = value;
                    savestringnameArray[stringCount] = varName;
                    stringCount++;

                    nameArray[varCount] = varName;
                    varCount++;
                }
                else
                {
                    stringVarArrayPlus[func,stringCountPlus[func]].originalValue = value;
                    stringVarArrayPlus[func,stringCountPlus[func]].stringValue = stringValue;
                    stringVarArrayPlus[func,stringCountPlus[func]].varName = varName;
                    stringCountPlus[func]++;

                    nameArrayPlus[func,varCountPlus[func]] = varName;
                    varCountPlus[func]++;
                }
                return true;
            default:
                return false;
        }
    }

    public void ArgsValueChanged()
    {
        argsNameText.text = argsNameInputField.text;
    } 
    public void ResetArgsInputField()
    {
        argsNameInputField.text = "";
    }
    public void ResetArgsNumber()
    {
        argsNumberText.text = "第1引数 :";
    }

    public void ArgsSet()
    {
        //とりあえずはtextごろごろ変えながらほぼAddVarと同じでいいっしょ
        //変数をargsArrayに登録させまくる
        //変数名が衝突しないか大事
        //将来的なことも考えてローカルと衝突しないかも確認
        int i;
        int argsFormat;
        string argsName;
        func = ObjectCollection.functionCount;
        //inputfieldの入力を確定
        ArgsValueChanged();

        //それぞれ入力された値を代入
        argsFormat = argsFormatDropdown.value;
        argsName = argsNameText.text.ToString();

        //実際のところグローバル変数とローカル変数に同じ名前をつけることはできる
        //ただ衝突するとめっちゃわかりにくいので例外処理する
        foreach(string a in nameArray)
        {
            if(argsName == a)
            {
                messageText.text =
                    "グローバル変数と同じ名前の引数は設定できません";
                ResetArgsInputField();
                return;
            }
        }
        for(i=0;i<argsCount[func];i++)
        {
            if(argsName == nameArrayPlus[func,i])
            {
                messageText.text =
                    "同じ関数のローカル変数と同じ名前の引数を設定することはできません";
                ResetArgsInputField();
                return;
            }
        }
        //引数に対してもサーチかける
        foreach(string a in argsNameArray)
        {
            if(argsName == a)
            {
                messageText.text =
                    "同じ関数の引数と同じ名前の引数を設定することはできません";
                ResetArgsInputField();
                return;
            }
        }

        foreach(char c in argsName)
        {
            if(!checkLetter(c))
            {
                messageText.text =
                    "引数には半角英数字または_(アンダーバー)のみ使用できます";
                ResetArgsInputField();
                return;
            }
        }
        char init;
        init = argsName[0];
        if('0' <= init && init <= '9')
        {
            messageText.text =
                "引数の頭文字に数字は使用できません";
            ResetArgsInputField();
            return;
        }
        foreach(string r in reservedWord)
        {
            if(argsName==r)
            {
                messageText.text =
                    "引数に予約語を用いることはできません";
                ResetArgsInputField();
                return;
            }
        }

        argsFormatArray[func,argsCount[func]] = argsFormat;
        argsNameArray[func,argsCount[func]] = argsName;

        argsCount[func]++;
        argsNumberText.text = "第"+ (argsCount[func]+1) +"引数 :";
        ResetArgsInputField();
        return;
    }
    public void CancelButtonClicked()
    {
        int i;
        func = ObjectCollection.functionCount;
        ResetArgsNumber();
        for(i=0;i<argsCount[func];i++)
        {
            argsFormatArray[func,argsCount[func]] = 0;
            argsNameArray[func,argsCount[func]] = null;
        }

        argsCount[func] = 0;
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
    }


    public void VarListButtonClicked()
    {
        func = ObjectCollection.CurrentFunction;
        int i;
        string type;

        varList.text = "\n\n";
        varList.text += "(Global)";
        varList.text += "\n";

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

        varList.text += "\n";
        varList.text += "(Local)";
        varList.text += "\n";

        for(i = 0;i < intCountPlus[func];i++)
        {
            varList.text += "int " +
                intVarArrayPlus[func,i].varName + " = " +
                intVarArrayPlus[func,i].originalValue + "\n";
        }
        for(i = 0;i < floatCountPlus[func];i++)
        {
            varList.text += "float " +
                floatVarArrayPlus[func,i].varName + " = " +
                floatVarArrayPlus[func,i].originalValue + "\n";
        }
        for(i = 0;i < stringCountPlus[func];i++)
        {
            varList.text += "char[] " +
                stringVarArrayPlus[func,i].varName + " = " +
                stringVarArrayPlus[func,i].originalValue + "\n";
        }

        varList.text += "\n";
        varList.text += "(Argument)";
        varList.text += "\n";


        for(i = 0;i < argsCount[func];i++)
        {
            switch(argsFormatArray[func,i])
            {
                case 0:
                    type = "int";
                    break;
                case 1:
                    type = "float";
                    break;
                case 2:
                    type = "char";
                    break;
                default:
                    type = "unknown";
                    break;
            }
            varList.text +=
                (i+1) + " : " + type + " " +
                argsNameArray[func,i] + "\n";
        }
    }

    //こっから
    public Dropdown VarDropdownPrintf;
    public Dropdown VarDropdownCalc1;
    public Dropdown VarDropdownCalc2;
    public Dropdown VarDropdownCalc3;
    public Dropdown VarDropdownIf1;
    public Dropdown VarDropdownIf2;
    public Dropdown VarDropdownWhile1;
    public Dropdown VarDropdownWhile2;


    public void UpdateVarDropdown(){ //vardropdownが呼び出されるたびに呼び出せばいいかなあ、、、の気持ち。 contentメニュー開くのと同時に呼び出す
        //削除が万が一追加されたときに備えて毎回変数リスト全読み込みしようかなあの気持ち
        VarDropdownPrintf.ClearOptions();
        VarDropdownIf1.ClearOptions();
        VarDropdownIf2.ClearOptions();
        VarDropdownCalc1.ClearOptions();
        VarDropdownCalc2.ClearOptions();
        VarDropdownCalc3.ClearOptions();
        VarDropdownWhile1.ClearOptions();
        VarDropdownWhile2.ClearOptions();
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
        VarDropdownWhile1.AddOptions(list);
        VarDropdownWhile2.AddOptions(list);
        VarDropdownWhile1.value=0;
        VarDropdownWhile2.value=0;
    }

    static public string whatisthis(int xth){
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

    static public string watchthis(int xth){
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

    //きるじょい
    static public string youshouldrun(int xth){
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
