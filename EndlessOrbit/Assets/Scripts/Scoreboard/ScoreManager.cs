using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    [SerializeField] Leaderboard leader;
    [SerializeField] GlobalLeaderboard globalLeader;
    [SerializeField] ErrorPanel error;

    
    void Awake()
    {
        if (instance== null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        LoadScores();

    }

    #region global

    private const string AddScore = "https://endless-orbit.herokuapp.com/add_score";
    private const string GetScores = "https://endless-orbit.herokuapp.com/scores";

    public void DisplayGlobalScores()
    {
        StartCoroutine(DisplayGlobal());
    }

    IEnumerator DisplayGlobal()
    {
        using (UnityWebRequest scores = UnityWebRequest.Get(GetScores))
        {
            char[] charsToTrim = new char[] { '[', ']' };
            scores.chunkedTransfer = false;
            UnityWebRequestAsyncOperation request = scores.SendWebRequest();

            yield return request;

            if (scores.responseCode == 500)
            {
                ErrorOccurred("Could not load leaderboard.");
            }
            else
            {
                GlobalScores gs = JsonUtility.FromJson<GlobalScores>("{\"result\":" + scores.downloadHandler.text.Trim() + "}");
                globalLeader.ActivateLeaderboard(gs.result, mostRecentScore);
            }
        }
    }

    void ErrorOccurred(string text)
    {
        error.gameObject.SetActive(true);
        error.SetText(text);
    }

    #endregion

    #region local
    int mostRecentScore = 0;

    int[] scores = new int[10];

    List<int> tempScores = new List<int>();

    void LoadScores()
    {
        if(File.Exists(Application.persistentDataPath + "/scores.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/scores.dat", FileMode.Open);

            Scores topScores = (Scores)bf.Deserialize(file);
            file.Close();

            scores = topScores.scores;
            mostRecentScore = scores[scores.Length - 1];
        }
    }

    public void RecordScore(int score)
    {
        if (score <= scores[0])
        {
            return;
        }

        mostRecentScore = score;

        scores[0] = score;
        Array.Sort(scores);
    }

    public void SaveScores()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/scores.dat", FileMode.Create);

        Scores topScores = new Scores();
        topScores.scores = scores;

        bf.Serialize(file, topScores);
        file.Close();
    }

    public void displayLocalScores()
    {
        tempScores.Clear();
        tempScores.AddRange(scores);
        tempScores.Reverse();
        leader.ActivateLeaderboard(tempScores, mostRecentScore);
    }

    public int GetHighScore()
    {
        return scores[scores.Length - 1];
    }

    public void DeleteAllData()
    {
        string path = Application.persistentDataPath + "/scores.dat";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        Array.Clear(scores, 0, 10);
        mostRecentScore = 0;
    }

    private void OnApplicationQuit()
    {
        SaveScores();
    }
    #endregion
}

[Serializable] 
public class GlobalScores
{
    public Players[] result;
}

[Serializable]
public class Players
{
    public string username;
    public int score;
}


[Serializable]
class Scores
{
    public int[] scores;
}