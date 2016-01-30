Shader "Vertex Colored/Vertex Colored Diffuse" {
Properties {
}
SubShader {
	Tags { "RenderType"="Opaque" }
	
CGPROGRAM
#pragma surface surf Lambert noforwardadd

struct Input {
	fixed4 color: Color;
};

void surf (Input IN, inout SurfaceOutput o) {
	
	o.Albedo = IN.color.rgb * 1;
    
}
ENDCG


}

Fallback "Mobile/Diffuse"
}
