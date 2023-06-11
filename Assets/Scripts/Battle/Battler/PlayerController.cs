using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Battler Battler{ get; set; }
    [SerializeField] BattlerBase battlerBase;
    void Start()
    {
        Battler = new Battler();
        Battler.Init(battlerBase);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
