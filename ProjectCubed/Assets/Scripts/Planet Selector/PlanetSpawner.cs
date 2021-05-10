using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shopPrefab;
    [SerializeField] private GameObject[] planetPrefabs;
    [SerializeField] private bool[] availablePlanetPrefabs;

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

    IEnumerator SpawnPlanetWithDelay()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        for(int i = 0; i < 3; i++)
        {
            PickPlanet();
            yield return wait;
            Instantiate(planetPrefabs[GameManagement.pickedPlanet], (nextPlanetLocation + new Vector3(i * 3, 0, 0)), Quaternion.identity);
        }
    }

    void PickPlanet()
    {
        if (Random.value < 0.33 && availablePlanetPrefabs[5] == true)
            GameManagement.pickedPlanet = 5;
        else
        {
            GameManagement.pickedPlanet = Random.Range(0, 5);
            while (availablePlanetPrefabs[GameManagement.pickedPlanet] == false || GameManagement.pickedPlanet == -1)
            {
                print(GameManagement.pickedPlanet + " is already picked! ");
                GameManagement.pickedPlanet = Random.Range(0, 4);
            }
        }
        availablePlanetPrefabs[GameManagement.pickedPlanet] = false;
        print("Picked Planet = " + GameManagement.pickedPlanet);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
