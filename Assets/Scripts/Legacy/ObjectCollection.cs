using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectCollection : MonoBehaviour
{
    //オブジェクト配列の宣言
    public GameObject[,] objectArray = new GameObject[64, 128];
    public string[,] SaveobjectArray = new string[64, 128];

    public GameObject[,] wireArray = new GameObject[64, 128];
    public GameObject[,] HorizontalwireArray = new GameObject[64, 128];
    //nakamiiii
    public string[,] content = new string[64, 128];
    public string[,] kata = new string[64, 128];
    public Text messageText;

    Ifreference[] ifArray = new Ifreference[128];
    Ifreference[,] ifArray2 = new Ifreference[256, 128];

    //次元を超える
    public string[,,] objectArray3D = new string[256, 64, 128];
    public string[,,] content3D = new string[256, 64, 128];
    public string[,,] kata3D = new string[256, 64, 128];

    public int[] DimensionalColumn = new int[256];
    public int[] DimensionalRow = new int[256];
    public int[] ifCount2D = new int[256];

    //オブジェクトのプロトタイプ宣言
    [SerializeField] GameObject CurrentPlace;
    [SerializeField] GameObject Printf_prefab;
    [SerializeField] GameObject If_prefab;
    [SerializeField] GameObject Blank_prefab;
    [SerializeField] GameObject Wire_prefab;
    [SerializeField] GameObject Wire_If_prefab;
    [SerializeField] GameObject Corner1_prefab;
    [SerializeField] GameObject Corner2_prefab;
    [SerializeField] GameObject ForStart_prefab;
    [SerializeField] GameObject ForEnd_prefab;
    [SerializeField] GameObject WhileStart_prefab;
    [SerializeField] GameObject WhileEnd_prefab;
    [SerializeField] GameObject Calc_prefab;
    [SerializeField] GameObject Break_prefab;
    [SerializeField] GameObject Subroutine_prefab;

    Camera mainCamera;

    float locationX;
    float locationY;
    float locationZ;
    public int CurrentColumn { get; set; }
    public int CurrentRow { get; set; }
    public int MaxColumn { get; set; }
    public int MaxRow { get; set; }
    public int row;
    public int column;
    public bool CheckResult;
    public bool whetherIf;
    public int tempRow;
    public int tempColumn;
    public int ifFlag;
    public int forFlag;
    public int whileFlag;

    public Vector3 Place;

    int ifCount;

    int preColumn;
    int preRow;

    public int Dimension = 0;
    public int TheDimension;
    public int MaxDimension = 0;

    //西田
    public float StartPosX;
    public float StartPosY;
    public int jibunX = 0;
    public int jibunY = 0;
    public int touch_flag = 1;

    public Text Label;
    Ifreference[,] ifArray3 = new Ifreference[64, 128];

    private GameObject ObjectSearch(string objname)
    {
        switch (objname)
        {
            case "Printf_prefab": return Printf_prefab;
            case "Blank_prefab": return Blank_prefab;
            case "If_prefab": return If_prefab;
            case "ForEnd_prefab": return ForEnd_prefab;
            case "ForStart_prefab": return ForStart_prefab;
            case "Corner1_prefab": return Corner1_prefab;
            case "Corner2_prefab": return Corner2_prefab;
            case "Yokodake_prefab": return Yokodake_prefab;
            case "Tatedake_prefab": return Tatedake_prefab;
            case "WhileStart_prefab": return WhileStart_prefab;
            case "WhileEnd_prefab": return WhileEnd_prefab;
            case "Break_prefab": return Break_prefab;
            case "Subroutine_prefab": return Subroutine_prefab;
            default: return null;
        }
    }

    //変数の初期化と初期設定
    public void Start()
    {
        ifCount = 0;
        ifFlag = 0;
        forFlag = 0;
        MaxColumn = 0;
        MaxRow = 0;
        CurrentColumn = 0;
        CurrentRow = 0;
        preColumn = 0;
        preRow = 0;
        ObjectInstall(Blank_prefab);
        CurrentPlace.transform.position = new Vector3(Constant.startX, Constant.startY, -1.0f);
        messageText = messageText.GetComponent<Text>();
        Label = Label.GetComponent<Text>();
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        makeInstance();

        //contentの型用
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 128; j++)
            {
                kata[i, j] = " ";
            }
        }
    }

    //列と行による位置決定
    public void Location(int column, int raw, int depth)
    {
        locationX = Constant.HorizontalSpace * column + Constant.startX;
        locationY = Constant.VerticalSpace * raw + Constant.startY;
        locationZ = depth;
        Place = new Vector3(locationX, locationY, locationZ);
    }

    //西田
    public void Update()
    {
        if (touch_flag == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //StartPosX = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
                //StartPosY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;
                int i, j;
                for (i = 0; i < MaxColumn + 1; i++)
                {
                    if (-1.5 + 4.0 * i < StartPosX && 1.5 + 4.0 * i > StartPosX)
                    {
                        jibunX = i;
                        break;
                    }
                    else if (i == MaxColumn)
                    {
                        jibunX = -1;
                    }
                }
                for (j = 0; j < MaxRow + 1; j++)
                {
                    if (1.5 - 1.5 * j > StartPosY && 0.5 - 1.5 * j < StartPosY)
                    {
                        jibunY = j;
                        break;
                    }
                    else if (j == MaxRow)
                    {
                        jibunY = -1;
                    }
                }
                if (jibunX == -1 || jibunY == -1) return;
                if (objectArray[jibunX, jibunY] == null) return;

                //TatedakeとYokodakeに行けないようにしようと思ったんです...
                switch (objectArray[jibunX, jibunY].name)
                {
                    case "Yokodake_prefab":
                        return;

                    case "Tatedake_prefab":
                        return;

                    default:
                        break;
                }
                CurrentColumn = jibunX;
                CurrentRow = jibunY;
                CurrentPosition();
            }
        }
    }

    public void RedoClicked()
    {
        for (int i = 0; i < MaxColumn; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                if (objectArray[i, j] != null)
                {
                    objectArray3D[Dimension, i, j] = objectArray[i, j].name;
                }
            }
        }
        for (int i = 0; i < MaxColumn; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                content3D[Dimension, i, j] = content[i, j];
            }
        }
        for (int i = 0; i < MaxColumn; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                kata3D[Dimension, i, j] = kata[i, j];
            }
        }
        DimensionalColumn[Dimension] = MaxColumn;
        DimensionalRow[Dimension] = MaxRow;
        Dimensional_Drift(1);
    }

    public void UndoClicked()
    {
        for (int i = 0; i < MaxColumn; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                if (objectArray[i, j] != null)
                {
                    objectArray3D[Dimension, i, j] = objectArray[i, j].name;
                }
            }
        }
        for (int i = 0; i < MaxColumn; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                content3D[Dimension, i, j] = content[i, j];
            }
        }
        for (int i = 0; i < MaxColumn; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                kata3D[Dimension, i, j] = kata[i, j];
            }
        }
        DimensionalColumn[Dimension] = MaxColumn;
        DimensionalRow[Dimension] = MaxRow;
        Dimensional_Drift(-1);
        CurrentColumn = 0;
        CurrentRow = MaxRow - 1;
        CurrentPosition();
    }

    void makeInstance()
    {
        int i, j;
        for (i = 0; i < 128; i++) ifArray[i] = new Ifreference();

        for (i = 0; i < 256; i++)
            for (j = 0; j < 128; j++) ifArray2[i, j] = new Ifreference();

        for (i = 0; i < 64; i++)
            for (j = 0; j < 128; j++) ifArray3[i, j] = new Ifreference();
    }

    //オブジェクトの設置
    void ObjectInstall(GameObject Prefab)
    {
        if (Prefab == null) return;

        Location(CurrentColumn, CurrentRow, 0);

        //オブジェクトをおこうとしている位置にオブジェクトがあれば削除
        //基本的にはBlankPrefab削除
        Destroy(objectArray[CurrentColumn, CurrentRow]);

        //オブジェクトを配列に代入
        objectArray[CurrentColumn, CurrentRow] =
            Instantiate(Prefab, Place, Quaternion.identity);

        //instantiateされたオブジェクトの名前に(Clone)がつかないようにする
        objectArray[CurrentColumn, CurrentRow].name = Prefab.name;

        textMake(CurrentColumn, CurrentRow, Prefab.name);
    }

    void textMake(int column, int row, string name)
    {
        Transform canv =
            objectArray[column, row].transform.Find("Canvas");
        if (canv == null) return;
        GameObject canvObj = canv.gameObject;
        Transform text = canvObj.transform.Find("Text");
        GameObject textObj = text.gameObject;
        Text ObjectText = textObj.GetComponent<Text>();

        //Debug.Log(content[column,row]);
        if (content[column, row] == null || content[column, row] == "")
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
                case "WhileStart_prefab":
                    ObjectText.text = "while";
                    break;
            }
        else
        {
            ObjectText.text = content[column, row];
        }
    }

    //縦ワイヤーの設置
    void WireInstall(GameObject Prefab)
    {
        if (Prefab == null) return;
        Location(CurrentColumn, CurrentRow, 0);
        Destroy(wireArray[CurrentColumn, CurrentRow]);
        Place += new Vector3(0, -0.75f, 1.0f);
        wireArray[CurrentColumn, CurrentRow] =
            Instantiate(Prefab, Place, Quaternion.identity);
        wireArray[CurrentColumn, CurrentRow].name = Prefab.name;
    }

    //横ワイヤーの設置
    void HorizontalWireInstall(GameObject Prefab)
    {
        if (Prefab == null) return;
        Location(CurrentColumn, CurrentRow, 0);
        Destroy(HorizontalwireArray[CurrentColumn, CurrentRow]);
        Place += new Vector3(0, 0, 1.0f);
        HorizontalwireArray[CurrentColumn, CurrentRow] =
            Instantiate(Prefab, Place, Quaternion.identity);
        HorizontalwireArray[CurrentColumn, CurrentRow].name = Prefab.name;
    }

    //printfボタン
    public void PrintfButtonClicked()
    {
        BeyondDimension();
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
    }

    //ifボタン
    public void IfButtonClicked()
    {
        switch (ifFlag)
        {
            case 0:
                BeyondDimension();
                break;
            case 1:
                break;
        }
        tempRow = CurrentRow;
        tempColumn = CurrentColumn;
        whetherIf = true;
        Replace();
        switch (ifFlag)
        {
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

        IFMATOME();
        ifFlag = 0;
        ifCount++;
    }

    public void ForButtonClicked()
    {
        switch (forFlag)
        {
            case 0:
                BeyondDimension();
                break;
            case 1:
                break;
        }
        tempRow = CurrentRow;
        tempColumn = CurrentColumn;
        whetherIf = false;
        Replace();
        switch (forFlag)
        {
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

    public void WhileButtonClicked()
    {
        switch (whileFlag)
        {
            case 0:
                BeyondDimension();
                break;
            case 1:
                break;
        }
        tempRow = CurrentRow;
        tempColumn = CurrentColumn;
        whetherIf = false;
        Replace();
        switch (whileFlag)
        {
            case 0:
                WhileStart();
                break;
            case 1:
                WhileEnd();
                break;
        }
        ButtonClicked();
        CurrentRow++;
        CurrentPosition();
    }

    void WhileStart()
    {
        ObjectInstall(WhileStart_prefab);
        whileFlag = 1;
    }

    void WhileEnd()
    {
        ObjectInstall(WhileEnd_prefab);
        whileFlag = 0;
    }

    public void BreakButtonClicked()
    {
        BeyondDimension();
        tempRow = CurrentRow;
        tempColumn = CurrentColumn;

        Replace();
        ObjectInstall(Break_prefab);
        ButtonClicked();
        CurrentPosition();
        whetherIf = false;
    }


    //全ボタン共通
    void ButtonClicked()
    {
        countColumnRow();
        WireSetting();

        //オブジェクトを途中で挿入しないかどうか
        //(新しい空白を必要とするかどうか)
        if (objectArray[CurrentColumn, CurrentRow + 1] != null)
        {
            if (objectArray[CurrentColumn, CurrentRow + 1].name == "Corner2_prefab") return;
        }
        else if (CheckResult) return;
        else if (objectArray[CurrentColumn, CurrentRow + 1] == null)
        {
            CurrentRow++;
            ObjectInstall(Blank_prefab);
        }
        countColumnRow();
        WireSetting();
        //illegalObjectCheck();
    }

    //すでにオブジェクトがその位置にあるか
    void Replace()
    {
        countColumnRow();
        //名前がBlankPrefabかどうかで空白か判断している
        if (objectArray[CurrentColumn, CurrentRow].name != "Blank_prefab")
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
        int i;
        for (CurrentRow = MaxRow; CurrentRow > tempRow - 1; CurrentRow--)
        {
            for (CurrentColumn = MaxColumn; CurrentColumn > -1; CurrentColumn--)
            {
                if (CurrentRow == 0)
                {
                    break;
                }
                if (CurrentRow == tempRow)
                {
                    if (wireArray[CurrentColumn, CurrentRow - 1] != null)
                    {
                        ObjectInstall(Blank_prefab);
                    }
                }
                else
                {
                    content[CurrentColumn, CurrentRow] = content[CurrentColumn, CurrentRow - 1];
                    content[CurrentColumn, CurrentRow - 1] = "";
                    kata[CurrentColumn, CurrentRow] = kata[CurrentColumn, CurrentRow - 1];
                    kata[CurrentColumn, CurrentRow - 1] = " ";

                    if (objectArray[CurrentColumn, CurrentRow - 1] != null)
                    {
                        ObjectInstall(objectArray[CurrentColumn, CurrentRow - 1]);
                        textMake(CurrentColumn, CurrentRow, objectArray[CurrentColumn, CurrentRow - 1].name);

                        switch (objectArray[CurrentColumn, CurrentRow - 1].name)
                        {
                            case "If_prefab":
                                for (i = 0; i < ifCount; i++)
                                    if (ifArray[i].ifStartRow == CurrentRow - 1)
                                        ifArray[i].ifStartRow = CurrentRow;
                                break;
                            case "Corner1_prefab":
                                for (i = 0; i < ifCount; i++)
                                    if (ifArray[i].ifEndRow == CurrentRow - 1)
                                        ifArray[i].ifEndRow = CurrentRow;
                                break;

                            case "Tatedake_prefab":
                                Destroy(objectArray[CurrentColumn, CurrentRow - 1]);
                                objectArray[CurrentColumn, CurrentRow - 1] = null;
                                break;

                            case "Yokodake_prefab":
                                Destroy(objectArray[CurrentColumn, CurrentRow - 1]);
                                objectArray[CurrentColumn, CurrentRow - 1] = null;
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

    void countColumnRow()
    {
        int column;
        int row;
        MaxColumn = 0;
        MaxRow = 0;
        for (row = 0; row < 128; row++)
        {
            if (objectArray[0, row] == null) break;
            MaxRow++;
        }
        //Debug.Log("maxRow="+maxRow);
        for (column = 1; column < 64; column++)
        {
            for (row = 0; objectArray[column, row] == null; row++)
                if (row == MaxRow)
                    if (objectArray[column, row] == null)
                    {
                        MaxColumn = column;
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
            case "Tatedake_prefab":
                SaveobjectArray[Column, Raw] = "Tatedake_prefab";
                break;
            case "Yokodake_prefab":
                SaveobjectArray[Column, Raw] = "Yokodake_prefab";
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
        for (i = 0; i < MaxColumn; i++)
        {
            for (j = 0; j < MaxRow; j++)
            {
                if (objectArray[i, j] != null)
                {
                    ObjectToString(objectArray[i, j].name, i, j);
                }
            }
        }

        //座標ごとにストレージに保存する（ブロックの種類）
        for (i = 0; i < MaxColumn; i++)
        {
            for (j = 0; j < MaxRow; j++)
            {
                PlayerPrefs.SetString("ObjectArray" + i + j, SaveobjectArray[i, j]);
                // Debug.Log(objectArray[i, j]);
            }
        }
        //座標ごとにストレージに保存する（ブロックの中身）
        for (i = 0; i < MaxColumn; i++)
        {
            for (j = 0; j < MaxRow; j++)
            {
                PlayerPrefs.SetString("contentArray" + i + j, content[i, j]);
                //Debug.Log(content[i, j]);
            }
        }
        //座標ごとストレージに保存する(変数の型情報)
        for (i = 0; i < MaxColumn; i++)
        {
            for (j = 0; j < MaxRow; j++)
            {
                PlayerPrefs.SetString("kataArray" + i + j, kata[i, j]);
                //Debug.Log(content[i, j]);
            }
        }
        PlayerPrefs.SetInt("maxColumn", MaxColumn);
        PlayerPrefs.SetInt("maxRow", MaxRow);
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
        MaxColumn = PlayerPrefs.GetInt("maxColumn", 0);
        MaxRow = PlayerPrefs.GetInt("maxRow", 0);
        tempColumn = -1;
        tempRow = -1;
        LoadObject();
    }
    //再配置
    void LoadObject()
    {
        //Debug.Log("ろーどかいしするよ");
        for (int i = 0; i < MaxColumn; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                GameObject Prefab;

                Prefab = ObjectSearch(SaveobjectArray[i, j]);
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
        messageText.text = "";
        if (ifFlag == 1 || forFlag == 1)
        {
            if (CurrentColumn != tempColumn)
            {
                CurrentColumn = preColumn;
                CurrentRow = preRow;
                messageText.text =
                    "始点としたオブジェクトのある列以外に終点を配置できません";
            }
            else if (CurrentRow <= tempRow)
            {
                CurrentColumn = preColumn;
                CurrentRow = preRow;
                messageText.text =
                    "始点としたオブジェクトより上に終点を配置できません";
            }
            else
            {
                //Debug.Log("ifCount = "+ifCount);
                for (j = 0; j < ifCount; j++)
                {
                    //ifStartが他のifの内側かどうか
                    if (ifArray[j].ifStartColumn == ifArray[ifCount].ifStartColumn)
                        if (ifArray[j].ifStartRow < ifArray[ifCount].ifStartRow)
                            if (ifArray[j].ifEndRow >= ifArray[ifCount].ifStartRow)
                            {
                                InOutCheck = true;
                                if (ifArray[j].ifDistance() < length)
                                {
                                    length = ifArray[j].ifDistance();
                                    temp = j;
                                }
                            }
                }
                //Debug.Log(length);
                if (length == 128) InOutCheck = false;
                //内
                if (InOutCheck)
                {
                    for (i = 0; i < MaxRow; i++)
                    {
                        if (ifArray[temp].ifStartRow < i)
                            if (ifArray[temp].ifEndRow >= i)
                            {
                                place[i] = 1;
                            }
                            else place[i] = 0;
                    }
                }
                //外
                else
                {
                    for (i = 0; i < MaxRow; i++)
                    {
                        act = false;
                        for (j = 0; j <= ifCount; j++)
                        {
                            if (ifArray[j].ifStartColumn == CurrentColumn)
                                if (ifArray[j].ifStartRow < i && ifArray[j].ifEndRow >= i)
                                {
                                    //i行目は位置的に利用不可能
                                    act = true;
                                }
                        }
                        if (act) place[i] = 0;
                        else place[i] = 1;

                    }
                }
                if (place[CurrentRow] == 0)
                {
                    CurrentColumn = preColumn;
                    CurrentRow = preRow;
                    messageText.text =
                        "ワイヤーが交差するようなオブジェクトの配置はできません";
                }
            }
        }
        Location(CurrentColumn, CurrentRow, -1);
        CurrentPlace.transform.position = Place;
        preRow = CurrentRow;
        preColumn = CurrentColumn;
    }

    void WireSetting()
    {
        for (CurrentRow = MaxRow + 1; CurrentRow > -1; CurrentRow--)
        {
            for (CurrentColumn = MaxColumn + 1; CurrentColumn > -1; CurrentColumn--)
            {
                if (wireArray[CurrentColumn, CurrentRow] != null)
                {
                    Destroy(wireArray[CurrentColumn, CurrentRow]);
                    wireArray[CurrentColumn, CurrentRow] = null;
                }
                if (HorizontalwireArray[CurrentColumn, CurrentRow] != null)
                {
                    Destroy(HorizontalwireArray[CurrentColumn, CurrentRow]);
                    HorizontalwireArray[CurrentColumn, CurrentRow] = null;
                }
                if (objectArray[CurrentColumn, CurrentRow] != null)
                {
                    switch (objectArray[CurrentColumn, CurrentRow].name)
                    {
                        case "Printf_prefab":
                            WireInstall(Wire_prefab);
                            break;

                        case "Blank_prefab":
                            if (CurrentRow != MaxRow - 1)
                            {
                                WireInstall(Wire_prefab);
                            }
                            break;

                        case "If_prefab":
                            if (CurrentColumn == tempColumn && CurrentRow == tempRow)
                            {
                                WireInstall(Wire_prefab);
                            }
                            else
                            {
                                WireInstall(Wire_prefab);
                                HorizontalWireInstall(Wire_If_prefab);
                            }
                            break;

                        case "ForEnd_prefab":
                            WireInstall(Wire_prefab);
                            break;

                        case "ForStart_prefab":
                            WireInstall(Wire_prefab);
                            break;

                        case "WhileEnd_prefab":
                            WireInstall(Wire_prefab);
                            break;

                        case "WhileStart_prefab":
                            WireInstall(Wire_prefab);
                            break;

                        case "Calc_prefab":
                            WireInstall(Wire_prefab);
                            break;

                        case "Break_prefab":
                            WireInstall(Wire_prefab);
                            break;

                        case "Subroutine_prefab":
                            WireInstall(Wire_prefab);
                            break;

                        case "Corner1_prefab":
                            WireInstall(Wire_prefab);
                            HorizontalWireInstall(Wire_If_prefab);
                            break;

                        case "Corner2_prefab":
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
    public GameObject Tatedake_prefab;
    public GameObject Yokodake_prefab;

    //上の奴を参考にしてつくった。1:blank 2:printf 3:if 4:ifend 5:fors 6:fore 99:null
    public int ItemCheck2()
    {
        if (!objectArray[CurrentColumn, CurrentRow])
        {
            return 0;
        }
        switch (objectArray[CurrentColumn, CurrentRow].name)
        {
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

            case "WhileStart_prefab":
                return 8;

            default:
                return 0;
        }

    }

    public int SeekThemOut()
    {
        //中は[どこから縦を伸ばしたらいいかな]を調べたいだけなのでmaxだけ返せばok.
        int ifkazu = 0;
        int maxifkazu = 0;
        bool ifaru = false;
        int x = ifArray[ifCount].ifStartColumn;
        int y = ifArray[ifCount].ifStartRow;
        for (y = ifArray[ifCount].ifStartRow + 1; y < ifArray[ifCount].ifEndRow; y++)
        {
            ifkazu = 0;
            x = ifArray[ifCount].ifStartColumn;
            if (objectArray[x, y].name == "If_prefab")
            {
                ifaru = true;
            }
            while (objectArray[x + 1, y] != null)
            {
                ifkazu++;
                x++;
                if (maxifkazu < ifkazu)
                {
                    maxifkazu = ifkazu;
                }
            }
        }
        if (ifaru)
        {
            //    Debug.Log("なかおる；；");
            while (!sonoXcheck(maxifkazu))
            {
                x--;
            }
        }
        else
        {
            maxifkazu = 0;
        }
        //  Debug.Log(maxifkazu);
        return maxifkazu;
    }

    public bool sonoXcheck(int mondaix)
    {
        int x = mondaix;
        int y = ifArray[ifCount].ifStartRow;
        for (y = ifArray[ifCount].ifStartRow + 1; y < ifArray[ifCount].ifEndRow; y++)
        {
            if (objectArray[x, y] != null)
            {
                if (objectArray[x, y].name == "Corner2_prefab")
                {
                    return true;
                }
            }
        }

        return false;
    }

    public int originalX;
    public int XonSentry;
    public void PlacingSentry(int nakax, int starty, int endy, int preflag)
    {
        //外は[スライドを何回、どこからどこまで、すればいいかな]を調べたいが...
        int x = nakax + 1;
        int y = starty;
        int flag = 0;
        for (y = starty; y <= endy; y++)
        {
            if (objectArray[x, y] != null)
            {
                flag = 1;
                // Debug.Log("そとにいる；；");
                break;
            }
        }
        if (flag == 0 && preflag == 1)
        {
            ObjectSlide(x, starty, endy);
            // Debug.Log("すらいどはじめまむ"+x.ToString()+starty.ToString()+endy.ToString());
        }
        else if (flag == 1)
        {
            int nextnakax = x;
            int nextstarty = SearchUpper(x, y);
            int nextendy = SearchLower(x, y);
            PlacingSentry(nextnakax, nextstarty, nextendy, 1);
            if (originalX == x)
            {
                return;
            }
            else
            {
                ObjectSlide(x, starty, endy);
                //   Debug.Log("すらいどしたよ");
            }
        }
    }

    public void ObjectSlide(int startx, int starty, int endy)
    {
        int x = CurrentColumn;
        int y = CurrentRow;
        CurrentColumn = startx;
        for (CurrentRow = starty; CurrentRow <= endy; CurrentRow++)
        {
            ObjectInstall(objectArray[CurrentColumn - 1, CurrentRow]);
            Destroy(objectArray[CurrentColumn - 1, CurrentRow]);
            objectArray[CurrentColumn - 1, CurrentRow] = null;
            content[CurrentColumn, CurrentRow] = content[CurrentColumn - 1, CurrentRow];
            content[CurrentColumn - 1, CurrentRow] = "";
            kata[CurrentColumn, CurrentRow] = kata[CurrentColumn - 1, CurrentRow];
            kata[CurrentColumn - 1, CurrentRow] = " ";
        }
        CurrentRow = starty;
        CurrentColumn--;
        ObjectInstall(Yokodake_prefab);
        CurrentRow = endy;

        //ifをずらしたのだからそれも教えてあげよう
        for (int i = 0; i < ifCount; i++)
        {
            if (ifArray[i].ifCornerRow == CurrentRow && ifArray[i].ifCornerColumn == CurrentColumn)
            {
                ifArray[i].ifCornerColumn++;
            }
        }
        ObjectInstall(Yokodake_prefab);
        CurrentColumn = x;
        CurrentRow = y;
        WireSetting();
    }

    // →、↓、← の順で処理するよ
    public void ImaIfShori(int nakax)
    {
        if (nakax == 0)
        {
            //仮に置いとく、外にifがあると使えない
            CurrentColumn = ifArray[ifCount].ifStartColumn + 1;
            CurrentRow = ifArray[ifCount].ifStartRow;
            ObjectInstall(Tatedake_prefab);
            //    Debug.Log("たておいた");
            //横幅をどれだけにするか求める
            //ifControl(ifFlag);
            CurrentRow = ifArray[ifCount].ifEndRow;
            ObjectInstall(Corner2_prefab);
            //   Debug.Log("こなおいた");
            ifArray[ifCount].ifCornerColumn = CurrentColumn;
            ifArray[ifCount].ifCornerRow = CurrentRow;
            CurrentRow--;
            while (CurrentRow > ifArray[ifCount].ifStartRow)
            {
                ObjectInstall(Blank_prefab);
                CurrentRow--;
            }
            CurrentColumn = ifArray[ifCount].ifEndColumn;
            CurrentRow = ifArray[ifCount].ifEndRow;
            return;
        }
        //中にあった場合。
        CurrentColumn = ifArray[ifCount].ifStartColumn;
        int imacolumn = CurrentColumn;
        nakax += CurrentColumn;
        CurrentRow = ifArray[ifCount].ifStartRow;
        //右にyoko配置
        for (CurrentColumn = ifArray[ifCount].ifStartColumn + 1; CurrentColumn < nakax + 1; CurrentColumn++)
        {
            ObjectInstall(Yokodake_prefab);
        }

        CurrentColumn = nakax + 1;
        ObjectInstall(Tatedake_prefab);

        for (CurrentRow = CurrentRow + 1; CurrentRow < ifArray[ifCount].ifEndRow; CurrentRow++)
        {
            ObjectInstall(Blank_prefab);
        }

        CurrentRow = ifArray[ifCount].ifEndRow;
        ObjectInstall(Corner2_prefab);

        ifArray[ifCount].ifCornerColumn = CurrentColumn;
        ifArray[ifCount].ifCornerRow = CurrentRow;

        for (CurrentColumn = CurrentColumn - 1;
            CurrentColumn > ifArray[ifCount].ifStartColumn;
            CurrentColumn--)
        {
            ObjectInstall(Yokodake_prefab);
        }

        WireSetting();
    }

    public int SearchLower(int x, int y)
    {
        //tatedakeにぶつかってからyokoを探してくれます
        while (objectArray[x, y].name != "Corner2_prefab")
        {
            y++;
        }
        return y;
    }

    public int SearchUpper(int x, int y)
    {
        //tatedakeにぶつかってからyokoを探してくれます
        while (objectArray[x, y].name != "Tatedake_prefab")
        {
            y--;
        }
        return y;
    }

    public void IFMATOME()
    {
        int ys = ifArray[ifCount].ifStartRow;
        int ye = ifArray[ifCount].ifEndRow;

        int p = SeekThemOut();
        originalX = p + CurrentColumn + 1;
        XonSentry = originalX - 1;
        PlacingSentry(XonSentry, ys, ye, 0);
        ImaIfShori(p);
    }

    public void BeyondDimension()
    {
        for (int i = 0; i < MaxColumn; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                if (objectArray[i, j] != null)
                {
                    objectArray3D[Dimension, i, j] = objectArray[i, j].name;
                }
            }
        }
        for (int i = 0; i < MaxColumn; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                content3D[Dimension, i, j] = content[i, j];
            }
        }
        for (int i = 0; i < MaxColumn; i++)
        {
            for (int j = 0; j < MaxRow; j++)
            {
                kata3D[Dimension, i, j] = kata[i, j];
            }
        }
        ifArrayBeyond();
        DimensionalColumn[Dimension] = MaxColumn;
        DimensionalRow[Dimension] = MaxRow;
        //  Debug.Log(Dimension + "がセーブされた");
        //    Debug.Log(objectArray3D[Dimension, 0, 0]);
        Dimension++;
        //    Debug.Log("現在の次元は"+Dimension);
        if (Dimension - 1 == MaxDimension) MaxDimension++;
    }

    void ifArrayBeyond()
    {
        ifCount2D[Dimension] = ifCount;
        for (int i = 0; i < ifCount; i++)
        {
            ifArray2[Dimension, i] = ifArray[i];
        }
    }

    void ifArrayJump()
    {
        ifCount = ifCount2D[TheDimension];
        for (int i = 0; i < ifCount; i++)
        {
            ifArray[i] = ifArray2[TheDimension, i];
        }
    }

    public void Dimensional_Drift(int Time)
    {
        if ((MaxDimension < Dimension + Time) || (0 > Dimension + Time))
        {
            Debug.Log("You don't have privilege of changing the Dimensions");
            return;
        }
        Reset();
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 128; j++)
            {
                if (objectArray[i, j] != null)
                {
                    Destroy(objectArray[i, j]);
                    objectArray[i, j] = null;
                    Destroy(wireArray[i, j]);
                    wireArray[i, j] = null;
                    Destroy(HorizontalwireArray[i, j]);
                    HorizontalwireArray[i, j] = null;
                }
            }
        }

        TheDimension = Dimension + Time;

        ifArrayJump();

        if (Dimension + Time == 0)
        {
            Location(0, 0, 0);
            Destroy(objectArray[0, 0]);

            //オブジェクトを配列に代入
            objectArray[0, 0] = Instantiate(Blank_prefab, Place, Quaternion.identity);

            //instantiateされたオブジェクトの名前に(Clone)がつかないようにする
            objectArray[0, 0].name = "Blank_prefab";
            textMake(CurrentColumn, CurrentRow, "Blank_prefab");
            MaxColumn = 1;
            MaxRow = 1;
            WireSetting();
            CurrentColumn = 0;
            CurrentRow = 0;
            Dimension = 0;
            return;
        }
        Debug.Log(TheDimension + "がよみこまれてるよ");

        for (int i = 0; i < DimensionalColumn[TheDimension]; i++)
        {
            for (int j = 0; j < DimensionalRow[TheDimension]; j++)
            {
                content[i, j] = content3D[TheDimension, i, j];
                kata[i, j] = kata3D[TheDimension, i, j];
                GameObject Prefab;
                switch (objectArray3D[TheDimension, i, j])
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
                    case "Yokodake_prefab":
                        Prefab = Yokodake_prefab;
                        break;
                    case "Tatedake_prefab":
                        Prefab = Tatedake_prefab;
                        break;
                    default:
                        Prefab = null;
                        break;
                }
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
        MaxColumn = DimensionalColumn[TheDimension];
        MaxRow = DimensionalRow[TheDimension];
        WireSetting();
        CurrentColumn = 0;
        CurrentRow = MaxRow - 1;
        CurrentPosition();
        Dimension = TheDimension;
    }

    public void Reset()
    {
        ifCount = 0;
        ifFlag = 0;
        forFlag = 0;
        MaxColumn = 0;
        MaxRow = 0;
        CurrentColumn = 0;
        CurrentRow = 0;
        preColumn = 0;
        preRow = 0;
        tempColumn = -1;
        tempRow = -1;
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
