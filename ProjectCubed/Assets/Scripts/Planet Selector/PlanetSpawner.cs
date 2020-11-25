using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private GameObject startPlanet;
    private GameObject lastPlanet;
    private Vector3 nextPlanetLocation;

    // Start is called before the first frame update
    void Start()
    {
        if (lastPlanet == null)
            lastPlanet = startPlanet;
        nextPlanetLocation = lastPlanet.transform.position + new Vector3(-3, 0, 3);
        StartCoroutine(SpawnPlanetWithDelay());
    }

    void SpawnPlanets()
    {
        print("spawn planets");
        for (int i = 0; i < 3; i++)
        {
            print(i);
            Instantiate(planetPrefab, (nextPlanetLocation + new Vector3(i * 3, 0, 0)), Quaternion.identity);
        }
    }

    IEnumerator SpawnPlanetWithDelay()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        for(int i = 0; i < 3; i++)
        {
            yield return wait;
            Instantiate(planetPrefab, (nextPlanetLocation + new Vector3(i * 3, 0, 0)), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
