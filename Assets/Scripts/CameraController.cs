using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    static int CurrentColumn = ObjectCollection.CurrentColumn;
    static int CurrentRow = ObjectCollection.CurrentRow;
    Camera mainCamera;

    void Start()
    {
        mainCamera = GameObject.Find ("MainCamera").GetComponent<Camera>();
    }



    public void reload() {
        CurrentColumn = ObjectCollection.CurrentColumn;
        CurrentRow = ObjectCollection.CurrentRow;
        ObjectCollection.Location(CurrentColumn,CurrentRow,-10);
        //カメラはプレイヤーと同じ位置にする
        mainCamera.transform.position = ObjectCollection.Place;

    }


}