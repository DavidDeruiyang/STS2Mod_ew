shader_type canvas_item;

uniform vec4 expansion_color : source_color = vec4(0.95, 0.08, 0.10, 0.28);
uniform float expansion_size : hint_range(0.0, 0.12) = 0.045;
uniform float expansion_speed : hint_range(0.0, 3.0) = 0.42;
uniform float expansion_softness : hint_range(0.0, 1.0) = 0.62;

float expanded_alpha(vec2 uv, float amount) {
	vec2 center = vec2(0.5, 0.55);
	vec2 scaled_uv = center + (uv - center) / (1.0 + amount);
	if (scaled_uv.x < 0.0 || scaled_uv.x > 1.0 || scaled_uv.y < 0.0 || scaled_uv.y > 1.0) {
		return 0.0;
	}

	return texture(TEXTURE, scaled_uv).a;
}

void fragment() {
	vec2 uv = UV;
	vec4 base = texture(TEXTURE, uv) * COLOR;
	float base_alpha = texture(TEXTURE, uv).a;

	float phase = fract(TIME * expansion_speed);
	float ghost = 0.0;
	for (int i = 0; i < 3; i++) {
		float layer_phase = fract(phase + float(i) / 3.0);
		float amount = expansion_size * layer_phase;
		float layer_alpha = max(expanded_alpha(uv, amount) - base_alpha, 0.0);
		float fade = pow(1.0 - layer_phase, 1.0 + expansion_softness);
		ghost += layer_alpha * fade;
	}

	vec4 expansion = vec4(expansion_color.rgb, clamp(ghost * expansion_color.a, 0.0, 1.0));
	COLOR = base + expansion * (1.0 - base.a);
}
