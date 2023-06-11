using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowChartSystem : MonoBehaviour
{
    // Instances
    [SerializeField] PlayerController player;
    [SerializeField] AddFlowSelectUI addFlowSelectUI;
    [SerializeField] SkillKindSelectUI skillKindSelectUI;
    [SerializeField] SkillSelectUI skillSelectUI;
    [SerializeField] SwipeController swipeController;

    // Lists
    List<FlowChartObject> objects = new List<FlowChartObject>();
    List<FlowChartObject> flowList = new List<FlowChartObject>();
    List<GameObject> presentPrefabs = new();
    List<GameObject> linePrefabs = new();

    //Prefabs
    GameObject VLinePrefab;
    GameObject HLinePrefab;

    FlowChartObject nextAddObject;

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

        if (original != null)
        {
            Debug.Log($"{original.Name}があった{place}に{addObject.Name}を追加します");

            IfEndObject ifTrueEndObject=null;
            IfEndObject ifFalseEndObject=null;
            WhileEndObject whileEndObject=null;

            if (addObject is IfObject)
            {
                ifTrueEndObject = new IfEndObject();
                ifFalseEndObject = new IfEndObject();

                (addObject as IfObject).TrueList.Add(ifTrueEndObject);
                ifTrueEndObject.Parent = (addObject as IfObject).TrueList;

                (addObject as IfObject).FalseList.Add(ifFalseEndObject);
                ifFalseEndObject.Parent = (addObject as IfObject).FalseList;
            }
            else if (addObject is WhileObject)
            {
                whileEndObject = new WhileEndObject();
                   (addObject as WhileObject).Children.Add(whileEndObject);
                whileEndObject.Parent = (addObject as WhileObject).Children;
            }
            List<FlowChartObject> parent = original.Parent;
            addObject.Parent = parent;

            int index = parent.IndexOf(original);
            if (index == -1)
            {
                Debug.Log("error: index == -1");
                return false;
            }
            if (parent != objects && original is BlankObject)
            {
                flowList.Remove(original);
                parent[index] = addObject;
            }
            else
                parent.Insert(index, addObject);

            DisplayFlow();

            flowList.Add(addObject);
            if (addObject is IfObject)
            {
                flowList.Add(ifTrueEndObject);
                flowList.Add(ifFalseEndObject);
                DisplayFlow();
            }
            else if (addObject is WhileObject)
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

    private bool IsInstallable(Vector3 place)
    {
        if (flowList.Find(obj => obj.Place == place) != null) return true;
        else return false;
    }

    private void DisplayFlow()
    {
        List<FlowChartObject> ifList = flowList.FindAll(obj => obj is IfObject);
        foreach (FlowChartObject obj in ifList)
        {
            if (obj is IfObject)
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
                    else
                    {
                        break;
                    }
                }
            }
        }
        Reset();
        MakeFlowChart(objects, 0, 0);
    }

    private void MakeFlowChart(List<FlowChartObject> list, int column, int row)
    {
        if (list.Count == 0)
        {
            return;
        }
        foreach (FlowChartObject obj in list)
        {
            linePrefabs.Add(Instantiate(VLinePrefab, LineLocation(column, row), Quaternion.identity));

            if (obj is IfObject)
            {
                linePrefabs.Add(
                    Instantiate(HLinePrefab, LineLocation(column, row),Quaternion.identity));
                IfObject ifObject = (IfObject)obj;
                obj.Place = Location(column, row);
                presentPrefabs.Add(Instantiate(obj.Prefab, Location(column, row), Quaternion.identity));

                //次の行以降を描写
                row++;
                MakeFlowChart(ifObject.TrueList, column, row);
                MakeFlowChart(ifObject.FalseList, column + ifObject.TrueHSize, row);
                row += ifObject.VSize - 1;
            }
            else if (obj is WhileObject)
            {
                WhileObject whileObject = (WhileObject)obj;
                obj.Place = Location(column, row);
                presentPrefabs.Add(
                    Instantiate(obj.Prefab, Location(column, row), Quaternion.identity)
                );
                row++;
                MakeFlowChart(whileObject.Children, column, row);
                row += whileObject.Size - 1;
            }
            else
            {
                presentPrefabs.Add(
                    Instantiate(obj.Prefab, Location(column, row), Quaternion.identity)
                );
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
        VLinePrefab = Resources.Load<GameObject>("Prefabs/FlowChart/VLinePrefab");
        HLinePrefab = Resources.Load<GameObject>("Prefabs/FlowChart/HLinePrefab");

        UIInit();
        AllClear();
    }

    private void UIInit()
    {
        addFlowSelectUI.OnTouch += () => swipeController.enabled = false;
        addFlowSelectUI.OnRelease += () => swipeController.enabled = true;

        skillKindSelectUI.OnTouch += () => swipeController.enabled = false;
        skillKindSelectUI.OnRelease += () => swipeController.enabled = true;

        addFlowSelectUI.SkillButtonOnClick += SkillKindSelect;
        addFlowSelectUI.IfButtonOnClick += AddIfObject;
        addFlowSelectUI.WhileButtonOnClick += AddWhileObject;
    }

    private void Test()
    {
        AddNewObject(new SkillObject(player.Battler.GetSkill()), Location(0, 0));
        AddNewObject(new SkillObject(player.Battler.GetSkill()), Location(0, 1));

        AddNewObject(new IfObject(), Location(0, 2));
        AddNewObject(new WhileObject(), Location(0, 3));
        AddNewObject(new SkillObject(player.Battler.GetSkill()), Location(0, 4));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Reset();
        }
        switch (state)
        {
            case State.View:
                break;
            case State.Adding:
                if(Input.GetMouseButtonDown(0))
                {
                    GameObject selectedObject;

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D rayCastHit2D = Physics2D.Raycast(ray.origin, ray.direction);
                    if (rayCastHit2D)
                    {
                        selectedObject = rayCastHit2D.transform.gameObject;
                        Vector3 place = selectedObject.transform.position;
                        StartCoroutine(SelectToWhereAddFlow(place));
                    }
                }
                break;
            case State.AddFlowSelect:
                break;
            case State.SkillKindSelect:
                break;
            case State.SkillSelect:
                break;
        }
    }
    public void View()
    {
        state = State.View;
    }

    public void AddFlowSelect()
    {
        state = State.AddFlowSelect;
        addFlowSelectUI.Open();
    }

    private void AddIfObject()
    {
        state = State.Adding;
        nextAddObject = new IfObject();
    }
    private void AddWhileObject()
    {
        state = State.Adding;
        nextAddObject = new WhileObject();
    }

    private void SkillKindSelect()
    {
        Debug.Log("SkillKindSelect");
        state = State.SkillKindSelect;
        skillKindSelectUI.Open();
    }

    private void SkillSelect()
    {
        state = State.SkillSelect;
        skillSelectUI.Open();
    }

    private void AddSkillObject()
    {
        state = State.Adding;
        nextAddObject = new SkillObject(player.Battler.GetSkill());
    }

    private IEnumerator SelectToWhereAddFlow(Vector3 place)
    {
        //場所をクリックしたらそこが配置可能かどうかを判定
        //配置可能なら、配置しますか？のアラートを出す
        //配置不可能なら配置不可能であることを表示

        if (IsInstallable(place))
        {
            bool isApplied;
            isApplied = true; //いずれ確認アラートを実装
            if (isApplied)
            {
                Debug.Log("Added");
                AddNewObject(nextAddObject, place);
                nextAddObject = null;
                addFlowSelectUI.Close();
                View();
                yield break;
            }
        }
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
        MakeFlowChart(objects, 0, 0);
    }
}
