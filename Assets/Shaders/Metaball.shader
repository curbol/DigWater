Shader "Custom/Metaball"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" { }
	}

	SubShader
	{
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag
			#include "UnityCG.cginc"	
			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct vertexInput
			{
				float4  vertex : SV_POSITION;
				float2  texcoord0 : TEXCOORD0;
			};
			
			vertexInput vert (appdata_base v)
			{
				vertexInput o;
				o.vertex = mul (UNITY_MATRIX_MVP, v.vertex);
				o.texcoord0 = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}

			half4 frag (vertexInput i) : COLOR
			{
				half4 color = tex2D(_MainTex, i.texcoord0);
				float threshold = 0.3f;

				if (color.r > threshold || color.g > threshold || color.b > threshold)
				{
					float majority = max(max(color.r, color.g), color.b);
					float multiplier = 1 / majority;
					color *= multiplier;
					color.a = 0.6f;
				}
			
				return color;
			} 
			ENDCG
		}
	}
	Fallback "VertexLit"
}