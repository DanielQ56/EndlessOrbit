using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] GameObject leaderboardPanel;
    [SerializeField] GameObject board;
    [SerializeField] GameObject ScorePrefab;

    public void ActivateLeaderboard(List<int> scores)
    {
        leaderboardPanel.SetActive(true);
        for(int i = 0; i < scores.Count; ++i)
        {
            GameObject clone = Instantiate(ScorePrefab, board.transform);
            clone.GetComponent<UserScore>().SetVariables(i + 1, (scores[i] > 0 ? scores[i].ToString() : ""));
        }
    }

    public void DeactivateLeaderboard()
    {
        foreach(Transform t in board.transform)
        {
            Destroy(t.gameObject);
        }
        leaderboardPanel.SetActive(false);
    }
}
