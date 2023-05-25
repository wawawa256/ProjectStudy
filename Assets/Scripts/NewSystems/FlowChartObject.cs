using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowChartObject
{
    public enum Kinds
    {
        Skill,
        IfTrue,
        IfFalse,
        IfEnd,
        For,
        ForEnd,
        While,
        WhileEnd
    }
    public Kinds Type{ get; set; }
    public GameObject Prefab { get; set; }
    public virtual void Init() { }
    public FlowChartObject()
    {
        
    }
}
