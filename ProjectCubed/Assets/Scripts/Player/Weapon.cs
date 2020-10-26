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
        Anim = GetComponent<Animator>();
        InputHandler = transform.parent.GetComponentInChildren<PlayerInputHandler>();


        //Set the firepoint transform to the transform of this object's child
        WeaponPivotPoint = GetComponentInChildren<Transform>();
        WeaponPivotPoint = transform.Find("RealPivotPoint");
        

        //Start weapon in IdleState
        StateMachine.Initialise(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }


}

