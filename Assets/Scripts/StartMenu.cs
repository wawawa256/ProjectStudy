using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject StartPanel;
    public GameObject SettingPanel;
    public GameObject SavePanel;

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
    public void SaveOpen()
    {
        SavePanel.SetActive(true);
    }
    public void SaveClose()
    {
        SavePanel.SetActive(false);
    }
}