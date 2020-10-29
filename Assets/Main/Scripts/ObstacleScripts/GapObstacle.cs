using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapObstacle : MonoBehaviour
{
    public bool continuous;
    public float gap =0.8f;
    public float sideGap = 0.5f;
    [Header("If !Continuous")]
    public float pieceGap = 0.3f;
    public float pieceLength = 0.3f;

    private float sideWallX;
    private GameManager gameManager;
    private CameraFollow cameraFollow;
    private GameObject leftObstacle;
    private GameObject rightObstacle;
    private Material obstacleMat;
    bool obstacleCreated;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        cameraFollow = FindObjectOfType<CameraFollow>();
    }
    private void Start()
    {
        sideWallX = cameraFollow.sideWallX;
        obstacleMat = gameManager.obstacleMat;

        CreateObstacles();
    }
    private void CreateObstacles()
    {
        if (obstacleCreated)
            return;
        obstacleCreated = true;
        leftObstacle = new GameObject("Left Obstacle");
        leftObstacle.transform.SetParent(transform);
        leftObstacle.transform.localPosition = Vector3.zero;

        float nearEndLeft = transform.position.x - gap / 2;
        float nearEndRight = transform.position.x + gap / 2;

        rightObstacle = new GameObject("Right Obstacle"); ;
        rightObstacle.transform.SetParent(transform);
        rightObstacle.transform.localPosition = Vector3.zero;


        MeshRenderer pieceRenderer;

        if (continuous)
        {

            GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
            piece.name = "Left Piece";
            piece.transform.position = new Vector3((nearEndLeft+ (-sideWallX + sideGap))/2, transform.position.y, transform.position.z);
            //print(piece.name+" position "+piece.transform.position);
            piece.transform.localScale = new Vector3(nearEndLeft- (-sideWallX + sideGap),1,0.1f);
            piece.transform.SetParent(leftObstacle.transform);
            pieceRenderer = piece.GetComponent<MeshRenderer>();
            pieceRenderer.sharedMaterial = obstacleMat;
            
            piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
            piece.name = "Right Piece";
            piece.transform.position = new Vector3(((sideWallX - sideGap) +nearEndRight)/2, transform.position.y, transform.position.z);
            //print(piece.name + " position " + piece.transform.position);

            piece.transform.localScale = new Vector3(((sideWallX - sideGap) - nearEndRight), 1,0.1f);
            piece.transform.SetParent(rightObstacle.transform);
            pieceRenderer = piece.GetComponent<MeshRenderer>();
            pieceRenderer.sharedMaterial = obstacleMat;
        }
        else
        {
            float leftLength = (nearEndLeft - (-sideWallX + sideGap));
            float remainingLength = leftLength;
            Vector3 pieceScale = new Vector3(pieceLength, 1, 0.1f);
            Vector3 piecePosition = new Vector3(-sideWallX + sideGap + pieceLength / 2, transform.position.y, transform.position.z);

            while (remainingLength > pieceLength)
            {
                GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
                piece.transform.position = piecePosition;
                piece.transform.localScale = pieceScale;
                piecePosition += Vector3.right * (pieceLength + pieceGap);
                remainingLength -= (pieceLength + pieceGap);
                piece.transform.SetParent(leftObstacle.transform);
                pieceRenderer = piece.GetComponent<MeshRenderer>();
                pieceRenderer.receiveShadows = false;
                pieceRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                pieceRenderer.sharedMaterial = obstacleMat;
            }

            float rightLength = ((sideWallX - sideGap) - nearEndRight);
            remainingLength = rightLength - sideGap;
            piecePosition = new Vector3(nearEndRight + pieceLength / 2, transform.position.y, transform.position.z);
            while ((remainingLength) > pieceLength)
            {
                GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
                piece.transform.position = piecePosition;
                piece.transform.localScale = pieceScale;
                piecePosition += Vector3.right * (pieceLength + pieceGap);
                remainingLength -= (pieceLength + pieceGap);
                piece.transform.SetParent(rightObstacle.transform);
                pieceRenderer = piece.GetComponent<MeshRenderer>();
                pieceRenderer.receiveShadows = false;
                pieceRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                pieceRenderer.sharedMaterial = obstacleMat;
            }
        }
    }

}
