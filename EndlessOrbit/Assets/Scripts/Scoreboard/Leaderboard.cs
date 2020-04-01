using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] GameObject boardHolder;
    [SerializeField] GameObject localBoard;
    [SerializeField] GameObject globalBoard;
    [SerializeField] ToggleParent toggleParent;

    bool recentScoreIdentified = false;

    private void OnEnable()
    {
        boardHolder.SetActive(false);
    }

    public void DisplayLeaderboard(bool isUnstable)
    {
        Debug.Log("Getting called");
        boardHolder.SetActive(true);
        toggleParent.UpdateToggles(isUnstable);
    }

    public void ActivateLeaderboard(List<int> scores, int recentScore)
    {
        localBoard.SetActive(true);
        globalBoard.SetActive(false);
        recentScoreIdentified = false;
        for (int i = 0; i < 10; ++i)
        {
            if (i < scores.Count && scores[i] > 0)
            {
                bool foundRecentScore = (recentScore > 0 && scores[i] == recentScore && !recentScoreIdentified);
                localBoard.transform.GetChild(i).gameObject.SetActive(true);
                localBoard.transform.GetChild(i).GetComponent<UserScore>().SetVariables(i + 1, (scores[i] > 0 ? scores[i].ToString() : ""), foundRecentScore);
                if (foundRecentScore)
                    recentScoreIdentified = true;
            }
            else
            {
                localBoard.transform.GetChild(i).gameObject.SetActive(false);
            }

        }
    }

    public void ActivateGlobalBoard(Players[] players, int recentScore, string playerName)
    {
        localBoard.SetActive(false);
        globalBoard.SetActive(true);
        recentScoreIdentified = false;
        for (int i = 0; i < 10; ++i)
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
