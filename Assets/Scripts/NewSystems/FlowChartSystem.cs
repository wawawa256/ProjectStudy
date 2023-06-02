using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowChartSystem : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] AddFlowSelectUI addFlowSelectUI;
    [SerializeField] SkillKindSelectUI skillKindSelectUI;
    [SerializeField] SkillSelectUI skillSelectUI;
    [SerializeField] SwipeController swipeController;
    List<FlowChartObject> objects = new List<FlowChartObject>();
    List<FlowChartObject> flowList = new List<FlowChartObject>();
    List<GameObject> presentPrefabs = new();
    List<GameObject> linePrefabs = new();

    private enum State
    {
        View,
        Adding,
        AddFlowSelect,
        SkillKindSelect,
        SkillSelect,
    }
    private State state;
    public bool AddNewObject(FlowChartObject addObject, Vector3 place)
    {
        FlowChartObject original = flowList.Find(obj => obj.Place == place); 
        
        if(original != null)
        {
            Debug.Log($"{original.Name}があった{place}に{addObject.Name}を追加します");

            IfEndObject ifTrueEndObject = new IfEndObject();
            IfEndObject ifFalseEndObject = new IfEndObject();
            WhileEndObject whileEndObject = new WhileEndObject();

            if(addObject is IfObject)
            {
                (addObject as IfObject).TrueList.Add(ifTrueEndObject);
                ifTrueEndObject.Parent = (addObject as IfObject).TrueList;

                (addObject as IfObject).FalseList.Add(ifFalseEndObject);
                ifFalseEndObject.Parent = (addObject as IfObject).FalseList;
            }
            else if(addObject is WhileObject)
            {
                (addObject as WhileObject).Children.Add(whileEndObject);
                whileEndObject.Parent = (addObject as WhileObject).Children;
            }
            List<FlowChartObject> parent = original.Parent;
            addObject.Parent = parent;
            int index = parent.IndexOf(original);
            if(index == -1)
            {
                Debug.Log("error: index == -1");
                return false;
            }
            if (parent != objects && original is BlankObject)
            { 
                flowList.Remove(original);
                parent[index] = addObject; 
            }
            else parent.Insert(index, addObject);

            DisplayFlow();
            
            flowList.Add(addObject);
            if(addObject is IfObject)
            {
                flowList.Add(ifTrueEndObject);
                flowList.Add(ifFalseEndObject);
                DisplayFlow();
            }
            if (addObject is WhileObject)
            {
                flowList.Add(whileEndObject);
                DisplayFlow();
            }
            return true;
        }
        else 
        {
            Debug.Log($"original == null なので{place}にObjectを追加できません");
            return false;
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
            linePrefabs.Add(Instantiate(Resources.Load<GameObject>("Prefabs/FlowChart/VLinePrefab"), LineLocation(column, row), Quaternion.identity));

            if (obj is IfObject)
            {
                linePrefabs.Add(Instantiate(Resources.Load<GameObject>("Prefabs/FlowChart/HLinePrefab"), LineLocation(column, row), Quaternion.identity));
                IfObject ifObject = (IfObject)obj;
                obj.Place = Location(column, row);
                presentPrefabs.Add(Instantiate(obj.Prefab, Location(column, row), Quaternion.identity));
                row++;
                UpdateFlow(ifObject.TrueList, column, row);
                UpdateFlow(ifObject.FalseList, column + ifObject.TrueHSize, row);
                row += ifObject.VSize - 1;
            }
            else if (obj is WhileObject)
            {
                WhileObject whileObject = (WhileObject)obj;
                obj.Place = Location(column, row);
                presentPrefabs.Add(Instantiate(obj.Prefab, Location(column, row), Quaternion.identity));
                row++;
                UpdateFlow(whileObject.Children, column, row);
                row += whileObject.Size - 1;
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
    private Vector3 LineLocation(int column, int row)
    {
        return Location(column, row) + new Vector3(0, 0, 1.0f);
    }

    private void Start()
    {
        Init();
        Test();
    }
    private void Init()
    {
        UIInit();
        AllClear();
    }
    private void UIInit()
    {
        addFlowSelectUI.Close();
        skillKindSelectUI.Close();
        skillSelectUI.Close();

        addFlowSelectUI.OnTouch += () => swipeController.enabled = false;
        addFlowSelectUI.OnRelease += () => swipeController.enabled = true;

        skillKindSelectUI.OnTouch += () => swipeController.enabled = false;
        skillKindSelectUI.OnRelease += () => swipeController.enabled = true;

        addFlowSelectUI.SkillButtonOnClick += SkillKindSelect;
        addFlowSelectUI.IfButtonOnClick += AddFlow;
        addFlowSelectUI.WhileButtonOnClick += AddFlow;
    }
    private void Test()
    {
        AddNewObject(new SkillObject(player.Battler.GetSkill()), Location(0, 0));
        AddNewObject(new SkillObject(player.Battler.GetSkill()), Location(0, 1));

        AddNewObject(new IfObject(), Location(0, 2));
        AddNewObject(new WhileObject(), Location(0, 3));
        AddNewObject(new SkillObject(player.Battler.GetSkill()), Location(0, 4));
        //AddNewObject(new SkillObject(player.Battler.GetSkill()), Location(1, 3));

        //AddNewObject(new IfObject(), Location(0, 4));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Reset();
        }
        switch(state)
        {
            case State.View:
                break;
            case State.Adding:
                AddFlowHandler();
                break;
            case State.AddFlowSelect:
                break;
            case State.SkillKindSelect:
                break;
            case State.SkillSelect:
                break;
        }
    }
    private void AddFlowSelect()
    {
        state = State.AddFlowSelect;
    }
    private void SkillKindSelect()
    {
        state = State.SkillKindSelect;
        Debug.unityLogger.Log("SkillKindSelect");
        skillKindSelectUI.Open();
    }
    private void SkillSelect()
    {
        state = State.SkillSelect;
    }
    private void AddFlow()
    {
        state = State.Adding;
    }
    private void AddFlowHandler()
    {
        //場所をクリックしたらそこが配置可能かどうかを判定
        //配置可能なら、配置しますか？のアラートを出す
        //配置不可能なら配置不可能であることを表示
    }
    private void Reset()
    {
        presentPrefabs.ForEach(obj => Destroy(obj));
        presentPrefabs.Clear();
        linePrefabs.ForEach(obj => Destroy(obj));
        linePrefabs.Clear();
    }
    public void AllClear()
    {
        state = State.View;
        objects.Clear();
        flowList.Clear();
        Reset();

        //最初のObjectを追加
        BlankObject startObject = new BlankObject();
        startObject.Place = Location(0, 0);
        startObject.Parent = objects;
        objects.Add(startObject);
        flowList.Add(startObject);
        presentPrefabs.Add(Instantiate(startObject.Prefab, Location(0, 0), Quaternion.identity));
        UpdateFlow(objects, 0, 0);
    }
}
