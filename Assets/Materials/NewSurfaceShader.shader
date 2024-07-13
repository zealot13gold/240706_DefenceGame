Shader "Custom/UnitMaterial"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        // 윤곽선
        _Outline_Bold("Outline Bold", Range(0, 1)) = 0
        _Outline_Color("Outline Color", Color)=(0, 1, 0, 1)
    }
    SubShader
    {
       Tags { "RenderType" = "Opaque"}                    // 서브쉐이더 동작방식 설정("키":"값")
			// Opaque : 대부분의 셰이더(노멀, 자체 조명, 반사, 터레인 셰이더)
            LOD 200

			cull front    //! 1Pass는 앞면을 그리지 않는다.
			// cull : 폴리곤에서 컬링할(드로우하지 않을) 면을 제어
			// front : 보는 사람과 같은 방향을 향하는 폴리곤을 렌더링하지 않음

			Pass
			{
				CGPROGRAM
				#pragma vertex _VertexFuc			// vertex 함수를 "_VertexFunc"로 지칭
				#pragma fragment _FragmentFuc
				#include "UnityCG.cginc"            // 유니티에서 렌더링에 필요한 기능 제공
				// include 기존 항목 아래에 추가
				//#include "AutoLight.cginc"

				struct ST_VertexInput    //! 버텍스 쉐이더 Input(버텍스 정보)
				{
					float4 vertex : POSITION;       // 버텍스 좌표
					float3 normal : NORMAL;         // 노말 벡터
				};

				struct ST_VertexOutput    //! 버텍스 쉐이더 Output
				{
					float4 vertex : SV_POSITION;        // 버텍스 좌표 -> fragment shader로 전달
				};

				float _Outline_Bold;
                fixed4 _Outline_Color;

				ST_VertexOutput _VertexFuc(ST_VertexInput stInput)   // 버텍스 쉐이더 내에서 버텍스 좌표를 노말방향으로 더해줘 크기를 조잘할 수 있는 코드 작성        // 계산 결과를 보냄
				{
					ST_VertexOutput stOutput;

					float3 fNormalized_Normal = normalize(stInput.normal);        //! 로컬 노말 벡터를 정규화 시킴
					float3 fOutline_Position = stInput.vertex + fNormalized_Normal * (_Outline_Bold * 0.1f); //! 버텍스 좌표에 노말 방향으로 더한다.

					stOutput.vertex = UnityObjectToClipPos(fOutline_Position);    //! 노말 방향으로 더해진 버텍스 좌표를 카메라 클립 공간으로 변환 
					return stOutput;
				}


				float4 _FragmentFuc(ST_VertexOutput i) : SV_Target                // 계산 결과로 점에 대한 컬러 지정, 외곽선
				{
					return _Outline_Color;
				}

				ENDCG
			}

        cull back

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
