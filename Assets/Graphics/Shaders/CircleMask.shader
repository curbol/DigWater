Shader "Custom/CircleMask"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" { }
		_Mask("Mask Texture", 2D) = "white" { }
	}

	SubShader
	{
		Lighting On
		ZWrite off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			SetTexture [_Mask] {combine texture}
			SetTexture [_MainTex] {combine texture, previous}
		}
	}
}