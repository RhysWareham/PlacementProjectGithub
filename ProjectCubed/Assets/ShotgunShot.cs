using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShot : MonoBehaviour
{


    float timer = 0;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {

        startTime = Time.time;
    }

    private void Update()
    {
        //Timer to destroy the shotgunShot holder after bullets are gone
        if (timer < startTime + 0.5f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
