// Upgrade NOTE: replaced 'SeperateSpecular' with 'SeparateSpecular'
// Upgrade NOTE: replaced 'glstate.matrix.modelview[0]' with 'UNITY_MATRIX_MV'
// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'glstate.matrix.projection' with 'UNITY_MATRIX_P'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader " Vertex Colored Outline" {

Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _SpecColor ("Spec Color", Color) = (1,1,1,1)
	_OutlineColor ("Outline Color", Color) = (0,0,0,1)
	_Outline ("Outline width", Range (.002, 0.03)) = .005
    _Emission ("Emmisive Color", Color) = (0,0,0,0)
    _Shininess ("Shininess", Range (0.01, 1)) = 0.7
    _MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
    Pass {
        Material {
            Shininess [_Shininess]
            Specular [_SpecColor]
            Emission [_Emission]   
        }
        ColorMaterial AmbientAndDiffuse
        Lighting On
        SeparateSpecular On
        SetTexture [_MainTex] {
            Combine texture * primary, texture * primary
        }
        SetTexture [_MainTex] {
            constantColor [_Color]
            Combine previous * constant DOUBLE, previous * constant
        }

    }
	Pass {
		Name "OUTLINE"
		Tags { "LightMode" = "Always" }
		
		CGPROGRAMV
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct appdata members vertex,normal)
#pragma exclude_renderers d3d11
		#pragma vertex vert

		struct appdata {
		    float4 vertex;
		    float3 normal;
		};

		struct v2f {
			float4 pos : POSITION;
			float4 color : COLOR;
			float fog : FOGC;
		};
		uniform float _Outline;
		uniform float4 _OutlineColor;

		v2f vert(appdata v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			float3 norm = mul ((float3x3)UNITY_MATRIX_MV, v.normal);
			norm.x *= UNITY_MATRIX_P[0][0];
			norm.y *= UNITY_MATRIX_P[1][1];
			o.pos.xy += norm.xy * o.pos.z * _Outline;

			o.fog = o.pos.z;
			o.color = _OutlineColor;
			return o;
		}
		ENDCG
		//Color (0,0,0,0)
		Cull Front
		ZWrite On
		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha
		SetTexture [_MainTex] { combine primary }
}
}

Fallback " VertexLit", 1
}