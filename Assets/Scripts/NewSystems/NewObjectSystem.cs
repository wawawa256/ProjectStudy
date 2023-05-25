using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewObjectSystem : MonoBehaviour
{
    [SerializeField] PlayerController player;
    List<FlowChartObject> objects = new List<FlowChartObject>();
    List<IfObject> ifObjects = new List<IfObject>();

    GameObject[] objectPrefabs;
    List<int> existIds = new List<int>();
    int[] alreadyCalculatedTrue;
    int[] alreadyCalculatedFalse; //もう計算したものはもう良くない？の気持ち

    int[] trueLengths;
    int[] falseLengths;
    int[] trueLengthsInList;
    int[] falseLengthsInList;

    public void AddNewObject(FlowChartObject obj)//どこに？は必要そう
    {
        objects.Add(obj);
        if (obj is IfObject)
        {
            ifObjects.Add((IfObject)obj);
        }
    }

    void RemoveObject(int code)
    {
        objects.RemoveAt(code);
    }

    void Display()
    {
        CalculateIfSize();
        PrintPrefabs();
    }

    void CalculateIfSize()
    {
        alreadyCalculatedTrue = new int[Constant.MAX_IF_OBJECTS];
        alreadyCalculatedFalse = new int[Constant.MAX_IF_OBJECTS];
        trueLengths = new int[Constant.MAX_IF_OBJECTS];
        falseLengths = new int[Constant.MAX_IF_OBJECTS];
        trueLengthsInList = new int[Constant.MAX_IF_OBJECTS];
        falseLengthsInList = new int[Constant.MAX_IF_OBJECTS];
        foreach (int id in existIds)
        {
            foreach (IfObject obj in ifObjects)
            {
                if (obj.Id == id)
                {
                    obj.TrueSize = GetTrueSize(id);
                    obj.FalseSize = GetFalseSize(id);
                    //Debug.Log("id:" + id + " true:" + obj.TrueSize + " false:" + obj.FalseSize);
                }
            }
        }
    }

    void PrintPrefabs()
    {
        Stack<int> nextStack = new Stack<int>();
        Stack<int> rowStack = new Stack<int>();
        int row = 0;
        int column = 0;
        int i = 0;
        foreach (FlowChartObject obj in objects)
        {
            if (obj is IfObject)
            {
                IfObject ifObject = (IfObject)obj;

                if (obj is IfTrueObject)
                {
                    //IfTrueObjectの大きさにresizeして表示する
                    nextStack.Push(row);
                }
                else if (obj is IfFalseObject)
                {
                    column += ifObject.TrueSize;
                    row = nextStack.Pop();
                    rowStack.Push(row);
                }
                else if (obj is IfEndObject)
                {
                    row = rowStack.Pop()+GetLongerLength(ifObject.Id);
                    column -= ifObject.TrueSize;
                }
            }
            objectPrefabs[i] = Instantiate(obj.Prefab, Location(column, row), Quaternion.identity);
            TextMake(i, obj.Name);
            i++;
            row++;
        }
    }

    Vector3 Location(int column, int row)
    {
        return new Vector3(Constant.HorizontalSpace * column, Constant.VerticalSpace * row + 1.0f, 0);
    }

    int GetSize(List<IfObject> list, int id)
    {
        //sizeは最大値の判定にのみ使用します。
        int size = 1;
        foreach (IfObject item in list)
        {
            if (item.Id == id)
            {
                return size;
            }
            else if (item is IfTrueObject)
            {
                if (size < item.Size)
                {
                    item.TrueSize = GetTrueSize(item.Id);
                    item.FalseSize = GetFalseSize(item.Id);
                    size = item.Size;
                }
            }
        }
        Debug.Log("error");
        alreadyCalculatedTrue[id] = -1;
        alreadyCalculatedFalse[id] = -1;
        return -1;
    }
    int GetTrueSize(int id)
    {
        if (alreadyCalculatedTrue[id] != 0)
        {
            return alreadyCalculatedTrue[id];
        }
        alreadyCalculatedTrue[id] = GetSize(GetListTrueToFalse(id), id);
        return alreadyCalculatedTrue[id];
    }
    int GetFalseSize(int id)
    {
        if (alreadyCalculatedFalse[id] != 0)
        {
            return alreadyCalculatedFalse[id];
        }
        alreadyCalculatedFalse[id] = GetSize(GetListFalseToEnd(id), id);
        return alreadyCalculatedFalse[id];
    }

    List<IfObject> GetListTrueToFalse(int id)
    {
        int start = 0;

        for (int i = 0;i<ifObjects.Count;i++)
        {
            if (ifObjects[i] is IfTrueObject && ifObjects[i].Id == id)
            {
                start = i;
            }
            else if (ifObjects[i] is IfFalseObject && ifObjects[i].Id == id)
            {
                int end = i;
                int size = end - start;

                //最初のifは含まないlistを返す
                return ifObjects.GetRange(start+1, size);
            }
        }
        Debug.Log("error");
        return null;
    }
    List<IfObject> GetListFalseToEnd(int id)
    {
        int start = 0;

        for (int i = 0; i < ifObjects.Count; i++)
        {
            if (ifObjects[i] is IfFalseObject && ifObjects[i].Id == id)
            {
                start = i;
            }
            else if (ifObjects[i] is IfEndObject && ifObjects[i].Id == id)
            {
                int end = i;
                int size = end - start;

                //最初のifは含まないlistを返す
                return ifObjects.GetRange(start+1, size);
            }
        }
        Debug.Log("error");
        return null;
    }
    int GetLongerLength(int id)
    {
        int trueLength = GetTrueLengths(id);
        int falseLength = GetFalseLengths(id);
        //Debug.Log("id:" + id + " true:" + trueLength + " false:" + falseLength);
        if (trueLength > falseLength)
        {
            return trueLength;
        }
        else
        {
            return falseLength;
        }
    }
    int GetTrueLengths(int id)
    {
        if (trueLengths[id] != 0)
        {
            return trueLengths[id];
        }
        bool isInScope = false;
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] is IfObject)
            {
                IfObject ifObject = (IfObject)objects[i];
                if (ifObject.Id == id && objects[i] is IfTrueObject)
                {
                    trueLengths[id] = 0;
                    isInScope = true;
                }
                else if (ifObject.Id == id && objects[i] is IfFalseObject)
                {
                    trueLengths[id] += 1;
                    return trueLengths[id];
                }
                else
                {
                    if (isInScope)
                    {
                        trueLengths[id] += GetLongerLength(ifObject.Id) + 1;
                        i += GetTrueLengthsInList(ifObject.Id) + GetFalseLengthsInList(ifObject.Id);
                    }
                }
            }
            else trueLengths[id] += 1;
        }
        Debug.Log("error");
        return -1;
    }
    int GetFalseLengths(int id)
    {
        if (falseLengths[id] != 0)
        {
            return falseLengths[id];
        }
        bool isInScope = false;
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] is IfObject)
            {
                IfObject ifObject = (IfObject)objects[i];
                if (ifObject.Id == id && ifObject is IfFalseObject)
                {
                    falseLengths[id] = 0;
                    isInScope = true;
                }
                else if (ifObject.Id == id && ifObject is IfEndObject)
                {
                    falseLengths[id] += 1;
                    return falseLengths[id];
                }
                else
                {
                    if(isInScope)
                    {
                        falseLengths[id] += GetLongerLength(ifObject.Id) + 1;
                        i += GetTrueLengthsInList(ifObject.Id) + GetFalseLengthsInList(ifObject.Id);
                    }
                }
            }
            else falseLengths[id] += 1;
        }
        Debug.Log("error");
        return -1;
    }
    int GetTrueLengthsInList(int id)
    {
        int start = 0;
        int end = 0;
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] is IfTrueObject)
            {
                if(((IfObject)objects[i]).Id == id)start = i;
            }
            if (objects[i] is IfFalseObject)
            {
                if (((IfObject)objects[i]).Id == id){
                    end = i;
                    return end - start;
                } 
            }
        }
        return -1;
    }
    int GetFalseLengthsInList(int id)
    {
        int start = 0;
        int end = 0;
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] is IfFalseObject)
            {
                if (((IfObject)objects[i]).Id == id) start = i;
            }
            if (objects[i] is IfEndObject)
            {
                if (((IfObject)objects[i]).Id == id)
                {
                    end = i;
                    return end - start;
                }
            }
        }
        return -1;
    }
    void ResizeFlow()
    {
        //ifのsizeに応じてFlowのサイズを変更する
    }

    void TextMake(int i, string _name)
    {
        Transform canv = objectPrefabs[i].transform.Find("Canvas");
        if (canv == null) return;
        GameObject canvObj = canv.gameObject;
        Transform text = canvObj.transform.Find("Text");
        GameObject textObj = text.gameObject;
        Text ObjectText = textObj.GetComponent<Text>();
        ObjectText.text = _name;
    }


    void Start()
    {
        Init();
        Test();
        Display();
    }
    void Init()
    {
        objectPrefabs = new GameObject[Constant.MAX_OBJECTS];
    }
    void Test()
    {
        AddNewObject(new SkillObject(player.Battler.GetSkill()));
        AddNewObject(new SkillObject(player.Battler.GetSkill()));
        AddNewObject(new IfTrueObject(3));
        AddNewObject(new IfTrueObject(4));
        AddNewObject(new SkillObject(player.Battler.GetSkill()));
        AddNewObject(new SkillObject(player.Battler.GetSkill()));
        //AddNewObject(new SkillObject(player.Battler.GetSkill()));
        AddNewObject(new IfFalseObject(4));
        AddNewObject(new IfTrueObject(2));
        //AddNewObject(new SkillObject(player.Battler.GetSkill()));
        AddNewObject(new IfFalseObject(2));
        //AddNewObject(new SkillObject(player.Battler.GetSkill()));
        AddNewObject(new IfEndObject(2));
        AddNewObject(new IfEndObject(4));
        AddNewObject(new IfFalseObject(3));
        AddNewObject(new SkillObject(player.Battler.GetSkill()));
        AddNewObject(new SkillObject(player.Battler.GetSkill()));
        AddNewObject(new SkillObject(player.Battler.GetSkill()));
        AddNewObject(new SkillObject(player.Battler.GetSkill()));
        AddNewObject(new SkillObject(player.Battler.GetSkill()));
        AddNewObject(new IfEndObject(3));
        //idは統一したものを使います
        existIds.Add(1);
        existIds.Add(2);
        existIds.Add(3);
        existIds.Add(4);
    }
    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            //Reset();
        }
    }
    void Reset()
    {
        foreach (GameObject obj in objectPrefabs)
        {
            Destroy(obj);
        }
    }
}
