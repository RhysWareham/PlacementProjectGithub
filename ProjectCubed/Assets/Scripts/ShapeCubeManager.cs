using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeCubeManager : MonoBehaviour
{
    [SerializeField]
    private Transform faceA;
    [SerializeField]
    private Transform faceB;
    [SerializeField]
    private Transform faceC;
    [SerializeField]
    private Transform faceD;
    [SerializeField]
    private Transform faceE;
    [SerializeField]
    private Transform faceF;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ShapeInfo.chosenShape == ShapeInfo.ShapeType.CUBE)
        {

        }
    }
}
