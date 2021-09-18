using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//ふぉっきち

public class Coding : MonoBehaviour
{
    public static GameObject[,] objectArray;
    public static string[,] content;
    public static string[,] kata;
    public static int[] maxColumn= new int[128];
    public static int[] maxRow = new int[128];
    public int column;
    public int row;
    int x, y,ifcount,nullcheak;
    public string objectName;
    public static int intCount;
    public static int floatCount;
    public static int stringCount;
    public static int functionCount;
    public static int codingflag = 0;
    public static string[] saveintvalueArray = new string[128];
    public static string[] saveintnameArray = new string[128];
    public static string[] savefloatvalueArray = new string[128];
    public static string[] savefloatnameArray = new string[128];
    public static string[] savestringvalueArray = new string[128];
    public static string[] savestringnameArray = new string[128];
    public static string[,,] functionArray = new string[64, 128, 256];
    public static string[,,] contentPlus = new string[64, 64, 128];
    public static string[,,] kataPlus = new string[64, 64, 128];
    public static string[] nameArray = new string[256];
    public static int[] formatArray = new int[256];
    public static int[,] argsFormatArray = new int[64, 128];
    public static string[,] argsNameArray = new string[64, 128];
    public static int[] argsCount = new int[64];
    public Text Code;
    DateTime TodayNow;
    public void CodeButtonClicked()
    {
        //変数の初期化、ColumnとRow嫌いだからxyに変える
        x = 0;
        y = 0;
        ifcount = 0;
        nullcheak = 0;
        codingflag = ObjectCollection.codingflag;
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
        functionArray = ObjectCollection.functionArray;
        contentPlus = ObjectCollection.contentPlus;
        kataPlus = ObjectCollection.kataPlus;
        functionCount = Subroutine.functionCount;
        nameArray = Subroutine.nameArray;
        formatArray = Subroutine.formatArray;
        argsFormatArray = VarSetting.argsFormatArray;
        argsNameArray = VarSetting.argsNameArray;
        argsCount = VarSetting.argsCount;

        Code = Code.GetComponent<Text>();

        //最初の決り文句みたいなやつ入れる
        TodayNow = DateTime.Now;
        Code.text = null;
        Code.text += "//"+TodayNow.Year.ToString() + "年 " + TodayNow.Month.ToString() + "月" + TodayNow.Day.ToString() + "日" + DateTime.Now.ToLongTimeString() +"\n\n\n";
        Code.text += "#include<stdio.h>\n";


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
        for (int i = 1; i < functionCount; i++)
        {
            Subroutine_Coding(i);
        }


        Code.text += "int main(void){\n";
        x = 0;
        y = 0;
        nullcheak = 0;
        ifcount = 0;
        //上から順に調べていく、左下まで行ったら押しまい、でも必要な内容が記述されてない場合はコーディングしない
        while ((((x != 0) || (y != maxRow[0])) && (nullcheak == 0)))
        {
            CodingCheck(0);
        }
        //決り文句入れて終了

        if (nullcheak == 1)
        {
            Code.text = null;
            Code.text =( "\n\n入力された情報が不十分なブロックが存在します\nif,calc,forを確認してください");
        }
        else
        {
            Code.text += "}";
        }

        GUIUtility.systemCopyBuffer = Code.text;
        Debug.Log(Code.text);
        WebPost.sourceCODE=Code.text;
    }

    public void Subroutine_Coding(int function)
    {
        x = 0;
        y = 0;
        nullcheak = 0;
        ifcount = 0;
        Code.text += FormatToStr(formatArray[function]) + " " + nameArray[function] + "(" + argset(function) + "){\n";
        while ((((x != 0) || (y != maxRow[function])) && (nullcheak == 0)))
        {
            CodingCheck(function);
        }
        space(spacecount(0));
        Code.text += "}\n";
    }

    string argset(int function)
    {
        string args = null;
        for (int i=0; i < argsCount[function]; i++)
        {
            if (i==0)  args += FormatToStr(argsFormatArray[function,i]) + " " + argsNameArray[function, i];      
            else args +=", " + FormatToStr(argsFormatArray[function, i]) + " " + argsNameArray[function, i];

        }
        return args;
    }
    string FormatToStr(int format)
    {
        switch (format)
        {
            case 0:
                return "int";
            case 1:
                return "float";
            case 2:
                return "bool";
            case 3:
                return "void";
        }
        return "void";
    }

    public void CodingCheck(int i)
    {
        switch (functionArray[i,x,y])
        {
            case "Printf_prefab":
                Code_Printf(i);
                break;
            case "WhileStart_prefab":
                Code_While(i);
                break;
            case "If_prefab":
                Code_If(i);
                break;
            case "Calc_prefab":
                Code_Calc(i);
                break;
            case "ForStart_prefab":
                Code_For(i);
                break;
            case "Subroutine_prefab":
                Code_Subrutine(i);
                break;
            case "Return_prefab":
                Code_Return(i);
                break;
            default:
                y++;
                break;

        }
    }

    public void Code_Return(int i)
    {
        space(spacecount(1));
        Code.text += contentPlus[i, x, y] + ";\n";
        y++;
    }

    void Code_For(int i)
    {
        if (contentPlus[i,x, y] == null || contentPlus[i,x, y] == "") nullcheak = 1;
        space(spacecount(1));
        ifcount++;
        Code.text += "for(" + contentPlus[i,x, y] + ")" + "{\n";
        y++;
        while (functionArray[i,x, y] != "ForEnd_prefab")
        {
            CodingCheck(i);
        }
        y++;
        space(spacecount(0));
        Code.text += "}\n";
        ifcount -= 1;
    }

    void Code_While(int i)
    {
        if (contentPlus[i, x, y] == null || contentPlus[i, x, y] == "") nullcheak = 1;
        space(spacecount(1));
        ifcount++;
        Code.text += "while(" + contentPlus[i, x, y] + ")" + "{\n";
        y++;
        while (functionArray[i, x, y] != "WhileEnd_prefab")
        {
            CodingCheck(i);
        }
        y++;
        space(spacecount(0));
        Code.text += "}\n";
        ifcount -= 1;
    }

    void Code_Calc(int i)
    {
        if (contentPlus[i,x, y] == null || contentPlus[i,x, y] == "") nullcheak = 1;
        space(spacecount(1));
        Code.text += contentPlus[i,x, y];
        Code.text += ";\n";
        y++;
    }
    public void Code_If(int i)
    {
        // 条件が空欄じゃないことを確認する
        if (contentPlus[i,x, y] == null || contentPlus[i,x, y] == "") nullcheak = 1;
        //ifの開始座標を記録する変数を用意
        int imaif;
        imaif = y;
        //コーディングのときの見栄えを良くする
        space(spacecount(1));
        ifcount++;
        y++;
        Code.text += "if(" + contentPlus[i,x, y - 1] + ")" + "{\n";

        //まずはtrueの方を調べる、コーナ1prefabが見つかるまで探索する

        while (functionArray[i,x, y]!= "Corner1_prefab")
        {
            //コーディングチェックで下方向に探索、またifが見つかったら再帰呼び出しみたいになる
            CodingCheck(i);
        }
        //コーナ1まで行ったら、最初の開始地点まで戻る
        y = imaif;
        space(spacecount(0));
        Code.text += "}else{\n";
        //ifの開始地点から否定側の分が横にどのくらいずれているかを確認する
        int scatterx = 1;
        //否定側は、（横だけ→縦だけ）または(ifprefab→たてだけ）のどっちか
        while (!((functionArray[i,x + 1, y] == "Tatedake_prefab") && ((functionArray[i,x, y] == "If_prefab") || (functionArray[i,x, y] == "Yokodake_prefab"))))
        {
            scatterx++;
            x++;
        }
        x++;
        y++;
        //コーナ2が見つかるまで下方向に探索
        while (functionArray[i,x, y] != "Corner2_prefab")
        {
            CodingCheck(i);
        }
        //最初のifのx座標をあわせるためにオーメンに任せる
        x = x - scatterx;
        //1マス下に行くと、ifを離脱する
        y++;
        space(spacecount(0));
        Code.text += "}\n";
        ifcount -= 1;
    }
    //中身見て出力するだけ

    public void Code_Printf(int i)
    {
        space(spacecount(1));
        switch (kataPlus[i,x, y])
        {
            case " ":
                Code.text += "printf(" + '"' + contentPlus[i,x, y] + '"' + ")" + ";";
                Code.text += "\n";
                y++;
                break;

            case "int":
                Code.text += "printf(" + '"' + "%d" + '"' + "," + contentPlus[i,x, y] + ")" + ";";
                Code.text += "\n";
                y++;
                break;
            case "float":
                Code.text += "printf(" + '"' + "%f" + '"' + "," + contentPlus[i,x, y] + ")" + ";";
                Code.text += "\n";
                y++;
                break;
            case "char":
                Code.text += "printf(" + '"' + "%s" + '"' + "," + contentPlus[i,x, y] + ")" + ";";
                Code.text += "\n";
                y++;
                break;
            default:
                Debug.Log("kataplisがだめだよ");
                y++;
                break;
        }
    }

    public void Code_Subrutine(int i)
    {
        space(spacecount(1));
        Code.text += contentPlus[i, x, y];
        Code.text += "\n";
        y++;
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
