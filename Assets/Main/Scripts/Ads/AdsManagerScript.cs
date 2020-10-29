using System;
using UnityEngine;
using GoogleMobileAds.Api;
public class AdsManagerScript : MonoBehaviour
{
    public static AdsManagerScript Instance;

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    //Original banner ID  ca-app-pub-9904614353552965/3452716196
    //Google Test banner ad ID	ca-app-pub-3940256099942544/6300978111
    public string androidBannerAdUnityID;

    //Original Interstitial ID  ca-app-pub-9904614353552965/9812743438
    //Google Test Interstitial ad ID  ca-app-pub-3940256099942544/1033173712
    public string androidInterstitialAdUnityID;

    //Original Interstitial video ID  not done
    //Google Test Interstitial video ad ID  ca-app-pub-3940256099942544/8691691433
    public string androidInterstitialVideoAdUnityID;

    //Original Rewarded ID  ca-app-pub-9904614353552965/1243471359
    //Google Test Rewarded ad ID  ca-app-pub-3940256099942544/5224354917
    public string androidRewardAdUnityID;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        
        // Initialize the Google Mobile Ads SDK.
        if (PluginStatus.Instance.adMob)
        {
            MobileAds.Initialize(initStatus => { });
            RequestBanner();//reques banner now, banner will be shown only after level 5
            RequestInterstitial();
            RequestRewarded();
        }
    }
    public void LoadBannerAd()
    {
        if (!PluginStatus.Instance.adMob)
        {
            print("Ads Disabled");
            return;
        }
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }
    public void ShowInterstitialAd()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            print("interstitial ad not loaded");
        }
    }
    public void ShowRewardedAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            print("Rewarded ad not loaded");
        }
    }
    private void RequestBanner()
    {
        string adUnitId;
#if UNITY_ANDROID
        {
            //string adUnitId = "ca-app-pub-3940256099942544/6300978111";
            adUnitId = androidBannerAdUnityID;
        }
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        #region AdCallbacks
        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;
        #endregion
    }
    private void RequestInterstitial()
    {
        if (!PluginStatus.Instance.adMob)
        {
            print("Ads Disabled");
            return;
        }
        string adUnitId;
#if UNITY_ANDROID
        {
            //string adUnitId = "";
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                adUnitId = androidInterstitialAdUnityID;
            }
            else
            {
                adUnitId = androidInterstitialVideoAdUnityID;
            }
        }
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif
        interstitial = new InterstitialAd(adUnitId);
        #region AdCallbacks
        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded += HandleOnInterstitialAdLoaded;
        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HandleOnInterstitialAdFailedToLoad;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnInterstitialAdOpened;
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnInterstitialAdClosed;
        // Called when the ad click caused the user to leave the application.
        interstitial.OnAdLeavingApplication += HandleOnInterstitialAdLeavingApplication;
        #endregion
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }
    
    private void RequestRewarded()
    {
        if (!PluginStatus.Instance.adMob)
        {
            print("Ads Disabled");
            return;
        }
        string adUnitId;
#if UNITY_ANDROID
        {
            //string adUnitId = "";
            adUnitId = androidRewardAdUnityID;
        }
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif
        rewardedAd = new RewardedAd(adUnitId);
        #region AdCallbacks
        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        #endregion
        
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }
    #region AllAdCallbackFunctions
    #region Banner AdCallbackFunctions
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    #endregion
    #region Interstitial Ad Callbacks
    public void HandleOnInterstitialAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnInterstitialAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        RequestInterstitial();
    }

    public void HandleOnInterstitialAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    #endregion
    #region Rewarded ad Callbacks
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        RequestRewarded();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.LoadNextLevel();
    }
    #endregion
    #endregion
}