using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject : FlowChartObject
{
    public SkillBase SkillBase { get; set; }
    public SkillObject(SkillBase skillBase, int id) : base(id)
    {
        SkillBase = skillBase;
        Init();
    }
    public override void Init()
    {
        base.Init();
        Prefab = Resources.Load<GameObject>("Prefabs/SkillObjectPrefab");
    }
}
