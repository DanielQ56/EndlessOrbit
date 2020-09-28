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
    bool RewardedContinue = false;
    bool RewardVideoLoading = false;

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
            MobileAds.Initialize(initStatus => {
            });
            RequestConfiguration childConfig = new RequestConfiguration.Builder().SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True).build();
            MobileAds.SetRequestConfiguration(childConfig);
            RequestConfiguration contentConfig = new RequestConfiguration.Builder().SetMaxAdContentRating(MaxAdContentRating.G).build();
            MobileAds.SetRequestConfiguration(contentConfig);
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
        showAds = b;
        if(!showAds)
        {
            HideBanner();
        }
    }

    //Found on Stackoverflow, solution by Maximillian Laumeister
    IEnumerator CheckInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if(www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    #region Reward Ad
    void SetupRewardedAds()
    {
        if (rewardedAd == null)
        {
            RewardVideoLoading = true;
            rewardedAd = new RewardedAd(rewardedAdID);

            rewardedAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
            rewardedAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
            rewardedAd.OnUserEarnedReward += HandleRewardBasedVideoRewarded;
            rewardedAd.OnAdClosed += HandleRewardBasedVideoClosed;
            AdRequest request = new AdRequest.Builder().Build();
            rewardedAd.LoadAd(request);
        }
        else
        {
            StartCoroutine(CheckInternetConnection((isConnected) =>
            {
                if (isConnected && !rewardedAd.IsLoaded())
                {
                    RewardVideoLoading = true;
                    rewardedAd = new RewardedAd(rewardedAdID);

                    rewardedAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
                    rewardedAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
                    rewardedAd.OnUserEarnedReward += HandleRewardBasedVideoRewarded;
                    rewardedAd.OnAdClosed += HandleRewardBasedVideoClosed;
                    AdRequest request = new AdRequest.Builder().Build();
                    rewardedAd.LoadAd(request);
                }
            }));
        }
    }


    public void ShowRewardedAd()
    {
        ScoreManager.instance.Loading(true);


        StartCoroutine(CheckInternetConnection((isConnected) => {

            if (isConnected && rewardedAd != null && rewardedAd.IsLoaded())
            {
                Debug.Log("Showing Ad");
                ScoreManager.instance.Loading(false);
                rewardedAd.Show();
                rewardedAd = null;
                SetupRewardedAds();
            }
            else
            {
                ScoreManager.instance.Loading(false);
                if (MainGameManager.instance != null)
                {
                    MainGameManager.instance.UnableToLoadVideo();
                }
                rewardedAd = null;
                SetupRewardedAds();
            }
        }));
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("Successfully Loaded Ad");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log("Failed to load reward ad video " + args.Message);
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
        if(showAds || (!showAds && bannerView != null))
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
            Debug.Log("Creating new fullScreenAd");
            fullScreenAd = new InterstitialAd(fullScreenAdID);
            fullScreenAd.OnAdClosed += OnFullScreenAdClosed;
            AdRequest request = new AdRequest.Builder().Build();
            fullScreenAd.LoadAd(request);
        }
        else
        {
            StartCoroutine(CheckInternetConnection((isConnected) =>
            {
                if(isConnected && !fullScreenAd.IsLoaded())
                {
                    fullScreenAd = new InterstitialAd(fullScreenAdID);
                    fullScreenAd.OnAdClosed += OnFullScreenAdClosed;
                    AdRequest request = new AdRequest.Builder().Build();
                    fullScreenAd.LoadAd(request);
                }
            }));
        }


    }

    public void ShowFullScreenAd()
    {

        if(fullScreenAd != null && fullScreenAd.IsLoaded())
        {
            fullScreenAd.Show();
            fullScreenAd = null;
        }
        else
        {
            MainGameManager.instance.ShowGameOverPanel();
            fullScreenAd = null;
        }
    }

    void OnFullScreenAdClosed(object sender, EventArgs args)
    {
        MainGameManager.instance.ShowGameOverPanel();

        RequestFullScreenAd();
    }
    #endregion

}
