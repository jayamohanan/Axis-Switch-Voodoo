using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tweak : MonoBehaviour
{
    public event System.Action valueCange;
    public GameObject tweekPanel;

    //InputFields

    [Header("Input Fields")]
    public InputField[] ifList= new InputField[4];

    [Header("Texts")]
    public Text[] ifTextList = new Text[4];

    [HideInInspector] public float[] ifValueList = new float[4];

    //Toggles

    [Header("Toggles")]
    public Toggle[] tList = new Toggle[4];
    //[HideInInspector] public bool[] tValueList = new bool[4];

    public MonoBehaviour refScript;
    private float parsedValue;
    public static Tweak Instance;

    void Awake()
    {
        if (Instance == null)
        {
            tweekPanel.SetActive(false);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    public void ToggleVisibility()
    {
        tweekPanel.SetActive(!tweekPanel.activeSelf);
    }
    public void OnValueChanged()
    {
        if (float.TryParse(ifList[0].text, out parsedValue))
        {
            if(parsedValue!=0)
            ifTextList[0].text = parsedValue.ToString();
        }
        if (float.TryParse(ifList[1].text, out parsedValue))
        {
            if(parsedValue!=0)
            ifTextList[1].text = parsedValue.ToString();
        }
        if (float.TryParse(ifList[2].text, out parsedValue))
        {
            if(parsedValue!=0)
            ifTextList[2].text = parsedValue.ToString();
        }
        if (float.TryParse(ifList[3].text, out parsedValue))
        {
            if(parsedValue!=0)
            ifTextList[3].text = parsedValue.ToString();
        }
        valueCange();
    }

    public void SetIF(int index, float value)
    {
        ifList[index - 1].text = value.ToString();
        ifValueList[index - 1] = value;
        ifTextList[index - 1].text = value.ToString();
    }
    public void SetToggle(int index, bool value)
    {
        tList[index-1].isOn = value;
    }
}
