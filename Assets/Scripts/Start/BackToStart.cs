using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToStart : MonoBehaviour
{
    public void OnStart()
    {
        SceneManager.LoadScene("StartGame");
    }
    public void OnStart2()
    {
        SceneManager.LoadScene("StartGame");
        SceneManager.LoadScene("EditScene");
    }
}