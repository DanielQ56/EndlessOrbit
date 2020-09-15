using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class GoogleAds : MonoBehaviour
{
    public static GoogleAds instance;

    string appID = "ca-app-pub-8915439303360774~3249557989";

    private BannerView bannerView = null;
    private string bannerID = "ca-app-pub-3940256099942544/6300978111";

    private InterstitialAd fullScreenAd = null;
    private string fullScreenAdID = "ca-app-pub-3940256099942544/1033173712";

    private RewardBasedVideoAd rewardedAd = null;
    private string rewardedAdID = "ca-app-pub-3940256099942544/5224354917";

    bool showAds = true;
    bool GotRewardsFromVideo = false;
    bool LoadedProperly = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded");
        MobileAds.Initialize(appID);
        RequestFullScreenAd();
        SetupRewardedAds();
        RequestRewardedAd();
    }

    public bool ShouldShowAds()
    {
        return showAds;
    }

    public void SetAdBool(bool b)
    {
        showAds = true;
    }

    #region Reward Ad
    void SetupRewardedAds()
    {
        if(rewardedAd == null)
        {
            rewardedAd = RewardBasedVideoAd.Instance;

            rewardedAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
            rewardedAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
            rewardedAd.OnAdRewarded += HandleRewardBasedVideoRewarded;
            rewardedAd.OnAdClosed += HandleRewardBasedVideoClosed;
        }

    }

    private void OnDisable()
    {
        if (rewardedAd != null)
        {
            rewardedAd.OnAdLoaded -= HandleRewardBasedVideoLoaded;
            rewardedAd.OnAdFailedToLoad -= HandleRewardBasedVideoFailedToLoad;
            rewardedAd.OnAdRewarded -= HandleRewardBasedVideoRewarded;
            rewardedAd.OnAdClosed -= HandleRewardBasedVideoClosed;
        }
    }

    public void RequestRewardedAd()
    {
        if (!rewardedAd.IsLoaded())
        {
            Debug.Log("Requesting Rewarded Ad");
            AdRequest request = new AdRequest.Builder().Build();
            rewardedAd.LoadAd(request, rewardedAdID);
        }
    }

    public void ShowRewardedAd()
    {
        if(rewardedAd.IsLoaded())
        {
            Debug.Log("Showing Ad");
            rewardedAd.Show();
        }
        else
        {
            if (MainGameManager.instance != null)
            {
                MainGameManager.instance.UnableToLoadVideo();
            }
        }
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("Successfully Loaded Ad");
        LoadedProperly = true;
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Failed to load reward ad video " + args.Message);
        LoadedProperly = false;
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        if (LoadedProperly)
        {
            if (MainGameManager.instance != null)
            {
                Debug.Log("Rewarding with a continue!");
                GotRewardsFromVideo = true;
                MainGameManager.instance.RewardedContinue();
            }
        }
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("Ad is closed!");
        if (!GotRewardsFromVideo)
            MainGameManager.instance.FinalGameOver();
        GotRewardsFromVideo = false;
        RequestRewardedAd();
    }

    #endregion

    #region Banner
    public void RequestBanner()
    {
        if (showAds)
        {
            bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

            AdRequest request = new AdRequest.Builder().Build();

            bannerView.LoadAd(request);
            bannerView.Show();
        }
    }

    public void HideBanner()
    {
        if(showAds)
        {
            bannerView.Hide();
        }
    }

    #endregion

    #region Fullscreen
    public void RequestFullScreenAd()
    {

        if (fullScreenAd == null || !fullScreenAd.IsLoaded())
        {
            fullScreenAd = new InterstitialAd(fullScreenAdID);
            fullScreenAd.OnAdClosed += OnFullScreenAdClosed;
            AdRequest request = new AdRequest.Builder().Build();

            fullScreenAd.LoadAd(request);
        }


    }

    public void ShowFullScreenAd()
    {

        if(fullScreenAd.IsLoaded())
        {
            fullScreenAd.Show();
            RequestFullScreenAd();
        }
        else
        {
            MainGameManager.instance.ShowGameOverPanel();
            RequestFullScreenAd();
        }
    }

    void OnFullScreenAdClosed(object sender, EventArgs args)
    {
        MainGameManager.instance.ShowGameOverPanel();
    }
    #endregion

}
