using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battler
{
    BattlerBase _base;
    int level;
    int attackPower;
    int defensePower;
    int speed;
    int maxHp;
    int currentHp;

    public BattlerBase Base => _base;
    
    public void Init(BattlerBase _base)
    {
        this._base = _base;
        level = Base.InitLevel;
        attackPower = Base.OffencePower;
        defensePower = Base.DefensePower;
        speed = Base.Speed;
        maxHp = Base.MaxHp;
        currentHp = maxHp;
    }
    
    public SkillBase GetSkill()
    {
        int index = Random.Range(0, Base.LearnableSkills.Count);
        return Base.LearnableSkills[index];
    }


}
