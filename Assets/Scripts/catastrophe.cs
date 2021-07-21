using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catastrophe : MonoBehaviour
{
    public void AllDestroy()
    {
        GameObject[] objects;
        objects = GameObject.FindGameObjectsWithTag("yourobj");
        for(int i = 0; i < objects.Length; ++i)
        {
	        Destroy(objects[i].gameObject);
        }
	
    }
}
