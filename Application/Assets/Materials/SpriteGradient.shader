// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 //RadialGradientQuad.shader
 Shader "Custom/SpriteGradient" {

 Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
         _ColorA ("Color A", Color) = (1, 1, 1, 1)
         _ColorB ("Color B", Color) = (0, 0, 0, 1)
         _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
         _Slide ("Slide", Range(0, 1)) = 0.5
   
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
 
        _ColorMask ("Color Mask", Float) = 15

 
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
 
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
   
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
 
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
 
        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
 
            #pragma multi_compile __ UNITY_UI_ALPHACLIP
       
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };
       
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
 
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.worldPosition = IN.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
 
                OUT.texcoord = IN.texcoord;
           
                #ifdef UNITY_HALF_TEXEL_OFFSET
                OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
                #endif
           
                OUT.color = IN.color;
                return OUT;
            }
 
            sampler2D _MainTex;
            float4 _MainTex_TexelSize; //i added
            float4 _MainTex_ST; //i added
			fixed4 _ColorA, _ColorB;
             float _Slide;
			 float4 _Center;
 
            fixed4 frag(v2f IN) : SV_Target
            {
				float t = length(IN.texcoord - float2(0.5, 0.5) + _Center) * 1.41421356237; // 1.141... = sqrt(2)
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color * lerp(_ColorA, _ColorB, t + (_Slide - 0.5) * 2);
           
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
           
                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif
 
                return color;
            }
        ENDCG
        }
    }
}
    