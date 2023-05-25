using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfFalseObject : IfObject
{
    public IfFalseObject(int id): base(id)
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
