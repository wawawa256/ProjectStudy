using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewObjectSystem : MonoBehaviour
{
    [SerializeField] PlayerController player;
    List<FlowChartObject> objects = new List<FlowChartObject>();
    List<FlowChartObject> flowList = new List<FlowChartObject>();
    List<GameObject> presentPrefabs = new();
    [SerializeField] GameObject ifPrefab;

    public void AddNewObject(FlowChartObject addObject, Vector3 place)
    {
        FlowChartObject original = flowList.Find(obj => obj.Place == place); 
        
        if(original != null)
        {
            Debug.Log($"{original.Name}があった{place}に{addObject.Name}を追加します");

            IfEndObject ifTrueEndObject = new IfEndObject();
            IfEndObject ifFalseEndObject = new IfEndObject();

            if(addObject is IfObject)
            {
                (addObject as IfObject).TrueList.Add(ifTrueEndObject);
                ifTrueEndObject.Parent = (addObject as IfObject).TrueList;

                (addObject as IfObject).FalseList.Add(ifFalseEndObject);
                ifFalseEndObject.Parent = (addObject as IfObject).FalseList;
            }
            List<FlowChartObject> parent = original.Parent;
            addObject.Parent = parent;
            ShowListForDebug(parent);
            int index = parent.IndexOf(original);
            if(index == -1)
            {
                Debug.Log("error: index == -1");
                return;
            }
            if (parent != objects && original is BlankObject)
            { 
                flowList.Remove(original);
                parent[index] = addObject; 
            }
            else parent.Insert(index, addObject);
            DisplayFlow(); //ifのサイズを調整
            flowList.Add(addObject);
            if(addObject is IfObject)
            {
                flowList.Add(ifTrueEndObject);
                flowList.Add(ifFalseEndObject);
                DisplayFlow();
            } 
            return;
        }
        else 
        {
            Debug.Log($"{place}にObjectを追加できません");
        }
    }

    private void DisplayFlow()
    {
        List<FlowChartObject> ifList = flowList.FindAll(obj => obj is IfObject);
        foreach(FlowChartObject obj in ifList)
        {
            if(obj is IfObject)
            {
                IfObject ifObject = (IfObject)obj;
                while (true)
                {
                    //Debug.Log($"ifObject.VSize:{ifObject.VSize}");
                    //if(ifObject.VSize>1)Debug.Log($"ifObject.TrueList[ifObject.VSize - 2]:{ifObject.TrueList[ifObject.VSize - 2]}");
                    BlankObject blankObject = new BlankObject();
                    if (ifObject.TrueVSize > ifObject.FalseVSize)
                    {
                        ifObject.FalseList.Insert(ifObject.FalseList.Count - 1, blankObject);
                        blankObject.Parent = ifObject.FalseList;
                        flowList.Add(blankObject);
                    }
                    else if (ifObject.TrueVSize < ifObject.FalseVSize)
                    {
                        ifObject.TrueList.Insert(ifObject.TrueList.Count - 1, blankObject);
                        blankObject.Parent = ifObject.TrueList;
                        flowList.Add(blankObject);
                    }
                    else break;
                }
            }
        }
        Reset();
        UpdateFlow(objects, 0, 0);
    }

    private void UpdateFlow(List<FlowChartObject> list, int column, int row)
    {
        if(list.Count == 0)
        {
            return;
        }
        foreach (FlowChartObject obj in list)
        {
            if (obj is IfObject)
            {
                IfObject ifObject = (IfObject)obj;
                obj.Place = Location(column, row);
                presentPrefabs.Add(Instantiate(obj.Prefab, Location(column, row), Quaternion.identity));
                row++;
                UpdateFlow(ifObject.TrueList, column, row);
                UpdateFlow(ifObject.FalseList, column + ifObject.TrueHSize, row);
                row += ifObject.VSize - 1;
            }
            else
            {
                presentPrefabs.Add(Instantiate(obj.Prefab, Location(column, row), Quaternion.identity));
                obj.Place = Location(column, row);
                row++;
            }
        }
    }

    private Vector3 Location(int column, int row)
    {
        return new Vector3(Constant.HorizontalSpace * column, Constant.VerticalSpace * row + 1.0f, 0);
    }

    private void Start()
    {
        Init();
        Test();
    }
    private void Init()
    {
        BlankObject startObject = new BlankObject();
        startObject.Place = Location(0, 0);
        startObject.Parent = objects;
        objects.Add(startObject);
        flowList.Add(startObject);
    }
    private void Test()
    {
        AddNewObject(new SkillObject(player.Battler.GetSkill()), Location(0, 0));
        AddNewObject(new SkillObject(player.Battler.GetSkill()), Location(0, 1));

        AddNewObject(new IfObject(), Location(0, 2));
        AddNewObject(new SkillObject(player.Battler.GetSkill()), Location(0, 3));
        AddNewObject(new SkillObject(player.Battler.GetSkill()), Location(1, 3));

        AddNewObject(new IfObject(), Location(0, 3));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Reset();
        }
    }
    private void Reset()
    {
        foreach (GameObject obj in presentPrefabs)
        {
            Destroy(obj);
        }
        presentPrefabs.Clear();
    }
    private void ShowListForDebug(List<FlowChartObject> list)
    {
        foreach (FlowChartObject obj in list)
        {
            Debug.Log(obj.Name);
        }
    }
}
