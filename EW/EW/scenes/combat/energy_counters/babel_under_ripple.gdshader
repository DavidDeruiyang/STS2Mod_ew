shader_type canvas_item;

uniform vec4 ripple_color : source_color = vec4(0.95, 0.05, 0.08, 0.24);
uniform float ripple_speed : hint_range(0.0, 4.0) = 0.55;
uniform float ripple_density : hint_range(2.0, 26.0) = 13.0;

void fragment() {
	vec2 uv = UV;
	vec2 center = vec2(0.5, 0.72);
	vec2 delta = (uv - center) * vec2(1.0, 2.75);
	float dist = length(delta);

	float ring = sin((dist * ripple_density - TIME * ripple_speed) * 6.2831853);
	ring = smoothstep(0.72, 1.0, ring);

	float basin = smoothstep(0.82, 0.28, dist) * smoothstep(0.36, 0.68, uv.y);
	float fade = 1.0 - smoothstep(0.70, 1.0, uv.y);
	float alpha = ring * basin * fade * ripple_color.a;

	COLOR = vec4(ripple_color.rgb, alpha) * COLOR;
}
