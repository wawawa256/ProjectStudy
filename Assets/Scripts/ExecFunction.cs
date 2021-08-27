using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecFunction : MonoBehaviour
{
    public GameObject Objcollection;

    public void ExecUpdateArgDropdown(){
        Objcollection = GameObject.Find("/ScriptObjects/ObjectCollection");
        //Objcollection.UpdateArgDropdowns();
    }
}
