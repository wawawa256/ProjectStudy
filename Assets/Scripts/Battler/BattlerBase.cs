using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattlerBase : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] int initLevel;
    [SerializeField] int offencePower;
    [SerializeField] int defensePower;
    [SerializeField] int speed;
    [SerializeField] int maxHp;
    [SerializeField] List<SkillBase> learnableSkills;

    public string Name { get => _name; set => _name = value; }
    public int InitLevel { get => initLevel; set => initLevel = value; }
    public int OffencePower { get => offencePower; set => offencePower = value; }
    public int DefensePower { get => defensePower; set => defensePower = value; }
    public int Speed { get => speed; set => speed = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public List<SkillBase> LearnableSkills { get => learnableSkills; set => learnableSkills = value; }
}