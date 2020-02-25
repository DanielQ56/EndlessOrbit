using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    [SerializeField] Leaderboard leader;
    [SerializeField] GlobalLeaderboard globalLeader;
    [SerializeField] GameObject LoadingPanel;
    [SerializeField] GameObject error;

    
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
        if(!PlayerPrefs.HasKey("Username"))
            PlayerPrefs.SetString("Username", "");

    }

    #region global

    private const string AddScore = "https://endless-orbit.herokuapp.com/add_score";
    private const string GetScores = "https://endless-orbit.herokuapp.com/scores";


    GlobalScores gs;
    bool shouldCheckForGlobal;

    bool retrieving = false;

    private void Start()
    {
        shouldCheckForGlobal = (PlayerPrefs.GetString("Username") != "");
    }

    void SaveScoreToGlobal()
    {
        if(shouldCheckForGlobal)
        {
            Debug.Log("Retrieving");
            StartCoroutine(RetrieveGlobal(false));
        }
    }

    void OnGlobalLeaderboard()
    {
        Debug.Log("Updated the scores");
        Players lastScore = gs.result[9];
        if(mostRecentScore >= lastScore.score && String.Compare(PlayerPrefs.GetString("Username"), lastScore.username) < 0)
        {
            StartCoroutine(SaveGlobal());
        }
        else
        {
            retrieving = false;
        }
    }

    IEnumerator SaveGlobal()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", PlayerPrefs.GetString("Username"));
        form.AddField("score", mostRecentScore);
        using(UnityWebRequest newScore = UnityWebRequest.Post(AddScore, form))
        {
            newScore.chunkedTransfer = false;
            UnityWebRequestAsyncOperation request = newScore.SendWebRequest();
            yield return request;
            if(newScore.responseCode == 500)
            {
                error.SetActive(true);
            }
            Debug.Log("Success!");
            retrieving = false;
        }
    }

    public void DisplayGlobalScores()
    {
        StartCoroutine(RetrieveGlobal(true));
    }

    IEnumerator RetrieveGlobal(bool display)
    {
        while (retrieving)
            yield return null;
        using (UnityWebRequest scores = UnityWebRequest.Get(GetScores))
        {
            retrieving = true;
            char[] charsToTrim = new char[] { '[', ']' };
            scores.chunkedTransfer = false;
            UnityWebRequestAsyncOperation request = scores.SendWebRequest();

            LoadingPanel.SetActive(display);
            yield return request;
            LoadingPanel.SetActive(false);

            if (scores.responseCode == 500)
            {
                error.SetActive(true);
            }
            else
            {
                gs = JsonUtility.FromJson<GlobalScores>("{\"result\":" + scores.downloadHandler.text.Trim() + "}");
                if (display)
                {
                    globalLeader.ActivateLeaderboard(gs.result, mostRecentScore, PlayerPrefs.GetString("Username"));
                    retrieving = false;
                }
                else
                    OnGlobalLeaderboard();

            }
           
        }
    }

    public void SetName(string name)
    {
        if(name == "")
        {
            shouldCheckForGlobal = false;
        }
        else
        {
            shouldCheckForGlobal = true;
        }
        PlayerPrefs.SetString("Username", name);
    }

    public string GetName()
    {
        return PlayerPrefs.GetString("Username");
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
        mostRecentScore = score;
        SaveScoreToGlobal();

        if (score <= scores[0])
        {
            return;
        }

 

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