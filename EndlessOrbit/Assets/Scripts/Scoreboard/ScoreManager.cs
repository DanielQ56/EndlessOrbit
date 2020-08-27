using System.Collections;
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
    [SerializeField] InfoPanel InfoPanel;


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

    public void StartProcess()
    {
        LoadScores();
    }

    #region global
    /* Old global leaderboard
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
    */

    public void Loading(bool loading)
    {
        LoadingPanel.SetActive(loading);
    }

    public void ProvideInfo(string info)
    {
        InfoPanel.gameObject.SetActive(true);
        InfoPanel.SetText(info);
    }

    public void DisplayGlobalLeaderboard()
    {
        GlobalLeaderboard.instance.DisplayLeaderboard();
    }

    #endregion


    #region local

    bool isSaving = false;

    int recentNormalScore = 0;
    int recentUnstableScore = 0;

    int[] normalScores;
    int[] unstableScores;

    List<int> tempScores = new List<int>();

    public void ChangeLeaderboards(bool isUnstable = false)
    {
        UpdateToggles(isUnstable);
    }

    public void UpdateToggles(bool isUnstable)
    {
        leader.DisplayLeaderboard(isUnstable);
    }

    public void DisplayScores(bool isUnstable)
    {
        DisplayLocalScores(isUnstable);
        
    }

    void LoadScores()
    {
        if(File.Exists(Application.persistentDataPath + "/EndlessOrbitScores.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/EndlessOrbitScores.dat", FileMode.Open);

            GameData data = (GameData)bf.Deserialize(file);
            file.Close();
            normalScores = (data.normalScores != null && data.normalScores.Length > 0) ? data.normalScores : new int[10];
            unstableScores = (data.unstableScores != null && data.unstableScores.Length > 0) ? data.unstableScores : new int[10];
            ShouldDisplay = data.ShouldDisplayHTP;
            PlayerManager.instance.SetNextBonusTime(System.DateTime.Parse(data.nextBonus));
            PlayerManager.instance.Setup(data.silverstars);
            PlayerManager.instance.SetupItems(data.itemsBought, data.selectedItem);
            recentNormalScore = normalScores[normalScores.Length - 1];
            recentUnstableScore = unstableScores[unstableScores.Length - 1];
            AudioManager.instance.SetStartingVolume(data.MusicVolume, data.EffectsVolume);
            GoogleAds.instance.SetAdBool(data.ShowAds);
        }
        else
        {
            PlayerManager.instance.SetNextBonusTime(default(System.DateTime));
            PlayerManager.instance.Setup();
            PlayerManager.instance.SetupItems();
            normalScores = new int[10];
            unstableScores = new int[10];
            AudioManager.instance.SetStartingVolume();
        }
        if(ShouldDisplay)
        {
            OpenHowToPlay();
        }

        GoogleAds.instance.SetupAds();

    }

    public void RecordScore(int score, bool isUnstable)
    {
        if (isUnstable)
            recentUnstableScore = score;
        else
            recentNormalScore = score;

        GlobalLeaderboard.instance.PostScoreToLeaderboard(score, isUnstable);

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
        data.silverstars = PlayerManager.instance.GetSilverStars();
        data.ShouldDisplayHTP = ShouldDisplay;
        data.ShowAds = GoogleAds.instance.ShouldShowAds();
        data.nextBonus = PlayerManager.instance.GetNextBonus().ToString();
        List<PurchasableItem> items = PlayerManager.instance.getAllItems();
        List<bool> bought = new List<bool>();
        for(int i = 0; i < items.Count; ++i)
        {
            //Debug.Log("Index: " + i + ", Bought: " + items[i].bought + ", Selected: " + items[i].selected);
            bought.Add(items[i].bought);
        }
        data.selectedItem = PlayerManager.instance.GetSelectedIndex();

        data.itemsBought = bought.ToArray();
        data.MusicVolume = AudioManager.instance.GetMusicVolume();
        data.EffectsVolume = AudioManager.instance.GetEffectsVolume();


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

    public void DeletePath()
    {
        /*string path = Application.persistentDataPath + "/scores.dat";
        if (File.Exists(path))
        {
            File.Delete(path);
        }*/
        string path = Application.persistentDataPath + "/EndlessOrbitScores.dat";
        if (File.Exists(path))
        {
            Debug.Log("DELETING PATH");
            File.Delete(path);
        }
        LoadScores();
    }
    #endregion

    #region How To Play

    [SerializeField] GameObject HowToPlay;

    bool ShouldDisplay = true;

    public void OpenHowToPlay()
    {
        HowToPlay.SetActive(true);
    }

    public void DoNotShowAgain(bool b)
    {
        ShouldDisplay = !b;
        Debug.Log(ShouldDisplay);
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
    public int[] normalScores;
    public int[] unstableScores;
    public bool[] itemsBought;
    public int silverstars;
    public int selectedItem;
    public bool ShouldDisplayHTP;
    public string nextBonus;
    public float MusicVolume;
    public float EffectsVolume;
    public bool ShowAds;
}


[Serializable]
class Scores
{
    public int[] scores;
}
#endregion