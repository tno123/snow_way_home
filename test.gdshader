shader_type canvas_item;

const float TWO_PI =  6.28319;

// Simple hash function for noise
float hash(vec2 p) {
    p = fract(p * vec2(127.1, 311.7));
    p += dot(p, p + 34.123);
    return fract(sin(dot(p, vec2(41.9, 289.5))) * 43758.5453);
}

float noise(vec2 st) {
    vec2 i = floor(st);
    vec2 f = fract(st);

    // Four corners in 2D of a tile
    float a = hash(i);
    float b = hash(i + vec2(1.0, 0.0));
    float c = hash(i + vec2(0.0, 1.0));
    float d = hash(i + vec2(1.0, 1.0));

    // Smooth interpolation
    vec2 u = f*f*f*(f*(f*6.0-15.0)+10.0);

    // Mix 4 corners percentages
    return mix(mix(a, b, u.x), mix(c, d, u.x), u.y);
}

void fragment() {
    vec2 flippedCoords = vec2(UV.x, UV.y);

    // Mirror the X-coordinate around the halfway point for seamlessness
    flippedCoords.x = flippedCoords.x < 0.5 ? flippedCoords.x * 2.0 : (1.0 - flippedCoords.x) * 2.0;

    float gridSize = 8.0;
    vec2 pixelSize = vec2(gridSize) / (1.0 / SCREEN_PIXEL_SIZE);
    vec2 quantizedCoords = floor(flippedCoords / pixelSize) * pixelSize;

    // Adjust scale for larger, smoother noise features
    float noiseScale = 0.8;
    float noiseX = noise(quantizedCoords * noiseScale + vec2(TIME * 0.05, 0));
    float noiseY = noise(quantizedCoords * noiseScale + vec2(0, TIME * 0.05));

    float movementScale = 0.5;
    float frequency = 4.0;
    float fogFactorX = (sin(TIME * movementScale + noiseX * TWO_PI * frequency) + 1.0) / 2.0;
    float fogFactorY = (cos(TIME * movementScale + noiseY * TWO_PI * frequency) + 1.0) / 2.0;

    float combinedFogFactor = (fogFactorX + fogFactorY) / 2.0;

     // Define parameters for the middle and transition sections
    float middleStart = 0.4; // Start of the more solid middle section
    float middleEnd = 0.6;   // End of the more solid middle section
    float transitionLength = 0.2; // Length of the transition section

    // Calculate distance from the midpoint
    float distanceFromMidpoint = abs(quantizedCoords.y - 0.5);

    // Piecewise definition of wispyFactor
    float wispyFactor;
    if (distanceFromMidpoint <= middleStart) {
        // Middle section: more solid
        wispyFactor = 1.0;
    } else if (distanceFromMidpoint <= middleEnd) {
        // Transition section: smooth transition to transparency
        float transitionProgress = (distanceFromMidpoint - middleStart) / transitionLength;
        wispyFactor = 1.0 - transitionProgress;
    } else {
        // Outer sections: more transparent
        wispyFactor = 0.0;
    }

    combinedFogFactor *= wispyFactor;

    float numLevels = 8.0;
    combinedFogFactor = floor(combinedFogFactor * numLevels) / numLevels;

    vec4 texColor = texture(TEXTURE, UV);
    
    // Lighter shade of yellow for the base color
    vec4 baseColor = vec4(1.0, 1.0, 1.0, 1.0);
    vec4 fogColor = vec4(0.9, 0.9, 0.5, 1.0);

    // Blend the fog color with the base color based on combinedFogFactor
    vec4 fogEffect = mix(baseColor, fogColor, combinedFogFactor);
    
    // Use wispyFactor to blend between the texture color and the fog effect
    // More wispyFactor -> More of the original texture color
    // Less wispyFactor -> More of the fog effect
    vec4 finalColor = mix(fogEffect, texColor, wispyFactor);

    // Adjust the final alpha value based on the combinedFogFactor and wispyFactor
    // This will ensure more transparency as we move outward
    finalColor.a *= (1.0 - combinedFogFactor) * wispyFactor;

    COLOR = finalColor;
}