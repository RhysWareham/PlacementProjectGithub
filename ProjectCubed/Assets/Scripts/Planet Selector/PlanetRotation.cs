using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField] private Vector3 RotateAmount;
    [SerializeField] private Color startColour;
    [SerializeField] private Color hoverColour;

    private bool mouseOver;
    private Ray ray;
    private RaycastHit hit;
    private GameObject currentHit;

    private PlayerInputHandler InputHandler;

    // Start is called before the first frame update
    void Start()
    {
        InputHandler = GameObject.FindGameObjectWithTag("InputHandler").GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(RotateAmount * Time.deltaTime);

        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        print(Mouse.current.position.ReadValue());

        Color startColor = Color.white;

        if (Physics.Raycast(ray, out hit))
        {
            currentHit = hit.collider.gameObject;
            if (currentHit.GetComponent<Collider>().tag.Equals("AvailablePlanet"))
            {
                mouseOver = true;
                currentHit.GetComponent<Renderer>().material.color = hoverColour;
            }
            else
            {
                mouseOver = false;
            }
        }


        if (!mouseOver && currentHit!= null)
        {
            if (currentHit.tag.Equals("AvailablePlanet"))
            {
                currentHit.GetComponent<Renderer>().material.color = startColor;
                currentHit = null;
            }
        }
    }
}
