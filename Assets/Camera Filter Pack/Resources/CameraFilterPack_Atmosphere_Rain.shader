// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

///////////////////////////////////////////
//  CameraFilterPack - by VETASOFT 2016 ///
///////////////////////////////////////////


Shader "CameraFilterPack/Atmosphere_Rain" { 
Properties 
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_TimeX ("Time", Range(0.0, 1.0)) = 1.0
_ScreenResolution ("_ScreenResolution", Vector) = (0.,0.,0.,0.)
}
SubShader
{
Pass
{
ZTest Always
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma target 3.0
#pragma glsl
#include "UnityCG.cginc"
uniform sampler2D _MainTex;
uniform sampler2D Texture2;
uniform float _TimeX;
uniform float _Value;
uniform float _Value2;
uniform float _Value3;
uniform float _Value4;
uniform float _Value5;
uniform float _Value6;
uniform float _Value7;
uniform float4 _ScreenResolution;
uniform float2 _MainTex_TexelSize;
struct appdata_t
{
float4 vertex   : POSITION;
float4 color    : COLOR;
float2 texcoord : TEXCOORD0;

};
struct v2f
{
half2 texcoord  : TEXCOORD0;
float4 vertex   : SV_POSITION;
fixed4 color    : COLOR;
};
v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}

float4 frag (v2f i) : COLOR
{
	float2 uv = i.texcoord.xy;
	#if UNITY_UV_STARTS_AT_TOP
	if (_MainTex_TexelSize.y < 0)
	uv.y = 1-uv.y;
	#endif
	float2 uv3 = uv;
	
	float2 uv2 = uv*_Value5;
	_TimeX*=_Value4;
	uv2.x+=_Value3*uv2.y;

	uv2*=3;
	uv2.x+=0.1;
	uv2.y+=_TimeX*0.8;
	float4 txt =tex2D(Texture2, uv2).r*0.3*_Value2;

	uv2*=0.65;
	uv2.x+=0.1;
	uv2.y+=_TimeX;
	txt+=tex2D(Texture2, uv2).r*0.5*_Value2;

	uv2*=0.65;
	uv2.x+=0.1;
	uv2.y+=_TimeX*1.2;
	txt+=tex2D(Texture2, uv2).r*0.7*_Value2;

	uv2*=0.5;
	uv2.x+=0.1;
	uv2.y+=_TimeX*1.2;
	txt+=tex2D(Texture2, uv2).r*0.9*_Value2;
		
	uv2*=0.4;
	uv2.x+=0.1;
	uv2.y+=_TimeX*1.2;
	txt+=tex2D(Texture2, uv2).r*0.9*_Value2;
	
	uv = i.texcoord.xy;

	uv+=float2(txt.r*_Value6,txt.r*_Value6);
	float4 nt=tex2D(_MainTex, uv)+txt;
	float4 old=tex2D(_MainTex, i.texcoord.xy);
	txt=lerp(old,nt,_Value);

	uv = i.texcoord.xy*0.001;
	uv.x+=_TimeX*0.4;
	uv.y=0;
	nt=lerp(txt,txt+tex2D(Texture2, uv).g*0.9*_Value2,_Value7);
	
	txt=lerp(old,nt,_Value);
	
	return  txt;
}
ENDCG
}
}
}