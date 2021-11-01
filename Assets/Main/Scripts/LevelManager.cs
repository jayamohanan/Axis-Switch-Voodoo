//using AppsFlyerSDK;
//using FlurrySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class LevelManager : MonoBehaviour
{
    public bool clearAllData;
    public bool loadSpecificLevel;
    public int level;

    [Header("Camera BG Colors")]
    public List<ColorScheme> colorSchemes;
    public int colorSchemeChangeInterval = 5;
    int colorSchemeIndex = 0;
    private MoveTransform moveTransform;

    public List<GameObject> levelsPrefabList;
    private int firstTimeOnlyLevelCount = 4;
    private int levelsCount;
    [HideInInspector] public Transform currentLevelTransform;
    [HideInInspector] public int currentLevel;
    private GameManager gameManager;
    private Camera mainCam;
    [HideInInspector] public Color currentBGColor;
    private void Awake()
    {
#if UNITY_EDITOR
        if (clearAllData)
            PlayerPrefs.DeleteAll();
#endif
        gameManager = FindObjectOfType<GameManager>();
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        if (currentLevel == 0)
        {
            //currentLevel = 1;
        }
        levelsCount = levelsPrefabList.Count;
        mainCam = Camera.main;
        moveTransform = FindObjectOfType<MoveTransform>();

        colorSchemeIndex = PlayerPrefs.GetInt("ColorSchemeIndex");
        //mainCam.backgroundColor = colorSchemes[colorSchemeIndex].cameraBGColor;
        moveTransform.lineMat.color = colorSchemes[colorSchemeIndex].lineColor;
        currentBGColor = colorSchemes[colorSchemeIndex].cameraBGColor;

    }
    int index;
    public void SpawnLevel(bool nextLevel = true)
    {
#if (UNITY_EDITOR)
        if (loadSpecificLevel)
            currentLevel = level;
#endif
        if (nextLevel)
        {
            currentLevel++;
            index = currentLevel - 1;
            if ((currentLevel - 1) != 0 && (currentLevel - 1) % colorSchemeChangeInterval == 0)
            {
                colorSchemeIndex++;
                if (colorSchemeIndex == colorSchemes.Count)
                    colorSchemeIndex = 0;
                //mainCam.backgroundColor = colorSchemes[colorSchemeIndex].cameraBGColor;
                //transform.GetChild(0).Find("BottomPlane").GetComponent<MeshRenderer>().material.color = colorSchemes[colorSchemeIndex].cameraBGColor;
                currentBGColor = colorSchemes[colorSchemeIndex].cameraBGColor;
                moveTransform.lineMat.color = colorSchemes[colorSchemeIndex].lineColor;
            }
        }
        //#if UNITY_IPHONE
        //        Dictionary<string, string> eventValues = new Dictionary<string, string>();
        //        eventValues.Add("Level", currentLevel.ToString());
        //        AppsFlyer.sendEvent(AFInAppEvents.LEVEL_STARTED, eventValues);
        //        Flurry.LogEvent("LEVEL_STARTED", eventValues, false);
        //#endif
        if (currentLevelTransform != null)
        {
            currentLevelTransform.gameObject.SetActive(false);
            Destroy(currentLevelTransform.gameObject);
        }

        GameObject levelPrefab;
        if (index >= levelsCount)
            index = (index - firstTimeOnlyLevelCount) % (levelsCount - firstTimeOnlyLevelCount) + firstTimeOnlyLevelCount;
            //index is currentLevel -1
        levelPrefab = levelsPrefabList[index];
        currentLevelTransform = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity, transform).transform;
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, currentLevel.ToString());
        if(PluginStatus.Instance!=null)
        if (PluginStatus.Instance.tinySauce)
            TinySauce.OnGameStarted(currentLevel.ToString());
        if (nextLevel)
        {
        }

    }
    private void OnApplicationPause(bool pause)
    {
#if (!UNITY_EDITOR)
        if (gameManager.gameWon)
        {
            PlayerPrefs.SetInt("CurrentLevel", (currentLevel));
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", (currentLevel - 1));
        }
        if(!gameManager.gameWon || !gameManager.gameOver)
        {
        if(PluginStatus.Instance!=null)
        if(PluginStatus.Instance.tinySauce)
            TinySauce.OnGameFinished(currentLevel.ToString(), false, ScoreManager.Instance.score);
        }
        PlayerPrefs.SetInt("ColorSchemeIndex", colorSchemeIndex);
#endif
    }
    private void OnApplicationQuit()
    {
#if (UNITY_EDITOR)
        if (gameManager.gameWon)
        {
            PlayerPrefs.SetInt("CurrentLevel", (currentLevel));
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", (currentLevel - 1));

        }
        if(!gameManager.gameWon || !gameManager.gameOver)
        {
            if (PluginStatus.Instance != null)
                if (PluginStatus.Instance.tinySauce)
                TinySauce.OnGameFinished(currentLevel.ToString(), false, ScoreManager.Instance.score);
        }
        PlayerPrefs.SetInt("ColorSchemeIndex", colorSchemeIndex);
#endif
    }

}
[System.Serializable]
public class ColorScheme
{
    public Color cameraBGColor;
    public Color lineColor;
}

