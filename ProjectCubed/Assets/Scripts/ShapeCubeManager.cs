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

    


    [SerializeField]
    private Transform[] spawnPoints;

    public enum Face
    {
        A,
        B,
        C,
        D,
        E,
        F
    };

    public Face currentFace = 0;
    private int iCurrentFace = 0;

    public bool[] faceComplete =
    {
        false, //Face A
        false, //Face B
        false, //Face C
        false, //Face D
        false, //Face E
        false  //Face F
    };
    
    [SerializeField] float threshHold = 5.0f;
    [SerializeField] float rayLength = 1.0f;

    private Transform Planet;
    private Vector3 targetFaceRotation = new Vector3(0, 0, 0);

    private void Awake()
    {
        ShapeInfo.chosenShape = ShapeInfo.ShapeType.CUBE;
        SetMaxNumOfEnemiesOnFace();
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Planet = GameObject.Find("PlanetHolder").transform.Find("Planet");
        CheckFaceForward();
    }

    // Update is called once per frame
    void Update()
    {
        //If the planet has finished rotating, check what face is forward
        if(ShapeInfo.planetRotationCompleted && !GameManagement.forwardFaceChecked)
        {
            GameManagement.PlanetCanRotate = false;
            //Check what face is on camera
            CheckFaceForward();
            GameManagement.forwardFaceChecked = true;
            //Correct the face rotations
            FaceRotationCorrection();

            //If the current face has not been completed yet
            if(faceComplete[(int)currentFace] == false)
            {
                //Lock the planet from being able to rotate
                GameManagement.PlanetCanRotate = false;
                //Set the number of enemies to be spawned on the current face
                SetMaxNumOfEnemiesOnFace();
                //Set canStartSpawning to true
                GameManagement.canStartSpawning = true;
            }
            else
            {
                //If the face is complete, allow the planet to rotate
                GameManagement.PlanetCanRotate = true;
            }
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

        #region PlanetPerpendicularCorrection
        ////If the planet is not at an exact 90 degree angle, 
        //if ((Planet.transform.localEulerAngles.x % 90) != 0 ||
        //    (Planet.transform.localEulerAngles.y % 90) != 0 ||
        //    (Planet.transform.localEulerAngles.z % 90) != 0)
        //{
        //    //Correct the rotation of the planet to ensure it is exactly 90 degrees, by dividing each axis by 10, 
        //    //rounding to the nearing whole number, and then multiplying by 10
        //    Planet.transform.rotation = Quaternion.Euler(Mathf.Round(Planet.transform.localEulerAngles.x / 10) * 10,
        //                                            Mathf.Round(Planet.transform.localEulerAngles.y / 10) * 10,
        //                                            Mathf.Round(Planet.transform.localEulerAngles.z / 10) * 10);
        //}

        Planet.transform.rotation = Quaternion.Euler(UnwrapAngles(CheckPlanetPerpendicularRotation(new Vector3(Planet.transform.localEulerAngles.x, Planet.transform.localEulerAngles.y, Planet.transform.localEulerAngles.z))));
        Planet.transform.localEulerAngles = UnwrapAngles(Planet.transform.localEulerAngles);
        #endregion

        Debug.Log((int)currentFace);
        iCurrentFace = (int)currentFace;
        Debug.Log(iCurrentFace);

        GOFaces[iCurrentFace].transform.localEulerAngles = UnwrapAngles(GOFaces[iCurrentFace].transform.localEulerAngles);

        //I want it to be either 360 - x or 0 - x as it is degrees for rotation
        //If the planet's rotation is not the same as the current face's rotation values...
        if (Planet.transform.localEulerAngles != GOFaces[iCurrentFace].transform.localEulerAngles)
        {

        }

        if (Planet.transform.localEulerAngles != new Vector3(GOFaces[iCurrentFace].transform.localEulerAngles.x * -1, GOFaces[iCurrentFace].transform.localEulerAngles.y * -1, GOFaces[iCurrentFace].transform.localEulerAngles.z * -1) &&
            Planet.transform.localEulerAngles != UnwrapAngles(new Vector3(0 - GOFaces[iCurrentFace].transform.localEulerAngles.x, 0 - GOFaces[iCurrentFace].transform.localEulerAngles.y, 0 - GOFaces[iCurrentFace].transform.localEulerAngles.z)))
        {
            targetFaceRotation = Planet.transform.localEulerAngles;
            //Check which axis are incorrect, and then set that axis to the value of the face's rotation multiplied by -1
            if (Planet.transform.localEulerAngles.x != GOFaces[iCurrentFace].transform.localEulerAngles.x)
            {

                targetFaceRotation = new Vector3(GOFaces[iCurrentFace].transform.localEulerAngles.x * -1, targetFaceRotation.y, targetFaceRotation.z);
                //Planet.transform.localEulerAngles = new Vector3(GOFaces[iCurrentFace].transform.localEulerAngles.x * -1, Planet.transform.localEulerAngles.y, Planet.transform.localEulerAngles.z);
            }
            if (Planet.transform.localEulerAngles.y != GOFaces[iCurrentFace].transform.localEulerAngles.y)
            {
                targetFaceRotation = new Vector3(targetFaceRotation.x, GOFaces[iCurrentFace].transform.localEulerAngles.y * -1, targetFaceRotation.z);
                //Planet.transform.localEulerAngles = new Vector3(Planet.transform.localEulerAngles.x, GOFaces[iCurrentFace].transform.localEulerAngles.y * -1, Planet.transform.localEulerAngles.z);
            }
            if (Planet.transform.localEulerAngles.z != GOFaces[iCurrentFace].transform.localEulerAngles.z)
            {
                targetFaceRotation = new Vector3(targetFaceRotation.x, targetFaceRotation.y, GOFaces[iCurrentFace].transform.localEulerAngles.z * -1);
                //Planet.transform.localEulerAngles = new Vector3(Planet.transform.localEulerAngles.x, Planet.transform.localEulerAngles.y, GOFaces[iCurrentFace].transform.localEulerAngles.z * -1);
            }

            targetFaceRotation = UnwrapAngles(targetFaceRotation);
            GameManagement.PlanetCanRotate = false;
            StartCoroutine(RotatePlanetCorrect());
            StartCoroutine(TimerForFaceRotation());
        }
        else
        {
            Debug.Log("The face doesn't need rotating");
        }

        
    }

    /// <summary>
    /// IEnumerator which rotates the planet gradually so that the current face is upright 
    /// </summary>
    /// <returns></returns>
    public IEnumerator RotatePlanetCorrect()
    {
        while(Planet.transform.localEulerAngles != targetFaceRotation)
        {
            Planet.transform.rotation = Quaternion.Slerp(Planet.transform.rotation, Quaternion.Euler(targetFaceRotation.x, targetFaceRotation.y, targetFaceRotation.z), Time.deltaTime * ShapeInfo.rotateSpeed);
            yield return 0;
        }
        
        //maybe unwrapanglesfor targetfacerotation?
        //Use this line elsewhere to ensure the angle to perfectly at 90 degrees
        Planet.transform.rotation = Quaternion.Euler(CheckPlanetPerpendicularRotation(new Vector3(Planet.transform.localEulerAngles.x, Planet.transform.localEulerAngles.y, Planet.transform.localEulerAngles.z)));
    }

    public IEnumerator TimerForFaceRotation()
    {
        float timeToCorrectFaceRotation = 1.0f;
        while(timeToCorrectFaceRotation > 0)
        {
            timeToCorrectFaceRotation -= Time.deltaTime;
            yield return 0;
        }
        
        Planet.transform.rotation = Quaternion.Euler(CheckPlanetPerpendicularRotation(new Vector3(targetFaceRotation.x, targetFaceRotation.y, targetFaceRotation.z)));
        GameManagement.PlanetCanRotate = true;

    }

    public Vector3 CheckPlanetPerpendicularRotation(Vector3 vec3)
    {
        //If the planet is not at an exact 90 degree angle, 
        if ((vec3.x % 90) != 0 ||
            (vec3.y % 90) != 0 ||
            (vec3.z % 90) != 0)
        {
            //Correct the rotation of the planet to ensure it is exactly 90 degrees, by dividing each axis by 10, 
            //rounding to the nearing whole number, and then multiplying by 10
            Vector3 newVec3 = new Vector3(Mathf.Round(vec3.x / 10) * 10,
                                                    Mathf.Round(vec3.y / 10) * 10,
                                                    Mathf.Round(vec3.z / 10) * 10);

            return newVec3;
        }
        else
        {
            return vec3;
        }
    }

    private void SetMaxNumOfEnemiesOnFace()
    {
        GameManagement.maxNumOfEnemiesForFace = Random.Range(minEnemiesOnFace, maxEnemiesOnFace);
        //GameManagement.maxNumOfEnemiesForFace = 2;
    }



    public void SetSpawnPoints(ref List<Transform> levelSpawnPoints)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            levelSpawnPoints.Add(spawnPoints[i]);
        }
    }
    
    
    /// <summary>
    /// Function to unwrap each axis at the same time, without needing to call the function for each individual axis
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public static Vector3 UnwrapAngles(Vector3 vec3)
    {
        return new Vector3(UnwrapAngle(vec3.x), UnwrapAngle(vec3.y), UnwrapAngle(vec3.z));
    }

    /// <summary>
    /// This function sets any angle which is negative to its positive alternate angle. I.E. -90 will become 270
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private static float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }

}
