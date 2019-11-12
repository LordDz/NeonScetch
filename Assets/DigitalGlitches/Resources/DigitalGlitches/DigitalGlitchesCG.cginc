// Copyright 2014-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

#ifndef DIGITAL_GLITCHES_CG_INCLUDED
#define DIGITAL_GLITCHES_CG_INCLUDED

#include "UnityCG.cginc"

uniform sampler2D _DIGITAL_GLITCHES_GlitchTex;
uniform half4 _DIGITAL_GLITCHES_GlitchTex_ST;
uniform sampler2D _DIGITAL_GLITCHES_OverlayTex;
uniform fixed _DIGITAL_GLITCHES_ShowOverlay;
uniform float _DIGITAL_GLITCHES_Intensity;
uniform fixed4 _DIGITAL_GLITCHES_ColorTint;
uniform fixed _DIGITAL_GLITCHES_BurnColors;
uniform fixed _DIGITAL_GLITCHES_DodgeColors;
uniform fixed _DIGITAL_GLITCHES_PerformUVShifting;
uniform fixed _DIGITAL_GLITCHES_PerformColorShifting;
uniform fixed _DIGITAL_GLITCHES_PerformBodyShifting;
uniform float _DIGITAL_GLITCHES_FilterRadius;
uniform float _DIGITAL_GLITCHES_FlipUp;
uniform float _DIGITAL_GLITCHES_FlipDown;
uniform float _DIGITAL_GLITCHES_Displace; 

inline fixed3 ColorBurn(fixed3 a, fixed3 b)
{
    return 1.0 - (1.0 - a) / b;
}

fixed4 ApplyGlitchEffect(fixed4 mainColor, sampler2D mainTex, float2 texCoord)
{
    float sinTime = abs(sin(_Time.y * _DIGITAL_GLITCHES_Intensity));
    float cosTime = abs(cos(_Time.z * _DIGITAL_GLITCHES_Intensity));

    // UV distortion.
    if ((texCoord.y < sinTime + _DIGITAL_GLITCHES_FilterRadius / 10.0 && texCoord.y > sinTime - _DIGITAL_GLITCHES_FilterRadius / 10.0) ||
        (texCoord.y < cosTime + _DIGITAL_GLITCHES_FilterRadius / 10.0 && texCoord.y > cosTime - _DIGITAL_GLITCHES_FilterRadius / 10.0))
    {
        if (texCoord.y < _DIGITAL_GLITCHES_FlipUp)
            texCoord.y = 1.0 - (texCoord.y + _DIGITAL_GLITCHES_FlipUp);

        if (texCoord.y > _DIGITAL_GLITCHES_FlipDown)
            texCoord.y = 1.0 - (texCoord.y - _DIGITAL_GLITCHES_FlipDown);

        texCoord += _DIGITAL_GLITCHES_Displace * _DIGITAL_GLITCHES_Intensity;
    }

    // UV shifting.
    fixed4 shiftedSample = tex2D(mainTex, lerp(texCoord, texCoord + 0.01 * _DIGITAL_GLITCHES_FilterRadius * _DIGITAL_GLITCHES_Intensity, _DIGITAL_GLITCHES_PerformBodyShifting));
    mainColor = lerp(mainColor, shiftedSample, _DIGITAL_GLITCHES_PerformUVShifting);

    // Colors distortion and shifting.
    fixed3 glitchColor = tex2D(_DIGITAL_GLITCHES_GlitchTex, texCoord * _DIGITAL_GLITCHES_GlitchTex_ST.xy + _DIGITAL_GLITCHES_GlitchTex_ST.zw);
    mainColor.rgb = lerp(mainColor.rgb, mainColor.rgb * (1.0 + _DIGITAL_GLITCHES_ColorTint.rgb), _DIGITAL_GLITCHES_PerformColorShifting);
    fixed3 burnedGlitch = lerp(ColorBurn(mainColor.rgb, glitchColor), mainColor.rgb / glitchColor, floor(abs(_DIGITAL_GLITCHES_FilterRadius)));
    fixed3 finalGlitch = lerp(mainColor.rgb * glitchColor, burnedGlitch, _DIGITAL_GLITCHES_BurnColors);
    fixed3 dodgedGlitch = lerp(finalGlitch - glitchColor, abs(finalGlitch - glitchColor), floor(abs(_DIGITAL_GLITCHES_FilterRadius)));
    finalGlitch = lerp(finalGlitch, dodgedGlitch, _DIGITAL_GLITCHES_DodgeColors);
    mainColor.rgb = lerp(mainColor.rgb, finalGlitch, _DIGITAL_GLITCHES_PerformColorShifting);

    // Apply overlay texture.
    fixed4 overlayColor = tex2D(_DIGITAL_GLITCHES_OverlayTex, texCoord);
    fixed4 overlayBlend = lerp(mainColor, mainColor + overlayColor, overlayColor.a);
    return lerp(mainColor, overlayBlend, _DIGITAL_GLITCHES_ShowOverlay);
}

#endif // DIGITAL_GLITCHES_CG_INCLUDED
