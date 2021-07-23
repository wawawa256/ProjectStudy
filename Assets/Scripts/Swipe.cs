using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Swipe : MonoBehaviour
{
    public float StartPosX;
    public float StartPosY;
    public float EndPosX;
    public float EndPosY;
    public float SwipeLengthX;
    public float SwipeLengthY;
    public float CameraPosX;
    public float CameraPosY;
    public float VerticalSpace = ObjectCollection.VerticalSpace;
    public float HorizontalSpace = ObjectCollection.HorizontalSpace;
    public float startX = ObjectCollection.startX;
    public float startY = ObjectCollection.startY;
    public float maxPosX;
    public float maxPosY;

    int CurrentColumn;
    int CurrentRow;

    int maxColumn;
    int maxRow;
    Camera mainCamera;

    void Start()
    {
        mainCamera = GameObject.Find ("MainCamera").GetComponent<Camera>();
    }

    void Update ()
    {
        if(Input.GetMouseButtonDown(0))
        {    
            maxColumn = ObjectCollection.maxColumn;
            maxRow = ObjectCollection.maxRow;
            maxPosX = HorizontalSpace * maxColumn;
            maxPosY = VerticalSpace * maxRow;

            StartPosX = mainCamera.ScreenToWorldPoint (Input.mousePosition).x;
            StartPosY = mainCamera.ScreenToWorldPoint (Input.mousePosition).y;
        }
        if (Input.GetMouseButton (0))
        {
            EndPosX = mainCamera.ScreenToWorldPoint (Input.mousePosition).x;
            EndPosY = mainCamera.ScreenToWorldPoint (Input.mousePosition).y;
            SwipeLengthX = StartPosX - EndPosX;
            SwipeLengthY = StartPosY - EndPosY;
            CameraPosX = mainCamera.transform.position.x;
            CameraPosY = mainCamera.transform.position.y;

            UpperSetting();

            mainCamera.transform.position = 
                new Vector3 (CameraPosX + SwipeLengthX, CameraPosY + SwipeLengthY, -10);
            StartPosX = mainCamera.ScreenToWorldPoint (Input.mousePosition).x;
            StartPosY = mainCamera.ScreenToWorldPoint (Input.mousePosition).y;
        }
    }

    void UpperSetting()
    {
        if(CameraPosX + SwipeLengthX < 0)
        {          
            CameraPosX = 0;
            SwipeLengthX = 0;
        }
        else if(CameraPosX + SwipeLengthX > maxPosX)
        {
            CameraPosX = maxPosX;
            SwipeLengthX = 0;
        }

        if(CameraPosY + SwipeLengthY > 0)
        {           
            CameraPosY = 0;
            SwipeLengthY = 0;
        }
        else if(CameraPosY + SwipeLengthY < maxPosY)
        {           
            CameraPosY = maxPosY;
            SwipeLengthY = 0;
        }
    }
}