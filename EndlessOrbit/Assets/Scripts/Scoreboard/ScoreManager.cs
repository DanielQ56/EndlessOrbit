﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    [SerializeField] Leaderboard leader;
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
        DeletePath();
        LoadScores();
    }

    #region global

    private const string AddNormalScore = "https://endless-orbit.herokuapp.com/add_normal_score";
    private const string GetNormalScores = "https://endless-orbit.herokuapp.com/normalscores";
    private const string AddUnstableScore = "https://endless-orbit.herokuapp.com/add_unstable_score";
    private const string GetUnstableScores = "https://endless-orbit.herokuapp.com/unstablescores";


    GlobalScores gs;

    bool retrieving = false;



    void SaveScoreToGlobal(bool isUnstable)
    {
        if (username.Length > 0)
        {
            StartCoroutine(RetrieveGlobal(false, isUnstable));
        }
    }

    void OnGlobalLeaderboard(bool isUnstable)
    {
        int recent = (isUnstable ? recentUnstableScore : recentNormalScore);
        Players lastScore = gs.result[9];
        if ( recent > lastScore.score || (recent == lastScore.score  && String.Compare(username, lastScore.username) < 0))
        {
            Debug.Log("Saving score of " + recent + " to user " + username);
            StartCoroutine(SaveGlobal(isUnstable, recent));
        }
        else
        {
            retrieving = false;
        }
    }

    IEnumerator SaveGlobal(bool isUnstable, int recent)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("score", recent);
        using(UnityWebRequest newScore = UnityWebRequest.Post(isUnstable ? AddUnstableScore : AddNormalScore, form))
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
    
    public void DisplayGlobalScores(bool isUnstable)
    {
        StartCoroutine(RetrieveGlobal(true, isUnstable));
    }

    IEnumerator RetrieveGlobal(bool display, bool isUnstable)
    {
        while (retrieving)
            yield return null;
        using (UnityWebRequest scores = UnityWebRequest.Get(isUnstable ? GetUnstableScores : GetNormalScores))
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
                    leader.ActivateGlobalBoard(gs.result, isUnstable ? recentUnstableScore : recentNormalScore, PlayerPrefs.GetString("Username"));
                    retrieving = false;
                }
                else
                    OnGlobalLeaderboard(isUnstable);

            }
           
        }
    }



    #endregion

    #region Username/Input
    string username;

    public void DisablePopup(GameObject g)
    {
        bool value = g.GetComponent<Toggle>().isOn ;
        Debug.Log("Popup value: " + value);
        popupDisabled = value;
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
    [SerializeField] GameObject Popup;

    bool popupDisabled = false;

    bool isOnGlobal = false;

    bool isSaving = false;

    int recentNormalScore = 0;
    int recentUnstableScore = 0;

    int[] normalScores = new int[10];
    int[] unstableScores = new int[10];

    List<int> tempScores = new List<int>();

    public void ChangeBoards(int value)
    {
        if(value == 0)
        {
            isOnGlobal = false;
        }
        else
        {
            isOnGlobal = true;
        }
    }

    public void ChangeLeaderboards(int value, bool isUnstable = false)
    {
        isOnGlobal = value == 0 ? false : true;
        UpdateToggles(isUnstable);
    }

    public void UpdateToggles(bool isUnstable)
    {
        leader.DisplayLeaderboard(isUnstable);
    }

    public void DisplayScores(bool isUnstable)
    {
        if(isOnGlobal)
        {
            DisplayGlobalScores(isUnstable);
        }
        else
        {
            Debug.Log("Loading Local Scores: " + isUnstable);
            DisplayLocalScores(isUnstable);
        }
    }

    void LoadScores()
    {
        if(File.Exists(Application.persistentDataPath + "/EndlessOrbitScores.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/EndlessOrbitScores.dat", FileMode.Open);

            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            normalScores = data.normalScores;
            unstableScores = data.unstableScores;
            username = (data.username == null ? "" : data.username);
            popupDisabled = data.popupDisabled;
            PlayerManager.instance.Setup(data.silverstars);
            PlayerManager.instance.SetupItems(data.itemsBought, data.selectedItem);
            recentNormalScore = normalScores[normalScores.Length - 1];
            recentUnstableScore = unstableScores[unstableScores.Length - 1];
        }
        else
        {
            username = "";
            PlayerManager.instance.Setup();
            PlayerManager.instance.SetupItems();
        }
        if(username.Length == 0 && !popupDisabled)
        {
            Popup.SetActive(true);
        }
    }

    public void RecordScore(int score, bool isUnstable)
    {
        if (isUnstable)
            recentUnstableScore = score;
        else
            recentNormalScore = score;

        SaveScoreToGlobal(isUnstable);

        if (score <= (isUnstable ? unstableScores[0] : normalScores[0]))
        {
            return;
        }

 
        if(isUnstable)
        {
            unstableScores[0] = score;
            Array.Sort(unstableScores);
        }
        else
        {
            normalScores[0] = score;
            Array.Sort(normalScores);
        }
    }

    public void SaveScores()
    {
        isSaving = true;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/EndlessOrbitScores.dat", FileMode.Create);

        GameData data = new GameData();
        data.normalScores = normalScores;
        data.unstableScores = unstableScores;
        data.username = username;
        data.silverstars = PlayerManager.instance.GetSilverStars();
        data.popupDisabled = popupDisabled;
        List<PurchasableItem> items = PlayerManager.instance.getAllItems();
        List<bool> bought = new List<bool>();
        for(int i = 0; i < items.Count; ++i)
        {
            Debug.Log("Index: " + i + ", Bought: " + items[i].bought + ", Selected: " + items[i].selected);
            bought.Add(items[i].bought);
        }
        data.selectedItem = PlayerManager.instance.GetSelectedIndex();

        data.itemsBought = bought.ToArray();


        bf.Serialize(file, data);
        file.Close();
        isSaving = false;
    }

    public void DisplayLocalScores(bool isUnstable)
    {
        tempScores.Clear();
        tempScores.AddRange(isUnstable ? unstableScores : normalScores);
        tempScores.Reverse();
        leader.ActivateLeaderboard(tempScores, isUnstable ? recentUnstableScore : recentNormalScore);
    }

    public int GetHighScore(bool isUnstable)
    {
        return (isUnstable ? unstableScores[unstableScores.Length - 1] : normalScores[normalScores.Length - 1]);
    }

    public void DeleteAllData()
    {
        PlayerManager.instance.SetToDefault();
        recentNormalScore = 0;
        recentUnstableScore = 0;
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            if(!isSaving)
                SaveScores();
        }
    }

    private void OnApplicationQuit()
    {
        if(!isSaving)
            SaveScores();
    }

    void DeletePath()
    {
        string path = Application.persistentDataPath + "/scores.dat";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        path = Application.persistentDataPath + "/EndlessOrbitScores.dat";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    #endregion
}
#region Custom Classes
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
    public int[] normalScores;
    public int[] unstableScores;
    public bool[] itemsBought;
    public int silverstars;
    public int selectedItem;
    public bool popupDisabled;
}


[Serializable]
class Scores
{
    public int[] scores;
}
#endregion