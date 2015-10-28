Shader "Custom/Metaball" 
{
	Properties 
	{    
		_MainTex ("Texture", 2D) = "white" { }    
	}

	SubShader 
	{
		Tags { "Queue" = "Transparent" }

		Pass 
		{
			Blend SrcAlpha OneMinusSrcAlpha     
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag	
			#include "UnityCG.cginc"	
			float4 _Color;
			sampler2D _MainTex;

			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};	

			float4 _MainTex_ST;	

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}	
	
			// Here goes the metaball magic
			float COLOR_TRESHHOLD=0.2; //To separate and process each color.		
			half4 frag (v2f i) : COLOR
			{		
				half4 texcol = tex2D (_MainTex, i.uv);
				half4 finalColor = texcol;

				float threshold = 0.3f;
				if(texcol.r > threshold || texcol.g > threshold || texcol.b > threshold)
				{
					finalColor = floor(finalColor * 6);
					finalColor.a = 0.5f;
				}

				if (texcol.b > threshold && texcol.b < threshold + 0.1f)
				{
					finalColor.a = 0.2f;
				}

				return finalColor;
			} 
			ENDCG
		}
	}
	Fallback "VertexLit"
} 