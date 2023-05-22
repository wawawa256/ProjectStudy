using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : SkillBase
{
    public SkillBase Base { get; }
    public Skill(SkillBase _base)
    {
        Base = _base;
    }
}
