using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Swipe : MonoBehaviour
{
    [SerializeField] ObjectCollection objectCollection;

    public float StartPosX;
    public float StartPosY;
    public float EndPosX;
    public float EndPosY;
    public float SwipeLengthX;
    public float SwipeLengthY;
    public float CameraPosX;
    public float CameraPosY;
    public float VerticalSpace;
    public float HorizontalSpace;
    public float startX;
    public float startY;
    public float maxPosX;
    public float maxPosY;

    int maxColumn;
    int maxRow;
    Camera mainCamera;
    float i = 0.0f;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        VerticalSpace = Constant.VerticalSpace;
        HorizontalSpace = Constant.HorizontalSpace;
        startX = Constant.startX;
        startY = Constant.startY;
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
            maxColumn = objectCollection.maxColumn;
            maxRow = objectCollection.maxRow;
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