shader_type canvas_item;

void fragment() {
    // Define the pixel size (e.g., 8x8 pixels)
    float pixelSize = 2.0;
    vec2 screenSize = vec2(textureSize(TEXTURE, 0));
    vec2 pixel = vec2(pixelSize) / screenSize;

    // Adjust UVs to snap to the pixel grid
    vec2 pixelUV = floor(UV / pixel) * pixel;

    // Define the top and bottom colors
    vec4 topColor = vec4(1.0, 1.0, 1.0, 0.0); // Light color at the top
    vec4 bottomColor = vec4(0.0, 0.0, 0.0, 1.0); // Dark color at the bottom

    // Calculate gradient based on the pixelized Y coordinate
    float gradientFactor = pow(pixelUV.y, 0.5);

    // Interpolate between top and bottom colors
    vec4 gradientColor = mix(topColor, bottomColor, gradientFactor);

    // Obtain the original texture color using pixelized UVs
    vec4 texColor = texture(TEXTURE, pixelUV);

    // Blend the texture color with the gradient color
    vec4 finalColor = texColor * gradientColor;

    // Apply the final color, maintaining the original alpha
    COLOR = vec4(finalColor.rgb, texColor.a);
}