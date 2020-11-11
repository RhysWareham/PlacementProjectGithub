using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInteractState : PlayerAbilityState
{
    public PlanetInteractState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }


    public override void Enter()
    {
        base.Enter();


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void RotatePlanet(Collider2D collision)
    {

        //If the planet is not mid turning
        if (player.midTurning == false && player.rotationTriggerEntered)
        {
            //Set midTurning to true
            player.midTurning = true;

            int leftRightDirection = 0;
            int upDownDirection = 0;

            //Switch statement to only go through the correct type of shape
            switch (ShapeInfo.chosenShape)
            {
                //If the shape is a CUBE...
                case ShapeInfo.ShapeType.CUBE:
                    {
                        //Check if the collision is a left/right wall
                        if (collision.gameObject.GetComponent("LeftRightWall") != null)
                        {
                            if (collision.gameObject.transform.position.x < player.transform.position.x)
                            {
                                leftRightDirection = -1;
                            }
                            else
                            {
                                leftRightDirection = 1;
                            }
                        }
                        else if (collision.gameObject.GetComponent("UpDownWall") != null)
                        {
                            if (collision.gameObject.transform.position.y < player.transform.position.y)
                            {
                                upDownDirection = 1;
                            }
                            else
                            {
                                upDownDirection = -1;
                            }
                        }

                        //RotateShape(leftRightDirection, upDownDirection);
                        player.StartCoroutine(player.Rotate(leftRightDirection, upDownDirection));

                        break;
                    }
            }
            Debug.Log("turning");
            GameManagement.faceCorrectionComplete = false;
            GameManagement.shapeTurnPhase = true;
            GameManagement.shapeTurning = true;
        }
    }

}
