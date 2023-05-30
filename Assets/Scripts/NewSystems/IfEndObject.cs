using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfEndObject : FlowChartObject
{
    public IfEndObject() : base()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Name = "IfEnd";
        Prefab = Resources.Load<GameObject>("Prefabs/IfEndPrefab");
    }
}
