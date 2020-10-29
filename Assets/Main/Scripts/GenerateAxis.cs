//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.Animations;

//public class GenerateAxis : MonoBehaviour
//{
//    public GameObject axisPrefab;
//    public int axisCount = 50;
//    public Vector3 nextPosition;
//    private Transform axisParent;
//    [HideInInspector] public float screenWidthWorld;
//    float axisGapX;
//    private float axisRadius;
//    private GameManager gameManager;

//    private MoveTransform moveTransform;
//    private List<Transform> axisList = new List<Transform>();
//    private Color fieldColor;
//    [HideInInspector] public float radius = 3;
//    public GameObject fieldSprite;

//    private void Awake()
//    {
//        moveTransform = FindObjectOfType<MoveTransform>();
//        gameManager = FindObjectOfType<GameManager>();

//        fieldColor = gameManager.fieldColor;
//        Gizmos.color = fieldColor;

//        axisRadius = moveTransform.radius;
//        axisParent = new GameObject("AxisParent").transform;
//        axisParent.gameObject.tag = "AxisParent";

//        float leftX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, Camera.main.transform.position.y)).x;
//        float rightX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, Camera.main.transform.position.y)).x;
//        screenWidthWorld = (rightX - leftX);
//        //GameObject cub1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
//        //GameObject cub2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
//        //cub1.transform.position = new Vector3(leftX, 0, 0);
//        //cub1.transform.localScale  = new Vector3(10, 10, 10);
//        //cub2.transform.localScale  = new Vector3(10, 10, 10);
//        //cub2.transform.position = new Vector3(rightX, 0, 0);
//        print("boundaryX " + screenWidthWorld);
//        axisGapX = screenWidthWorld - 2 * (axisRadius * 1.35f);
//        nextPosition = new Vector3(-axisGapX / 2, 0, -8);

//        GenerateAxisPoints();
//    }
//    GameObject axis;
//    GameObject axisSpriteObj;
//    float axisScaleFactor = 0.5f;
//    private void GenerateAxisPoints()//
//    {
//        int xdirection;
//        for (int i = 0; i < axisCount; i++)
//        {
//            if (i % 2 == 0)
//                xdirection = 1;
//            else
//                xdirection = -1;
//            axis = Instantiate(axisPrefab, nextPosition, Quaternion.identity, axisParent) as GameObject;
//            axisSpriteObj = Instantiate(fieldSprite, nextPosition, Quaternion.identity, axis.transform) as GameObject;
//            axisSpriteObj.transform.localEulerAngles = new Vector3(-90, 0, 0);
//            axisSpriteObj.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
//            axisSpriteObj.GetComponent<SpriteRenderer>().size = Vector2.one * 6 / axisScaleFactor;

//            axisList.Add(axis.transform);
//            axis.transform.localScale *= axisScaleFactor;
//            axis.GetComponent<SphereCollider>().radius = 3 / axisScaleFactor;
//            nextPosition += new Vector3(axisGapX * xdirection, 0, 6);
//        }
//    }
//    //    private void OnDrawGizmos()//
//    //    {
//    //#if(UNITY_EDITOR)
//    //        if (!EditorApplication.isPlaying)
//    //            return;
//    //#endif
//    //        Gizmos.color = fieldColor;
//    //        //for (int i = -3; i < 4; i++)
//    //        //{

//    //        //    gizmoIndex = currentAxisIndex + i;
//    //        //    if(gizmoIndex>=0 && gizmoIndex < axisParent.childCount)
//    //        //    {
//    //        //        //print("Gizmo indx "+gizmoIndex+ " currentAxisIndex "+ currentAxisIndex);
//    //        //        Gizmos.DrawSphere(axisList[gizmoIndex].position, radius);
//    //        //        //Gizmos.draw
//    //        //    }
//    //        //    else
//    //        //    {
//    //        //        //print("false @ gizmoIndex "+gizmoIndex);
//    //        //    }
//    //        //}
//    //        print("axis list count " + axisList.Count);

//    //        foreach (Transform t in axisList)
//    //        {
//    //            Gizmos.DrawSphere(t.position, radius);
//    //        }
//    //    }
//}
