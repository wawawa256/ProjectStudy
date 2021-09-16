using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject StartPanel;
    public GameObject SettingPanel;

    public void OnStart()
    {
        StartPanel.SetActive(false);
    }
    public void SettingOpen()
    {
        SettingPanel.SetActive(true);
    }
    public void StartClose()
    {
        StartPanel.SetActive(true);
    }
    public void SettingClose()
    {
        SettingPanel.SetActive(false);
    }
}