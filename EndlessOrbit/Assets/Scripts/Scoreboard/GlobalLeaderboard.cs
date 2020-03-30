using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLeaderboard : MonoBehaviour
{
    [SerializeField] GameObject globalBoard;

    bool recentScoreIdentified = false;

    public void ActivateLeaderBoard(Players[] players, int recentScore, string playerName)
    {
        globalBoard.transform.SetAsFirstSibling();
        recentScoreIdentified = false;
        for (int i = 0; i < globalBoard.transform.childCount; ++i)
        {
            if (i < players.Length && players[i].score > 0)
            {
                bool foundRecentScore = (recentScore > 0 && players[i].score == recentScore && !recentScoreIdentified && playerName == players[i].username);
                globalBoard.transform.GetChild(i).gameObject.SetActive(true);
                globalBoard.transform.GetChild(i).GetComponent<GlobalScore>().SetVariables(i + 1, players[i].username, players[i].score, foundRecentScore);
                if (foundRecentScore)
                    recentScoreIdentified = true;
            }
            else
            {
                globalBoard.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
