using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileEndObject : FlowChartObject
{
    public WhileEndObject() : base()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Name = "WhileEnd";
        Prefab = Resources.Load<GameObject>("Prefabs/FlowChart/WhileEndPrefab");
    }
}
