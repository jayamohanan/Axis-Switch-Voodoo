using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScaling : MonoBehaviour
{
    [Header("Scale")]
    public bool scale;
    public float minScaleFactor = 0.0f;
    public float scaleSpeed =0.05f;
    float currentScaleFactor;
    float targetScaleFactor;

    [Header("MoveSideWays")]
    public bool moveSideWays;
    public float speed = 5f;

    CameraFollow cameraFollow;
    float screenWidth;
    private float spriteWidth;
    float rightX;
    float leftX;
    float xLimitLeft;
    float xLimitRight;
    int direction = 1;
    private GameManager gameManager;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        cameraFollow = FindObjectOfType<CameraFollow>();
    }
    void Start()
    {
        //Scale
        currentScaleFactor = Random.Range(minScaleFactor, 1);
        if(currentScaleFactor-minScaleFactor>(1-minScaleFactor)/2)
        {
            targetScaleFactor = minScaleFactor;
        }
        else
        {
            targetScaleFactor = 1;
        }

        //Move
        screenWidth = cameraFollow.screenWidth;
        if (gameManager.useCustomObstacle)
        {
            leftX = -cameraFollow.sideWallX;
            rightX = cameraFollow.sideWallX;
        }
        else//use camera vboundary values
        {
            leftX = cameraFollow.screenLeftX;
            rightX = cameraFollow.screenRightX;
        }
        
        screenWidth = cameraFollow.screenWidth;
        //spriteWidth = GetComponent<AxisScript>().fieldSprite.size.x;
        spriteWidth = GetComponent<AxisScript>().fieldRadius;

        xLimitLeft = leftX + spriteWidth  * (1.2f);
        xLimitRight = rightX - spriteWidth* (1.2f);
    }

    void Update()
    {
        if (scale)
        {
            //currentScaleFactor = Mathf.Lerp(currentScaleFactor, targetScaleFactor, scaleSpeed);
            currentScaleFactor = Mathf.MoveTowards(currentScaleFactor, targetScaleFactor, scaleSpeed);
            if(currentScaleFactor == 1)
            {
                targetScaleFactor = minScaleFactor;
            }
            else if(currentScaleFactor == minScaleFactor)
            {
                targetScaleFactor = 1;
            }
            
            transform.localScale = Vector3.one * currentScaleFactor;
        }
        if (moveSideWays)
        {
            transform.position += Vector3.right * Time.deltaTime * speed * direction;
            if (transform.position.x >= xLimitRight)
            {
                direction = -1;
            }
            if (transform.position.x <= xLimitLeft)
            {
                direction = 1;
            }
        }
    }
}
