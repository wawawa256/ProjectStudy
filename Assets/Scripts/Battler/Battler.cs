using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battler : MonoBehaviour
{
    BattlerBase _base;
    int level;
    int attackPower;
    int defensePower;
    int speed;
    int maxHp;
    int currentHp;

    public BattlerBase Base => _base;
    
    public void Init()
    {
        level = Base.InitLevel;
        attackPower = Base.OffencePower;
        defensePower = Base.DefensePower;
        speed = Base.Speed;
        maxHp = Base.MaxHp;
        currentHp = maxHp;
    }

    

}
