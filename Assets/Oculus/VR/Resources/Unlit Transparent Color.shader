Shader "Oculus/Unlit Transparent Color" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
}

SubShader {
    Tags {"Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100
    Fog {Mode Off}

    ZTest Off
	ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    Color [_Color]

    Pass {}
}
}
