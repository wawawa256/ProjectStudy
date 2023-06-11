using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfObject : FlowChartObject
{
    public int HSize { get => TrueHSize + FalseHSize; }
    public int VSize { get => TrueVSize > FalseVSize ? TrueVSize : FalseVSize; }
    public int TrueHSize { get => GetTrueHSize(); }
    public int FalseHSize { get => GetFalseHSize(); }
    public int TrueVSize { get => GetTrueVSize(); }
    public int FalseVSize { get => GetFalseVSize(); }

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
    int GetTrueVSize()
    {
        int trueVSize = 1;
        foreach (var item in TrueList)
        {
            if (item is IfObject)
            {
                trueVSize += (item as IfObject).VSize;
            }
            else if (item is WhileObject)
            {
                trueVSize += (item as WhileObject).Size;
            }
            else trueVSize++;
        }
        return trueVSize;
    }
    int GetFalseVSize()
    {
        int falseVSize = 1;
        foreach (var item in FalseList)
        {
            if (item is IfObject)
            {
                falseVSize += (item as IfObject).VSize;
            }
            else if (item is WhileObject)
            {
                falseVSize += (item as WhileObject).Size;
            }
            else falseVSize++;
        }
        return falseVSize;
    }
    public IfObject(): base()
    {
        Name = "IfObject";
        Prefab = Resources.Load<GameObject>("Prefabs/FlowChart/IfPrefab");
        TrueList = new List<FlowChartObject>();
        FalseList = new List<FlowChartObject>();

        Init();
    }
}