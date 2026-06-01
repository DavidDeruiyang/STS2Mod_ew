extends AnimatedSprite2D


func ew_play_death() -> void:
	if sprite_frames == null or not sprite_frames.has_animation("die"):
		return

	play("die")
