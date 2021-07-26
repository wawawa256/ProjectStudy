using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectCollection : MonoBehaviour
{
    //オブジェクト配列の宣言
    public static GameObject[,] objectArray = new GameObject[64,128];
    public static string[,] SaveobjectArray = new string[64,128];

    public GameObject[,] wireArray = new GameObject[64,128];
    public GameObject[,] HorizontalwireArray = new GameObject[64,128];
    //nakamiiii
    public static string[,] content = new string[64,128];
    public static string[,] kata = new string[64,128];
    public Text messageText;

    Ifreference[] ifArray = new Ifreference[128];


    //オブジェクトのプロトタイプ宣言
    public GameObject CurrentPlace;
    public GameObject Printf_prefab;
    public GameObject If_prefab;
    public GameObject Blank_prefab;
    public GameObject Wire_prefab;
    public GameObject Wire_If_prefab;
    public GameObject Corner1_prefab;
    public GameObject Corner2_prefab;
    public GameObject ForStart_prefab;
    public GameObject ForEnd_prefab;
    public GameObject Calc_prefab;

    Camera mainCamera;

    //定数の宣言
    public const float VerticalSpace = -1.5f;
    public const float HorizontalSpace = 4.0f;
    public const float startX = 0f;
    public const float startY = 1.0f;

    //変数の宣言
    static float locationX;
    static float locationY;
    static float locationZ;
    public static int CurrentColumn;
    public static int CurrentRow;
    public static int maxColumn;
    public static int maxRow;
    public static int row;
    public static int column;
    public static bool CheckResult;
    public static bool whetherIf;
    public static int tempRow;
    public static int tempColumn;
    public static int ifFlag;
    public static int forFlag;
    public static Vector3 Place;
    public VarSetting varsettingcs;

    int ifCount;

    int preColumn;
    int preRow;

    //西田
    public float  StartPosX;
    public float  StartPosY;
    public int jibunX = 0;
    public int jibunY = 0;
    public static int touch_flag = 1;

    //変数の初期化と初期設定
    public void Start()
    {
        ifCount = 0;
        ifFlag = 0;
        forFlag = 0;
        maxColumn = 0;
        maxRow = 0;
        CurrentColumn = 0;
        CurrentRow = 0;
        preColumn = 0;
        preRow = 0;
        ObjectInstall(Blank_prefab);
        CurrentPlace.transform.position = new Vector3(startX,startY,-1.0f);
        messageText = messageText.GetComponent<Text>();
        mainCamera = GameObject.Find ("MainCamera").GetComponent<Camera>();
        makeInstance();

        // 西田
        // mainCamera = GameObject.Find ("MainCamera").GetComponent<Camera>();

        //contentの型用
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 128; j++)
            {
                kata[i,j] = " ";
            }
        }
    }

    //列と行による位置決定
    public static void Location(int column,int raw, int depth)
    {
        locationX = HorizontalSpace * column + startX;
        locationY = VerticalSpace * raw + startY;
        locationZ = depth;
        Place = new Vector3(locationX,locationY,locationZ);
    }

     //西田
    public void Update ()
    {
        if(touch_flag == 1){
            if(Input.GetMouseButtonDown(0)){
                Debug.Log("Update");
                StartPosX = mainCamera.ScreenToWorldPoint (Input.mousePosition).x;
                StartPosY = mainCamera.ScreenToWorldPoint (Input.mousePosition).y;
                int i,j;
                for(i = 0;i<maxColumn+1;i++){
                    if(-1.5+4.0*i<StartPosX && 1.5+4.0*i>StartPosX){//i列目にあるよ
                        jibunX = i;
                        break;
                    }
                    else if(i == maxColumn){
                        jibunX = 64;
                    }
                }
                for(j = 0; j<maxRow+1;j++){
                    if(1.5-1.5*j>StartPosY && 0.5-1.5*j<StartPosY){//j行目にあるよ
                        jibunY = j;
                        break;
                    }
                    else if(j==maxRow){
                        jibunY = 129;
                    }
                }
                if(jibunX == 64 || jibunY == 129)return;
                if(objectArray[jibunX,jibunY]==null)return;
                //Location(jibunX,jibunY,-1);
                //CurrentPlace.transform.position = Place;
                CurrentColumn = jibunX;
                CurrentRow = jibunY;
                CurrentPosition();
                Debug.Log("update fin");
            }
        }
        
    }

    void makeInstance()
    {
        int i;
        for(i=0;i<128;i++) ifArray[i] = new Ifreference();
    }

    //オブジェクトの設置
    void ObjectInstall(GameObject Prefab)
    {
        if(Prefab == null)return;

        Location(CurrentColumn,CurrentRow,0);

        //オブジェクトをおこうとしている位置にオブジェクトがあれば削除
        //基本的にはBlankPrefab削除
        Destroy(objectArray[CurrentColumn,CurrentRow]);

        //オブジェクトを配列に代入
        objectArray[CurrentColumn,CurrentRow]=
            Instantiate(Prefab,Place,Quaternion.identity);

        //instantiateされたオブジェクトの名前に(Clone)がつかないようにする
        objectArray[CurrentColumn, CurrentRow].name = Prefab.name;

        textMake(CurrentColumn,CurrentRow,Prefab.name);
    }

    void textMake(int column,int row,string name)
    {
        Transform canv =
            objectArray[column,row].transform.Find("Canvas");
        if(canv == null) return;
        GameObject canvObj = canv.gameObject;
        Transform text = canvObj.transform.Find("Text");
        GameObject textObj = text.gameObject;
        Text ObjectText = textObj.GetComponent<Text>();

        Debug.Log(content[column,row]);
        if(content[column,row] == null || content[column,row] == "")
        switch (name)
        {
        case "Printf_prefab":
            ObjectText.text = "printf";
            break;
        case "If_prefab":
            ObjectText.text = "if";
            break;
        case "ForStart_prefab":
            ObjectText.text = "for";
            break;
        case "Calc_prefab":
            ObjectText.text = "Calculate";
            break;

        }
        else
        {
            ObjectText.text = content[column,row];
        }
    }

    //縦ワイヤーの設置
    void WireInstall(GameObject Prefab)
    {
        if(Prefab == null)return;
        Location(CurrentColumn,CurrentRow,0);
        Destroy(wireArray[CurrentColumn,CurrentRow]);
        Place += new Vector3(0,-0.75f,1.0f);
        wireArray[CurrentColumn,CurrentRow]=
            Instantiate(Prefab,Place,Quaternion.identity);
        wireArray[CurrentColumn,CurrentRow].name=Prefab.name;
    }

    //横ワイヤーの設置
    void HorizontalWireInstall(GameObject Prefab)
    {
        if(Prefab == null) return;
        Location(CurrentColumn,CurrentRow,0);
        Destroy(HorizontalwireArray[CurrentColumn,CurrentRow]);
        Place += new Vector3(0,0,1.0f);
        HorizontalwireArray[CurrentColumn,CurrentRow]=
            Instantiate(Prefab,Place,Quaternion.identity);
        HorizontalwireArray[CurrentColumn,CurrentRow].name=Prefab.name;
    }

    //printfボタン
    public void PrintfButtonClicked()
    {
        //Debug.Log("printfButton");
        //置こうとしている場所を一度保存しておく
        tempRow = CurrentRow;
        tempColumn = CurrentColumn;

        //置こうとしている場所にオブジェクトがすでにあるかどうか
        //あるならオブジェクトの挿入を行う
        Replace();

        ObjectInstall(Printf_prefab);

        ButtonClicked();

        CurrentPosition();
        whetherIf = false;
        //Debug.Log("printf fin");
    }

    //ifボタン
    public void IfButtonClicked()
    {
        tempRow = CurrentRow;
        tempColumn = CurrentColumn;
        whetherIf = true;
        Replace();
        switch(ifFlag){
        case 0:
            IfStart();
            break;
        case 1:
            IfEnd();
            break;
        }
        ButtonClicked();
        CurrentRow++;
        CurrentPosition();
    }

    void IfStart()
    {
        ObjectInstall(If_prefab);
        ifArray[ifCount].ifStartColumn = CurrentColumn;
        ifArray[ifCount].ifStartRow = CurrentRow;
        ifFlag = 1;
    }

    void IfEnd()
    {
        ObjectInstall(Corner1_prefab);
        ifArray[ifCount].ifEndColumn = CurrentColumn;
        ifArray[ifCount].ifEndRow = CurrentRow;

        
        //仮に置いとく、外にifがあると使えない
        CurrentColumn++;
        //横幅をどれだけにするか求める
        //ifControl(ifFlag);

        ObjectInstall(Corner2_prefab);
        ifArray[ifCount].ifCornerColumn = CurrentColumn;
        ifArray[ifCount].ifCornerRow = CurrentRow;
        CurrentRow--;
        while(CurrentRow>ifArray[ifCount].ifStartRow)
        {
            ObjectInstall(Blank_prefab);
            CurrentRow--;
        }
        CurrentColumn = ifArray[ifCount].ifEndColumn;
        CurrentRow = ifArray[ifCount].ifEndRow;
        //IFMATOME();
        ifFlag = 0;
        ifCount++;
    }

    //置こうとしてる場所がifの間かどうか
    //そうなら外のifを重ならないように動かす
    //columnの最大値を考えればいける
    //置こうとしてる始点と終点の間にifがあるかどうか
    //そうならCornerを内側のifと重ならないように動かす
    void ifControl(int mode)
    {
        int column;
        int row;
        int i;
        column=CurrentColumn;
        switch(mode)
        {
            case 0:
                break;
            case 1:
            //If_prefabが置こうとしているところより上にあるか
            //ifArray[ifCount]は今置こうとしているオブジェクトについての情報
            //ifArray[ifCount].ifEndRowはCurrentRowと同値
                for(row=ifArray[ifCount].ifStartRow;row>=0;row--)
                {
                    if(objectArray[column,row]!=null)
                    switch(objectArray[column,row].name)
                    {
                    case "If_prefab":
                        for(i=0;i<=ifCount;i++)
                        {
                            if(row==ifArray[i].ifStartRow)
                            {
                                //置こうとしてるifEndの位置より下に検知したifのifEndが存在するか
                                //存在するなら検知したifの組み合わせは外にあるはず
                                if(ifArray[ifCount].ifEndRow<ifArray[i].ifEndRow)
                                if(ifArray[i].ifCornerColumn-ifArray[ifCount].ifStartColumn==1)
                               　{
                                    //columnが隣同士ならifArray[i]をずらす
                                }
                            }
                        }
                        break;
                    case "Corner1_prefab":
                        break;
                    default:
                        break;
                    }
                }
                break;
            default:
                break;
        }
    }


    public void ForButtonClicked()
    {
        tempRow = CurrentRow;
        tempColumn = CurrentColumn;
        whetherIf = false;
        Replace();
        switch(forFlag){
        case 0:
            ForStart();
            break;
        case 1:
            ForEnd();
            break;
        }
        ButtonClicked();
        CurrentRow++;
        CurrentPosition();
    }

    void ForStart()
    {
        ObjectInstall(ForStart_prefab);
        forFlag = 1;
    }

    void ForEnd()
    {
        ObjectInstall(ForEnd_prefab);
        forFlag = 0;
    }

    //calculateボタン
    public void CalcButtonClicked()
    {
        //置こうとしている場所を一度保存しておく
        tempRow = CurrentRow;
        tempColumn = CurrentColumn;

        //置こうとしている場所にオブジェクトがすでにあるかどうか
        //あるならオブジェクトの挿入を行う
        Replace();

        ObjectInstall(Calc_prefab);

        ButtonClicked();

        CurrentPosition();
        whetherIf = false;
    }

    //printf,if共通
    void ButtonClicked()
    {
        countColumnRow();
        WireSetting();
        //オブジェクトを途中で挿入しないかどうか
        //(新しい空白を必要とするかどうか)

        if (objectArray[CurrentColumn,CurrentRow+1]!= null)
        {
            if(objectArray[CurrentColumn,CurrentRow+1].name=="Corner2_prefab") return;
        }

        else if(CheckResult) return;

        else if(objectArray[CurrentColumn,CurrentRow+1] == null)
        {
            CurrentRow++;
            ObjectInstall(Blank_prefab);
        }
        countColumnRow();
        WireSetting();
        illegalObjectCheck();
        //ifArrayCheck();
    }

    //すでにオブジェクトがその位置にあるか
    void Replace()
    {
        countColumnRow();
        //名前がBlankPrefabかどうかで空白か判断している
        if(objectArray[CurrentColumn,CurrentRow].name!="Blank_prefab")
        {
            CheckResult = true;
            ObjectReplace();
            return;
        }
        CheckResult = false;
        return;
    }

    //一つ上のオブジェクトをコピーして下に移動
    void ObjectReplace()
    {
        //bool p;
        int i;
        for(CurrentRow=maxRow;CurrentRow>tempRow-1;CurrentRow--)
        {
            for(CurrentColumn=maxColumn;CurrentColumn>-1;CurrentColumn--)
            {
                if(CurrentRow==0){
                    break;
                }
                if(CurrentRow==tempRow)
                {
                    if(wireArray[CurrentColumn,CurrentRow-1]!=null)
                    {
                        ObjectInstall(Blank_prefab);
                    }
                }
                else
                {
                    content[CurrentColumn,CurrentRow]=content[CurrentColumn,CurrentRow-1];
                    content[CurrentColumn,CurrentRow-1]="";
                    kata[CurrentColumn,CurrentRow]=kata[CurrentColumn,CurrentRow-1];
                    kata[CurrentColumn,CurrentRow-1]=" ";

                    if(objectArray[CurrentColumn,CurrentRow-1]!=null)
                    {
                        ObjectInstall(objectArray[CurrentColumn,CurrentRow-1]);
                        textMake(CurrentColumn,CurrentRow,objectArray[CurrentColumn,CurrentRow-1].name);

                        switch(objectArray[CurrentColumn,CurrentRow-1].name)
                        {
                        case "If_prefab":
                            for(i=0;i<ifCount;i++)
                            if(ifArray[i].ifStartRow == CurrentRow-1)
                                ifArray[i].ifStartRow = CurrentRow;
                            break;
                        case "Corner1_prefab":
                            for(i=0;i<ifCount;i++)
                            if(ifArray[i].ifEndRow == CurrentRow-1)
                                ifArray[i].ifEndRow = CurrentRow;
                            break;

                        case "Tatedake_prefab":
                            Destroy(objectArray[CurrentColumn,CurrentRow-1]);
                            objectArray[CurrentColumn,CurrentRow-1]=null;
                            break;
                        default:
                            break;
                        }
                    }
                }
            }
        }
        CurrentRow = tempRow;
        CurrentColumn = tempColumn;
    }

    void ifArrayCheck()
    {
        int i;
        for(i=0;i<ifCount;i++)
        {
            Debug.Log(
                "i="+i+
                ",ifStartRow="+ifArray[i].ifStartRow+
                ",ifEndRow="+ifArray[i].ifEndRow
                );
        }
    }

    void illegalObjectCheck()
    {
        int column;
        int row;
        for(row=1;row<maxRow;row++)
        for(column=1;column<maxColumn;column++)
        if(wireArray[column,row-1]==null
            &&( (objectArray[column,row].name!="Tatedake_prefab"&&objectArray[column,row].name!="Yokodake_prefab") || objectArray[column,row]==null)
            )
        {
            Destroy(objectArray[column,row]);
            objectArray[column,row]=null;
        }
    }

    void countColumnRow()
    {
        int column;
        int row;
        maxColumn = 0;
        maxRow = 0;
        for(row = 0;row < 128;row++)
        {
            if(objectArray[0,row] == null) break;
            maxRow++;
        }
        //Debug.Log("maxRow="+maxRow);
        for(column = 1;column<64;column++)
        {
            for(row = 0;objectArray[column,row]==null;row++)
            if(row == maxRow)
            if(objectArray[column,row]==null)
            {
                maxColumn = column;
                //Debug.Log("maxColumn="+maxColumn);
                return;
            }
        }
    }

    public void ObjectToString(string name, int Column, int Raw)
    {
        switch (name)
        {
            case "Printf_prefab":
                SaveobjectArray[Column, Raw] = "Printf_prefab";
                break;
            case "Blank_prefab":
                SaveobjectArray[Column, Raw] = "Blank_prefab";
                break;
            case "If_prefab":
                SaveobjectArray[Column, Raw] = "If_prefab";
                break;
            case "ForEnd_prefab":
                SaveobjectArray[Column, Raw] = "ForEnd_prefab";
                break;
            case "ForStart_prefab":
                SaveobjectArray[Column, Raw] = "ForStart_prefab";
                break;
            case "Corner1_prefab":
                SaveobjectArray[Column, Raw] = "Corner1_prefab";
                break;
            case "Corner2_prefab":
                SaveobjectArray[Column, Raw] = "Corner2_prefab";
                break;
            case "Calc_prefab":
                SaveobjectArray[Column, Raw] = "Calc_prefab";
                break;

        }
       // Debug.Log(SaveobjectArray[Column, Raw]);
    }

    public void Saveuhihihi()

    {
        //Debug.Log(maxColumn);
        //Debug.Log(maxRow);
        int i, j;
        i = 0;
        j = 0;

        //gameobject型をstringの配列に変える
        for (i = 0; i < maxColumn; i++)
        {
            for (j = 0; j < maxRow; j++)
            {
                if (objectArray[i, j] != null)
                {
                    ObjectToString(objectArray[i, j].name, i, j);
                }
            }
        }

        //座標ごとにストレージに保存する（ブロックの種類）
        for (i = 0; i < maxColumn; i++)
        {
            for (j = 0; j < maxRow; j++)
            {
                PlayerPrefs.SetString("ObjectArray" + i + j, SaveobjectArray[i, j]);
                // Debug.Log(objectArray[i, j]);
            }
        }
        //座標ごとにストレージに保存する（ブロックの中身）
        for (i = 0; i < maxColumn; i++)
        {
            for (j = 0; j < maxRow; j++)
            {
                PlayerPrefs.SetString("contentArray" + i + j, content[i, j]);
                //Debug.Log(content[i, j]);
            }
        }
        //座標ごとストレージに保存する(変数の型情報)
        for (i = 0; i < maxColumn; i++)
        {
            for (j = 0; j < maxRow; j++)
            {
                PlayerPrefs.SetString("kataArray" + i + j, kata[i, j]);
                //Debug.Log(content[i, j]);
            }
        }
        PlayerPrefs.SetInt("maxColumn", maxColumn);
        PlayerPrefs.SetInt("maxRow", maxRow);
        PlayerPrefs.Save();
    }
    //ストレージから読み込み
    public void Loaduhihihi()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("maxColumn", 0) + 1; i++)
        {
            for (int j = 0; j < PlayerPrefs.GetInt("maxRow", 0) + 1; j++)
            {
                SaveobjectArray[i, j] = PlayerPrefs.GetString("ObjectArray" + i + j, null);
                content[i, j] = PlayerPrefs.GetString("contentArray" + i + j, null);
                kata[i, j] = PlayerPrefs.GetString("kataArray" + i + j, null);
                // Debug.Log(content[i, j]);
            }
        }
        maxColumn = PlayerPrefs.GetInt("maxColumn", 0);
        maxRow = PlayerPrefs.GetInt("maxRow", 0);
        LoadObject();
    }
    //再配置
    void LoadObject()
    {
        //Debug.Log("ろーどかいしするよ");
        for (int i = 0; i < maxColumn; i++)
        {
            for (int j = 0; j < maxRow; j++)
            {
                GameObject Prefab;

                switch (SaveobjectArray[i, j])
                {

                    case "Printf_prefab":
                        Prefab = Printf_prefab;
                        //Debug.Log(Prefab);
                        break;
                    case "Blank_prefab":
                        Prefab = Blank_prefab;
                        // Debug.Log(Prefab);
                        break;
                    case "If_prefab":
                        Prefab = If_prefab;
                        // Debug.Log(Prefab);
                        break;
                    case "ForEnd_prefab":
                        Prefab = ForEnd_prefab;
                        // Debug.Log(Prefab);
                        break;
                    case "ForStart_prefab":
                        Prefab = ForStart_prefab;
                        // Debug.Log(Prefab);
                        break;
                    case "Corner1_prefab":
                        Prefab = Corner1_prefab;
                        //Debug.Log(Prefab);
                        break;
                    case "Corner2_prefab":
                        Prefab = Corner2_prefab;
                        // Debug.Log(Prefab);
                        break;
                    case "Calc_prefab":
                        Prefab = Calc_prefab;
                        break;
                    default:
                        Prefab = null;
                        break;
                }
                //nullはinstantiate不可
                CurrentColumn = i;
                CurrentRow = j;
                if (Prefab != null)
                {
                    //Debug.Log(Prefab);
                    Location(i, j, 0);

                    //オブジェクトをおこうとしている位置にオブジェクトがあれば削除
                    //基本的にはBlankPrefab削除
                    Destroy(objectArray[i, j]);

                    //オブジェクトを配列に代入
                    objectArray[i, j] =
                        Instantiate(Prefab, Place, Quaternion.identity);

                    //instantiateされたオブジェクトの名前に(Clone)がつかないようにする
                    objectArray[i, j].name = Prefab.name;
                    textMake(CurrentColumn, CurrentRow, Prefab.name);
                }
            }
        }
        WireSetting();
        CurrentColumn = 0;
        CurrentRow = 0;
    }

    //ボタン操作上
    public void UpButtonClicked()
    {
        if(CurrentRow == 0) return;
        if(objectArray[CurrentColumn,CurrentRow-1]!= null)
        {
            CurrentRow--;
            CurrentPosition();
        }
    }

    //ボタン操作下
    public void DownButtonClicked()
    {
        if(objectArray[CurrentColumn,CurrentRow+1]!= null)
        {
            CurrentRow++;
            CurrentPosition();
        }
    }

    //ボタン操作左
    public void LeftButtonClicked()
    {
        if(CurrentColumn == 0) return;
        if(objectArray[CurrentColumn-1,CurrentRow]!= null)
        {
            CurrentColumn--;
            CurrentPosition();
        }
    }

    //ボタン操作右
    public void RightButtonClicked()
    {
        if(objectArray[CurrentColumn+1,CurrentRow]!= null)
        {
            CurrentColumn++;
            CurrentPosition();
        }
    }

    //赤枠の位置決定
    void CurrentPosition()
    {
        int i;
        int j;
        int length;
        int temp;
        int[] place = new int[64];
        bool InOutCheck;
        bool act;
        i = 0;
        j = 0;
        length = 128;
        temp = 0;
        InOutCheck = false;
        act = false;
        //Debug.Log(CurrentRow);
        if(ifFlag==1 || forFlag==1)
        {
            if(CurrentColumn!=tempColumn)
            {
                CurrentColumn = preColumn;
                CurrentRow=preRow;
                messageText.text = "始点としたオブジェクトのある列以外に終点を配置できません";
            }
            else if(CurrentRow<=tempRow)
            {
                CurrentColumn = preColumn;
                CurrentRow = preRow;
                messageText.text = "始点としたオブジェクトより上に終点を配置できません";
            }
            else
            {
                //Debug.Log("ifCount = "+ifCount);
                for(j=0;j<ifCount;j++)
                {
                //ifStartが他のifの内側かどうか
                    if(ifArray[j].ifStartColumn==ifArray[ifCount].ifStartColumn)
                    if(ifArray[j].ifStartRow<ifArray[ifCount].ifStartRow)
                    if(ifArray[j].ifEndRow>=ifArray[ifCount].ifStartRow)
                    {
                        InOutCheck = true;
                        if(ifArray[j].ifDistance()<length)
                        {
                            length = ifArray[j].ifDistance();
                            temp = j;
                        }
                    }
                }
                //Debug.Log(length);
                if(length==128) InOutCheck = false;
                //内
                if(InOutCheck)
                {
                    //Debug.Log("uchi");
                    for(i=0;i<maxRow;i++)
                    {
                        if(ifArray[temp].ifStartRow<i)
                        if(ifArray[temp].ifEndRow>=i)
                        {
                            place[i]=1;
                        }
                        else place[i]=0;
                    }
                }
                //外
                else
                {
                    //Debug.Log("soto");
                    for(i=0;i<maxRow;i++)
                    {
                        act = false;
                        for(j=0;j<=ifCount;j++)
                        {
                            if(ifArray[j].ifStartColumn == CurrentColumn)
                            if(ifArray[j].ifStartRow<i && ifArray[j].ifEndRow>=i)
                            {
                                //i行目は位置的に利用不可能
                                act = true;
                            }
                        }
                        if(act) place[i] = 0;
                        else place[i] = 1;

                    }
                }
                if(place[CurrentRow]==0)
                {
                    CurrentColumn = preColumn;
                    CurrentRow = preRow;
                    messageText.text = "ワイヤーが交差するようなオブジェクトの配置はできません";
                }
            }
        }
        Location(CurrentColumn,CurrentRow,-1);
        CurrentPlace.transform.position = Place;
        //reload();
        preRow = CurrentRow;
        preColumn = CurrentColumn;
        //Debug.Log("maxRow = "+maxRow);
        //Debug.Log("maxColumn ="+maxColumn);
    }

    void WireSetting()
    {
        for(CurrentRow=maxRow;CurrentRow>-1;CurrentRow--)
        {
            for(CurrentColumn=maxColumn;CurrentColumn>-1;CurrentColumn--)
            {
                if(wireArray[CurrentColumn, CurrentRow]!=null){
                    Destroy(wireArray[CurrentColumn,CurrentRow]);
                    wireArray[CurrentColumn,CurrentRow]=null;
                }
                if(HorizontalwireArray[CurrentColumn, CurrentRow]!=null){
                    Destroy(HorizontalwireArray[CurrentColumn,CurrentRow]);
                    HorizontalwireArray[CurrentColumn,CurrentRow]=null;
                }
                if(objectArray[CurrentColumn, CurrentRow]!=null){
                    /*Debug.Log(objectArray[CurrentColumn, CurrentRow].name
                        +","+CurrentColumn+","+CurrentRow);*/

                    switch (objectArray[CurrentColumn, CurrentRow].name)
                    {
                    case "Printf_prefab":
                        WireInstall(Wire_prefab);
                        break;

                    case "Blank_prefab":
                        if(CurrentRow!=maxRow-1)
                        {
                            WireInstall(Wire_prefab);
                        }
                        break;

                    case "If_prefab":
                        if(CurrentColumn == tempColumn && CurrentRow == tempRow)
                        {
                            WireInstall(Wire_prefab);
                        }
                        else
                        {
                            WireInstall(Wire_prefab);
                            HorizontalWireInstall(Wire_If_prefab);
                            CurrentColumn++; //庭野完成したらここ破壊
                            WireInstall(Wire_prefab);
                            CurrentColumn--;
                        }
                        break;

                    case "ForEnd_prefab":
                        WireInstall(Wire_prefab);
                        break;

                    case "ForStart_prefab":
                        WireInstall(Wire_prefab);
                        break;

                    case "Corner1_prefab":
                        WireInstall(Wire_prefab);
                        HorizontalWireInstall(Wire_If_prefab);
                        break;

                    case "Corner2_prefab":
                        break;

                    case "Calc_prefab":
                        WireInstall(Wire_prefab);
                        break;

                    case "Tatedake_prefab":
                        WireInstall(Wire_prefab);
                        break;

                    case "Yokodake_prefab":
                        HorizontalWireInstall(Wire_If_prefab);
                        break;

                    default:
                        break;
                    }
                }
            }
        }
        CurrentRow = tempRow;
        CurrentColumn = tempColumn;
    }
//
//庭野ゾーン
//
    //できればUI系はButtonScriptにまとめたいと思ってる。
    //そうすると他のメニューとも連携取れやすいはず。byたくみん
    //変数#1 メニュー表示のためにメニュー(GameObject)を追加
    //public GameObject IfMenu;
    //public GameObject PrintfMenu;
    //public GameObject ForMenu;
    //public GameObject CalcMenu;
    public GameObject Tatedake_prefab;
    public GameObject Yokodake_prefab;
    public string enzansi;
    //変数#2 inputfieldから入力を受け取るため、textにその今のあれをブチ込むためにどっちも取得
    public InputField PrintfInputField;
    public InputField IfInputField1;
    public InputField IfInputField2;
    public InputField CalcInputField1;
    public InputField CalcInputField2;
    public InputField ForInputField1; //starti
    public InputField ForInputField2; //endi
    public InputField ForInputField3; //d
    //textやつ
    public Text PrintfDisplay;
    public Text IfDisPlay;
    public Text CalcDisplay;
    public Text ForDisplay;
    public Dropdown VarDropdownPrintf;
    public Dropdown VarDropdownCalc1;
    public Dropdown VarDropdownCalc2;
    public Dropdown VarDropdownCalc3;
    public Dropdown VarDropdownIf1;
    public Dropdown VarDropdownIf2;


    //値取得パート。


    //もう死ねよこの関数
    /*public void GetContentOn(){
        //見づらくなるから一瞬おく。あとで返してあげよう
        int imanani=ItemCheck2();
        switch(imanani){
            case 2: //printfなとき
                VarDropdownPrintf.value=0;
                break;

            case 3: //ifなとき...
                VarDropdownIf1.value=0;
                VarDropdownIf2.value=0;
                //フォームクリアしない。複数あるから。
                break;

            //case 5: //forなとき、実装後回しにしてる・・・。

            case 7: //calc!
                VarDropdownCalc1.value=

            default:
                Debug.Log("GO SLEEP BITCH");
                break;
        }
    }*/

    //ｋのへんは選択肢変更時に呼び出してinputfieldの中身リセット
    public void GetContentOn2(){
        PrintfInputField.text="";
        return;
    }
    public void GetContentOn3(){
        IfInputField1.text="";
        return;
    }
    public void GetContentOn4(){
        IfInputField2.text="";
        return;
    }
    public void GetContentOn5(){
        CalcInputField1.text="";
        return;
    }
    public void GetContentOn6(){
        CalcInputField2.text="";
        return;
    }
    public void GetContentOn7(){
        ForInputField1.text="";
        ForInputField2.text="";
        ForInputField3.text="";
    }
    public void zenkesi(){
        GetContentOn2();
        GetContentOn3();
        GetContentOn4();
        GetContentOn5();
        GetContentOn6();
        GetContentOn7();
    }
    //じゃあinputfield変更時に消す奴もいるんじゃね？
    public void ResetChoice1(){
        VarDropdownPrintf.value=0;
        return;
    }
    public void ResetChoice2(){
        VarDropdownIf1.value=0;
        return;
    }
    public void ResetChoice3(){
        VarDropdownIf2.value=0;
        return;
    }
    public void ResetChoice4(){
        VarDropdownCalc1.value=0;
        return;
    }
    public void ResetChoice5(){
        VarDropdownCalc2.value=0;
        return;
    }
    /*public void ResetChoice6(){
        varsettingcs.VarDropdownCalc3.value=0;
        return;
    }*/

    //inputfieldの変更、選択肢変更の時の共通処理
    public void GetVarContent(){
        GameObject ObjectHere = objectArray[CurrentColumn,CurrentRow];
        string DataHere = content[CurrentColumn,CurrentRow];
        int imanani=ItemCheck2();
        switch(imanani){
            case 2://printf
                string vartext;
                if(VarDropdownPrintf.value!=0){
                    vartext = VarSetting.whatisthis(VarDropdownPrintf.value);
                    kata[CurrentColumn,CurrentRow]=VarSetting.watchthis(VarDropdownPrintf.value);
                    content[CurrentColumn, CurrentRow] = VarSetting.youshouldrun(VarDropdownPrintf.value);
                }
                else{
                    vartext = PrintfInputField.text.ToString();
                    content[CurrentColumn, CurrentRow] = vartext;
                }
                DataHere = vartext;
                PrintfDisplay.text = DataHere;
                textMake(CurrentColumn,CurrentRow,"Printf_prefab");
                break;

            case 3://if
                string vartext1, vartext2;
                if(VarDropdownIf1.value!=0){
                    vartext1=VarSetting.youshouldrun(VarDropdownIf1.value);
                }else{
                    vartext1='"'+IfInputField1.text.ToString()+'"';
                }
                if(VarDropdownIf2.value!=0){
                    vartext2=VarSetting.youshouldrun(VarDropdownIf2.value);
                }else{
                    vartext2='"'+IfInputField2.text.ToString()+'"';
                }
                DataHere=vartext1+enzansi+vartext2;
                IfDisPlay.text = DataHere;
                content[CurrentColumn, CurrentRow] = DataHere;
                textMake(CurrentColumn,CurrentRow,"If_prefab");
                break;

            case 5://for
                string vartextfor1,vartextfor2,vartextfor3;
                int txt1,txt2,txt3;
                vartextfor1=ForInputField1.text.ToString();
                vartextfor2=ForInputField2.text.ToString();
                vartextfor3=ForInputField3.text.ToString();
                try{
                    txt1=int.Parse(vartextfor1);
                    txt2=int.Parse(vartextfor2);
                    txt3=int.Parse(vartextfor3);
                }catch{
                    messageText.text="整数以外を入力できません";
                    break;
                }
                string enzansidocchi="";
                string enzansidocchi2="";
                if(txt1==txt2){
                    messageText.text="死ね";
                    break;
                }else if(txt1>txt2){
                    enzansidocchi=">=";
                    enzansidocchi2="-=";
                }else{
                    enzansidocchi="<=";
                    enzansidocchi2="+=";
                }
                DataHere="for(int i="+txt1+";"+"i"+enzansidocchi+txt2+";i"+enzansidocchi2+txt3+")";//for(int i=init;i<=)
                ForDisplay.text=DataHere;
                content[CurrentColumn,CurrentRow]="DataHere";
                break;

            case 7://calc
                string vartext3,vartext4;
                if(VarDropdownCalc1.value!=0){
                    vartext3=VarSetting.youshouldrun(VarDropdownCalc1.value);
                }else{
                    vartext3=CalcInputField1.text.ToString();
                }
                if(VarDropdownCalc2.value!=0){
                    vartext4=VarSetting.youshouldrun(VarDropdownCalc2.value);
                }else{
                    vartext4=CalcInputField2.text.ToString();
                }
                string aaa;
                if(VarDropdownCalc3.value==0){
                    aaa="";
                }else{
                    aaa=VarSetting.youshouldrun(VarDropdownCalc3.value);
                }
                DataHere=aaa+"="+vartext3+enzansi2+vartext4;
                CalcDisplay.text=DataHere;
                content[CurrentColumn, CurrentRow] = DataHere;
                textMake(CurrentColumn, CurrentRow,"Calc_prefab");
                break;

            default:
                break;
        }
        Debug.Log("値変えたよ");
     //   content[CurrentColumn,CurrentRow]=DataHere;
    }

    //dropdown取得。これは選択するたびにかってにだしてくれる
    public Dropdown dropdown;
    public void GetDropDown(){
        switch(dropdown.value){
            case 0:
                enzansi="";
                break;
            case 1:
                enzansi=">";
                break;
            case 2:
                enzansi=">=";
                break;
            case 3:
                enzansi="<";
                break;
            case 4:
                enzansi="<=";
                break;
            case 5:
                enzansi="==";
                break;
            case 6:
                enzansi="!=";
                break;
            default:
                break;
        }
        GetVarContent();
    }
    public Dropdown dropdownenzansi2;
    public string enzansi2;
    public void GetDropDown2(){
        switch(dropdownenzansi2.value){
            case 0:
                enzansi2="";
                break;
            case 1:
                enzansi2="+";
                break;
            case 2:
                enzansi2="-";
                break;
            case 3:
                enzansi2="*";
                break;
            case 4:
                enzansi2="/";
                break;
            case 5:
                enzansi2="%";
                break;
            case 6:
                enzansi2="";
                break;
        }
        GetVarContent();
    }

    //上の奴を参考にしてつくった。1:blank 2:printf 3:if 4:ifend 5:fors 6:fore 99:null
    public static int ItemCheck2(){
        if(!objectArray[CurrentColumn,CurrentRow]){
            return 0;
        }
        switch(objectArray[CurrentColumn,CurrentRow].name){
            case "BlankPrefab":
                return 1;

            case "Printf_prefab":
                return 2;

            case "If_prefab":
                return 3;

            case "Endif_prefab":
                return 4;

            case "ForStart_prefab":
                return 5;

            case "ForEnd_prefab":
                return 6;

            case "Calc_prefab":
                return 7;

            default:
                return 0;
        }

    }

    public int SeekThemOut(){ //中は[どこから縦を伸ばしたらいいかな]を調べたいだけなのでmaxだけ返せばok.
        int ifkazu=0;
        int maxifkazu=0;
        int x=ifArray[ifCount].ifStartColumn;
        int y=ifArray[ifCount].ifStartRow;
        for(y=ifArray[ifCount].ifStartRow+1;y<ifArray[ifCount].ifEndRow;y++){
            if(objectArray[x,y].name=="If_prefab"){
                ifkazu++;
                if(maxifkazu<ifkazu){
                    maxifkazu++;
                }
            }else if(objectArray[x,y].name=="Corner1_prefab"){
                ifkazu--;
            }
        }
        if(maxifkazu>0){
            Debug.Log("なかおる；；");
        }
        return maxifkazu;
    }

    public void PlacingSentry(int nakax,int starty,int endy,int preflag){ //外は[スライドを何回、どこからどこまで、すればいいかな]を調べたいが...
        int x=nakax+1+CurrentColumn;
        int y=starty;
        int flag=0;
        for(y=starty;y<=endy;y++){
            if(objectArray[x,y]!=null){
                flag=1;
                Debug.Log("そとにいる；；");
                break;
            }
        }
        if(flag==0&&preflag==1){
            ObjectSlide(x,starty,endy);
            Debug.Log("すらいどはじめまむ");
        }else if(flag==1){
            int nextnakax=x;
            int nextstarty=SearchUpper(x,y);
            int nextendy=SearchLower(x,y);
            PlacingSentry(nextnakax,nextstarty,nextendy,1);
            ObjectSlide(x,starty,endy);
            Debug.Log("すらいどしたよ");
        }
    }

    public void ObjectSlide(int startx,int starty,int endy){
        int x=CurrentColumn;
        int y=CurrentRow;
        CurrentColumn=startx;
        for(CurrentRow=starty;CurrentRow<endy;CurrentRow++){
            ObjectInstall(objectArray[CurrentColumn-1,CurrentRow]);
            Destroy(objectArray[CurrentColumn-1,CurrentRow]);
            objectArray[CurrentColumn-1,CurrentRow]=null;
            content[CurrentColumn,CurrentRow]=content[CurrentColumn-1,CurrentRow];
            content[CurrentColumn-1,CurrentRow]="";
            kata[CurrentColumn,CurrentRow]=kata[CurrentColumn-1,CurrentRow];
            kata[CurrentColumn-1,CurrentRow]=" ";
            }
        CurrentRow=starty;
        CurrentColumn--;
        ObjectInstall(Yokodake_prefab);
        CurrentRow=endy;
        for(int i=0;i<ifCount;i++){ //ifをずらしたのだからそれも教えてあげよう
            if(ifArray[i].ifCornerRow==CurrentRow&&ifArray[i].ifCornerColumn==CurrentColumn){
                ifArray[i].ifCornerColumn++;
            }
        }
        ObjectInstall(Yokodake_prefab);
        CurrentColumn=x;
        CurrentRow=y;
        WireSetting();
    }

    public void ImaIfShori(int nakax){ // →、↓、← の順で処理するよ
        if(nakax==0){
            //仮に置いとく、外にifがあると使えない
            CurrentColumn=ifArray[ifCount].ifStartColumn+1;
            CurrentRow=ifArray[ifCount].ifStartRow;
            ObjectInstall(Tatedake_prefab);
            Debug.Log("たておいた");
            //横幅をどれだけにするか求める
            //ifControl(ifFlag);
            CurrentRow=ifArray[ifCount].ifEndRow;
            ObjectInstall(Corner2_prefab);
            Debug.Log("こなおいた");
            ifArray[ifCount].ifCornerColumn = CurrentColumn;
            ifArray[ifCount].ifCornerRow = CurrentRow;
            CurrentRow--;
            while(CurrentRow>ifArray[ifCount].ifStartRow)
            {
                ObjectInstall(Blank_prefab);
                CurrentRow--;
            }
            CurrentColumn = ifArray[ifCount].ifEndColumn;
            CurrentRow = ifArray[ifCount].ifEndRow;
            return;
        }
        CurrentColumn=ifArray[ifCount].ifStartColumn;
        int imacolumn=CurrentColumn;
        nakax+=CurrentColumn;
        CurrentRow=ifArray[ifCount].ifStartRow;
        //右にyoko配置
        for(CurrentColumn=ifArray[ifCount].ifStartColumn+1;CurrentColumn<nakax+1;CurrentColumn++){
            ObjectInstall(Yokodake_prefab);
            Debug.Log("よこおいた");
            Debug.Log(CurrentColumn.ToString()+CurrentRow.ToString());
        }
        
        //右端に到着、tateおく
        CurrentColumn=nakax+1;
        ObjectInstall(Tatedake_prefab);
        Debug.Log("たておいた");
        Debug.Log(CurrentColumn.ToString()+CurrentRow.ToString());
        //下までblankいれる
        //CurrentRow++;
        for(CurrentRow=CurrentRow+1;CurrentRow<ifArray[ifCount].ifEndRow;CurrentRow++){
            ObjectInstall(Blank_prefab);
            Debug.Log("Blankいれた");
            Debug.Log(CurrentColumn.ToString()+CurrentRow.ToString());
        }
        
        //下ついた。corner2おく
        CurrentRow=ifArray[ifCount].ifEndRow;
        ObjectInstall(Corner2_prefab);
        Debug.Log("cor2おいた");
        Debug.Log(CurrentColumn.ToString()+CurrentRow.ToString());
         //ついでにarrayに情報いれる
        ifArray[ifCount].ifCornerColumn=CurrentColumn;
        ifArray[ifCount].ifCornerRow=CurrentRow;
        //左にyokoおいてく
        //CurrentColumn--;
        for(CurrentColumn=CurrentColumn-1;CurrentColumn>ifArray[ifCount].ifStartColumn;CurrentColumn--){
            ObjectInstall(Yokodake_prefab);
            Debug.Log("よこおいた");
            Debug.Log(CurrentColumn.ToString()+CurrentRow.ToString());
        }
        
        WireSetting();
    }

    public int SearchRight(int x,int y){ //ifにぶつかってからtatedakeをさがします
        while(objectArray[x,y].name!="Tatedake_prefab"){
            x++;
        }
        return x;
    }
    public int SearchLower(int x,int y){ //tatedakeにぶつかってからyokoを探してくれます
        while(objectArray[x,y].name!="Corner2_prefab"){
            y++;
        }
        return y;
    }

    public int SearchUpper(int x,int y){ //tatedakeにぶつかってからyokoを探してくれます
        while(objectArray[x,y].name!="Tatedake_prefab"){
            y--;
        }
        return y;
    }

    public void IFMATOME(){
        int ys=ifArray[ifCount].ifStartRow;
        int ye=ifArray[ifCount].ifEndRow;

        int p=SeekThemOut();
        PlacingSentry(p,ys,ye,0);
        ImaIfShori(p);
    }

    public void reload()
    {
        Location(CurrentColumn,CurrentRow,-10);
        mainCamera.transform.position = Place + new Vector3(0f,-1.0f,0f);
    }
}

public class Ifreference
{
    public int ifStartColumn;
    public int ifStartRow;
    public int ifEndColumn;
    public int ifEndRow;
    public int ifCornerColumn;
    public int ifCornerRow;
    public int ifDistance()
    {
        int distance;
        distance = ifEndRow - ifStartRow;
        return distance;
    }
}
