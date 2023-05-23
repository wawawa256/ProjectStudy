using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattlerBase : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] int initLevel;
    [SerializeField] int offenceBasePower;
    [SerializeField] int defenceBasePower;
    [SerializeField] List<SkillBase> learnableSkills;

    public string Name { get => _name; set => _name = value; }
    public int InitLevel { get => initLevel; set => initLevel = value; }
    public int OffenceBasePower { get => offenceBasePower; set => offenceBasePower = value; }
    public int DefenceBasePower { get => defenceBasePower; set => defenceBasePower = value; }
    public List<SkillBase> LearnableSkills { get => learnableSkills; set => learnableSkills = value; }
}