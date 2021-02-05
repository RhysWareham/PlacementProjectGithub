using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManagement
{
    public static bool shapeTurnPhase = false;
    public static bool shapeTurning = false;
    public static bool shapeTurned = false;
    public static bool shapeStationary = true;
    public static bool faceCorrectionComplete = true;

    public static int enemiesLeftAliveOnFace;

    public static int maxNumOfEnemiesForFace;
    public static bool canStartSpawning = true;
    public static bool enemySpawningComplete;

    public static bool forwardFaceChecked = false;

    public static bool faceComplete = false;
    public static bool PlanetCanRotate = true;

    //Each angle for animations
    public static int currentLegsBodyAngle = 0;
    public static int previousLegsBodyAngle = 0;

    public static bool debug = false;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    //Set which shape has been chosen.
    //    //Set the current face as face 1

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //If a shape has been chosen
    //    //ShapeInfo.chosenShape = (int)ShapeInfo.ShapeType.CUBE;

    //}
}
