using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public bool useCustomObstacle = true;
    public bool goToLevel;
    public int levelToGo;
    public Transform player;
    public bool clearAllData;
    [HideInInspector] public Transform pathSphereParent;
    [HideInInspector] public bool gameOver;
    [HideInInspector] public bool gameWon;
    CameraFollow cameraFollow;
    public Toggle shrinkToggle;
    [HideInInspector]public bool shrink;//rotation axis reduces and reaches centre of axis
    public InputField speedIF;
    [HideInInspector] public float speed = 300;
    public Sprite bottomSprite;
    public List<Color> cameraBGColors;
    public int boxDepth;
    private int attempts;
    private MoveTransform moveTransform;
    public Material obstacleMat;
    public Material bottomPlaneMat;
    [Header("Gameover")]
    public GameObject gameoverPanel;
    public Image levelStatusPanelImage;
    public Text levelStatusText;
    public Button nextButton;
    public Button retryButton;
    public Button skipButton;
    public Text scoreValueText;
    public GameObject highScoreTextObj;
    public Text mutiText;
    public Text coinCountText;
    public int coinCount;
    public Text attemptText;
    public Text origScoreText;
    public Text multiplierText;
    public Text finalScoreText;
    [HideInInspector] public int scoreMultiplier;
    public Text multiplierTextInGame;
    [HideInInspector] public Color multiplierColor;

    [HideInInspector] public Vector2 inGameMultiplierFinalPosition;
    public Transform levelParent;
    [HideInInspector] public Transform currentLevelTransform;
    [HideInInspector] public List<Transform> axisList = new List<Transform>();
    private int levelStrength;
    public Text levelText;
    public event System.Action LevelLoadEvent;
    [Header("Colors")]
    public Color winColor;
    public Color failColor;
    public Color attemptColor;
    public Color firstAttemptColor;
    private bool loadedBannerAd;
    public int currentLevel;
    private LevelManager levelManager;

    void Awake()
    {
#if (UNITY_EDITOR)
        if (clearAllData)
            PlayerPrefs.DeleteAll();
#endif
        levelManager = FindObjectOfType<LevelManager>();
        moveTransform = FindObjectOfType<MoveTransform>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        inGameMultiplierFinalPosition = multiplierTextInGame.transform.position; ;
         
    }
    private void Start()
    {
        LoadNextLevel();
        SetScreenPositions();

    }
    Vector2 topPoint;
    Vector2 bottomPoint;
    Vector2 rightPoint;
    Vector2 leftPoint;

    Vector2 origScorePos;
    Vector2 multiplierPos;
    Vector2 finalScorePos;
    Vector2 nextButtonPos;
    private void SetScreenPositions()
    {
        origScorePos = origScoreText.transform.position;
        multiplierPos = multiplierText.transform.position;
        finalScorePos = finalScoreText.transform.position;
        nextButtonPos = nextButton.transform.position;

        Camera cam = Camera.main;
        float z = moveTransform.transform.position.z;
        topPoint = cam.ViewportToScreenPoint(new Vector3(0.5f, 1.2f,z));
        bottomPoint = cam.ViewportToScreenPoint(new Vector3(0.5f, -0.2f,z));
        rightPoint = cam.ViewportToScreenPoint(new Vector3(1.2f, 0.5f,z));
        leftPoint = cam.ViewportToScreenPoint(new Vector3(-0.2f, 0.5f,z));
    }
    [HideInInspector]public Vector2 multiplierInitialPosition;
    [HideInInspector]public Transform multiplierTransform;
    public IEnumerator GameOverDelay(bool win)//
    {
        if (gameOver)
            yield break;
        gameWon = win;
        if (win)
        {
            Vector2 initialPosScreen = Camera.main.WorldToScreenPoint(multiplierTransform.position);

            multiplierTextInGame.transform.position = new Vector3(initialPosScreen.x, initialPosScreen.y, 0);
            multiplierTextInGame.gameObject.SetActive(true);
            multiplierTextInGame.color = multiplierColor;

            LeanTween.moveX(multiplierTextInGame.gameObject, inGameMultiplierFinalPosition.x, /*0.2f*/2f).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveY(multiplierTextInGame.gameObject, inGameMultiplierFinalPosition.y, /*0.2f*/2f).setEase(LeanTweenType.easeOutCubic);

            multiplierTextInGame.text = "x" + moveTransform.levelMultiplier.ToString();
        }
        cameraFollow.stopFollow = true;
        gameOver = true;
        yield return new WaitForSecondsRealtime(1);
        OnGameOver(win);
    }
    private void OnGameOver(bool win)
    {
        SetGameOverPanel(win);
    }
    private void SetGameOverPanel(bool win)
    {
        //attemptText.gameObject.SetActive(false);
        origScoreText.gameObject.SetActive(false);
        multiplierText.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(false);
        multiplierTextInGame.gameObject.SetActive(false);

        levelStatusText.text = "LEVEL COMPLETED";
        nextButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        int coinCount = moveTransform.strength;

        attempts++;
        if (win)
        {
            //if (coinCount > 0)
            //{
                
            //    coinCountText.gameObject.SetActive(true);
            //    mutiText.gameObject.SetActive(true);
            //    coinCountText.text = coinCount.ToString();
            //    mutiText.text = moveTransform.levelMultiplier.ToString();
            //    mutiText.color = multiplierColor;
            //}
            if (attempts == 1)
            {
                attemptText.color = firstAttemptColor;
                attemptText.text = "FIRST ATTEMPT!";
            }
            else
            {
                attemptText.color = attemptColor;
                attemptText.text = "ATTEMPTS "+ attempts.ToString();
            }
            StartCoroutine("ScoreDisplayCoroutine");

            levelStatusPanelImage.color = winColor;
            levelStatusText.text = "LEVEL "+ levelManager.currentLevel + " COMPLETED";
            GameAnalytics.NewDesignEvent("Attempts:"+currentLevel.ToString(), attempts);
            attempts = 0;
            if(levelManager.currentLevel>5 && levelManager.currentLevel % 3 == 0)
            {
                print("Invoking interstitial ads");
                Invoke("CallInterstitilaAd",1);
            }
        }
        else
        {
            attemptText.color = attemptColor;
            attemptText.text = "ATTEMPT " + attempts.ToString();
            if (attempts > 1)
                attemptText.text = "ATTEMPTS " + attempts.ToString();
            levelStatusPanelImage.color = failColor;
            coinCountText.gameObject.SetActive(false);
            mutiText.gameObject.SetActive(false);
            levelStatusText.text = "LEVEL " + levelManager.currentLevel +" FAILED";
            retryButton.gameObject.SetActive(true);
            if (attempts > 2)
                skipButton.gameObject.SetActive(true);
        }
        gameoverPanel.SetActive(true);
        moveTransform.DestroyExplosionParticles();
    }
    private void CallInterstitilaAd()
    {
        print("Calling int ad script");
        if (PluginStatus.Instance.adMob)
            AdsManagerScript.Instance.ShowInterstitialAd();
    }
    private IEnumerator ScoreDisplayCoroutine()
    {
        nextButton.gameObject.SetActive(false);

        float t = /*0.08f*/1f;
        origScoreText.rectTransform.anchoredPosition = new Vector2(origScoreText.rectTransform.anchoredPosition.x, topPoint.x);
        multiplierText.rectTransform.anchoredPosition = new Vector2(leftPoint.x, multiplierText.rectTransform.anchoredPosition.y);
        finalScoreText.rectTransform.anchoredPosition = new Vector2(finalScoreText.rectTransform.anchoredPosition.x, bottomPoint.y);

        yield return new WaitForSecondsRealtime(0.1f);
        float score = ScoreManager.Instance.score;
        origScoreText.text = score.ToString();
        origScoreText.gameObject.SetActive(true);
        LeanTween.moveY(origScoreText.gameObject, origScorePos.y, t).setEase(LeanTweenType.easeOutBack);

        yield return new WaitForSecondsRealtime(0.25f);
        float multiplier = moveTransform.levelMultiplier;
        multiplierText.text = "x"+moveTransform.levelMultiplier.ToString();
        multiplierText.color = multiplierColor;
        multiplierText.gameObject.SetActive(true);
        LeanTween.moveX(multiplierText.gameObject, multiplierPos.x, t).setEase(LeanTweenType.easeOutBack);

        yield return new WaitForSecondsRealtime(0.25f);
        float finalScore = (score * multiplier);
        finalScoreText.text = (score * multiplier).ToString();
        ScoreManager.Instance.Addpoints(finalScore);
        finalScoreText.gameObject.SetActive(true);
        LeanTween.moveY(finalScoreText.gameObject, finalScorePos.y, t).setEase(LeanTweenType.easeOutBack).setOnComplete(() =>
        {
            nextButton.gameObject.SetActive(true);
            //Uncomment bottom for tweening
            //nextButton.transform.position = bottomPoint;
            //LeanTween.moveY(nextButton.gameObject, nextButtonPos.y,/* 0.05f*/0.5f).setEase(LeanTweenType.easeOutBack);
        });
    }

    public void JumpToLevel()//
    {
        if (Tweak.Instance.ifList[1].text != null)
        {
            levelManager.currentLevel = int.Parse(Tweak.Instance.ifList[1].text);
            levelManager.SpawnLevel(false);
            LoadLevel();
        }
    }

    public void LoadNextLevel()//Next button
    {
        levelManager.SpawnLevel();//Looper Next level
        if(!loadedBannerAd && levelManager.currentLevel > 2)
        {
            loadedBannerAd = true;
            if(PluginStatus.Instance!=null)
            if (PluginStatus.Instance.adMob)
                AdsManagerScript.Instance.LoadBannerAd();
        }
        LoadLevel();
    }
    private void LoadLevel()
    {
        pathSphereParent = new GameObject("PathSphereParent").transform;
        currentLevelTransform = levelManager.currentLevelTransform;
        pathSphereParent.SetParent(currentLevelTransform);

        Time.timeScale = 1;
        ScoreManager.Instance.score = 1;
        levelText.text = "LEVEL " + (levelManager.currentLevel).ToString();
        gameOver = false;
        levelStrength = 0;
        gameoverPanel.SetActive(false);
        cameraFollow.OnLevelLoad();
        player.gameObject.SetActive(true);
        axisList.Clear();
        Transform axes = currentLevelTransform.Find("Axes");

        foreach (Transform t in axes)
        {
            axisList.Add(t);
            AxisScript axisScipt = t.GetComponent<AxisScript>();
            axisScipt.counted = false;
            levelStrength += axisScipt.coinCount;
        }
        if(LevelLoadEvent!=null)
        LevelLoadEvent();
        moveTransform.OnNextLevel();

        ScoreManager.Instance.OnLevelLoad();
        cameraFollow.CreateBoundary();
    }
    public void ReloadLevel()//Retry Button
    {
        gameoverPanel.SetActive(false);
        levelManager.SpawnLevel(false);//Dont increment level
        LoadLevel();
    }
    public void CallRewardVideoAd()//Skip Button
    {
        if(PluginStatus.Instance.adMob)
        AdsManagerScript.Instance.ShowRewardedAd();
    }
}
