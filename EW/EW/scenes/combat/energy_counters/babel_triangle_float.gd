extends Control

@export var pulse_seconds := 3.8
@export var alpha_min := 0.32
@export var alpha_max := 0.58

var _time := 0.0


func _process(delta: float) -> void:
	_time += delta
	var wave := sin(_time * TAU / pulse_seconds)
	modulate.a = lerp(alpha_min, alpha_max, (wave + 1.0) * 0.5)
