using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shopPrefab;
    [SerializeField] private GameObject[] planetPrefabs;
    [SerializeField] private bool[] availablePlanetPrefabs;
    private int pickedPlanet = -1;

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
            Instantiate(planetPrefabs[pickedPlanet], (nextPlanetLocation + new Vector3(i * 3, 0, 0)), Quaternion.identity);
        }
    }

    void PickPlanet()
    {
        if (Random.value < 0.33 && availablePlanetPrefabs[5] == true)
            pickedPlanet = 5;
        else
        {
            pickedPlanet = Random.Range(0, 5);
            while (availablePlanetPrefabs[pickedPlanet] == false || pickedPlanet == -1)
            {
                print(pickedPlanet + " is already picked! ");
                pickedPlanet = Random.Range(0, 4);
            }
        }
        availablePlanetPrefabs[pickedPlanet] = false;
        print("Picked Planet = " + pickedPlanet);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
