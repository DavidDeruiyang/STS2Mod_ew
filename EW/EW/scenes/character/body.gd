extends AnimatedSprite2D

const RETURN_TO_IDLE_ANIMS := ["attack", "hurt", "cast"]
const DEATH_ANIMS := ["Dead", "dead", "die"]
const QUICK_RETURN_DELAY_FRAMES := 1
const HURT_RETURN_DELAY_SECONDS := 0.1
const HLZY_SCENE := preload("res://EW/scenes/summons/hlzy/hlzy_visual.tscn")
const HLZY_NODE_PREFIX := "HLZYCompanion"
const HLZY_SCALE := Vector2(1.25, 1.25)
const HLZY_POSITIONS := [
	Vector2(-713, -630),
	Vector2(-760, -695),
	Vector2(-965, -600),
]

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


func ew_restart_animation(animation_name: StringName) -> void:
	if death_started:
		print("EW restart skipped because death started: ", animation_name)
		return

	if sprite_frames == null or not sprite_frames.has_animation(animation_name):
		print("EW restart animation missing: ", animation_name)
		return

	var was_same_animation := animation == animation_name
	play(animation_name)
	set_frame_and_progress(0, 0.0)

	if was_same_animation:
		animation_version += 1
		print("EW animation restarted: ", animation_name)


func ew_spawn_hlzy(slot_index := -1) -> Node2D:
	var visuals := get_parent()
	if visuals == null:
		push_warning("EW HLZY spawn skipped: Body parent Visuals node was not found.")
		return null

	var resolved_slot := _resolve_hlzy_slot(slot_index)
	if resolved_slot < 0:
		push_warning("EW HLZY spawn skipped: no free companion slot.")
		return null

	var node_name := _hlzy_node_name(resolved_slot)
	var existing := visuals.get_node_or_null(node_name)
	if existing is Node2D:
		print("EW HLZY already present in slot: ", resolved_slot)
		return existing

	var hlzy := HLZY_SCENE.instantiate() as Node2D
	if hlzy == null:
		push_warning("EW HLZY spawn skipped: scene root is not Node2D.")
		return null

	hlzy.name = node_name
	hlzy.position = HLZY_POSITIONS[resolved_slot]
	hlzy.scale = HLZY_SCALE
	visuals.add_child(hlzy)
	print("EW HLZY spawned in slot: ", resolved_slot, " at ", hlzy.position)
	return hlzy


func ew_clear_hlzy(slot_index := -1) -> void:
	var visuals := get_parent()
	if visuals == null:
		return

	if slot_index < 0:
		for child in visuals.get_children():
			if child.name.begins_with(HLZY_NODE_PREFIX):
				child.queue_free()
		return

	var existing := visuals.get_node_or_null(_hlzy_node_name(clampi(slot_index, 0, HLZY_POSITIONS.size() - 1)))
	if existing != null:
		existing.queue_free()


func ew_hlzy_count() -> int:
	var visuals := get_parent()
	if visuals == null:
		return 0

	var count := 0
	for child in visuals.get_children():
		if child.name.begins_with(HLZY_NODE_PREFIX):
			count += 1

	return count


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


func _resolve_hlzy_slot(requested_slot: int) -> int:
	if requested_slot >= 0:
		return clampi(requested_slot, 0, HLZY_POSITIONS.size() - 1)

	var visuals := get_parent()
	if visuals == null:
		return -1

	for slot_index in range(HLZY_POSITIONS.size()):
		if visuals.get_node_or_null(_hlzy_node_name(slot_index)) == null:
			return slot_index

	return -1


func _hlzy_node_name(slot_index: int) -> String:
	return "%s%d" % [HLZY_NODE_PREFIX, slot_index]
