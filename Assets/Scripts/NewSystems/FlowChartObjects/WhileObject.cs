using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileObject : FlowChartObject
{
    public int Size { get => GetSize(); }

    public List<FlowChartObject> Children { get; set; }
    int GetSize()
    {
        int size = 1;
        foreach (var item in Children)
        {
            if (item is IfObject)
            {
                size += (item as IfObject).VSize;
            }
            else if (item is WhileObject)
            {
                size += (item as WhileObject).Size;
            }
            else size++;
        }
        return size;
    }
    public WhileObject() : base()
    {
        Name = "WhileObject";
        Prefab = Resources.Load<GameObject>("Prefabs/FlowChart/WhilePrefab");
        Children = new List<FlowChartObject>();
        Init();
    }
}