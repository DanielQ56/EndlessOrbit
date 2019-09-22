using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] GameObject leaderboardPanel;
    [SerializeField] GameObject board;
    [SerializeField] GameObject ScorePrefab;

    bool recentScoreIdentified = false;

    public void ActivateLeaderboard(List<int> scores, int recentScore)
    {
        Debug.Log(recentScore);
        leaderboardPanel.SetActive(true);
        for(int i = 0; i < scores.Count; ++i)
        {
            bool foundRecentScore = (recentScore > 0 && scores[i] == recentScore && !recentScoreIdentified);
            GameObject clone = Instantiate(ScorePrefab, board.transform);
            clone.GetComponent<UserScore>().SetVariables(i + 1, (scores[i] > 0 ? scores[i].ToString() : ""), foundRecentScore);
            if (foundRecentScore)
                recentScoreIdentified = true;

        }
        recentScoreIdentified = false;
    }

    public void DeactivateLeaderboard()
    {
        foreach(Transform t in board.transform)
        {
            Destroy(t.gameObject);
        }
        leaderboardPanel.SetActive(false);
    }

    public void ClearLeaderboard()
    {
        StartCoroutine(ClearBoard());
    }
    
    IEnumerator ClearBoard()
    {
        foreach (Transform t in board.transform)
        {
            Destroy(t.gameObject);
        }
        ScoreManager.instance.DeleteAllData();
        yield return new WaitForSeconds(1f);
        ScoreManager.instance.displayScores();
    }
}
