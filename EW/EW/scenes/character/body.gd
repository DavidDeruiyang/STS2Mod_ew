extends AnimatedSprite2D

const RETURN_TO_IDLE_ANIMS := ["attack", "hurt", "cast"]
const DEATH_ANIMS := ["dead", "die"]

var death_started := false
var animation_version := 0


func _ready() -> void:
	print("EW Body ready")

	if sprite_frames != null:
		print("EW animations: ", sprite_frames.get_animation_names())

	animation_changed.connect(_on_animation_changed)
	animation_finished.connect(_on_animation_finished)


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

	# Never return to idle after death.
	if finished_animation in DEATH_ANIMS:
		death_started = true
		return

	# Only Attack / Hit / Cast should return to idle.
	if finished_animation not in RETURN_TO_IDLE_ANIMS:
		return

	# Important:
	# Wait a couple frames before forcing idle.
	# This gives the game time to switch Hit -> Dead if the hit killed the player.
	await get_tree().process_frame
	await get_tree().process_frame

	# If death started during those frames, do not override it.
	if death_started:
		print("EW not returning to idle because death started")
		return

	# If another animation started, do not override it.
	if animation_version != version_when_finished:
		print("EW not returning to idle because animation changed to: ", animation)
		return

	# If we are still on the finished animation, safely return to idle.
	if animation == finished_animation:
		print("EW returning to idle from: ", finished_animation)
		play("idle")
