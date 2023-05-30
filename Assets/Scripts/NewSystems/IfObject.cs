using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfObject : FlowChartObject
{
    public int HSize { get => TrueHSize + FalseHSize; }
    public int VSize { get => TrueVSize > FalseVSize ? TrueVSize : FalseVSize; }
    public int TrueHSize { get => GetTrueHSize(); }
    public int FalseHSize { get => GetFalseHSize(); }
    public int TrueVSize { get => TrueList.Count; }
    public int FalseVSize { get => FalseList.Count; }

    public List<FlowChartObject> TrueList { get; set; }
    public List<FlowChartObject> FalseList { get; set; }
    int GetTrueHSize()
    {
        int trueHSize = 1;
        foreach (var item in TrueList)
        {
            if(item is IfObject)
            {
                if((item as IfObject).HSize > trueHSize)
                {
                    trueHSize = (item as IfObject).HSize;
                }
            }
        }
        return trueHSize;
    }
    int GetFalseHSize()
    {
        int falseHSize = 1;
        foreach (var item in TrueList)
        {
            if (item is IfObject)
            {
                if ((item as IfObject).HSize > falseHSize)
                {
                    falseHSize = (item as IfObject).HSize;
                }
            }
        }
        return falseHSize;
    }
    public IfObject(): base()
    {
        Name = "IfObject";
        Prefab = Resources.Load<GameObject>("Prefabs/IfPrefab");
        TrueList = new List<FlowChartObject>();
        FalseList = new List<FlowChartObject>();

        Init();
    }
}