using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject explosionPrefab;
    private GameObject ball;
    public GameObject basicLevelObject;
    public GameObject firstLevelObject;
    public GameObject secondAxisLevelObject;

    public GameObject boundaryCubeObject;//Big boundary trigger
    public GameObject startLevelsPanelObject;//Big boundary trigger

    public Text instructionText;
    BallTutorial ballTutorial;

    //private bool readyStep1;
    //private bool readyStep2;
    ////private bool readyStep3;

    //private bool executedStep1Action;
    //private bool executedStep2Action;
    //private bool executedStep3Action;

    private bool activatedStep1Ball;
    private bool activatedStep2Ball;
    private bool activatedStep3Ball;

    private bool startedStep1;
    private bool startedStep2;
    private bool startedStep3;

    private bool printed;
    //private bool printedStep2;
    //private bool printedStep3;
    private bool doneStep1;
    private bool doneStep2;
    private bool doneStep3;

    void Awake()
    {
        boundaryCubeObject.SetActive(true);
        basicLevelObject.SetActive(true);

        firstLevelObject.SetActive(false);
        startLevelsPanelObject.SetActive(false);

        HideText();
    }
    private void Start()
    {
        InstantiateBall();
    }
    void InstantiateBall()
    {
        ball = Instantiate(ballPrefab);
        ballTutorial = ball.GetComponent<BallTutorial>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!startedStep1 && Time.time > 1f)
        {
            startedStep1 = true;
            ShowText("Tap anywhere on screen to release the ball from axis.");
        }
        //if(startedStep1)
        //{
        //    ballTutorial.throwActive = true;

        //}
        if(startedStep1 && !activatedStep1Ball && !doneStep1  && printed)
        {
            activatedStep1Ball = true;
            printed = false;
            ballTutorial.throwActive = true;
        }
        if(startedStep2 && !activatedStep2Ball && !doneStep2 &&  printed)
        {
            activatedStep2Ball = true;
            printed = false;
            ballTutorial.throwActive = true;
        }
        if(startedStep3 && !activatedStep3Ball && !doneStep3 &&  printed)
        {
            activatedStep2Ball = true;
            printed = false;
            ballTutorial.throwActive = true;
        }
        if (((activatedStep1Ball && !doneStep1) || (activatedStep2Ball && !doneStep2) || (activatedStep3Ball && !doneStep3)) && Input.touchCount > 0)
        {
#if (!UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS))
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                ///executedStep1Action = true;
                HideText();
            }
#elif (UNITY_EDITOR || UNITY_WEBGL)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //executedStep1Action = true;
                HideText();
            }
#endif
        }
    }
    private void Step2()
    {
        doneStep1 = true;
        startedStep2 = true;
        boundaryCubeObject.SetActive(false);
        basicLevelObject.SetActive(false);
        firstLevelObject.SetActive(true);//make level active before ball instantiation

        InstantiateBall();
        //ShowText("Good, Look at the position of the ball and tap to release in the required direction");
        ShowText("Good, now Tap at the correct moment\nso as to pass the ball through\nfinish(green) line.");
    }
    private void Step3()
    {
        doneStep2 = true;
        firstLevelObject.SetActive(false);
        secondAxisLevelObject.SetActive(true);
        InstantiateBall();
        startedStep3 = true;
        instructionText.text = "";
        ShowText("Perfect! Final step,  Jump to second\naxis and then to the finish(green) line.");
    }

    private void ShowText(string text)
    {
        //instructionText.text = text;
        instructionText.gameObject.SetActive(true);
        instructionText.text = "";
        StartCoroutine("PrintText", text);
    }
    private void HideText()
    {
        instructionText.gameObject.SetActive(false);
    }

    public void OutOfTrigger()
    {
        if (startedStep1 && !doneStep1)
        {
            doneStep1 = true;
            Invoke("Step2", 1.5f);
        }
    }
    float t1;
    float t2;
    IEnumerator PrintText(string txt)
    {
        for (int i = 0; i < txt.Length; i++)
        {
            instructionText.text = instructionText.text + txt[i];
            yield return new WaitForSeconds(0.03f);
        }
        printed = true;
    }
    public void OnFinishLineTouched()
    {
        if (!doneStep2)
        {
            doneStep2 = true;
            Destroy(ball, 1);
            Invoke("Step3", 1);
        }
        else if (!doneStep3)
        {   
            doneStep3 = true;
            PlayerPrefs.SetInt("TutorialCompleted", 100);
            ShowText("Well Done!   That's all.");
            Invoke("ShowStartLevelsPanel", 4);
        }
    }
    private void ShowStartLevelsPanel()
    {
        startLevelsPanelObject.SetActive(true);
    }

    //Strat button
    public void StartLevelScene()
    {
        SceneManager.LoadScene(1);
    }
    public void InstantiateBallAgain()
    {
        Invoke("InstantiateDelay", 1f);
    }
    private void InstantiateDelay()
    {
        instructionText.gameObject.SetActive(true);
        instructionText.text = "Hit obstacle!  Try again";
        InstantiateBall();
        ballTutorial.throwActive = true;
    }
}
