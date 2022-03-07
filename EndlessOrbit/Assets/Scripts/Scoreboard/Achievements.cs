using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System;

public class Achievements : MonoBehaviour
{
    #region Achievement IDs
    //Game Based Achievements
    string[] gameAchievements = 
        {"CgkIg7v6uKIbEAIQBA", "CgkIg7v6uKIbEAIQBQ",
         "CgkIg7v6uKIbEAIQBg", "CgkIg7v6uKIbEAIQBw" };

    //Orbit Based Achievements
    string[] orbitAchievements =
        {"CgkIg7v6uKIbEAIQCQ", "CgkIg7v6uKIbEAIQCg",
         "CgkIg7v6uKIbEAIQCw", "CgkIg7v6uKIbEAIQDA"};

    //Collected Star Based Achievements
    string[] collectAchievements =
        {"CgkIg7v6uKIbEAIQDQ", "CgkIg7v6uKIbEAIQDg",
         "CgkIg7v6uKIbEAIQDw", "CgkIg7v6uKIbEAIQEA"};

    //Spent Star Based Achievements
    string[] spentAchievements =
        {"CgkIg7v6uKIbEAIQEQ", "CgkIg7v6uKIbEAIQEg",
         "CgkIg7v6uKIbEAIQEw"};

    //Unlock Based Achievements
    string[] unlockAchievements =
        {"CgkIg7v6uKIbEAIQFA", "CgkIg7v6uKIbEAIQFg",
         "CgkIg7v6uKIbEAIQFw"};
    #endregion

    public static Achievements instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void UnlockAchievement(string id)
    {
        Social.ReportProgress(id, 100.0f, (bool success) => { });
    }

    private void IncrementAchievement(string id, int increment)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(id, increment, (bool success) => { });
    }

    public void DisplayAchievements()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    #region Game Achievements
    //Game Based Achievements
    public void IncrementGames()
    {
        UnlockAchievement(gameAchievements[0]);
        for(int i = 1; i < gameAchievements.Length; ++i)
        {
            IncrementAchievement(gameAchievements[i], 1);
        }
    }

    public void IncrementOrbits(int orbits)
    {
        for (int i = 0; i < orbitAchievements.Length; ++i)
        {
            IncrementAchievement(orbitAchievements[i], orbits);
        }
    }

    public void IncrementCollectedStars(int stars)
    {
        for (int i = 0; i < collectAchievements.Length; ++i)
        {
            IncrementAchievement(collectAchievements[i], stars);
        }
    }

    public void IncrementSpentStars(int stars)
    {
        for (int i = 0; i < spentAchievements.Length; ++i)
        {
            IncrementAchievement(spentAchievements[i], stars);
        }
    }

    //Unlock Based Achievements
    //Rating
    public void Rate()
    {
        UnlockAchievement(unlockAchievements[0]);
    }

    //Spin
    public void Spin()
    {
        UnlockAchievement(unlockAchievements[1]);
    }

    //Continue
    public void Continue()
    {
        UnlockAchievement(unlockAchievements[2]);
    }
    #endregion
}
