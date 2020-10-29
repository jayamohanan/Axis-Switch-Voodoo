using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("TutorialCompleted") == 0)
        {
            StartCoroutine("LoadNextScene", 2);//Tutorial
        }
        else
        {
            StartCoroutine("LoadNextScene", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator LoadNextScene(int buildIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
