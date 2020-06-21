Shader "Custom/InvisibleMask"
{
    SubShader
    {
        //Draw before all geometry objects (queue = 2001)
        Tags { "Queue" = "Geometry-1" }
        //Dont write to any colour channels
        ColorMask 0
        //Don't write to deptch buffer
        ZWrite Off

        Stencil
        {
            Ref 1
            Comp Always
            Pass Replace

        }
    }
}


//Shader "Custom/InvisibleMask" {
//    SubShader{
//        // draw after all opaque objects (queue = 2001):
//        Tags { "Queue" = "Geometry+1" }
//        Pass {
//          Blend Zero One // keep the image behind it
//        }
//    }
//}

//Rhys Wareham