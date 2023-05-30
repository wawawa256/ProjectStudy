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
            Debug.Log($"{place}に{addObject.Name}を追加します");

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
            int index = parent.IndexOf(original);
            parent.Insert(index, addObject);
            flowList.Add(addObject);
            Reset();
            AdjustIfSize(); //ifのサイズを調整
            UpdateFlow(objects, 0, 0);
            if(addObject is IfObject)
            {
                flowList.Add(ifTrueEndObject);
                flowList.Add(ifFalseEndObject);
            }
            return;
        }
        else 
        {
            Debug.Log($"{place}にObjectを追加できません");
        }
    }

    private void AdjustIfSize()
    {
        List<FlowChartObject> ifList = flowList.FindAll(obj => obj is IfObject);
        foreach(FlowChartObject obj in ifList)
        {
            if(obj is IfObject)
            {
                IfObject ifObject = (IfObject)obj;
                while (true)
                {
                    BlankObject blankObject = new BlankObject();
                    if (ifObject.TrueList.Count > ifObject.FalseList.Count)
                    {
                        ifObject.FalseList.Insert(ifObject.FalseList.Count - 1,blankObject);
                        blankObject.Parent = ifObject.FalseList;
                        flowList.Add(blankObject);
                    }
                    else if (ifObject.TrueList.Count < ifObject.FalseList.Count)
                    {
                        ifObject.TrueList.Insert(ifObject.TrueList.Count - 1, blankObject);
                        blankObject.Parent = ifObject.TrueList;
                        flowList.Add(blankObject);
                    }
                    else if(ifObject.TrueList.Count>1 && ifObject.FalseList.Count>1)
                    {
                        if (
                            ifObject.TrueList[ifObject.TrueList.Count - 2] is BlankObject &&
                            ifObject.FalseList[ifObject.FalseList.Count - 2] is BlankObject
                            )
                        {
                            ifObject.TrueList.RemoveAt(ifObject.TrueList.Count - 1);
                            ifObject.FalseList.RemoveAt(ifObject.FalseList.Count - 1);
                            flowList.Remove(ifObject.TrueList[ifObject.TrueList.Count - 1]);
                            flowList.Remove(ifObject.FalseList[ifObject.FalseList.Count - 1]);
                        } //多分大丈夫なはず
                    }
                    else break;
                }
            }
        }
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
                row += ifObject.VSize;
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
}
