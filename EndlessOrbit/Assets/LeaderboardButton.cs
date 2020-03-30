using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardButton : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(DisplayLeaderboard);
    }

    void DisplayLeaderboard()
    {
        if(ScoreManager.instance)
        {
            ScoreManager.instance.UpdateToggles(MainGameManager.instance != null ? MainGameManager.instance.isUnstableMode(): false);
        }
    }

    private void OnDestroy()
    {
        this.GetComponent<Button>().onClick.RemoveListener(DisplayLeaderboard);
    }
}
