using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFading : MonoBehaviour
{
    Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        print("alphu is "+material.color.a);
    }
}
