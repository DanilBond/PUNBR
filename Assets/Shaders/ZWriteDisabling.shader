Shader "Custom/ZWriteDisabling"
{
	SubShader{
		Tags{"RenderType" = "Opaque"}
		Pass{
			ZWrite Off
		}
	}
}