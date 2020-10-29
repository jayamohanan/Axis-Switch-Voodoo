using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
public class AxisScript : MonoBehaviour
{
    public bool biDirectional;
    public bool antiClockWise;
    public float rotateRadius = 1;
    public float fieldRadius = 1;
    public int coinCount;
    public GameObject coinPrefab;

    public Transform fieldCylinder;
    public Transform rotationAreaCylinder;
    public SphereCollider sphereCollider;
    [Header("Arm")]
    public Transform arm;
    public bool showArm;//
    [Range(0, 360)]
    public int armAngle = 45;
    public bool rotateArm;
    public float armSpeed = 5;

    private CameraFollow cameraFollow;
    private float xPosLimit;
    private float screenWidth;
    float leftX;
    float rightX;
    private float margin;
    private Transform coinParent;
    private GameManager gameManager;
    [Range(0, 1)]
    public float cylinderHeight = 0.3f;
    private float sideWallX;
    public float obstacleMargin = 0.5f;
    private float obstacleNearEndLeft;
    private float obstacleNearEndRight;
    [HideInInspector] public bool counted;
    private float armLength;
    private Vector3 rotateCylinderScale;
    private Vector3 fieldCylinderScale;
    void Awake()//
    {
        gameManager = FindObjectOfType<GameManager>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        Camera cam = Camera.main;
        leftX = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, cam.transform.position.y)).x;
        rightX = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, cam.transform.position.y)).x;
        screenWidth = rightX - leftX;
        margin = screenWidth * 0.1f;
    }
    private void OnEnable()
    {
        //gameManager.LevelLoadEvent += OnLevelLoad;
    }
    private void OnDisable()
    {
        //gameManager.LevelLoadEvent -= OnLevelLoad;
    }
    void Start()//
    {
        //sideWallX = cameraFollow.sideWallX;
    }
    Vector3 coinPosition;
    float initialCoinAngle;
    Transform coin;

    GameObject leftWing;
    GameObject rightWing;
    private void SetWings()
    {

        if(leftWing == null)//if a wing doesn't exists already
        {
            obstacleNearEndLeft = transform.position.x - rotateRadius - obstacleMargin;
            obstacleNearEndRight = transform.position.x + rotateRadius + obstacleMargin;

            leftWing = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftWing.name = "LeftWing";
            leftWing.transform.localScale = new Vector3(obstacleNearEndLeft - (sideWallX), 1, 0.5f);
            leftWing.transform.position = new Vector3((obstacleNearEndLeft + sideWallX) / 2, transform.position.y, transform.position.z);

            rightWing = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightWing.name = "RightWing";
            rightWing.transform.localScale = new Vector3(sideWallX - obstacleNearEndRight, 1, 0.5f);
            rightWing.transform.position = new Vector3((sideWallX + obstacleNearEndRight) / 2, transform.position.y, transform.position.z);
        }
    }
    public void OnLevelLoad()
    {
        if(coinParent!=null)
            Destroy(coinParent.gameObject);
        coinParent = new GameObject("CoinParent").transform;
        coinParent.SetParent(transform);
    }
    private void SetCoins()
    {
        initialCoinAngle = 360.0f / coinCount;
        coinCount = Random.Range(0,6);
        float currenetCountAngle ;
        coinPosition = transform.position + new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f, 1f)).normalized * rotateRadius;
        for (int i = 0; i < coinCount; i++)
        {
            currenetCountAngle = i* initialCoinAngle;
            coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity, coinParent).transform;
            coin.RotateAround(transform.position, Vector3.up, currenetCountAngle);
        }
    }
    private void Update()
    {
        if(Application.IsPlaying(gameObject))
        if (showArm)
        {
            if (rotateArm)
            {
                arm.RotateAround(transform.position, Vector3.up, armSpeed*Time.deltaTime);
            }
        }
    }
    //float xLimit;
    //float xPosition;
    private void OnValidate()///
    {
        if(rotationAreaCylinder != null)
        {
            rotateCylinderScale = new Vector3(2 * rotateRadius, (cylinderHeight - 0.05f), 2 * rotateRadius);
            rotationAreaCylinder.localScale = rotateCylinderScale;
        }
        if (fieldCylinder != null)
        {
            fieldCylinderScale = new Vector3(2 * fieldRadius, cylinderHeight, 2 * fieldRadius);
            fieldCylinder.transform.localScale = fieldCylinderScale;
        }
        if (arm != null)
        {
            arm.gameObject.SetActive(showArm);
            armLength = rotateRadius /*- fieldRadius*/ - 0.5f;
            arm.transform.localScale = new Vector3(0.3f, 0.3f, armLength);
            arm.transform.position = transform.position + new Vector3(0, 0, /*fieldRadius+*/0.5f + armLength / 2);
            arm.transform.LookAt(transform);
            arm.transform.RotateAround(transform.position, Vector3.up, armAngle);
        }
        sphereCollider.radius = fieldRadius;//
    }
}
