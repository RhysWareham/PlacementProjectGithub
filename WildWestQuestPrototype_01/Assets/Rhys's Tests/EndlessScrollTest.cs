using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessScrollTest : MonoBehaviour
{

    [SerializeField]
    private float speed = 5;
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2((Time.time * speed)%1, 0);
    }
}

//Rhys Wareham