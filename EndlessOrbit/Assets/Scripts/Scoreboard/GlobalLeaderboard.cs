using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLeaderboard : MonoBehaviour
{
    [SerializeField] GameObject leaderboardPanel;
    [SerializeField] GameObject board;

    bool recentScoreIdentified = false;

    private void Start()
    {
        leaderboardPanel.SetActive(false);
    }

    public void ActivateLeaderboard(Players[] players, int recentScore, string playerName)
    {
        leaderboardPanel.SetActive(true);
        Debug.Log(players.Length);
        recentScoreIdentified = false;
        for (int i = 0; i < board.transform.childCount; ++i)
        {
            if (i < players.Length && players[i].score > 0)
            {
                bool foundRecentScore = (recentScore > 0 && players[i].score == recentScore && !recentScoreIdentified && playerName == players[i].username);
                board.transform.GetChild(i).gameObject.SetActive(true);
                board.transform.GetChild(i).GetComponent<GlobalScore>().SetVariables(i + 1, players[i].username, players[i].score, foundRecentScore);
                if (foundRecentScore)
                    recentScoreIdentified = true;
            }
            else
            {
                board.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
