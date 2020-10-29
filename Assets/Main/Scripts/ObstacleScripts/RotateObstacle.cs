using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    public bool rotate = true;
    [Range(1,10)]
    public float speed = 1;
    void Start()
    {
        speed *= 100;
    }
    void Update()
    {
        if (rotate)
        {
            transform.Rotate(Vector3.up * (Time.deltaTime * speed));
        } 
    }
}
