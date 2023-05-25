using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfTrueObject : IfObject
{
    public IfTrueObject(int id) : base(id)
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        TrueSize = 1;
        FalseSize = 1;
        Prefab = Resources.Load<GameObject>("Prefabs/IfObjectPrefab");
    }
}
