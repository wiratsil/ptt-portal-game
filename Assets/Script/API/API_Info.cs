using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using InfoAPI;

public class API_Info : Singleton<API_Info>
{
    [SerializeField]
    public MyInfo myInfo;

    public void GetInfo()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        yield return new WaitUntil(() => API_Register.Instance.registed);
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Get("https://universal-leaderboards.hocco.work/projects/me"))
        {
            www.SetRequestHeader("Authorization", "Bearer " + API_Login.Instance.loginData.jwt);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                myInfo = MyInfo.CreateFromJSON(www.downloadHandler.text);
            }
        }
    }

}

namespace InfoAPI
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    [System.Serializable]
    public class CreatedBy
    {
        public int id ;
        public string firstname ;
        public string lastname ;
        public object username ;
        public string email ;
        public string password ;
        public object resetPasswordToken ;
        public object registrationToken ;
        public bool isActive ;
        public object blocked ;
    }
    [System.Serializable]
    public class UpdatedBy
    {
        public int id ;
        public string firstname ;
        public string lastname ;
        public object username ;
        public string email ;
        public string password ;
        public object resetPasswordToken ;
        public object registrationToken ;
        public bool isActive ;
        public object blocked ;
    }
    [System.Serializable]
    public class Owner
    {
        public int id ;
        public string username ;
        public string email ;
        public string provider ;
        public string password ;
        public object resetPasswordToken ;
        public object confirmationToken ;
        public bool confirmed ;
        public bool blocked ;
        public int role ;
        public int created_by ;
        public int updated_by ;
        public DateTime created_at ;
        public DateTime updated_at ;
        public int project ;
    }
    [System.Serializable]
    public class Game
    {
        public int id ;
        public string name ;
        public int project ;
        public DateTime published_at ;
        public int created_by ;
        public int updated_by ;
        public DateTime created_at ;
        public DateTime updated_at ;
    }
    [System.Serializable]
    public class Info
    {
        public string name ;
    }
    [System.Serializable]
    public class Player
    {
        public int id ;
        public string identity ;
        public Info info ;
        public object game ;
        public DateTime published_at ;
        public int created_by ;
        public int updated_by ;
        public DateTime created_at ;
        public DateTime updated_at ;
        public int project ;
    }
    [System.Serializable]
    public class MyInfo
    {
        public int id ;
        public string name ;
        public DateTime published_at ;
        public CreatedBy created_by ;
        public UpdatedBy updated_by ;
        public DateTime created_at ;
        public DateTime updated_at ;
        public int users_permissions_user ;
        public Owner owner ;
        public List<Game> games ;
        public List<Player> players ;

        public static MyInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<MyInfo>(jsonString);
        }
    }

}


