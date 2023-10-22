shader_type canvas_item;

uniform float uLoopSpeed = 1.0;

// Function to generate pseudo-random noise
float noise(vec2 p) {
    return fract(sin(dot(p, vec2(12.9898, 78.233))) * 43758.5453);
}

void vertex() {
	float n = noise(VERTEX.xy + TIME);
	float loopOffset = sin(TIME * uLoopSpeed) * 2.5;
	VERTEX.x += loopOffset;
}


void fragment() {
	vec2 flippedCoords = vec2(UV.x,UV.y);
	float gridSize = 8.0;
	vec2 pixelSize = vec2(gridSize) / (1.0/SCREEN_PIXEL_SIZE);
	vec2 quantizedCoords = floor(flippedCoords / pixelSize) * pixelSize;
	
	float fogFactorX = (sin(TIME + quantizedCoords.x*10.0) + 1.0) / 2.0;
	float fogFactorY = (sin(TIME + quantizedCoords.y*10.0) + 1.0)/ 2.0;
	
	float combinedFogFactor = (fogFactorX + fogFactorY) * 0.5;
	float wispyFactor = pow(quantizedCoords.y,4.);
	combinedFogFactor = mix(1.0,wispyFactor * combinedFogFactor,0.5);
	
	float numLevels = 8.0;
	combinedFogFactor = floor(combinedFogFactor * numLevels)/numLevels;
	
	vec4 baseColor = vec4(0.5);
	baseColor = mix(baseColor,vec4(0.9,0.8,0.5,1.0),baseColor.a);
	vec4 fogColor = vec4(vec3(0.5),1.0);
	COLOR = mix(baseColor,fogColor,combinedFogFactor);
}
