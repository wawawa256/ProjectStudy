using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfTrueObject : IfObject
{
    public IfTrueObject(int id, List<FlowChartObject> parent) : base(id, parent)
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        TrueHSize = 1;
        FalseHSize = 1;
        Name = "IfTrue";
        Prefab = Resources.Load<GameObject>("Prefabs/IfTrueObjectPrefab");
    }
}
