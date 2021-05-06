using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class PublicFunctions
{
    /// <summary>
    /// Function to unwrap each axis at the same time, without needing to call the function for each individual axis
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public static Vector3 UnwrapAngles(Vector3 vec3)
    {
        return new Vector3(UnwrapAngle(vec3.x), UnwrapAngle(vec3.y), UnwrapAngle(vec3.z));
    }

    /// <summary>
    /// This function sets any angle which is negative to its positive alternate angle. I.E. -90 will become 270
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }

    /// <summary>
    /// Function to return the distance between two points
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <returns></returns>
    public static float ReturnDistance(Vector2 pos1, Vector2 pos2)
    {
        //A^2 + B^2 = C^2
        float h = Mathf.Sqrt(((pos1.x - pos2.x) * (pos1.x - pos2.x)) +
            ((pos1.y - pos2.y) * (pos1.y - pos2.y)));

        //Return the distance between pos1 and pos2
        return h;
    }
}
