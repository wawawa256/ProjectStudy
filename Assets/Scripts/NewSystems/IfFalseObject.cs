using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfFalseObject : FlowChartObject
{
    public IfFalseObject(): base()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Name = "IfFalse";
        Prefab = Resources.Load<GameObject>("Prefabs/IfFalseObjectPrefab");
    }
}
