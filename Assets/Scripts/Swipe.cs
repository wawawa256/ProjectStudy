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
    public static int[] maxRow = new int[128];
    public static int CurrentFunction;
    Camera mainCamera;
    float i = 0.0f;

    void Start()
    {
        mainCamera = GameObject.Find ("MainCamera").GetComponent<Camera>();
       // Debug.Log(mainCamera.orthographicSize);
    }
    public void zoom_in()
    {
        if(i>0.0f)
        {
            i = i - 0.5f;
            mainCamera.orthographicSize = 5.0f + i;  
        }
        
    }
    public void zoom_out()
    {
        i = i + 0.5f;
        mainCamera.orthographicSize = 5.0f + i;  
    }
    

    void Update ()
    {

        if(Input.GetMouseButtonDown(0))
        {
            CurrentFunction = ObjectCollection.CurrentFunction;
            maxColumn = ObjectCollection.maxColumn;
            maxRow = ObjectCollection.maxRow;
            maxPosX = HorizontalSpace * maxColumn;
            maxPosY = VerticalSpace * maxRow[CurrentFunction];

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