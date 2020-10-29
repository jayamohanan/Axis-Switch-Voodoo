using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [HideInInspector] public int score;
    public Text scoreText;
    public Text totalPointsText;
    
    private int highScore;
    private int totalPoints;
    private GameManager gameManager;
    [Header("Slider")]
    public GameObject scoreSlider;
    public Image greenImage;

    private float totalDistance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            highScore = PlayerPrefs.GetInt("HighScore");
            totalPoints = PlayerPrefs.GetInt("TotalPoints");
            totalPointsText.text = totalPoints.ToString();
            gameManager = FindObjectOfType<GameManager>();
        }
        else
            Destroy(gameObject);
    }
    public void OnLevelLoad()
    {
        totalDistance = gameManager.axisList[0].position.z - gameManager.currentLevelTransform.Find("FinishPoint").position.z;
        currentSliderValue = 0;
        greenImage.fillAmount = 0;
        score = 1;
        scoreText.text = score.ToString();
    }
    public void IncrementScore(int a)
    {
        score += a;
        scoreText.text = score.ToString();
    }
    private float currentSliderValue = 0;
    public void SetDistanceSlider(float percent)//[0,1]
    {
        if (percent > currentSliderValue)
        {
            currentSliderValue = percent;
            greenImage.fillAmount = percent;
        }
    }
    public void SetScore(int a)
    {
        score = a;
        scoreText.text = score.ToString();
        AudioManager.Instance.PlayTapSound();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public bool IsHighScore()
    {   
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            return true;
        }
        return false;
    }
    public void Addpoints(float points)
    {
        totalPoints += (int)points;
        totalPointsText.text = totalPoints.ToString();
        PlayerPrefs.SetInt("TotalPoints", totalPoints);
    }
    private void OnApplicationQuit()
    {
       
    }
}
