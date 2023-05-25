using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfTrueObject : IfObject
{
    public int Size { get => TrueSize + FalseSize; }
    public int TrueSize { get; set; }
    public int FalseSize { get; set; }
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
