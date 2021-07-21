using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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