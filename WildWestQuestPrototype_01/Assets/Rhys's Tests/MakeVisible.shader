Shader "Custom/MakeVisible"
{
   SubShader
   {
        Stencil
        {
            Ref 1
            Comp Equal
        }
   }
}
