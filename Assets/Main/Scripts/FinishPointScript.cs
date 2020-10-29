using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishPointScript : MonoBehaviour
{
    public int finishMultliplierCount = 5;
    float screenWidth;
    CameraFollow cameraFollow;
    public float finishMultliplierDepth = 1;
    float unitWidth;
    private Vector3 initialPosition;
    public List<Color> colors;
    public bool setRandomPosition;
    private GameManager gameManager;

    [Header("Multiplier Text")]
    public GameObject canvasPrefab;
    public float canvasHeight = 4;
    private List<int> xValues = new List<int>() { 10,5,3};
    private List<int> xOrder = new List<int>();

    private float finishPointWidth;
    private void Awake()
    {
        initialPosition = transform.position;
        cameraFollow = FindObjectOfType<CameraFollow>();
        gameManager = FindObjectOfType<GameManager>();
        //Camera calculations for screen width are carried in awake
    }
    float lastAxisLeftRadius;
    void Start()
    {
        if (gameManager.currentLevel > 20)
        {
            finishMultliplierCount = 3;
        }
        finishPointWidth = cameraFollow.sideWallX * 2;
        screenWidth = cameraFollow.screenWidth;
        //unitWidth = screenWidth / finishMultliplierCount;
        unitWidth = transform.localScale.x / finishMultliplierCount;

        if (setRandomPosition)
        {
            //Transform currentLevelTransform = gameManager.levelParent.GetChild(gameManager.currentLevel);
            //Transform axes = currentLevelTransform.Find("Axes");
            //int lastAxisIndex = axes.childCount - 1;
            //float lastAxisZ = axes.GetChild(lastAxisIndex).position.z;
            //transform.position = Vector3.forward * (lastAxisZ +Random.Range(10,17));

        }
        SetFinishBox();
        transform.localScale = new Vector3(finishPointWidth, transform.localScale.y, transform.localScale.z);
    }
    private void SetFinishBox()
    {
        Transform cubeTransform;
        Material cubeMat;
        Vector3 canvasPosition;
        GameObject canvasObj;
        RectTransform canvasRect;
        float screenLeftX = -transform.localScale.x/2;
        Vector3 cubePosition = new Vector3(screenLeftX+ unitWidth/2, initialPosition.y, initialPosition.z);
        SetMultiplierOrder(finishMultliplierCount);
        for (int i = 0; i < finishMultliplierCount; i++)
        {
            cubeTransform = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            cubeTransform.name = "Cube " + i.ToString();
            cubeTransform.gameObject.tag = "FinishPoint";
            BoxCollider bc = cubeTransform.GetComponent<BoxCollider>();
            bc.size = new Vector3(1,2,5);
            bc.center = new Vector3(0,0,(5-cubeTransform.localScale.z)/2);
            cubeTransform.transform.localScale = new Vector3(unitWidth, gameManager.boxDepth, transform.localScale.z/3);
            cubeTransform.GetComponent<BoxCollider>().isTrigger = true;
            cubeTransform.position = cubePosition;
            cubePosition += Vector3.right*unitWidth;

            int index = xOrder.IndexOf(i);
            Color cubeMatColor = new Color();
            cubeMatColor = colors[index];
            cubeMatColor.a = 0.5f;
            cubeMat = new Material(Shader.Find("Standard"));
            cubeMat.color = cubeMatColor;
            MeshRenderer mr = cubeTransform.GetComponent<MeshRenderer>();
            mr.material = cubeMat;
            //Color color = cubeMat.color;
            //color.a = 0.5f;
            //cubeMat.color = color;


            //canvasPosition = cubeTransform.position + new Vector3(0,gameManager.boxDepth/2+1,finishMultliplierDepth/2+canvasHeight/2);
            canvasPosition = cubeTransform.position + new Vector3(0,0, -(finishMultliplierDepth/2*1.1f));
            canvasObj = Instantiate(canvasPrefab,canvasPosition, Quaternion.identity);
            canvasObj.transform.localEulerAngles = Vector3.right * 0;
            canvasObj.transform.localScale *= 1.5f;

            Canvas canvas = canvasObj.GetComponent<Canvas>();//
            canvas.worldCamera = Camera.main;

            canvasObj.GetComponentInChildren<Text>().text = xValues[index] + "x";
            MultiplierData mData =  cubeTransform.gameObject.AddComponent<MultiplierData>();
            mData.multiplier = xValues[index];
            canvasRect = canvasObj.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(1, canvasHeight);
            canvasObj.transform.SetParent(cubeTransform);
            cubeTransform.SetParent(transform);
        }
    }

    private void SetMultiplierOrder(int count)
    {
        Transform lastAxis = gameManager.axisList[gameManager.axisList.Count - 1];
        lastAxisLeftRadius = lastAxis.position.x - lastAxis.GetComponent<AxisScript>().rotateRadius;
        xOrder.Clear();
        if(count == 1)
        {
            xOrder.Add(0);

        }
        else if (count == 2)
        {
            if(lastAxisLeftRadius < 0)
            {
                xOrder.Add(0);
                xOrder.Add(1);
            }
            else
            {
                xOrder.Add(1);
                xOrder.Add(0);
            }
        }
        else if(count == 3)
        {
            //actual value is -2.76
            float limitValue = 5.2f;
            if (lastAxisLeftRadius < -limitValue)
            {//1,2,3
                xOrder.Add(0);
                xOrder.Add(1);
                xOrder.Add(2);
            }
            else if (lastAxisLeftRadius >= -limitValue && lastAxisLeftRadius < 0)
            {//2,1,3
                xOrder.Add(1);
                xOrder.Add(0);
                xOrder.Add(2);
            }
            else if (lastAxisLeftRadius >= 0 && lastAxisLeftRadius < limitValue)
            {//2,3,1
                xOrder.Add(1);
                xOrder.Add(2);
                xOrder.Add(0);
            }
            else if (lastAxisLeftRadius >= limitValue)
            {//3,2,1
                xOrder.Add(2);
                xOrder.Add(1);
                xOrder.Add(0);
            }
        }
    }
}
