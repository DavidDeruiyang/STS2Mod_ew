shader_type canvas_item;

uniform float reveal_progress : hint_range(0.0, 1.0) = 1.0;
uniform float vanish_progress : hint_range(0.0, 1.0) = 0.0;
uniform float softness : hint_range(0.01, 0.35) = 0.16;
uniform float grain_strength : hint_range(0.0, 0.4) = 0.18;
uniform vec4 edge_color : source_color = vec4(0.75, 0.92, 1.0, 1.0);
uniform vec4 outline_color : source_color = vec4(1.0, 1.0, 1.0, 0.95);
uniform float outline_size : hint_range(0.0, 6.0) = 2.0;
uniform float outline_strength : hint_range(0.0, 1.0) = 0.75;

float hash(vec2 p) {
	p = fract(p * vec2(123.34, 456.21));
	p += dot(p, p + 45.32);
	return fract(p.x * p.y);
}

void fragment() {
	vec4 tex = texture(TEXTURE, UV) * COLOR;
	vec2 px = TEXTURE_PIXEL_SIZE * outline_size;
	float outline_alpha = 0.0;
	outline_alpha = max(outline_alpha, texture(TEXTURE, UV + vec2(px.x, 0.0)).a);
	outline_alpha = max(outline_alpha, texture(TEXTURE, UV + vec2(-px.x, 0.0)).a);
	outline_alpha = max(outline_alpha, texture(TEXTURE, UV + vec2(0.0, px.y)).a);
	outline_alpha = max(outline_alpha, texture(TEXTURE, UV + vec2(0.0, -px.y)).a);
	outline_alpha = max(outline_alpha, texture(TEXTURE, UV + vec2(px.x, px.y)).a);
	outline_alpha = max(outline_alpha, texture(TEXTURE, UV + vec2(-px.x, px.y)).a);
	outline_alpha = max(outline_alpha, texture(TEXTURE, UV + vec2(px.x, -px.y)).a);
	outline_alpha = max(outline_alpha, texture(TEXTURE, UV + vec2(-px.x, -px.y)).a);
	float outer_alpha = max(outline_alpha - tex.a, 0.0) * outline_strength;

	float grain = hash(floor(UV * vec2(36.0, 42.0)));
	float wind = UV.x + (grain - 0.5) * grain_strength + sin((UV.y + TIME * 1.8) * 18.0) * 0.025;

	float reveal_mask = smoothstep(wind - softness, wind + softness, reveal_progress);
	float vanish_mask = 1.0 - smoothstep(wind - softness, wind + softness, vanish_progress);
	float mask = clamp(reveal_mask * vanish_mask, 0.0, 1.0);

	float reveal_edge = 1.0 - smoothstep(0.0, softness, abs(wind - reveal_progress));
	float vanish_edge = 1.0 - smoothstep(0.0, softness, abs(wind - vanish_progress));
	float edge = max(reveal_edge * step(reveal_progress, 0.98), vanish_edge * step(0.02, vanish_progress));

	tex.rgb = mix(tex.rgb, edge_color.rgb, edge * 0.55);
	tex.a *= mask;
	vec4 outline = vec4(outline_color.rgb, outer_alpha * outline_color.a * mask);
	COLOR = mix(outline, tex, tex.a);
}
