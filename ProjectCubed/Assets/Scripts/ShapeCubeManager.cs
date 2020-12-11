using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeCubeManager : MonoBehaviour
{
    //[SerializeField]
    //private Transform faceA;
    //[SerializeField]
    //private Transform faceB;
    //[SerializeField]
    //private Transform faceC;
    //[SerializeField]
    //private Transform faceD;
    //[SerializeField]
    //private Transform faceE;
    //[SerializeField]
    //private Transform faceF;

    [SerializeField]
    public GameObject[] GOFaces;

    private float timerStart = 3;
    private float rotationTimer = 0;

    private int minEnemiesOnFace = 5;
    private int maxEnemiesOnFace = 15;

    private LevelManager levelManager;

    [SerializeField]
    private Transform[] spawnPoints;

    private enum Face
    {
        A,
        B,
        C,
        D,
        E,
        F
    };

    private Face currentFace = 0;
    private int iCurrentFace = 0;
    
    [SerializeField] float threshHold = 5.0f;
    [SerializeField] float rayLength = 1.0f;

    private void Awake()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        ShapeInfo.chosenShape = ShapeInfo.ShapeType.CUBE;
        SetMaxNumOfEnemiesOnFace();
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //If the faceCorrection has not been completed, but the planet has finished rotating
        if(!GameManagement.faceCorrectionComplete && ShapeInfo.planetRotationCompleted)
        {
            //Call the face correction function
            FaceRotationCorrection();
            SetMaxNumOfEnemiesOnFace();
        }


    }

    /// <summary>
    /// Function that checks which face the player is on
    /// </summary>
    void CheckFaceForward()
    {
        UnityEngine.Debug.DrawRay(this.transform.position, this.transform.up * this.rayLength, Color.red);

        UnityEngine.Debug.DrawRay(this.transform.position, -this.transform.up * this.rayLength, Color.magenta);

        UnityEngine.Debug.DrawRay(this.transform.position, this.transform.forward * this.rayLength, Color.blue);

        UnityEngine.Debug.DrawRay(this.transform.position, -this.transform.forward * this.rayLength, Color.cyan);

        UnityEngine.Debug.DrawRay(this.transform.position, this.transform.right * this.rayLength, Color.yellow);

        UnityEngine.Debug.DrawRay(this.transform.position, -this.transform.right * this.rayLength, Color.gray);

        // theta = arcos( a • b / |a| • |b|)
        float upAngle = Mathf.Acos(Vector3.Dot(this.transform.up, Camera.main.transform.forward) / (this.transform.up.magnitude * Camera.main.transform.forward.magnitude));
        upAngle *= 180.0f / Mathf.PI; // In Degrees not radians.

        float downAngle = Mathf.Acos(Vector3.Dot(-this.transform.up, Camera.main.transform.forward) / (this.transform.up.magnitude * Camera.main.transform.forward.magnitude));
        downAngle *= 180.0f / Mathf.PI; // In Degrees not radians.

        float forwardAngle = Mathf.Acos(Vector3.Dot(this.transform.forward, Camera.main.transform.forward) / (this.transform.forward.magnitude * Camera.main.transform.forward.magnitude));
        forwardAngle *= 180.0f / Mathf.PI; // In Degrees not radians.

        float backwardAngle = Mathf.Acos(Vector3.Dot(-this.transform.forward, Camera.main.transform.forward) / (this.transform.forward.magnitude * Camera.main.transform.forward.magnitude));
        backwardAngle *= 180.0f / Mathf.PI; // In Degrees not radians.

        float rightAngle = Mathf.Acos(Vector3.Dot(this.transform.right, Camera.main.transform.forward) / (this.transform.right.magnitude * Camera.main.transform.forward.magnitude));
        rightAngle *= 180.0f / Mathf.PI; // In Degrees not radians.

        float leftAngle = Mathf.Acos(Vector3.Dot(-this.transform.right, Camera.main.transform.forward) / (this.transform.right.magnitude * Camera.main.transform.forward.magnitude));
        leftAngle *= 180.0f / Mathf.PI; // In Degrees not radians.


        //Set which face is currently facing the camera
        if (forwardAngle < this.threshHold)
        {
            UnityEngine.Debug.Log("Face A is facing the Camera");
            currentFace = (Face)0;
        }

        if (leftAngle < this.threshHold)
        {
            UnityEngine.Debug.Log("Face B is facing the Camera");
            currentFace = (Face)1;
        }

        if (backwardAngle < this.threshHold)
        {
            UnityEngine.Debug.Log("Face C is facing the Camera");
            currentFace = (Face)2;
        }
        
        if (rightAngle < this.threshHold)
        {
            UnityEngine.Debug.Log("Face D is facing the Camera");
            currentFace = (Face)3;
        }
        
        if (downAngle < this.threshHold)
        {
            UnityEngine.Debug.Log("Face E is facing the Camera");
            currentFace = (Face)4;
        }

        if (upAngle < this.threshHold)
        {
            UnityEngine.Debug.Log("Face F is facing the Camera");
            currentFace = (Face)5;
        }


        


        

        //https://forum.unity.com/threads/determine-which-face-of-a-cube-is-facing-the-camera.317066/ Credit - Polymorphik
    }

    /// <summary>
    /// Function to check that the current face has the correct rotation, so the level is not upside down etc
    /// </summary>
    void FaceRotationCorrection()
    {
        if (GameManagement.shapeTurnPhase)
        {
            if (GameManagement.shapeTurning == true && GameManagement.shapeStationary == true)
            {
                rotationTimer = timerStart;
                GameManagement.shapeStationary = false;
            }

            //Start a timer for the rotation of the shape
            if (rotationTimer > 0)
            {
                rotationTimer -= Time.deltaTime;
            }
            else
            {
                GameManagement.shapeTurned = true;
                GameManagement.shapeTurning = false;
                GameManagement.shapeTurnPhase = false;
            }

            if (GameManagement.shapeTurned)
            {
                CheckFaceForward();
                GameManagement.shapeTurnPhase = false;

            }
        }
        else
        {
            Debug.Log((int)currentFace);
            iCurrentFace = (int)currentFace;
            Debug.Log(iCurrentFace);
            //if the current face rotation axis are not 0, set them to zero
            if (GOFaces[iCurrentFace].transform.localRotation.x != 0 ||
                GOFaces[iCurrentFace].transform.localRotation.y != 0 ||
                GOFaces[iCurrentFace].transform.localRotation.z != 0)
            {
                //DOESNT WORK YET, NEED TO FIX!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                GOFaces[(int)currentFace].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                Debug.Log("Face doesn't need rotating");
            }
            
            GameManagement.shapeStationary = true; 
            GameManagement.faceCorrectionComplete = true;

        }
    }

    private void SetMaxNumOfEnemiesOnFace()
    {
        GameManagement.maxNumOfEnemiesForFace = Random.Range(minEnemiesOnFace, maxEnemiesOnFace);
    }

    public void SetSpawnPoints(ref List<Transform> levelSpawnPoints)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            levelSpawnPoints.Add(spawnPoints[i]);
        }
    }
}
