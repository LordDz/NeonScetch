// Copyright 2014-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.


Shader "Image Effects/Camera Glitch" 
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Pass
        {
            ZTest Always
            Cull Off
            ZWrite Off
            Fog { Mode off }

            CGPROGRAM

            #include "UnityCG.cginc"
            #include "DigitalGlitchesCG.cginc"

            #pragma vertex ComputeVertex
            #pragma fragment ComputeFragment

            uniform sampler2D _MainTex;
            uniform float _InverseUV; 

            struct vertexOutput
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            vertexOutput ComputeVertex(appdata_img v)
            {
                vertexOutput o;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord.xy;

                #if UNITY_UV_STARTS_AT_TOP
		        if (_InverseUV > 0.0)
			        o.uv.y = 1.0 - o.uv.y;
		        #endif	

                return o;
            }

            fixed4 ComputeFragment(vertexOutput o) : COLOR
            {
                fixed4 mainColor = tex2D(_MainTex, o.uv.xy);
                return ApplyGlitchEffect(mainColor, _MainTex, o.uv.xy);
            }

            ENDCG
        }
    }

    Fallback off
}
