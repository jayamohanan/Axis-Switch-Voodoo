using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginStatus : MonoBehaviour
{
    public bool adMob;
    public bool tinySauce;
    public bool gameAnalytics;
    public bool appsflyer;
    public bool flurry;
    public static PluginStatus Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
