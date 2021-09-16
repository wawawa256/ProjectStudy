using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public void wiki()
    {
        Application.OpenURL("http://ucchon.php.xdomain.jp/wiki.php");
    }
    public void question()
    {
        Application.OpenURL("http://ucchon.php.xdomain.jp/newindex.php");
    }
}