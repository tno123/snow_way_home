shader_type canvas_item;

uniform float blue = 0.0;

void vertex(){
	//VERTEX += vec2(cos(TIME)*100.0,cos(TIME)*100.0);
}

void fragment(){
	COLOR = texture(TEXTURE,UV);
	COLOR.b = blue;
	//COLOR = vec4(UV,0.8,0.5);
}