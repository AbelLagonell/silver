class_name ColliderFrameData
extends Area2D

const combat_collider = preload("../util/collider_manip.gd")

@export var colliders: Dictionary = {}
@export var cFrame: int = 0

@export_group("Colors and Trees")
@export var debug_color = Color(Color.SKY_BLUE, 0.41)
@export var suffix: String = "C"

func _ready():
	set_collision_layer_value(1, false)
	set_collision_layer_value(9, true)
	set_collision_mask_value(1, false)
	set_collision_mask_value(9, true)
	# Ensure the dictionary is initialized
	if not colliders:
		colliders = {0: {}}

func _add_collision_current(frame: int = cFrame):
	if (frame < 0): frame = 0
	if not colliders.has(frame):
		colliders[frame] = {}
	
	var shape_name = "F%d%s%d" % [frame, suffix, colliders[frame].size()]
	var capsule_shape = CapsuleShape2D.new()
	var collision_shape = CollisionShape2D.new()
	
	collision_shape.shape = capsule_shape
	collision_shape.name = shape_name
	collision_shape.set_script(combat_collider)
	collision_shape.frame = frame
	collision_shape.debug_color = debug_color
	collision_shape.set_disabled(false)
	
	colliders[frame][shape_name] = capsule_shape
	add_child(collision_shape)
	collision_shape.owner = EditorInterface.get_edited_scene_root()

func _add_collision_next():
	cFrame += 1
	_add_collision_current()

func _reset_frames_all():
	# Loop through all the frames and reset them
	for frame in colliders.keys():
		_reset_frames_current(frame)

func _reset_frames_current(frame:int = cFrame):
	var temp = colliders[frame]
	# Empty the array
	colliders[frame] = {}
	# Free the objects in the tree
	for shape_name in temp:
		get_node(shape_name).queue_free()

func _remove_frame_all():
	# Remove all frame data completely
	cFrame = 0
	for frame in colliders.keys():
		_remove_frame_current(frame)

func _remove_frame_current(frame:int = cFrame):
	_sub_frame()
	var temp = colliders[frame]
	colliders.erase(frame)
	for shape_name in temp:
		get_node(shape_name).queue_free()

func _remove_single_collider(frame: int, shape_name: String):
	if (colliders.get(frame) != null):
		if (colliders[frame].get(shape_name) != null):
			get_node(shape_name).queue_free()
			colliders[frame].erase(shape_name)

func _hide_hurt_boxes(frame: int = 0):
	for f in colliders:
		for shape_name in colliders[f]:
			get_node(shape_name).set_disabled(true)
	
	if colliders.has(frame):
		for shape_name in colliders[frame]:
			get_node(shape_name).set_disabled(false)

func _add_frame():
	cFrame += 1
	if colliders.is_empty():
		cFrame = 0
	elif cFrame >= colliders.size():
		cFrame = 0

func _sub_frame():
	cFrame -= 1
	if cFrame < 0:
		cFrame = colliders.size() - 1 if not colliders.is_empty() else 0
	
func start():
	cFrame = 0
	_hide_hurt_boxes()

func next_frame():
	_add_frame()
	_hide_hurt_boxes(cFrame)

func end():
	for f in colliders:
		for shape_name in colliders[f]:
			#var node: CollisionShape2D = get_node(shape_name)
			#node.disabled = true
			get_node(shape_name).set_disabled(true)
