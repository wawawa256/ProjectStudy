using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ふぉっきち

public class Coding : MonoBehaviour
{
    public static GameObject[,] objectArray;
    public static string[,] content;
    public static string[,] kata;
    public int maxColumn;
    public int maxRow;
    public int column;
    public int row;
    int x, y,ifcount,nullcheak;
    public string objectName;
    public static int intCount;
    public static int floatCount;
    public static int stringCount;
    public static string[] saveintvalueArray = new string[128];
    public static string[] saveintnameArray = new string[128];
    public static string[] savefloatvalueArray = new string[128];
    public static string[] savefloatnameArray = new string[128];
    public static string[] savestringvalueArray = new string[128];
    public static string[] savestringnameArray = new string[128];
    public Text Code;

    public void CodeButtonClicked()
    {
        //変数の初期化、ColumnとRow嫌いだからxyに変える
        x = 0;
        y = 0;
        ifcount = 0;
        nullcheak = 0;
        objectArray = ObjectCollection.objectArray;
        maxColumn = ObjectCollection.maxColumn;
        maxRow = ObjectCollection.maxRow;
        content = ObjectCollection.content;
        kata = ObjectCollection.kata;
        intCount = VarSetting.intCount;
        floatCount = VarSetting.floatCount;
        stringCount = VarSetting.stringCount;
        saveintnameArray = VarSetting.saveintnameArray;
        saveintvalueArray = VarSetting.saveintvalueArray;
        savefloatnameArray = VarSetting.savefloatnameArray;
        savefloatvalueArray = VarSetting.savefloatvalueArray;
        savestringnameArray = VarSetting.savestringnameArray;
        savestringvalueArray = VarSetting.savestringvalueArray;
        Code = Code.GetComponent<Text>();

        //最初の決り文句みたいなやつ入れる
        Code.text = null;
        Code.text = "#include<stdio.h>\n" +
                    "int main(void){\n";

        for (int i = 0; i < intCount; i++)
        {
           space(spacecount(1));
           Code.text += "int " +
           saveintnameArray[i] + " = " +
           saveintvalueArray[i] + ";\n";
        }
        for (int i = 0; i < floatCount; i++)
        {
            space(spacecount(1));
            Code.text += "float " +
            savefloatnameArray[i] + " = " +
            savefloatvalueArray[i] + ";\n";
        }
        for (int i = 0; i < stringCount; i++)
        {
            space(spacecount(1));
            Code.text += "char " +
            savestringnameArray[i] + "[] = " +'"'+
            savestringvalueArray[i]+'"' + ";\n";
        }
            //上から順に調べていく、左下まで行ったら押しまい、でも必要な内容が記述されてない場合はコーディングしない
            while ((((x != 0) || (y != maxRow)) && (nullcheak == 0)))
        {
            CodingCheck();
        }
        //決り文句入れて終了

        if (nullcheak == 1)
        {
            Code.text = null;
            Code.text =( "入力された情報が不十分なブロックが存在します");
        }
        else
        {
            Code.text += "\t\treturn 0;\n";
            Code.text += "}";
        }

        GUIUtility.systemCopyBuffer = Code.text;
    }

    //中身見て出力するだけ
    public void Code_Printf()
    {
        space(spacecount(1));
        switch (kata[x, y])
        {
            case " ":
                Code.text += "printf(" + '"' + content[x, y] + '"' + ")" + ";";
                Code.text += "\n";
                y++;
                break;

            case "int":
                Code.text += "printf(" + '"' + "%d" + '"' +","+content[x,y]+ ")" + ";";
                Code.text += "\n";
                y++;
                break;
            case "float":
                Code.text += "printf(" + '"' + "%f" + '"' + "," + content[x, y] + ")" + ";";
                Code.text += "\n";
                y++;
                break;
            case "char":
                Code.text += "printf(" + '"' + "%s" + '"' + "," + content[x, y] + ")" + ";";
                Code.text += "\n";
                y++;
                break;
            default:
                y++;
                break;
        }
    }
    //下方向にどんどん進んで、何があるか調べる
    public void CodingCheck()
    {
        switch (objectArray[x, y].name)
        {
            case "Printf_prefab":
                Code_Printf();
                // Debug.Log(maxRow);
                break;

            case "If_prefab":
                Code_If();
                break;
            case "Calc_prefab":
                Code_Calc();
                break;
            case "ForStart_prefab":
                Code_For();
                break;
            default:
                y++;
                break;

        }
    }

    public void Code_If()
    {
        // 条件が空欄じゃないことを確認する
        if (content[x, y] == null || content[x, y] == "") nullcheak = 1;
        //ifの開始座標を記録する変数を用意
        int imaif;
        imaif = y;
        //コーディングのときの見栄えを良くする
        space(spacecount(1));
        ifcount++;
        y++;
        Code.text += "if(" + content[x, y - 1] + ")" + "{\n";

        //まずはtrueの方を調べる、コーナ1prefabが見つかるまで探索する

        while (objectArray[x, y].name != "Corner1_prefab")
        {
            //コーディングチェックで下方向に探索、またifが見つかったら再帰呼び出しみたいになる
            CodingCheck();
        }
        //コーナ1まで行ったら、最初の開始地点まで戻る
        y = imaif;
        space(spacecount(0));
        Code.text += "}else{\n";
        //ifの開始地点から否定側の分が横にどのくらいずれているかを確認する
        int scatterx = 1;        
        //否定側は、（横だけ→縦だけ）または(ifprefab→たてだけ）のどっちか
        while (!((objectArray[x + 1, y].name == "Tatedake_prefab") && ((objectArray[x, y].name == "If_prefab") || (objectArray[x, y].name == "Yokodake_prefab"))))
        {
            scatterx++;
            x++;
        }
        x++;
        y++;
        //コーナ2が見つかるまで下方向に探索
            while (objectArray[x, y].name != "Corner2_prefab")
        {
            CodingCheck();
        }
        //最初のifのx座標をあわせるためにオーメンに任せる
        x=x-scatterx;
        //1マス下に行くと、ifを離脱する
        y++;
        space(spacecount(0));
        Code.text += "}\n";
        ifcount -=1;
    }

    public void Code_Calc()
    {
        if (content[x, y] == null || content[x, y] == "") nullcheak = 1;
        space(spacecount(1));
        Code.text += content[x,y];
        Code.text += ";\n";
        y++;
    }

    public void Code_For()
    {
        if (content[x, y] == null || content[x, y] == "") nullcheak = 1;
        space(spacecount(1));
        ifcount++;
        Code.text += "for(" + content[x, y] + ")" + "{\n";
        y++;
        while (objectArray[x, y].name != "ForEnd_prefab")
        {
            CodingCheck();
        }
        y++;
        space(spacecount(0));
        Code.text += "}\n";
        ifcount -= 1;
    }

    //任意の数だけ\tしてくれる
    public void space(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Code.text+="\t";
        }
    }

    //ifの数によって、\tの数を見やすいように調整してくれる
    public int spacecount(int x)
    {
        return 1+ x + ifcount;

    }
}
