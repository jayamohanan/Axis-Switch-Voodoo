using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelLooper : MonoBehaviour
{
    public bool loadLevel;
    public int level;
    private Queue<int> lastFive = new Queue<int>();
    private List<int> freeLevels = new List<int>();
    public Transform levelParent;
    public List<GameObject> levelPrefabList;
    [HideInInspector]public Transform levelTransform;
    private int currentLevelInProgress;//from 1 to infinity
    private int currentLevelIndex;//from 1 to 20
    private GameManager gameManager;

    private int unfinishedLevelIndex;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        currentLevelInProgress = PlayerPrefs.GetInt("currentLevelInProgress");
        unfinishedLevelIndex = PlayerPrefs.GetInt("UnfinishedLevelIndex");
        if (currentLevelInProgress == 0)
            currentLevelInProgress = 1;
        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            //span 5 leevls
            for (int i = 0; i < 5; i++)
            {
                lastFive.Enqueue(i + 1);
            }
            for (int i = 5; i < levelPrefabList.Count; i++)
            {
                freeLevels.Add(i + 1);
            }
        }
    }
    GameObject nextLevelObject;
    public void LoadNextRandomLevel()
    {
        if(freeLevels.Count == 0)
        {
            for (int i = 5; i < levelPrefabList.Count; i++)
            {
                freeLevels.Add(i + 1);
            }
        }
        int levelPrefabIndex;
        if (unfinishedLevelIndex == 0)
        {
            levelPrefabIndex = Random.Range(0, freeLevels.Count);
            //levelPrefabIndex = level - 1;
            currentLevelIndex = freeLevels[levelPrefabIndex];
            //SwapCollectionValues(levelPrefabIndex);
            int a = freeLevels[levelPrefabIndex];
            //nextLevelObject = Instantiate(levelPrefabList[a], Vector3.zero, Quaternion.identity, levelParent);
            LoadLevel(a);
        }
        else
        {
            levelPrefabIndex = unfinishedLevelIndex;
            //nextLevelObject = Instantiate(levelPrefabList[levelPrefabIndex], Vector3.zero, Quaternion.identity, levelParent);
            LoadLevel(levelPrefabIndex);
        }
    }
    public void ReloadLevel()
    {
        LoadLevel(currentLevelIndex);
    }
    private void LoadLevel(int level)
    {
        if (nextLevelObject != null)
            Destroy(nextLevelObject);
        nextLevelObject = Instantiate(levelPrefabList[level], Vector3.zero, Quaternion.identity, levelParent);
        levelTransform = nextLevelObject.transform;

        gameManager.pathSphereParent = new GameObject("PathSphereParent").transform;
        gameManager.pathSphereParent.SetParent(levelTransform);
        gameManager.levelText.text = "LEVEL " + (currentLevelInProgress).ToString();
    }
    private void OnApplicationPause(bool pause)
    {
        if (gameManager.gameWon)
        {
            currentLevelInProgress++;
            PlayerPrefs.SetInt("currentLevelInProgress", currentLevelInProgress);
            PlayerPrefs.SetInt("UnfinishedLevelIndex", 0);//No need to coontinue
        }
        else if (!gameManager.gameWon)
        {
            PlayerPrefs.SetInt("UnfinishedLevelIndex", currentLevelIndex);
        }
    }
    //private void SwapCollectionValues(int value)
    //{
    //    freeLevels.Remove(value);
    //    if (lastFive.Count == 5)
    //    {
    //        int removedIndex = lastFive.Dequeue();
    //        freeLevels.Add(removedIndex);
    //    }
    //    lastFive.Enqueue(value);
    //}
}
