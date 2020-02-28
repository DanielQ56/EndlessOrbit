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
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadScores();
    }

    #region global

    private const string AddScore = "https://endless-orbit.herokuapp.com/add_score";
    private const string GetScores = "https://endless-orbit.herokuapp.com/scores";


    GlobalScores gs;
    bool shouldCheckForGlobal;

    bool retrieving = false;

    string username;


    void SaveScoreToGlobal()
    {
        Debug.Log("Username: " + username);
        if ((shouldCheckForGlobal = (username != "")))
        {
            Debug.Log("Retrieving");
            StartCoroutine(RetrieveGlobal(false));
        }
    }

    void OnGlobalLeaderboard()
    {

        Players lastScore = gs.result[9];
        Debug.Log("Most recent score is: " + mostRecentScore + ", the last place score is: " + lastScore.score);
        if (mostRecentScore > lastScore.score || (mostRecentScore == lastScore.score  && String.Compare(username, lastScore.username) < 0))
        {
            Debug.Log("Saving score of " + mostRecentScore + " to user " + username);
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
        form.AddField("username", username);
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
        username = name;
    }

    public string GetName()
    {
        return username;
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

            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            scores = data.scores;
            username = data.username;
            PlayerManager.instance.Setup(data.goldstars, data.silverstars);
            mostRecentScore = scores[scores.Length - 1];
        }
        else
        {
            username = "";
            PlayerManager.instance.Setup();
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
        Debug.Log("Saving scores.");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/scores.dat", FileMode.Create);

        GameData data = new GameData();
        data.scores = scores;
        data.username = username;
        data.goldstars = PlayerManager.instance.GetGoldStars();
        data.silverstars = PlayerManager.instance.GetSilverStars();

        bf.Serialize(file, data);
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

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("In pause method: pause is " + pause);
        if(pause)
        {
            SaveScores();
        }
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
class GameData
{
    public string username;
    public int[] scores;
    public int goldstars;
    public int silverstars;
}


[Serializable]
class Scores
{
    public int[] scores;
}