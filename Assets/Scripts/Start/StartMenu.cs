using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject StartPanel;

    public void OnStart()
    {
        StartPanel.SetActive(false);
    }
    public void StartClose()
    {
        StartPanel.SetActive(true);
    }
}