using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField] private Vector3 RotateAmount;
    [SerializeField] private Color startColour;
    [SerializeField] private Color hoverColour;

    private Ray ray;
    private RaycastHit hit;
    private GameObject currentHit;
    private GameObject lastHit;

    private PlayerInputHandler InputHandler;

    // Start is called before the first frame update
    void Start()
    {
        InputHandler = GameObject.FindGameObjectWithTag("InputHandler").GetComponent<PlayerInputHandler>();
        this.GetComponent<Renderer>().material.color = startColour;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(RotateAmount * Time.deltaTime);

        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            currentHit = hit.collider.gameObject;
            if (currentHit.GetComponent<Collider>().tag.Equals("AvailablePlanet"))
            {
                //print("start");
                currentHit.GetComponent<Renderer>().material.color = hoverColour;
                lastHit = currentHit;
            }
            else
            {
                if(lastHit != null)
                {
                    if (lastHit.GetComponent<Collider>().tag.Equals("AvailablePlanet"))
                    {
                        //print("change colour");
                        lastHit.GetComponent<Renderer>().material.color = startColour;
                    }
                }

            }
        }

    }
}
