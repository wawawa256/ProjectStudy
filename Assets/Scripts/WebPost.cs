using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;


public class WebPost : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(letsgo());
    }

    public void callfor(){
        if(CodingPanek.activeInHierarchy){
            StartCoroutine(letsgo());
        }
    }

    
    public Text ResultText;
    public GameObject kurukuru;
    public GameObject CodingPanek;
    public string gotid;
    public string statusresult;
    public static string sourceCODE;
    private IEnumerator letsgo(){
        paizaJSON testpaiza = new paizaJSON();
        string url = "http://api.paiza.io:80/runners/create";
        //testpaiza.source_code="#include <stdio.h>\nint main(){\n  printf(\"Hello,World!\");\n   return 0; }";
        testpaiza.source_code=sourceCODE;
        testpaiza.language="c";
        testpaiza.api_key="guest";
        string PaizaJS = JsonUtility.ToJson(testpaiza);
        Debug.Log(PaizaJS);
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(PaizaJS);

        var request = new UnityWebRequest(url, "POST");
        request.method = "POST";
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(postData);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-type", "application/json");
        yield return request.SendWebRequest();

        //Result result = new Result();

        switch ( request.result )
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log( "リクエスト中" );
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log( "リクエスト成功" );
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log
                (
                    @"サーバとの通信に失敗。
リクエストが接続できなかった、
セキュリティで保護されたチャネルを確立できなかったなど。"
                );
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log
                (
                    @"サーバがエラー応答を返した。
サーバとの通信には成功したが、
接続プロトコルで定義されているエラーを受け取った。"
                );
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log
                (
                    @"データの処理中にエラーが発生。
リクエストはサーバとの通信に成功したが、
受信したデータの処理中にエラーが発生。
データが破損しているか、正しい形式ではないなど。"
                );
                break;

         default: break;
        }

        Debug.Log(request.downloadHandler.text);
        string gotdeta=request.downloadHandler.text;
        string ID=gotdeta.Substring(11);
        //Debug.Log(ID);
        string gachiID=ID.Substring(0, ID.IndexOf("\""));
        //Debug.Log(gachiID);
        gotid=gachiID;
        /*while(statusresult!="completed"){
            StartCoroutine(getstatus());
        }*/
        //StartCoroutine(getstatus());
        yield return new WaitForSeconds(3);
        StartCoroutine(getdetail());
    }

    private IEnumerator getstatus(){
        //gotid="dFXHpdPwpgN_isbom2idfg";
        string url = "http://api.paiza.io:80/runners/get_status?id="+gotid+"&api_key=guest";
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        switch ( request.result )
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log( "リクエスト中" );
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log( "リクエスト成功" );
                break;

         default: break;
        }
        Debug.Log(request.downloadHandler.text);
        STATUS stat = JsonUtility.FromJson<STATUS>(request.downloadHandler.text);
        statusresult=stat.status;
        Debug.Log(statusresult);
        }

    private IEnumerator getdetail(){
        //gotid="dFXHpdPwpgN_isbom2idfg";
        string url = "http://api.paiza.io:80/runners/get_details?id="+gotid+"&api_key=guest";
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        switch ( request.result )
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log( "リクエスト中" );
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log( "リクエスト成功" );
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log
                (
                    @"サーバとの通信に失敗。
リクエストが接続できなかった、
セキュリティで保護されたチャネルを確立できなかったなど。"
                );
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log
                (
                    @"サーバがエラー応答を返した。
サーバとの通信には成功したが、
接続プロトコルで定義されているエラーを受け取った。"
                );
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log
                (
                    @"データの処理中にエラーが発生。
リクエストはサーバとの通信に成功したが、
受信したデータの処理中にエラーが発生。
データが破損しているか、正しい形式ではないなど。"
                );
                break;

         default: break;
        }
        Debug.Log(request.downloadHandler.text);
        RESULT res = JsonUtility.FromJson<RESULT>(request.downloadHandler.text);
        Debug.Log(res.build_result);
        Debug.Log(res.stdout);
        kurukuru.SetActive(false);
        ResultText.text=res.stdout;
    }

}

[System.Serializable]
public class paizaJSON{
    public string source_code;
    public string language;
    public string input;
    public bool longpoll;
    public double longpoll_timeout;
    public string api_key;
}

[System.Serializable]
public class STATUS{
    public string id;
    public string status;
    public string error;
}

[System.Serializable]
public class RESULT{
    public string id;
    public string language;
    public string note;
    public string status;
    public string build_stdout;
    public string build_stderr;
    public string build_exit_code;
    public string build_time;
    public string build_memory;
    public string build_result;
    public string stdout;
    public string stderr;
    public string exit_code;
    public string time;
    public string memory;
    public string result;
}