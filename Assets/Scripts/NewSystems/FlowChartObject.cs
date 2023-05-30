using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowChartObject
{
    public string Name { get; set; }
    public List<FlowChartObject> Parent { get; set; }
    
    public GameObject Prefab { get; set; }
    public Vector3 Place { get; set; }
    public virtual void Init() { }
    public FlowChartObject()
    {
    }
}
