using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System;

//Uses video by Turbo Makes Games on Youtube as guideline

public class GlobalLeaderboard : MonoBehaviour
{
    public static GlobalLeaderboard instance = null;

    bool isSignedIn = false;
    


    const string GlobalNormalID = "CgkIg7v6uKIbEAIQAA";
    const string GlobalUnstableID = "CgkIg7v6uKIbEAIQAQ";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        AuthenticateUser();
    }

    public void AuthenticateUser()
    {
        ScoreManager.instance.Loading(true);

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (result) =>
         {
             Debug.Log("THE RESULT OF SIGNING IN: " + result.ToString());

             if(result == SignInStatus.Success)
             {
                 ScoreManager.instance.ProvideInfo("Successfully signed in!");
                 isSignedIn = true;
             }
             else if(result == SignInStatus.Canceled)
             {
                 ScoreManager.instance.ProvideInfo("Log in cancelled.");
             }
             else
             {
                 ScoreManager.instance.ProvideInfo("Unable to login.");
             }

             ScoreManager.instance.Loading(false);
         });

    }

    public void SignOut()
    {
        if(isSignedIn)
        {
            PlayGamesPlatform.Instance.SignOut();
            isSignedIn = false;
        }
    }

    public bool IsPlayerSignedIn()
    {
        return isSignedIn;
    }

    public void PostScoreToLeaderboard(int score, bool isUnstable)
    {
        if (isSignedIn)
        {
            if (isUnstable)
            {
                Social.ReportScore((long)score, GlobalUnstableID, (bool success) =>
                {
                    if (success)
                    {
                        Debug.Log("Successfully reported");
                    }
                });
            }
            else
            {
                Social.ReportScore((long)score, GlobalNormalID, (bool success) =>
                {
                    if (success)
                    {
                        Debug.Log("Successfully reported");
                    }
                });

            }
        }
    }

    public void DisplayLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }
}
