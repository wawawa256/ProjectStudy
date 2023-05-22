using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] ObjectCollection objectCollection;

    int CurrentColumn;
    int CurrentRow;
    Camera mainCamera;

    void Start()
    {
        mainCamera = GameObject.Find ("MainCamera").GetComponent<Camera>();
        CurrentColumn = objectCollection.CurrentColumn;
        CurrentRow = objectCollection.CurrentRow;
    }

    public void Reload() {
        CurrentColumn = objectCollection.CurrentColumn;
        CurrentRow = objectCollection.CurrentRow;
        objectCollection.Location(CurrentColumn,CurrentRow,-10);
        //カメラはプレイヤーと同じ位置にする
        mainCamera.transform.position = objectCollection.Place;

    }


}