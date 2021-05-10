using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region PlayerStateMachine
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDodgeState DodgeState { get; private set; }
    public PlayerOnLandState OnLandState { get; private set; }
    public PlanetInteractState InteractState { get; private set; }

    #endregion

    #region PlayerComponents
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }



    [SerializeField]
    private SpriteRenderer legs;
    [SerializeField]
    private SpriteRenderer[] angledBodyHeadArray;
    public int numOfBodyAngles;
    private bool playerVisible = true;

    [SerializeField]
    private SpriteRenderer weaponSprite;


    #endregion

    #region UIStuff

    [SerializeField]
    public List<Image> heartList;
    public GameObject heartPrefab;
    public Transform firstHeartPlace;
    public HeartManager heartManager;

    #endregion

    #region PlanetRotation

    public Transform Planet { get; private set; }
    public bool midTurning;
    public bool rotationTriggerEntered;

    private GameObject spawnPointHolder;
    private List<Transform> spawnPoints = new List<Transform>();
    private List<Transform> xRightPlayerSpawns = new List<Transform>();
    private List<Transform> xLeftPlayerSpawns = new List<Transform>();
    private List<Transform> yTopPlayerSpawns = new List<Transform>();
    private List<Transform> yBottomPlayerSpawns = new List<Transform>();
    #endregion

    #region PlayerVariables
    public Vector2 CurrentVelocity { get; private set; }

    public bool alreadyMoving;

    [SerializeField]
    public PlayerData playerData;


    public int FacingRight = 1; //-1 - Left, 1 - Right

    private Vector2 workspace;

    #endregion

    public float spriteBlinkingTimer = 0.0f;
    public float spriteBlinkingMiniDuration = 0.1f;
    public float spriteBlinkingTotalTimer = 0.0f;
    public float spriteBlinkingTotalDuration = 1.0f;
    public bool startBlinking = false;

    public Transform currentPos;
    public int preVerticalRotation = 0;
    public int preHorizontalRotation = 0;

    private float planetRotateTimer;
    private float planetRotateTimerMax = 5f;
    private float planetRotateStartTime;
    private bool planetTimerStarted = false;

    //Whenever the game starts, we will have a state machine created for the player
    private void Awake()
    {
        //Initialise
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        DodgeState = new PlayerDodgeState(this, StateMachine, playerData, "inDodge");
        OnLandState = new PlayerOnLandState(this, StateMachine, playerData, "onLand");
        InteractState = new PlanetInteractState(this, StateMachine, playerData, "idle");

        GameManagement.playerAlive = true;
        //Set up Anim and bodySprite arrays
        //Anim = new Animator[angledBodyHeadArray.Length];
        //angledBodyHeadArray = new SpriteRenderer[angledLegsArray.Length];
    }

    private void Start()
    {
        spawnPointHolder = GameObject.FindGameObjectWithTag("SpawnPointHolder");
        GetSpawnPoints();
        //Changed this from GetComponent<Animator>();
        //Anim = transform.Find("Legs").GetComponent<Animator>();
        Anim = legs.gameObject.GetComponent<Animator>();
        numOfBodyAngles = 10;

        //Set the initial angle for the player to be facing right
        GameManagement.currentHeadBodyAngle = 0;
        GameManagement.previousHeadBodyAngle = 0;

        SetBodyAngleActive();

        //Ensure the correct angled sprites are enables
        angledBodyHeadArray[GameManagement.currentHeadBodyAngle].enabled = true;

        //Anim = transform.Find("Legs7").GetComponent<Animator>(); //Legs7 is Right direction
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        Planet = GameObject.Find("PlanetHolder").transform.Find("Planet");

        //Set max health
        playerData.maxHealth = playerData.startingHealth;
        playerData.attackDamage = playerData.startingDamage;

        //SpriteRenderers for the blinking effect
        //legs = transform.Find("Legs").GetComponent<SpriteRenderer>();
        //body = legs.transform.Find("Body").GetComponent<SpriteRenderer>();
        //head = legs.transform.Find("Head").GetComponent<SpriteRenderer>();

        //Angle direction 7

        //legs7 = Anim.gameObject.GetComponent<SpriteRenderer>();
        //headBody7 = Anim.gameObject.transform.Find("headBody7").GetComponent<SpriteRenderer>();

        //Make if or for loop or something to idk what im doing now. ive drawn a blank

        //currentLegs = 1;//angledLegsArray[];

        //Set the current health to be starting health
        playerData.currentHealth = playerData.startingHealth;

        heartManager = GameObject.Find("Canvas").transform.Find("LifeHearts").GetComponent<HeartManager>();

        weaponSprite = GameObject.Find("Guitar").GetComponent<SpriteRenderer>();
        //Initialise statemachine
        StateMachine.Initialise(IdleState);

        alreadyMoving = false;
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        if (startBlinking)
        {
            SpriteBlinkingEffect();
        }

        //if(GameManagement.planetRotating == true && !planetTimerStarted) 
        //{
        //    planetRotateStartTime = Time.time;
        //    planetTimerStarted = true;
        //    GameManagement.planetRotating = false;
        //    GameManagement.zoomOut = true;
        //}
        //if (planetTimerStarted)
        //{
        //    //If timer isn't over yet
        //    if (Time.time < (planetRotateStartTime + planetRotateTimerMax))
        //    {
        //        planetRotateTimer += Time.deltaTime;
        //    }
        //    else
        //    {
        //        //If rotationtimer is over
        //        GameManagement.playerTurnCollidersOn = true;
        //        planetRotateTimer = 0f;
        //    }
        //    if (GameManagement.playerTurnCollidersOn == true && GameManagement.playerCollidersOn == false)
        //    {
        //        planetTimerStarted = false;
        //        //Find nearest spawn point on other side of face
        //        Transform respawnPoint = GetClosestSpawnPoint(preVerticalRotation, preHorizontalRotation);
        //        //Set new position
        //        this.transform.position = respawnPoint.position;
        //        //transform.parent.position = respawnPoint.position;



        //    }
        //}



        StateMachine.CurrentState.LogicUpdate();
    }

    public void RepositionPlayer()
    {
        //Find nearest spawn point on other side of face
        Transform respawnPoint = GetClosestSpawnPoint(preVerticalRotation, preHorizontalRotation);
        //Set new position
        this.transform.position = respawnPoint.position;
        
        preVerticalRotation = 0;
        preHorizontalRotation = 0;
    }

    public void TurnPlayerOn()
    {
        //Set player transparency back to 1, so visible
        ChangePlayerTransparency(1f);
        //Turn player colliders back on
        SetPlayerCollidersActive(true);

    }

    public void TurnPlayerOff()
    {
        //Set player transparency to 0, to make player and weapon invisible
        ChangePlayerTransparency(0f);
        //Turn colliders off
        SetPlayerCollidersActive(false);
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }


    public void SetVelocityY(float velY)
    {
        workspace.Set(CurrentVelocity.x, velY);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityXY(float velX, float velY)
    {
        workspace.Set(velX, velY);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityWithDirection(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    /// <summary>
    /// Function to check if the player sprite is facing the correct direction
    /// </summary>
    /// <param name="xInput"></param>
    public void CheckToFlipLegs(float xInput)
    {
        //If moving right, and the player is not facing right, call the Flip function
        if ((xInput > 0 && FacingRight < 0) ||
                  xInput < 0 && FacingRight > 0)
        {
            Flip();
        }

    }

    public void Flip()
    {

        if (legs.flipX == false)
        {
            legs.flipX = true;

        }
        else
        {
            legs.flipX = false;
        }

        FacingRight *= -1;
        //transform.Rotate(0.0f, 180.0f, 0.0f);
    }


    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.tag == "EnemyProjectile")
    //    {
    //        TakeDamage(1);
    //    }
    //}

    /// <summary>
    /// Function to check if player is inside 
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerStay2D(Collider2D collision)
    {
        //If player has clicked Interact, and no enemies are alive after enemy spawning is finished
        if ((InputHandler.InteractInput && GameManagement.enemiesLeftAliveOnFace <= 0 && GameManagement.enemySpawningComplete && GameManagement.PlanetCanRotate)
            || (GameManagement.UnlockRotation && InputHandler.InteractInput))
        {
            //If player has collided with the boundaries
            if (collision.gameObject.layer == 9)
            {
                rotationTriggerEntered = true;

                //call the rotation function in PlayerInteractState
                InteractState.RotatePlanet(collision);
            }
        }
        else
        {
            InputHandler.InteractInput = false;
        }
    }


    /// <summary>
    /// Function which slowly rotates the planet to the next chosen face
    /// </summary>
    /// <param name="verticalRotation"></param>
    /// <param name="horizontalRotation"></param>
    /// <returns></returns>
    public IEnumerator RotatePlanet(int verticalRotation, int horizontalRotation)
    {
        preHorizontalRotation = horizontalRotation;
        preVerticalRotation = verticalRotation;

        currentPos = transform;
        //GameManagement.planetRotating = true;

        //Turn player sprite off and colliders off
        TurnPlayerOff();
        GameManagement.zoomOut = true;

        
        float angle = ShapeInfo.anglesBtwFaces[(int)ShapeInfo.chosenShape];
        Quaternion finalRotation = Quaternion.Euler(angle * horizontalRotation, angle * verticalRotation, 0) * Planet.transform.rotation;

        while (Planet.transform.rotation != finalRotation)
        {
            Planet.transform.rotation = Quaternion.Slerp(Planet.transform.rotation, finalRotation, Time.deltaTime * ShapeInfo.rotateSpeed);
            yield return 0;

        }


        //NEED TO CHECK IF THE SPAWN POINT IS COLLIDING WITH SOMETHING, IF NOT, CAN SPAWN PLAYER.
        //NEED TO CHECK WHICH SPAWN POINT IS CLOSEST TO PLAYER'S ORIGINAL POSITION

        ////If right
        //if (verticalRotation == 1)
        //{

        //    xLeftPlayerSpawns[0].position = new Vector3(xLeftPlayerSpawns[0].position.x, currentPos.position.y, currentPos.position.z);
        //    currentPos.position = xLeftPlayerSpawns[0].position;
        //}
        //else if (verticalRotation == -1)
        //{

        //    xRightPlayerSpawns[0].position = new Vector3(xRightPlayerSpawns[0].position.x, currentPos.position.y, currentPos.position.z);
        //    currentPos.position = xRightPlayerSpawns[0].position;
        //    currentPos.position = new Vector3(5.3f, currentPos.position.y, currentPos.position.z);
        //}
        //else if (horizontalRotation == 1)
        //{
        //    yTopPlayerSpawns[0].position = new Vector3(currentPos.position.x, yTopPlayerSpawns[0].position.y, currentPos.position.z);
        //    currentPos.position = yTopPlayerSpawns[0].position;
        //}
        //else if (horizontalRotation == -1)
        //{
        //    yBottomPlayerSpawns[0].position = new Vector3(currentPos.position.x, yBottomPlayerSpawns[0].position.y, currentPos.position.z);
        //    currentPos.position = yBottomPlayerSpawns[0].position;
        //}




        ///Fix player position, Need to put this in separate function
        //If going right or left
        //if (verticalRotation == 1 || verticalRotation == -1)
        //{
        //    //Set x axis multiplied by -1
        //    currentPos.position = new Vector3(sideFaceSpawnX * (verticalRotation*-1), currentPos.position.y, currentPos.position.z);
        //}
        ////If going up or down
        //if (horizontalRotation == 1 || horizontalRotation == -1)
        //{
        //    //Set y axis multiplied by -1
        //    currentPos.position = new Vector3(currentPos.position.x, currentPos.position.y * -1, currentPos.position.z);

        //}

        //Transform respawnPoint = GetClosestSpawnPoint(verticalRotation, horizontalRotation);
        
        ////Set new position
        //this.transform.position = respawnPoint.position;
        

        

        ShapeInfo.planetRotationCompleted = true;
        GameManagement.forwardFaceChecked = false;
        InputHandler.InteractInput = false;
        rotationTriggerEntered = false;
        midTurning = false;
    }

    public Transform GetClosestSpawnPoint(int verticalDirection, int horizontalDirection)
    {

        Transform closestSpawnPoint = xLeftPlayerSpawns[0];
        //If player has gone to right Face
        if(verticalDirection == 1)
        {
            //Move currentPos to the Left side of the Face
            currentPos.position = new Vector2(currentPos.position.x * -1, currentPos.position.y);    
            closestSpawnPoint = ReturnClosestPosition(xLeftPlayerSpawns, closestSpawnPoint);
        }
        else if(verticalDirection == -1)
        {
            currentPos.position = new Vector2(currentPos.position.x * -1, currentPos.position.y);
            closestSpawnPoint = ReturnClosestPosition(xRightPlayerSpawns, closestSpawnPoint);
        }
        if(horizontalDirection == 1)
        {
            currentPos.position = new Vector2(currentPos.position.x, currentPos.position.y * -1);
            closestSpawnPoint = ReturnClosestPosition(yBottomPlayerSpawns, closestSpawnPoint);
        }
        else if(horizontalDirection == -1)
        {
            currentPos.position = new Vector2(currentPos.position.x, currentPos.position.y * -1);
            closestSpawnPoint = ReturnClosestPosition(yTopPlayerSpawns, closestSpawnPoint);
        }

        Debug.Log(closestSpawnPoint);
        return closestSpawnPoint;
    }

    public Transform ReturnClosestPosition(List<Transform> listOfPositions, Transform closestPosition)
    {
        
        //Foreach spawnpoint in xLeftspawns
        foreach (Transform transform in listOfPositions)
        {
            //If the distance between this spawnpoint and player is less than previous spawn point and player...
            if (PublicFunctions.ReturnDistance(transform.position, currentPos.position) <
                PublicFunctions.ReturnDistance(closestPosition.position, currentPos.position))
            {
                if(transform.GetComponent<SpawnPoint>().canSpawn)
                {
                    //Set closest spawn point to transform
                    closestPosition = transform;

                }
            }
        }

        return closestPosition;
    }

    public void GetSpawnPoints()
    {
        //For each child inside spawnPointPrefab,
        foreach (Transform child in spawnPointHolder.transform)
        {
            //Add spawn point from spawnPoint prefab to list
            spawnPoints.Add(child);
        }
        for(int i = 0; i < 5; i++)
        {
            yTopPlayerSpawns.Add(spawnPoints[i]);
        }
        for(int i = 0; i < 5; i++)
        {
            yBottomPlayerSpawns.Add(spawnPoints[(spawnPoints.Count -1) - i]);
        }
        for(int i = 0; i < 5; i++)
        {
            xLeftPlayerSpawns.Add(spawnPoints[i * 5]);
        }
        for(int i = 0; i < 5; i++)
        {
            xRightPlayerSpawns.Add(spawnPoints[(i * 5) + 4]);
        }

        
    }
    

    public void TakeDamage(int damage)
    {
        if (!startBlinking)
        {
            //Subtract damage from the player's health
            playerData.currentHealth -= damage;
            heartManager.RemoveHeart();

            //Check if player is dead
            if (CheckDead())
            {
                //Player is dead
                KillPlayer();
            }
            else
            {
                StunPlayer();
            }
        }

    }



    public bool CheckDead()
    {
        if (playerData.currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void KillPlayer()
    {
        GameManagement.playerAlive = false;
        Destroy(transform.parent.gameObject);
    }

    public void StunPlayer()
    {
        //Do the flashy thing here and add a timer so player cannot be damaged for a couple seconds
        startBlinking = true;
    }

    private void SpriteBlinkingEffect()
    {
        //Increase the spriteBlinkingTotal timer
        spriteBlinkingTotalTimer += Time.deltaTime;

        //If the timer has reached its total duration...
        if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
        {
            //Set startBlinking to false
            startBlinking = false;

            //Set the timer back to 0
            spriteBlinkingTotalTimer = 0.0f;

            //Create a new temp variable to store the color info
            var tempColour = angledBodyHeadArray[0].color;
            //Set the alpha to 1, to be opague
            tempColour.a = 1f;

            //Set legs colour to temp colour
            legs.color = tempColour;

            //For loop going through all bodyHead sprites in array
            for (int i = 0; i < angledBodyHeadArray.Length; i++)
            {
                //Set current instance of bodyHead to tempColor.
                angledBodyHeadArray[i].color = tempColour;
            }

            //Set playerVisible to true
            playerVisible = true;

            return;
        }

        //Increase the spriteBlinking timer
        spriteBlinkingTimer += Time.deltaTime;
        //If the blinking timer has reached the interval times between turning on and off...
        if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
        {
            //Restart the timer
            spriteBlinkingTimer = 0.0f;

            //Create a new temp variable to store the color info
            var tempColour = angledBodyHeadArray[0].color;

            //If player is visible
            if (playerVisible)
            {
                //Set the alpha to 0, to be transparrent
                tempColour.a = 0f;

                //Set legs colour to temp colour
                legs.color = tempColour;

                //For loop going through all bodyHead sprites in array
                for (int i = 0; i < angledBodyHeadArray.Length; i++)
                {
                    //Set current instance of bodyHead to tempColor.
                    angledBodyHeadArray[i].color = tempColour;
                }

                //Set player visible to false
                playerVisible = false;
            }
            else
            {
                //Set the alpha to 1, to be opague
                tempColour.a = 1f;

                //Set legs colour to temp colour
                legs.color = tempColour;

                //For loop going through all bodyHead sprites in array
                for (int i = 0; i < angledBodyHeadArray.Length; i++)
                {
                    //Set current instance of bodyHead to tempColor.
                    angledBodyHeadArray[i].color = tempColour;
                }

                //Set player visible to true
                playerVisible = true;
            }
        }

    }


    public void SetBodyAngleActive()
    {
        //Disable all body sprites
        for (int i = 0; i < angledBodyHeadArray.Length; i++)
        {
            angledBodyHeadArray[i].enabled = false;
        }

        //Enable current body sprite
        angledBodyHeadArray[GameManagement.currentHeadBodyAngle].enabled = true;


    }


    public float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }

    /// <summary>
    /// Function to set the transparency of the player sprite
    /// </summary>
    /// <param name="alphaValue"></param>
    public void ChangePlayerTransparency(float alphaValue)
    {
        //Create a new temp variable to store the color info
        var tempColour = angledBodyHeadArray[0].color;
        //Set the alpha to alphaValue
        tempColour.a = alphaValue;

        //Set legs and weapon colour to temp colour
        legs.color = tempColour;
        weaponSprite.color = tempColour;

        //For loop going through all bodyHead sprites in array
        for (int i = 0; i < angledBodyHeadArray.Length; i++)
        {
            //Set current instance of bodyHead to tempColor.
            angledBodyHeadArray[i].color = tempColour;
        }
    }


    /// <summary>
    /// Function to set the player colliders active
    /// </summary>
    /// <param name="trueFalse"></param>
    public void SetPlayerCollidersActive(bool trueFalse)
    {
        List<CapsuleCollider2D> colliders = new List<CapsuleCollider2D>();
        CapsuleCollider2D[] colls = transform.GetComponents<CapsuleCollider2D>();
        CapsuleCollider2D[] collsLegs = legs.transform.GetComponents<CapsuleCollider2D>();

        //colliders.Add(GetComponent<CapsuleCollider2D>());
        //Foreach collider on player
        foreach (CapsuleCollider2D cols in colls)
        {
            //Turn on/off
            cols.enabled = trueFalse;
        }
        //For each collider on legs
        foreach(CapsuleCollider2D cols in collsLegs)
        {
            //Turn on/off
            cols.enabled = trueFalse;
        }

        //this.GetComponentInParent<CapsuleCollider2D>().enabled = trueFalse;
        if(trueFalse)
        {
            GameManagement.playerTurnCollidersOn = false;
        }
        
        GameManagement.playerCollidersOn = trueFalse;
    }
}