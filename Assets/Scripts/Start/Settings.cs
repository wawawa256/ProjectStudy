using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickWiki()
    {
        Application.OpenURL("http://ucchon.php.xdomain.jp/wiki.php");//""の中には開きたいWebページのURLを入力します
    }
    public void OnClickQuetion()
    {
        Application.OpenURL("http://ucchon.php.xdomain.jp/newindex.php");//""の中には開きたいWebページのURLを入力します
    }
}