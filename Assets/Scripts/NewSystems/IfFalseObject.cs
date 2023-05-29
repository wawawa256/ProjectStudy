using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfFalseObject : IfObject
{
    public IfFalseObject(int id, List<FlowChartObject> parent): base(id, parent)
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
