using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Material Skybox;
    public float SkyboxRotationY = 0.0f;
    public float SkyboxRotationX = 0.0f;
    public float SkyboxRotationZ = 0.0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_RotationX", SkyboxRotationX * Time.time);
        RenderSettings.skybox.SetFloat("_RotationY", SkyboxRotationY * Time.time);
        RenderSettings.skybox.SetFloat("_RotationZ", SkyboxRotationZ * Time.time);
    }
}

// Link to skybox generation =  https://wwwtyro.github.io/space-3d/