using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 offset;
    public Transform playerTransform;
    private MoveTransform moveTransform;
    public bool stopFollow;
    /*[HideInInspector] */public bool cameraAtStart;
    public Vector3 targetPosition;
    public Transform sideWalls;
    private Transform leftSideWall;
    private Transform rightSideWall;
    Camera cam;
    [HideInInspector] public float screenWidth;
    public float sideWallWidthVisible;
    private GameManager gameManager;
    [Tooltip("Distaance from first axis to bottomWall")]
    public float bottomWallDistance = 3;
    public float sideWallX =12;
    private LevelManager levelManager;
    void Awake()//
    {
        gameManager = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();
        obstacleMat = gameManager.obstacleMat;
        cam = GetComponent<Camera>();
        leftSideWall = sideWalls.GetChild(0);
        rightSideWall = sideWalls.GetChild(1);
        offset = playerTransform.position - transform.position;
        moveTransform = playerTransform.GetComponent<MoveTransform>();
        targetPosition = transform.position;
        SetSideWalls();
    }
    public void OnLevelLoad()
    {
        stopFollow = false;
        cameraAtStart = false;
    }
    public float screenLeftX;
    public float screenRightX;
    private void SetSideWalls()
    {
        float wallWidth = leftSideWall.localScale.x;
        screenLeftX = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, cam.transform.position.y)).x;
        screenRightX = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, cam.transform.position.y)).x;

        screenWidth = screenRightX - screenLeftX;
        leftSideWall.position = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, cam.transform.position.y))-Vector3.right*(wallWidth/2- sideWallWidthVisible);
        rightSideWall.position = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, cam.transform.position.y))+ Vector3.right * (wallWidth / 2 - sideWallWidthVisible);
        Material wallMat = rightSideWall.GetComponent<MeshRenderer>().material;
        float bottomY = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, cam.transform.position.y)).z;
    }
    Transform levelTransform;
    private Material obstacleMat;
    private float wallColliderWidth = 8;
    public void CreateBoundary()//
    {
        int depth= gameManager.boxDepth;
        levelTransform = gameManager.currentLevelTransform;
        Transform axes = levelTransform.Find("Axes");
        float bottomZ = Mathf.Infinity;
        for (int i = 0; i < axes.childCount; i++)
        {
            Transform childAxes = axes.GetChild(i);
            if (childAxes.gameObject.activeSelf)
            {
                float childLowestZ = childAxes.transform.position.z - childAxes.GetComponent<AxisScript>().rotateRadius;
                if (childLowestZ < bottomZ)
                {
                    bottomZ = childLowestZ;
                }
            }
        }
        BoxCollider collider;
        bottomZ -= bottomWallDistance;
        float topZ = levelTransform.Find("FinishPoint").transform.position.z;
        float sideWallLength = Mathf.Abs(topZ - bottomZ);
        float sideWallZ = (topZ + bottomZ) / 2;
        //Left
        GameObject leftB = GameObject.CreatePrimitive(PrimitiveType.Cube);
        collider = leftB.GetComponent<BoxCollider>();
        collider.size = new Vector3(wallColliderWidth, 2, 1);
        collider.center = new Vector3(-(wallColliderWidth- leftB.transform.localScale.x)/2, 0, 0);

        leftB.transform.localScale = new Vector3(0.3f, depth, sideWallLength);
        leftB.transform.position = new Vector3(-sideWallX, 0,sideWallZ);
        //Right
        GameObject rightB = GameObject.CreatePrimitive(PrimitiveType.Cube);
        collider = rightB.GetComponent<BoxCollider>();
        collider.size = new Vector3(wallColliderWidth, 2, 1);
        collider.center = new Vector3((wallColliderWidth - leftB.transform.localScale.x)/2, 0, 0);
        rightB.transform.localScale = new Vector3(0.3f, depth, sideWallLength);
        rightB.transform.position = new Vector3(sideWallX, 0, sideWallZ);
        
        //Bottom
        GameObject bottomB = GameObject.CreatePrimitive(PrimitiveType.Cube);
        collider = bottomB.GetComponent<BoxCollider>();
        collider.size = new Vector3(1, 2, wallColliderWidth);
        collider.center = new Vector3(0, 0, -(wallColliderWidth - leftB.transform.localScale.x)/2);

        bottomB.transform.localScale = new Vector3(2*sideWallX, depth, 0.3f);
        bottomB.transform.position = new Vector3(0, 0, bottomZ);

        //Parenting
        leftB.GetComponent<MeshRenderer>().material = obstacleMat;
        rightB.GetComponent<MeshRenderer>().material = obstacleMat;
        bottomB.GetComponent<MeshRenderer>().material = obstacleMat;

        //Bottom Plane
        //GameObject bottomPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //bottomPlane.name = "BottomPlane";
        //print("jaya1");
        //if(bottomPlane.GetComponent<MeshCollider>()!=null)
        //bottomPlane.GetComponent<MeshCollider>().enabled = false;
        //else
        //    print("Mesh collider not found");
        //print("jaya2");

        //gameManager.bottomPlaneMat.color = levelManager.currentBGColor;
        //print("jaya6");

        //bottomPlane.GetComponent<MeshRenderer>().material = gameManager.bottomPlaneMat;
        //print("jaya5");

        //bottomPlane.GetComponent<MeshRenderer>().receiveShadows = false;
        //print("jaya3");

        //bottomPlane.transform.position = new Vector3(0, -0.5f, sideWallZ);
        //bottomPlane.transform.localScale = new Vector3(2 * sideWallX, 0.01f, sideWallLength);
        //bottomPlane.transform.localScale /= 10;
        
        GameObject bottomSprite = new GameObject("BottomSprite");
        print("jaya1");
        SpriteRenderer sr = bottomSprite.AddComponent<SpriteRenderer>();
        sr.sprite = gameManager.bottomSprite;
        sr.color = levelManager.currentBGColor;
        sr.sortingLayerName = "Background";
        bottomSprite.transform.position = new Vector3(0, -depth/2, sideWallZ);
        bottomSprite.transform.localScale = new Vector3(2 * sideWallX, sideWallLength, 1f);
        bottomSprite.transform.localEulerAngles = new Vector3(90,0,0);

        //gameManager.bottomPlaneMat.color =
        //print("jaya6");

        //bottomPlane.GetComponent<MeshRenderer>().material = gameManager.bottomPlaneMat;
        //print("jaya5");

        //bottomPlane.GetComponent<MeshRenderer>().receiveShadows = false;
        //print("jaya3");

        //bottomPlane.transform.position = new Vector3(0, -0.5f, sideWallZ);
        //bottomPlane.transform.localScale = new Vector3(2 * sideWallX, 0.01f, sideWallLength);
        //bottomPlane.transform.localScale /= 10;

        leftB.transform.SetParent(levelTransform);
        rightB.transform.SetParent(levelTransform);
        bottomB.transform.SetParent(levelTransform);
        bottomSprite.transform.SetParent(levelTransform);
    }
    private Vector3 speed = Vector3.zero;
    public float zOffset = 8;
    void LateUpdate()//
    {
        if (!stopFollow)
        {
            if (moveTransform.move)
            {
                targetPosition = new Vector3(transform.position.x, transform.position.y, playerTransform.position.z + zOffset);
            }
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref speed, 0.5f);
            if(!cameraAtStart && Vector3.Distance(transform.position, targetPosition) < 2f)
            {
                cameraAtStart = true;
            }
        }
    }
    public void OnAxisChange()
    {
        Transform axes = gameManager.currentLevelTransform.Find("Axes");
        int currentAxisSiblingIndex = moveTransform.currentAxis.GetSiblingIndex();
        if((currentAxisSiblingIndex+1) < axes.childCount)
        {
            if(axes.GetChild(currentAxisSiblingIndex + 1).position.z < moveTransform.currentAxis.transform.position.z)
            {
                targetPosition = (moveTransform.currentAxis.transform.position + axes.GetChild(currentAxisSiblingIndex + 1).position) / 2;
            }
            else{
                targetPosition = moveTransform.currentAxis.transform.position;
            }
        }
        else
        {
            targetPosition = moveTransform.currentAxis.transform.position;
        }
        targetPosition = new Vector3(transform.position.x, transform.position.y, targetPosition.z+ zOffset);
    }
}
