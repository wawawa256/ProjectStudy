using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankObject : FlowChartObject
{
    public BlankObject() : base()
    {
        Name = "Blank";
        Prefab = Resources.Load<GameObject>("Prefabs/BlankPrefab");
        Init();
    }
}
