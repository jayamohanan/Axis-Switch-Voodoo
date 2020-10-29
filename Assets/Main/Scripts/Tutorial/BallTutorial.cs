#region Comments
//using GameAnalyticsSDK;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class BallTutorial : MonoBehaviour
//{
//    public GameObject scoreCollectionTextPrefab;
//    public GameObject axisSkipTextPrefab;
//    public Transform gameManagerCanvas;
//    public GameObject explosionPrefab;
//    public GameObject scaleEffectPrefab;
//    private MeshRenderer mr;
//    private Rigidbody rb;
//    private bool showPath;
//    private bool vibrate;
//    private int vibrationAmount = 11;
//    private bool set;
//    private LevelManager levelManager;
//    private float lineWidth = 0.15f;
//    private Camera mainCam;
//    private bool giveBonus;

//    public GameObject pathSpherePrefab;
//    private Transform pathSphereParent;
//    [HideInInspector] public bool rotate;
//    Vector3 targetPos;
//    private Vector3 centreDir;
//    Vector3 throwDir;
//    public bool move;
//    [HideInInspector] public int strength;
//    [HideInInspector] public bool levelFinished;
//    float moveSpeed = 30;
//    LineRenderer line;
//    [HideInInspector] public Material lineMat;
//    [HideInInspector] public float rotateSpeed = 360;//Revert
//    CameraFollow cameraFollow;
//    [HideInInspector] public Transform currentAxis;
//    [HideInInspector] public Transform nextAxis;
//    private int relativeAxisPosition;
//    private GameManager gameManager;
//    private Material material;
//    float outsideX;
//    [HideInInspector] public float radius = 3;
//    [HideInInspector] public List<Transform> axisList = new List<Transform>();
//    Transform explosionParent;
//    public Color originalColor;
//    public Color axisTouchColor;

//    float bottomPosition;//first axis + radius
//    float topPosition;
//    float distanceToTop;
//    float previousZ;
//    private Vector3 lastLaunchPosition;

//    Vector3 previousPathPosition;
//    float pathSphereDistance;
//    [HideInInspector] public int levelMultiplier;
//    Vector3 upVector;
//    int axisCount;
//    int currentAxisIndex;
//    int nextAxisIndex;

//    private Vector3 skipScoreStartPos;
//    //private Vector3 skipScoreTargetPos;
//    float skipScoreTargetHeight;
//    private float colliderRadius;


//    void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//        levelManager = FindObjectOfType<LevelManager>();
//        explosionParent = new GameObject("Explosion Parent").transform;
//        colliderRadius = GetComponent<SphereCollider>().radius;

//        line = GetComponent<LineRenderer>();
//        line.startWidth = lineWidth;
//        line.endWidth = lineWidth;
//        line.positionCount = 0;
//        lineMat = line.material;
//    }
//    private void Start()
//    {
//        skipScoreStartPos = new Vector2(Screen.width / 2, Screen.height * 0.7f);
//        skipScoreTargetHeight = Screen.height * 0.75f;
//        Tweak.Instance.valueCange += OnEditValueChanged;
//    }
//    Vector3 rayOrigin;
//    Vector3 rayDirection;

//    //private void OnDrawGizmos()
//    //{
//    //    if (move)
//    //    {
//    //        Gizmos.color = Color.red;
//    //        Gizmos.DrawRay(rayOrigin, rayDirection * 100);
//    //    }
//    //}
//    public void OnNextLevel()
//    {
//    }
//    GameObject pathSphere;
//    float debugRadius;
//    Quaternion q;
//    public bool inUpdate;
//    private Vector3 newPosition;
//    float distance_CurrentAxis;//dist b/w player from current axis
//    float distance_NextAxis;//distance b/w current and next axis
//    void Update()
//    {

//#if (!UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS))
////#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
//    if (cameraFollow.cameraAtStart  && !gameManager.gameOver)
//    {
//        if (rotate && Input.touchCount>0)
//        {
//            if(Input.GetTouch(0).phase == TouchPhase.Began)
//            {
//                rotate = false;
//                centreDir = (targetPos - transform.position).normalized;
//                throwDir = Vector3.Cross(centreDir, Vector3.up);
//                move = true;
//                lastLaunchPosition = transform.position;
//                SpawnPathSpere();
//                throwDir = Vector3.Cross(centreDir, Vector3.up);
//                move = true;
//                if (Physics.SphereCast(transform.position, colliderRadius, throwDir, out hitInfo, 1000, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
//                {
//                    print("general sphere cast on "+hitInfo.collider.tag+" name "+ hitInfo.collider.name);

//                    if (hitInfo.collider.tag == "Axis" || hitInfo.collider.tag == "FinishPoint")
//                    {
//                        giveBonus = true;
//                        print("true cast tag is "+ hitInfo.collider.tag+" name "+ hitInfo.collider.name);
//                    }
//                }
//                else
//                {
//                    print("No sphere cats");
//                }
//            }
//        }
//    }

//#elif (UNITY_EDITOR || UNITY_WEBGL)//
//        if (throwActive)
//        {
//            if (rotate && Input.GetKeyDown(KeyCode.Space))
//            {
//                rotate = false;
//                centreDir = (targetPos - transform.position).normalized;
//                throwDir = Vector3.Cross(centreDir, Vector3.up);
//                move = true;

//                rayOrigin = transform.position;
//                rayDirection = throwDir;
//                lastLaunchPosition = transform.position;
//                SpawnPathSpere();

//                if (Physics.SphereCast(transform.position, colliderRadius, throwDir, out hitInfo, 1000, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
//                {
//                    print("sphere cast on " + hitInfo.collider.tag + " name " + hitInfo.collider.name);
//                    if (hitInfo.collider.tag == "Axis" || hitInfo.collider.tag == "FinishPoint")
//                    {
//                        giveBonus = true;
//                        print("true cast tag is " + hitInfo.collider.tag);
//                    }
//                }
//                else
//                {
//                    print("No sphere cats");
//                }
//            }
//        }
//#endif
//        if (inUpdate)
//        {
//            if (move)
//            {
//                rb.MovePosition(rb.position + throwDir * Time.deltaTime * moveSpeed * rotationDirection);
//                line.positionCount = 0;
//            }
//            if (rotate)//
//            {
//                upVector = Vector3.up;
//                q = Quaternion.AngleAxis(Time.deltaTime * rotateSpeed * rotationDirection, upVector);
//                rb.MovePosition(q * (rb.transform.position - targetPos) + targetPos);
//                rb.MoveRotation(rb.transform.rotation * q);

//                line.SetPosition(0, rb.position);
//                line.SetPosition(1, targetPos);
//            }
//        }
//    }
//    RaycastHit hitInfo;
//    private void FixedUpdate()//
//    {
//        if (move)
//            SpawnPathSpere();
//        if (!inUpdate)
//        {
//            if (move)
//            {
//                newPosition = rb.position + throwDir * Time.deltaTime * moveSpeed * rotationDirection;

//                rb.MovePosition(Vector3.Lerp(rb.position, newPosition, 1f));
//                line.positionCount = 0;
//            }
//            if (rotate)
//            {
//                upVector = Vector3.up;
//                q = Quaternion.AngleAxis(Time.deltaTime * rotateSpeed * rotationDirection, upVector);
//                rb.MovePosition(q * (rb.transform.position - targetPos) + targetPos);
//                rb.MoveRotation(rb.transform.rotation * q);

//                line.SetPosition(0, rb.position);
//                line.SetPosition(1, targetPos);
//            }
//        }
//    }
//    int rotationDirection = 1;
//    AxisScript axisScript;
//    private void OnTriggerEnter(Collider other)//
//    {
//        if (gameManager.gameOver)
//            return;
//        if (move)
//        {
//            if (other.tag == "Axis")
//            {
//                giveBonus = false;//stop checking for bonus
//                axisScript = other.GetComponent<AxisScript>();
//                other.GetComponent<MeshRenderer>().material.color = axisTouchColor;
//                rotationDirection = 1;
//                //if (axisScript.biDirectional)//
//                //{
//                //    Vector3 refDirection = (lastLaunchPosition - other.transform.position);
//                //    Vector3 contactDirection = transform.position - other.transform.position;
//                //    float angle = Vector3.SignedAngle(refDirection, contactDirection, Vector3.up);
//                //    if (angle >= 0)
//                //        rotationDirection = 1;
//                //    else
//                //        rotationDirection = -1;
//                //}
//                if (axisScript.antiClockWise)
//                    rotationDirection = -1;

//                radius = axisScript.rotateRadius;
//                currentAxis = other.transform;

//                GameObject scaleEffectObj = Instantiate(scaleEffectPrefab, other.transform.position, Quaternion.identity);
//                Color color = material.color;
//                color.a = 0.2f;
//                scaleEffectObj.GetComponent<MeshRenderer>().material.color = color;
//                Vector3 initialLocalScale = other.transform.GetChild(0).localScale * 1.05f;
//                scaleEffectObj.transform.localScale = initialLocalScale;
//                Vector3 finalScale = new Vector3(other.transform.GetChild(1).localScale.x * 1.2f, initialLocalScale.y, other.transform.GetChild(1).localScale.z * 1.2f);
//                LeanTween.alpha(scaleEffectObj, 0, 0.34f).setEase(LeanTweenType.linear);
//                LeanTween.scale(scaleEffectObj, finalScale, 0.35f).setEase(LeanTweenType.linear).setOnComplete(() =>
//                {
//                    Destroy(scaleEffectObj);
//                });
//                print("color effect on axis " + other.name);
//                if (!axisScript.counted)
//                {
//                    ScoreManager.Instance.IncrementScore(1);
//                    axisScript.counted = true;
//                    ShowScoreCollectionAnim(scoreCollectionTextPrefab, mainCam.WorldToScreenPoint(other.transform.position), "+1", 150, 2);
//                    AudioManager.Instance.PlayTapSound();

//                }
//                RotateAroundAxis(other.transform.position);
//                cameraFollow.OnAxisChange();
//                int lastAxisIndex = currentAxisIndex;
//                currentAxisIndex = axisList.IndexOf(other.transform);
//                for (int i = lastAxisIndex + 1; i < currentAxisIndex; i++)
//                {
//                    AxisScript axisScript = axisList[i].GetComponent<AxisScript>();
//                    if (!axisScript.counted)
//                    {
//                        ShowScoreCollectionAnim(scoreCollectionTextPrefab, mainCam.WorldToScreenPoint(axisList[i].position), "+2", 150, 1);
//                        ShowScoreCollectionAnim(axisSkipTextPrefab, skipScoreStartPos, "AXIS SKIPPED +2", 150, 1.5f);
//                        ScoreManager.Instance.Addpoints(2);
//                        AudioManager.Instance.PlayTapSound();
//                        axisScript.counted = true;
//                    }
//                }
//                if (axisCount > (currentAxisIndex + 1))
//                {
//                    nextAxisIndex = currentAxisIndex + 1;
//                    nextAxis = axisList[nextAxisIndex];
//                    distance_NextAxis = Vector3.Distance(nextAxis.position, currentAxis.position) + 2;
//                }
//                else
//                    nextAxisIndex = -1;
//            }
//        }
//        if (rotate)
//        {
//            if (other.tag == "Coin")
//            {
//                other.gameObject.SetActive(false);
//                strength++;
//            }
//        }
//        if (other.tag == "FinishPoint")//
//        {
//            if (levelFinished)
//                return;
//            giveBonus = false;
//            levelMultiplier = other.GetComponent<MultiplierData>().multiplier;
//            levelFinished = true;
//            moveSpeed /= 5;
//            gameManager.multiplierColor = other.GetComponent<MeshRenderer>().material.color;
//            material.color = other.GetComponent<MeshRenderer>().material.color;
//            gameManager.multiplierInitialPosition = other.transform.position/* + Vector3.up * 0.5f*/;
//            gameManager.multiplierTransform = other.transform/* + Vector3.up * 0.5f*/;
//            gameManager.StartCoroutine("GameOverDelay", true);//
//            AudioManager.Instance.PlaySuccessSound();
//            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelManager.currentLevel.ToString());
//            TinySauce.OnGameFinished(levelManager.currentLevel.ToString(), true, ScoreManager.Instance.score);

//        }
//    }
//    private void RotateAroundAxis(Vector3 centrePosition)
//    {
//        transform.position = centrePosition + (transform.position - centrePosition).normalized * radius;
//        rotate = true;
//        move = false;
//        targetPos = centrePosition;
//        line.positionCount = 2;
//        line.SetPosition(0, transform.position);
//        line.SetPosition(1, centrePosition);

//    }
//    private void OnCollisionEnter(Collision collision)
//    {
//        move = false;
//        giveBonus = false;
//        if (gameManager.gameOver)
//            return;
//        Vector3 contactPoint = collision.contacts[0].point;
//        gameObject.SetActive(false);
//        int rightDirection;
//        if (contactPoint.x > currentAxis.position.x)
//            rightDirection = 1;
//        else
//            rightDirection = -1;
//        Explode(contactPoint, rightDirection);
//        AudioManager.Instance.PlayGlassBreakSound();
//        gameManager.StartCoroutine("GameOverDelay", false);
//        //GameAnalytics.Initialize();
//        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelManager.currentLevel.ToString());
//        TinySauce.OnGameFinished(levelManager.currentLevel.ToString(), false, ScoreManager.Instance.score);
//    }
//    public void Restart()
//    {
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }
//    private float explosionForce = 1f;
//    private Rigidbody pieceRb;
//    private void Explode(Vector3 position, int rightDirection)//forceDirection left or right
//    {
//        List<Rigidbody> pieceRbs = new List<Rigidbody>();
//        Vector3 forceDirection;
//        Transform explosionPieces = Instantiate(explosionPrefab, transform.position, Quaternion.identity, explosionParent).transform;
//        explosionPieces.transform.localScale *= transform.localScale.x;
//        foreach (Transform t in explosionPieces)
//        {
//            pieceRbs.Add(t.GetComponent<Rigidbody>());
//            t.GetComponent<MeshRenderer>().sharedMaterial = material;
//        }
//        foreach (Rigidbody rb in pieceRbs)
//        {
//            forceDirection = new Vector3((Random.Range(0, 1)) * rightDirection, 0, Random.Range(-1, 1));
//            rb.AddForce(forceDirection * explosionForce, ForceMode.Impulse);
//        }
//    }
//    public void DestroyExplosionParticles()
//    {
//        int explosionParticleCount = explosionParent.childCount;
//        for (int i = explosionParticleCount - 1; i >= 0; i--)//Clearing all 
//        {
//            if (explosionParent.GetChild(i) != null)
//                Destroy(explosionParent.GetChild(i).gameObject);
//        }
//    }

//    //private int FindAxisIndex(Transform axis)
//    //{
//    //    for (int i = 0; i < axisCount; i++)
//    //    {
//    //        if(axisList[i] == axis)
//    //        {
//    //            return i;
//    //        }
//    //    }
//    //    return -1;
//    //}
//    private void SpawnPathSpere()
//    {
//        if (!levelFinished)
//        {
//            if (showPath || vibrate)
//            {
//                previousPathPosition = transform.position;
//                if (showPath)
//                    pathSphere = Instantiate(pathSpherePrefab, previousPathPosition, Quaternion.identity, gameManager.pathSphereParent);
//                if (vibrate)
//                    Vibration.Vibrate(vibrationAmount);
//            }
//        }
//    }
//    private void OnEditValueChanged()
//    {
//        if (!set)
//            return;
//        showPath = Tweak.Instance.tList[0].isOn;
//        vibrate = Tweak.Instance.tList[1].isOn;
//        if (Tweak.Instance.ifList[0].text != null)
//            vibrationAmount = (int)float.Parse(Tweak.Instance.ifList[0].text);
//    }//1.8 for +1
//    private void ShowScoreCollectionAnim(GameObject textPrefab, Vector2 position, string textValue, float animHeight, float time)
//    {
//        GameObject textObject = Instantiate(textPrefab, gameManagerCanvas);
//        textObject.GetComponent<Text>().text = textValue;
//        textObject.transform.position = position;
//        LeanTween.alphaText(textObject.GetComponent<RectTransform>(), 0, time - 0.1f).setEase(LeanTweenType.easeOutCubic);
//        LeanTween.moveY(textObject, textObject.transform.position.y + animHeight, time).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
//        {
//            Destroy(textObject);
//        });
//    }
//}

#endregion
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BallTutorial : MonoBehaviour
{
    private Transform axis;
    bool rotate;
    bool move;
    public bool throwActive;
    Vector3 throwDir;
    Vector3 centreDir;
    Vector3 targetPos;
    float rotateSpeed = 210;
    float moveSpeed = 23;
    LineRenderer line;
    LineRenderer throwDirLine;
    float lineWidth = 0.1f;
    float radius;
    private Material material;
    private bool ballUsed;
    private TutorialManager tutorialManager;
    Transform currentAxis;
    private void Awake()
    {
        GameObject level = GameObject.FindGameObjectWithTag("TutorialLevel");
        currentAxis = axis = level.transform.Find("Axes").GetChild(0);  
        if(axis == null)
        {
            print("NULL");
        }
        tutorialManager = FindObjectOfType<TutorialManager>();
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        axisScript = axis.GetComponent<AxisScript>();
        material = GetComponent<MeshRenderer>().material;
        throwDirLine = transform.GetChild(0).GetComponent<LineRenderer>();

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = 0;
        radius = axisScript.rotateRadius;

        throwDirLine.startWidth = lineWidth;
        throwDirLine.endWidth = lineWidth;
        throwDirLine.positionCount = 2;
        //lineMat = line.material;
    }
    private void Start()
    {
        RotateAroundAxis(axis.position);
    }

    private void Update()
    {
#if (!UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS))
        //#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
         if (throwActive)
        {
            if (rotate && Input.touchCount>0)
            {
                if(Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    rotate = false;
                    centreDir = (targetPos - transform.position).normalized;
                    throwDir = Vector3.Cross(centreDir, Vector3.up);
                    move = true;
                }
            }
        }
#elif (UNITY_EDITOR || UNITY_WEBGL)//
        if (throwActive)
        {
            if (rotate && Input.GetKeyDown(KeyCode.Space))
            {
                rotate = false;
                centreDir = (targetPos - transform.position).normalized;
                throwDir = Vector3.Cross(centreDir, Vector3.up);
                move = true;
                //rayOrigin = transform.position;
                //rayDirection = throwDir;
                //lastLaunchPosition = transform.position;

            }
        }
#endif
    }
    Vector3 newPosition;
    Vector3 upVector;
    Rigidbody rb;   
    Quaternion q;
    AxisScript axisScript;
    private void FixedUpdate()//
    {
        if (move)
        {
            newPosition = rb.position + throwDir * Time.deltaTime * moveSpeed;

            rb.MovePosition(Vector3.Lerp(rb.position, newPosition, 1f));
            line.positionCount = 0;
        }
        if (rotate)
        {
            upVector = Vector3.up;
            q = Quaternion.AngleAxis(Time.deltaTime * rotateSpeed , upVector);
            rb.MovePosition(q * (rb.transform.position - targetPos) + targetPos);
            rb.MoveRotation(rb.transform.rotation * q);

            line.SetPosition(0, rb.position);
            line.SetPosition(1, targetPos);

            //centreDir = (targetPos - transform.position).normalized;
            //throwDir = Vector3.Cross(centreDir, Vector3.up);
            //throwDirLine.SetPosition(0, rb.position);
            //throwDirLine.SetPosition(1, rb.position+ throwDir *10);

        }
    }
    private void RotateAroundAxis(Vector3 centrePosition)
    {
        transform.position = centrePosition + (transform.position - centrePosition).normalized * radius;
        //transform.RotateAround(centrePosition, Vector3.up, Random.Range(0,360));
        rotate = true;
        move = false;
        targetPos = centrePosition;
        line.positionCount = 2;
        line.SetPosition(0, rb.position);
        line.SetPosition(1, centrePosition);
    }
    private void OnTriggerExit(Collider other)
    {
        print("OnTriggerExit");
        if (other.tag == "TutorialBounday")
        if (!ballUsed)
            {
                ballUsed = true;
                tutorialManager.OutOfTrigger();
                Destroy(gameObject,1.2f);
            }
    }
    private void OnTriggerEnter(Collider other)
    {
        print("OnTriggerEnter");
        print("trigger enter "+other.name);
        if(other.tag == "Axis" && move)
        {
            radius = axisScript.rotateRadius;
            currentAxis = other.transform;
            RotateAroundAxis(currentAxis.position);
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayTapSound();
            }
        }
        if(!ballUsed && other.tag == "FinishPoint")
        {
            ballUsed = true;
            tutorialManager.OnFinishLineTouched();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySuccessSound();
            }
        }
        else
        {
            print("Unables");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        print("OnCollisionEnter");
        if (!ballUsed)
        {
            ballUsed = true;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            print("Explodin");
            if(AudioManager.Instance!=null)
            AudioManager.Instance.PlayGlassBreakSound();
            Explode(collision.contacts[0].point, 1);
            tutorialManager.InstantiateBallAgain();
            //print("again insta");
        }
        else
        {
            print("Ball used true");
        }
    }
    private float explosionForce = 1f;

    private void Explode(Vector3 position, int rightDirection)//forceDirection left or right
    {
        List<Rigidbody> pieceRbs = new List<Rigidbody>();
        Vector3 forceDirection;
        Transform explosionPieces = Instantiate(tutorialManager.explosionPrefab, transform.position, Quaternion.identity, transform).transform;
        explosionPieces.transform.localScale *= transform.localScale.x;
        Destroy(explosionPieces.gameObject, 1);
        Destroy(gameObject, 1.1f);
        foreach (Transform t in explosionPieces)
        {
            pieceRbs.Add(t.GetComponent<Rigidbody>());
            t.GetComponent<MeshRenderer>().sharedMaterial = material;
        }
        foreach (Rigidbody rb in pieceRbs)
        {
            forceDirection = new Vector3((Random.Range(0, 1)) * rightDirection, 0, Random.Range(-1, 1));
            rb.AddForce(forceDirection * explosionForce, ForceMode.Impulse);
        }
    }
}