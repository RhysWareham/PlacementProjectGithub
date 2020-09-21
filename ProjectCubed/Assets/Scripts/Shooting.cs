using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float bulletForce = 10f;


    // Update is called once per frame
    void Update()
    {
        //Fire1 is set to mouse button 1 by default in Unity
        if(Input.GetButtonDown("Fire1"))
        {
            SpawnBullet();
        }
    }

    private void SpawnBullet()
    {
        //Create a new bullet using the prefab, firepoint position and the rotation of the firepoint
        GameObject newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        //Get the rigidbody from the new bullet
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        //Add an impulse force to the rigidbody, heading up from the position & rotation of the firepoint
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
