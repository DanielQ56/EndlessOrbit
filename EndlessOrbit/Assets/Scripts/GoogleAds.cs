using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class GoogleAds : MonoBehaviour
{
    public static GoogleAds instance = null;

    string appID = "ca-app-pub-8915439303360774~3249557989";

    private BannerView bannerView = null;
    private string bannerID = "ca-app-pub-3940256099942544/6300978111";

    private InterstitialAd fullScreenAd = null;
    private string fullScreenAdID = "ca-app-pub-3940256099942544/1033173712";

    private RewardedAd rewardedAd = null;
    private string rewardedAdID = "ca-app-pub-3940256099942544/5224354917";

    bool showAds = true;
    bool GotRewardsFromVideo = false;
    bool RewardLoaded = false;
    bool RewardedContinue = false;

    bool SceneLoaded = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Debug.Log("DESTROYING COPY OF GAME ADS OBJECT");
            Destroy(this.gameObject);
        }


    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(!SceneLoaded)
        {
            SceneLoaded = true;
            MobileAds.Initialize(initStatus => { });
            RequestFullScreenAd();
            SetupRewardedAds();

            if(scene.buildIndex == 0)
            {
                RequestBanner();
            }

            SceneLoaded = false;
        }
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
        if (rewardedAd == null)
        {
            Debug.Log("Setuping up rewarded ads");
            RewardLoaded = false;
            rewardedAd = new RewardedAd(rewardedAdID);

            rewardedAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
            rewardedAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
            rewardedAd.OnUserEarnedReward += HandleRewardBasedVideoRewarded;
            rewardedAd.OnAdClosed += HandleRewardBasedVideoClosed;
            AdRequest request = new AdRequest.Builder().Build();
            rewardedAd.LoadAd(request);
        }

    }


    public void ShowRewardedAd()
    {
        StartCoroutine(WaitForRewardedAd());
    }

    IEnumerator WaitForRewardedAd()
    {
        ScoreManager.instance.Loading(true);
        while(!RewardLoaded)
        {
            yield return null;
        }
        ScoreManager.instance.Loading(false);

        if (rewardedAd.IsLoaded())
        {
            Debug.Log("Showing Ad");
            rewardedAd.Show();
            rewardedAd = null;
        }
        else
        {
            if (MainGameManager.instance != null)
            {
                MainGameManager.instance.UnableToLoadVideo();
            }
        }
        RewardLoaded = false;
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("Successfully Loaded Ad");
        RewardLoaded = true;
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log("Failed to load reward ad video " + args.Message);
        RewardLoaded = true;
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        if (MainGameManager.instance != null)
        {
            Debug.Log("Rewarding with a continue!");
            GotRewardsFromVideo = true;
            RewardedContinue = true;
            MainGameManager.instance.RewardedContinue();
        }
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        StartCoroutine(WaitForReward());
    }

    IEnumerator WaitForReward()
    {
        ScoreManager.instance.Loading(true);
        for(int i = 0; i < 10; ++i)
        {
            yield return new WaitForSeconds(0.1f);
            if(RewardedContinue)
            {
                RewardedContinue = false;
                break;
            }
        }
        ScoreManager.instance.Loading(false);

        if (!GotRewardsFromVideo)
        {
            MainGameManager.instance.FinalGameOver();
        }
        GotRewardsFromVideo = false;
        SetupRewardedAds();
        Debug.Log("Done waiting for rewards");
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
        if (fullScreenAd == null)
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
            fullScreenAd = null;
        }
        else
        {
            MainGameManager.instance.ShowGameOverPanel();
        }
    }

    void OnFullScreenAdClosed(object sender, EventArgs args)
    {
        MainGameManager.instance.ShowGameOverPanel();

        RequestFullScreenAd();
    }
    #endregion

}
