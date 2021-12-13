using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LeaderBoardAPI;

public class API_Scoreboard : Singleton<API_Scoreboard>
{
    [SerializeField]
    public LeaderBoard[] leaderBoard;

    public void GetLeaderBoard()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        yield return new WaitUntil(() => API_Register.Instance.registed);
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Get("https://universal-leaderboards.hocco.work/scores/leaderboards"))
        {
            www.SetRequestHeader("Authorization", "Bearer " + API_Login.Instance.loginData.jwt);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                leaderBoard = getJsonArray<LeaderBoard>(www.downloadHandler.text);
            }
        }
    }
    public static T[] getJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }

}

namespace LeaderBoardAPI
{

    [System.Serializable]
    public class Info
    {
        public string name;
    }
    [System.Serializable]
    public class Player
    {
        public string identity;
        public Info info;
    }
    [System.Serializable]
    public class LeaderBoard
    {
        public int rank;
        public Player player;
        public int score;
    }

}


