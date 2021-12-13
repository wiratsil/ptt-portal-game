using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

public class Portal_Manager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenInTab(string url);

    public PlayerBoard playerBoard1;
    public PlayerBoard playerBoard2;
    public Transform leaderParent;
    [Space]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI scoreText;
    [Space]
    public bool testing;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetLeaderboard());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _OpenUrl(string url)
    {
        if (!API_Register.Instance.registed)
            return;
        //Application.OpenURL(url);
        OpenInTab(url);
    }

    IEnumerator SetLeaderboard()
    {
        yield return new WaitUntil(() => API_Scoreboard.Instance.leaderBoard.Length > 0);
        for (int i = 0; i < API_Scoreboard.Instance.leaderBoard.Length; i++)
        {
            if (i >= 50)
                break;
            PlayerBoard clone;

            if (i % 2 == 0)
            {
                clone = Instantiate(playerBoard1, leaderParent);
                clone.rankText.text = API_Scoreboard.Instance.leaderBoard[i].rank.ToString();
                clone.nameText.text = API_Scoreboard.Instance.leaderBoard[i].player.info.name;
                clone.scoreText.text = API_Scoreboard.Instance.leaderBoard[i].score.ToString();
            }
            else
            {
                clone = Instantiate(playerBoard2, leaderParent);
                clone.rankText.text = API_Scoreboard.Instance.leaderBoard[i].rank.ToString();
                clone.nameText.text = API_Scoreboard.Instance.leaderBoard[i].player.info.name;
                clone.scoreText.text = API_Scoreboard.Instance.leaderBoard[i].score.ToString();
            }
        }

        //Set Info
        for (int i = 0; i < API_Scoreboard.Instance.leaderBoard.Length; i++)
        {
            string[] userId = API_Scoreboard.Instance.leaderBoard[i].player.identity.Split('-');
            string myUserId = GetUserId();

            if (myUserId == userId[1])
            {
                nameText.text = API_Scoreboard.Instance.leaderBoard[i].player.info.name;
                rankText.text = API_Scoreboard.Instance.leaderBoard[i].rank.ToString();
                scoreText.text = API_Scoreboard.Instance.leaderBoard[i].score.ToString();
            }
        }
    }


    string GetUserId()
    {
        string[] urls;
        string userId = "";

        if (testing)
        {
            urls = "www.game.com?user_id=1234".Split(new string[] { "?user_id=" }, System.StringSplitOptions.None);
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
