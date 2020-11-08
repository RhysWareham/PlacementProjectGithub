using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShapeInfo
{
    public enum ShapeType
    {
        CUBE                    =   0,
        THREE_SIDED_PYRAMID     =   1,


    }

    public static int[] numOfFaces = { 6, 4 };
    public static int[] numOfEdgesPerFace = { 4, 3 };
    public static float[] anglesBtwFaces = { 90, 60 };

    public static ShapeType chosenShape = 0;
    public static float rotateSpeed = 4f;
    public static bool planetRotationCompleted = true;


}
