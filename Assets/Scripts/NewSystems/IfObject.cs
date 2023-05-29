using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfObject : FlowChartObject
{
    public int Id { get; }//この概念消したいかも
    public int HSize { get => TrueHSize + FalseHSize; }
    public int VSize { get => TrueVSize > FalseVSize ? TrueVSize : FalseVSize; }
    public int TrueHSize { get; set; }
    public int FalseHSize { get; set; }
    public int TrueVSize { get => TrueList.Count; }
    public int FalseVSize { get => FalseList.Count; }

    public List<FlowChartObject> TrueList { get; set; }
    public List<FlowChartObject> FalseList { get; set; }
    public IfObject(int id, List<FlowChartObject> parent): base(parent)
    {
        Id = id;
        Init();
    }
}