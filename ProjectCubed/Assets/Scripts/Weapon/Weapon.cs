using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponStateMachine StateMachine { get; private set; }
    public WeaponIdleState IdleState { get; private set; }
    public WeaponShootState ShootState { get; private set; }
    public WeaponJamState JamState { get; private set; }

    [SerializeField]
    private WeaponData weaponData;


    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }

    public Transform WeaponPivotPoint { get; private set; }

    public Transform WeaponFirepoint { get; private set; }

    public SpriteRenderer WeaponSprite { get; private set; }

    [SerializeField]
    public GameObject Bullet;

    private bool weaponRightFacing;

    public bool canShoot;

    private void Awake()
    {
        StateMachine = new WeaponStateMachine();

        IdleState = new WeaponIdleState(this, StateMachine, weaponData, "idle");
        ShootState = new WeaponShootState(this, StateMachine, weaponData, "hasShot");
        JamState = new WeaponJamState(this, StateMachine, weaponData, "jammed");
    
    }


    // Start is called before the first frame update
    void Start()
    {
        //Get inputHandler from Player object
        InputHandler = transform.parent.GetComponentInChildren<PlayerInputHandler>();
       

        //Set the firepoint transform to the transform of this object's child
        WeaponPivotPoint = transform.Find("RealPivotPoint");
        WeaponFirepoint = WeaponPivotPoint.transform.Find("Weapon");

        //Retrieve the SpriteRenderer and Animator for the weapon before WeaponFirepoint changes to the next child
        WeaponSprite = WeaponFirepoint.GetComponent<SpriteRenderer>();
        Anim = WeaponFirepoint.GetComponent<Animator>();
        WeaponFirepoint = WeaponFirepoint.transform.Find("FirePoint");

        //Correct the firepoint position
        AdjustFirepoint();
        

        //Start weapon in IdleState
        StateMachine.Initialise(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void CheckWeaponPlacement()
    {
        float weaponAngle;
        weaponAngle = WeaponPivotPoint.transform.localEulerAngles.z;
        

        //If angle is less than 0, and pointing to the right of the player, and the weapon is not facing right
        if (((weaponAngle < 0 && weaponAngle >= -180) || weaponAngle > 180) && !weaponRightFacing)
        {
            //Set flipY to false
            WeaponSprite.flipY = false;
            AdjustFirepoint();
            weaponRightFacing = true;
        }
        //If angle is more than 0, meaning it is pointing to the left, and the weapon is facing right
        else if (((weaponAngle >= 0 && weaponAngle < 180) || weaponAngle < -180) && weaponRightFacing)
        {
            //Set flipY to true
            WeaponSprite.flipY = true;
            AdjustFirepoint();
            weaponRightFacing = false;
        }

        //If the weapon is above the waist height, set the sorting order to be 0
        //Behind the player
        if(weaponAngle < 90 || weaponAngle > -90)
        {
            WeaponSprite.sortingOrder = 0;
        }
        //Set sorting order to in front of player, when below waist height
        else
        {
            WeaponSprite.sortingOrder = 3;
        }
    }

    /// <summary>
    /// Function which adjusts the position of the firepoint when the weapon has been flipped
    /// </summary>
    public void AdjustFirepoint()
    {
        //Must adjust the localPosition, not global position
        WeaponFirepoint.localPosition = new Vector3(WeaponFirepoint.localPosition.x,
                                                    WeaponFirepoint.localPosition.y * -1,
                                                    WeaponFirepoint.localPosition.z);
    }

    public void SpawnBullet()
    {
        //Create a new bullet using the prefab, firepoint position and the rotation of the firepoint
        GameObject newBullet = Instantiate(Bullet, WeaponFirepoint.position, WeaponFirepoint.rotation);
        //Get the rigidbody from the new bullet
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        //Add an impulse force to the rigidbody, heading up from the position & rotation of the firepoint
        rb.AddForce(WeaponFirepoint.up * weaponData.bulletForce, ForceMode2D.Impulse);
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
}

