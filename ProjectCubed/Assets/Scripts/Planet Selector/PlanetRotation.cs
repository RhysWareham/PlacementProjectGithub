using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField] private Vector3 RotateAmount;
    [SerializeField] private string planetScene;
    //[SerializeField] private Color startColour;
    //[SerializeField] private Color hoverColour;

    private Ray ray;
    private RaycastHit hit;
    private GameObject currentHit;
    private GameObject lastHit;
    private bool hasClicked = false;

    private PlayerInputHandler InputHandler;

    void Start()
    {
        InputHandler = GameObject.FindGameObjectWithTag("InputHandler").GetComponent<PlayerInputHandler>();
        //this.GetComponent<Renderer>().material.color = startColour;
    }

    void Update()
    {
        MouseRaycast();
    }

    private void MouseRaycast()
    {
        this.transform.Rotate(RotateAmount * Time.deltaTime);

        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            currentHit = hit.collider.gameObject;
            print(currentHit.name);
            if (currentHit.GetComponent<Collider>().tag.Equals("AvailablePlanet"))
            {
                //print("start");
                //currentHit.GetComponent<Renderer>().material.color = hoverColour;
                lastHit = currentHit;
                
                if (Mouse.current.leftButton.isPressed && hasClicked == false)
                {
                    hasClicked = true;
                    LoadSelectedPlanet(currentHit);
                }
            }
            else
            {
                if (lastHit != null)
                {
                    if (lastHit.GetComponent<Collider>().tag.Equals("AvailablePlanet"))
                    {
                        //print("change colour");
                        //lastHit.GetComponent<Renderer>().material.color = startColour;
                    }
                }

            }
        }
    }

    private void LoadSelectedPlanet(GameObject selectedPlanet)
    {
        SceneManager.LoadScene(selectedPlanet.GetComponent<PlanetRotation>().planetScene); //Change this to level clicked.
        hasClicked = false;
        GameManagement.newPlanet = true;
    }
}


