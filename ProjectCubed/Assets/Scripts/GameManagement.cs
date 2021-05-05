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
    public static int currentHeadBodyAngle = 0;
    public static int previousHeadBodyAngle = 0;

    public static bool debug = false;

    public static bool playerAlive = false;

    public static bool menuOnScreen = false;
    public static bool pauseIsPressed = false;
    public static bool gameIsPaused = false;
    public static bool uiMouseClicked = false;

    public static int currentPlanet = 1;
    public static bool newPlanet = false;

    public static bool faceChecked = false;
    public static bool clearedTextChecked = true;


    public enum UpgradeType
    {
        WeaponChange,
        PassiveChange,
        ActiveItem
    //Need to change weapon animation and firing/bullets animations and scripts for each weapontype in the enum i 
    //need to make. Also need to add a new layer and tag for Upgrades, and a function for when player picks it up
    };

    public enum WeaponUpgrades
    { 
        NORMAL,
        SHOTGUN,
        BURST
    };

    public static float shotDistance = 100f;

    public static UpgradeType currentUpgradeType;
    public static WeaponUpgrades currentWeaponType = WeaponUpgrades.NORMAL;


    public static bool UnlockRotation = false;
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

//Rhys Wareham