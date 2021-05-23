// Procedural Candle Flame by scarletshark - www.barr.codes

const float pi = 3.14;
const float flameEdgeWidth = 0.1;
const float flameThinningFactor = 6.0;
const float flameRampCenter = 0.15;
const float flameBrightness = 2.25;
const float bgVignetteSize = 1.3;
const float bgJitterSpeed = 0.15;
const float bgJitterPower = 0.16;
const vec2 center = vec2(0.5, 0.5);
const vec2 bgOffset = vec2(0, -0.1);
const vec4 flameEdgeColor = vec4(0, 0.8, 1, 1);

const vec4[2] bgColors = vec4[2]
(
	vec4(0, 0, 0.05,1),
    vec4(0.35, 0.21, 0.15, 1)
);

const float[2] flameNoiseScales = float[2]
( 
	9.25,
	8.0
);

const vec2[2] flameNoiseSpeeds = vec2[2]
( 
	vec2(-0.06, -0.27),
	vec2(-0.03, -0.21)
);

const vec4[3] flameRampColors = vec4[3]
(
    vec4(0.4, 0.4, 1, 1),
    vec4(1, 0.25, 0.05, 1),
    vec4(1, 1, 0.8, 1)
);
    
float circularVignette(in vec2 screenUV, in vec2 offset, in float scale)
{
    return 1.0 - (length(screenUV - offset - center) / 0.5) / scale;
}

float circularVignette(in vec2 screenUV) 
{ 
    return circularVignette(screenUV, vec2(0, 0), 1.0); 
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
	vec2 screenUV = fragCoord.xy / iResolution.xy;
    vec2 flameUV1 = screenUV / flameNoiseScales[0] + iTime * flameNoiseSpeeds[0];
    vec2 flameUV2 = screenUV / flameNoiseScales[1] + iTime * flameNoiseSpeeds[1] + center;
    float bgJitterTime = iTime * bgJitterSpeed;
    
    // bg motion jitter
    vec2 bgJitter = vec2(
        texture(iChannel0, vec2(bgJitterTime, 0)).r - 0.5, 
        texture(iChannel0, vec2(0, bgJitterTime)).r - 0.5
    );
    
    // bg scale jitter
    float bgScale = 1.0 + bgJitterPower / 2.0 - (bgJitter.x + 0.5) * bgJitterPower;
    
    // background light
    vec4 bgColor = mix(
        bgColors[0], 
        bgColors[1], 
        circularVignette(
            screenUV, 
            bgJitter * bgJitterPower + bgOffset,
            bgScale * bgVignetteSize
        )
    );

    // flame motion
    float fade = texture(iChannel0, flameUV1).r * texture(iChannel0, flameUV2).r;
    
    // adding some fill into the bottom of the flame before fading the edges
    fade = clamp(fade + (1.0 - screenUV.y) * 0.8, 0.0, 1.0);

    // thin the flame on the lateral edges
    if (abs(screenUV.x - 0.5) > 0.5 / flameThinningFactor)
    {
        // no need to continue math outside the lateral edges
        fragColor = bgColor;
        return;
    }
    else
    {
    	fade *= cos((screenUV.x - 0.5) * pi * flameThinningFactor);
    }
    
    // create ovular shape of flame
    if (screenUV.y < 0.5)
    {
		fade *= circularVignette(screenUV);   
    }
    else
    {
		fade *= 1.0 - abs(screenUV.x - center.x) / 0.5;
    }
    
    vec4 flameColor;
    float rampCenter = flameRampCenter + bgJitter.x * 0.1;
    
    // 3-color ramp
    if (screenUV.y < rampCenter)
    {
        flameColor = mix(flameRampColors[0], flameRampColors[1], screenUV.y / rampCenter);
    }
    else
    {
        flameColor = mix(flameRampColors[1], flameRampColors[2], (screenUV.y - rampCenter) / (1.0 - rampCenter));
    }
    
    // vertical flame death
    float clampVal = screenUV.y;
    
    // edge fade & edge coloring
    if (fade < clampVal)
    {
        fragColor = bgColor;
        return;
    }
    else if (fade < clampVal + flameEdgeWidth)
    {
        fade = (fade - clampVal) / flameEdgeWidth;
        flameColor = mix(flameEdgeColor, flameColor, fade);
    }
    else
    {
		fade = 1.0;
    }

    flameColor *= flameBrightness * fade;
    flameColor.a = fade;
	fragColor = bgColor + flameColor;
}