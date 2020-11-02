using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnToBeat : MonoBehaviour
{
    [SerializeField] GameObject CubePrefab;

    // Calculations
    [SerializeField] private float BPM = 120;
    [SerializeField] private float shotsPerSecond;
    [SerializeField] AudioSource song;

    [SerializeField] bool canShoot = false;
    [SerializeField] float timer = 0;
    [SerializeField] float minTimer = 0;
    [SerializeField] int timerTest = 0;


    [SerializeField] float check;

    // Start is called before the first frame update
    void Start()
    {
        shotsPerSecond = 1 / (BPM / 60);
        print(shotsPerSecond);

        timer = 0;
        minTimer = 0;
        timerTest = 0;
        song.Play();

    }

    //IEnumerator ExecuteAfterTime(float time)
    //{
    //    yield return new WaitForSeconds(time);

    //    canShoot = false;
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        shotsPerSecond = 1 / (BPM / 60);
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        timer = Mathf.Round(timer * 100f) / 100f;
        minTimer = Mathf.Round(minTimer * 100f) / 100f;
        shotsPerSecond = Mathf.Round(shotsPerSecond * 100f) / 100f;

        check = timer % shotsPerSecond;

        if (Mathf.Approximately(timer % shotsPerSecond, 0.0f) || Mathf.Approximately(timer % shotsPerSecond, 0.05f))
        {
            canShoot = true;

            timerTest++;
        }
        //else
        //{
        //    StartCoroutine(ExecuteAfterTime(0.01f));
        //}

        if(minTimer >= 60)
        {
            minTimer = 0;
            print("BMP is set to: " + BPM + ".\nCalculated BPM is: " + timerTest);
            timerTest = 0;
        }
        

        if (Input.GetMouseButtonDown(0) && canShoot == true)
        {
            canShoot = false;
            Vector3 worldPos;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1000f))
            {
                worldPos = hit.point;
            }
            else
            {
                worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            }
            Instantiate(CubePrefab, worldPos, Quaternion.identity);
            print("Instantiating");
        }

        timer += Time.deltaTime;
        minTimer += Time.deltaTime;
    }
}
