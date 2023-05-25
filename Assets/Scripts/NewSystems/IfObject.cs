using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfObject : FlowChartObject
{
    public int Id { get; }
    public int Size { get => TrueSize + FalseSize; }
    public int TrueSize { get; set; }
    public int FalseSize { get; set; }
    public IfObject(int id)
    {
        Id = id;
        Init();
    }
}