using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] GameObject leaderboardPanel;
    [SerializeField] GameObject board;
    [SerializeField] GameObject ScorePrefab;

    bool recentScoreIdentified = false;

    private void Start()
    {
        leaderboardPanel.SetActive(false);
    }

    public void ActivateLeaderboard(List<int> scores, int recentScore)
    {
        leaderboardPanel.SetActive(true);
        recentScoreIdentified = false;
        for (int i = 0; i < scores.Count; ++i)
        {
            bool foundRecentScore = (recentScore > 0 && scores[i] == recentScore && !recentScoreIdentified);
            board.transform.GetChild(i).GetComponent<UserScore>().SetVariables(i + 1, (scores[i] > 0 ? scores[i].ToString() : ""), foundRecentScore);
            if (foundRecentScore)
                recentScoreIdentified = true;

        }
    }

    public void DeactivateLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }

    public void ClearLeaderboard()
    {
        StartCoroutine(ClearBoard());
    }
    
    IEnumerator ClearBoard()
    {
        ScoreManager.instance.DeleteAllData();
        leaderboardPanel.gameObject.SetActive(false);
        yield return null;
        ScoreManager.instance.displayScores();
    }
}
