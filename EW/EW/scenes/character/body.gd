extends AnimatedSprite2D

const RETURN_TO_IDLE_ANIMS := ["attack", "hurt", "cast"]
const DEATH_ANIMS := ["Dead", "dead", "die"]
const QUICK_RETURN_DELAY_FRAMES := 1
const HURT_RETURN_DELAY_SECONDS := 0.1

var death_started := false
var animation_version := 0


func _ready() -> void:
	print("EW Body ready")

	if sprite_frames != null:
		print("EW animations: ", sprite_frames.get_animation_names())

	animation_changed.connect(_on_animation_changed)
	animation_finished.connect(_on_animation_finished)


func ew_play_death() -> void:
	death_started = true
	print("EW death animation forced")

	if sprite_frames == null or not sprite_frames.has_animation("die"):
		print("EW death animation missing: die")
		return

	if animation != "die":
		play("die")


func _on_animation_changed() -> void:
	animation_version += 1
	print("EW animation changed to: ", animation)

	if animation in DEATH_ANIMS:
		death_started = true
		print("EW death animation detected")


func _on_animation_finished() -> void:
	var finished_animation := animation
	var version_when_finished := animation_version

	print("EW animation finished: ", finished_animation)

	if finished_animation in DEATH_ANIMS:
		death_started = true
		return

	if finished_animation not in RETURN_TO_IDLE_ANIMS:
		return

	if finished_animation == "hurt":
		await get_tree().create_timer(HURT_RETURN_DELAY_SECONDS).timeout
	else:
		for i in range(QUICK_RETURN_DELAY_FRAMES):
			await get_tree().process_frame

	if death_started:
		print("EW not returning to idle because death started")
		return

	if animation_version != version_when_finished:
		print("EW not returning to idle because animation changed to: ", animation)
		return

	if animation == finished_animation:
		print("EW returning to idle from: ", finished_animation)
		play("idle")
