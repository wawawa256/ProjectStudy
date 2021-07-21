using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ふぉっきち

public class Coding : MonoBehaviour
{
    public static GameObject[,] objectArray;
    public static string[,] content;
    public int maxColumn;
    public int maxRow;
    public int column;
    public int row;
    int x, y,ifcount;
    public string objectName;

    public Text Code;

    public void CodeButtonClicked()
    {
        //変数の初期化、ColumnとRow嫌いだからxyに変える
        x = 0;
        y = 0;
        ifcount = 0;
        objectArray = ObjectCollection.objectArray;
        maxColumn = ObjectCollection.maxColumn;
        maxRow = ObjectCollection.maxRow;
        content = ObjectCollection.content;

        Code = Code.GetComponent<Text>();
        //最初の決り文句みたいなやつ入れる
        Code.text = null;
        Code.text = "#include<stdio.h>\n" +
                    "int main(void){\n";
        //上から順に調べていく、左下まで行ったら押しまい
        while ((x != 0) || (y != maxRow))
        {
            CodingCheck();
        }
        //決り文句入れて終了
        Code.text += "\treturn 0;\n";
        Code.text += "}";
    }

    //中身見て出力するだけ
    public void Code_Printf()
    {
        space(spacecount(1));
        Code.text += "printf(" + '"'+ content[x, y] + '"' +  ")" + ";";
        Code.text += "\n";
        y++;
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

            default:
                y++;
                break;

        }
    }

    public void Code_If()
    {

       // int ifxcount;
        int ifycount;
       //ifxcount = 0;
        ifycount = 0;
        y++;
        space(spacecount(1));
        ifcount++;
        Code.text += "if(" + content[x, y - 1] + ")" + "{\n";

        //まずはtrueの方を調べる、コーナ1prefabが見つかるまで探索する

        while (objectArray[x, y].name != "Corner1_prefab")
        {
            //コーディングチェックで下方向に探索、またifが見つかったら再帰呼び出しみたいになる
            CodingCheck();
            //何個調べたかカウントする
            ifycount++;
        }
        //カウントした分上に戻って、1ます横に行き、false側でも同じことをする
        y = y - ifycount;
        x++;
        ifycount = 0;
        space(spacecount(0));
        Code.text += "}else{\n";
        while (objectArray[x, y].name != "Corner2_prefab")
        {
            CodingCheck();
        }
        x--;
        y++;
        space(spacecount(0));
        Code.text += "}\n";
        ifcount -=1;
    }

    public void Code_Calc()
    {
        space(spacecount(1));
        Code.text += content[x,y];
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