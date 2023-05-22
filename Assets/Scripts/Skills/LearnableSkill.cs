using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LearnableSkill : SkillBase
{
    public SkillBase Base { get; set; }
    public LearnableSkill(SkillBase _base)
    {
        Base = _base;
    }
}
