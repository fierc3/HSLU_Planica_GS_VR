Shader "Custom/DepthOnlyOccluder"
{
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent-10" }

        Pass
        {
            ZWrite On
            ColorMask 0 // don’t draw color
        }
    }
}
