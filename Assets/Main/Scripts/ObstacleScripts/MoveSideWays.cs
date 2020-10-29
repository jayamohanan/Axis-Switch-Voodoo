using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSideWays : MonoBehaviour
{
    CameraFollow cameraFollow;
    float screenWidth;
    private float spriteWidth;
    float rightX;
    float leftX;

    float xLimitLeft;
    float xLimitRight;
    public bool moveSideWays  = true;
    public float speed = 5f;
    int direction = 1;
    private void Awake()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
    }
    void Start()
    {
        screenWidth = cameraFollow.screenWidth;
        leftX = cameraFollow.screenLeftX;
        rightX = cameraFollow.screenRightX;
        screenWidth = cameraFollow.screenWidth;
        //spriteWidth = GetComponent<AxisScript>().fieldSprite.size.x;

        xLimitLeft = leftX + spriteWidth / 2 * (1.2f);
        xLimitRight = rightX - spriteWidth / 2 * (1.2f);
    }
    void Update()
    {
        if (moveSideWays)
        {
            transform.position += Vector3.right * Time.deltaTime * speed * direction;
            if(transform.position.x>= xLimitRight)
            {
                direction = -1;
            }
            if(transform.position.x<= xLimitLeft)
            {
                direction = 1;
            }
        }
    }
}
