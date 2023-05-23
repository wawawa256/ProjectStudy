using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public void OnClickWiki()
    {
        Application.OpenURL("http://ucchon.php.xdomain.jp/wiki.php");//""の中には開きたいWebページのURLを入力します
    }
    public void OnClickQuetion()
    {
        Application.OpenURL("http://ucchon.php.xdomain.jp/newindex.php");//""の中には開きたいWebページのURLを入力します
    }
}