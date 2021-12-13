using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class API_Register : Singleton<API_Register>
{
    public string registerData;
    public bool testing;
    public bool registed;

    public void Start()
    {
        registed = false;
        //StartCoroutine(Register());
    }

    public void StartRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Get("https://api.happenn.com/v2/projects/297/users/" + GetUserId()))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                registerData = www.downloadHandler.text;
                Debug.Log(registerData);
                StartCoroutine(AddPlayer());
            }
        }
    }
    IEnumerator AddPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("player", "PTTCGDay2021-" + GetUserId());
        form.AddField("info", registerData);

        using (UnityWebRequest www = UnityWebRequest.Post("https://universal-leaderboards.hocco.work/players/add", form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + API_Login.Instance.loginData.jwt);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                registed = true;
            }
        }
    }


    string GetUserId()
    {
        string[] urls;
        string userId = "";

        if (testing)
        {
            urls = "www.game.com?user_id=222501".Split(new string[] { "?user_id=" }, System.StringSplitOptions.None);
        }
        else
        {
            urls = Application.absoluteURL.Split(new string[] { "?user_id=" }, System.StringSplitOptions.None);
        }

        if (urls.Length > 1)
        {
            userId = urls[1].Trim();
        }
        return userId;
    }
}
