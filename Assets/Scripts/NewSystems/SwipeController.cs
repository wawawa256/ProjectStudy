using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    float StartPosX;
    float StartPosY;

    void Start()
    {
        mainCamera.orthographicSize = 5.0f;
    }

    //いずれピンチインピンチアウトも実装したい :TODO
    public void Zoom_in()
    {
        if (mainCamera.orthographicSize > 0f)
        {
            mainCamera.orthographicSize -= 0.5f;
        }
    }
    public void Zoom_out()
    {
        mainCamera.orthographicSize += 0.5f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPosX = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
            StartPosY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;
        }
        if (Input.GetMouseButton(0))
        {
            float EndPosX = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
            float EndPosY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;

            float SwipeLengthX = StartPosX - EndPosX;
            float SwipeLengthY = StartPosY - EndPosY;
            
            float CameraPosX = mainCamera.transform.position.x + SwipeLengthX;
            float CameraPosY = mainCamera.transform.position.y + SwipeLengthY;

            mainCamera.transform.position = new Vector3(CameraPosX, CameraPosY, -10);
            StartPosX = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
            StartPosY = mainCamera.ScreenToWorldPoint(Input.mousePosition).y;
        }
    }
}