using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfEndObject : IfObject
{
    public IfEndObject(int id) : base(id)
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Prefab = Resources.Load<GameObject>("Prefabs/IfEndObjectPrefab");
    }
}
