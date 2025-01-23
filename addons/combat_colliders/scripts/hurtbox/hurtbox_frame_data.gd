@tool
extends Area2D

const combat_collider = preload("res://addons/combat_colliders/scripts/util/collider_frame_data.gd")

## Dictionary to store CollisionShape2Ds by frame
@export var hurtboxes: Dictionary = {}
## Current active frame
@export var cFrame: int = 0

func _ready():
	# Ensure the dictionary is initialized
	if not hurtboxes:
		hurtboxes = {0: {}}

func _add_collision_current(frame: int = cFrame):
	if (frame < 0): frame = 0
	if not hurtboxes.has(frame):
		hurtboxes[frame] = {}
	
	var shape_name = "F%dHU%d" % [frame, hurtboxes[frame].size()]
	
	# Create CapsuleShape2D resource
	var capsule_shape = CapsuleShape2D.new()
	
	# Create CollisionShape2D and set its shape
	var collision_shape = CollisionShape2D.new()
	collision_shape.shape = capsule_shape
	collision_shape.name = shape_name
	collision_shape.debug_color = Color("white", 0.1)
	collision_shape.set_script(combat_collider)
	collision_shape.frame = frame
	
	# Add to dictionary with generated name
	hurtboxes[frame][shape_name] = capsule_shape
	
	# Add CollisionShape2D as child
	add_child(collision_shape)
	collision_shape.owner = EditorInterface.get_edited_scene_root()

func _add_collision_next():
	cFrame += 1
	_add_collision_current()

func _reset_frames_all():
	# Loop through all the frames and reset them
	for frame in hurtboxes.keys():
		_reset_frames_current(frame)

func _reset_frames_current(frame:int = cFrame):
	var temp = hurtboxes[frame]
	# Empty the array
	hurtboxes[frame] = {}
	# Free the objects in the tree
	for shape_name in temp:
		get_node(shape_name).queue_free()

func _remove_frame_all():
	# Remove all frame data completely
	cFrame = 0
	for frame in hurtboxes.keys():
		_remove_frame_current(frame)

func _remove_frame_current(frame:int = cFrame):
	sub_frame()
	var temp = hurtboxes[frame]
	hurtboxes.erase(frame)
	for shape_name in temp:
		get_node(shape_name).queue_free()

func _remove_single_collider(frame: int, shape_name: String):
	if (hurtboxes.get(frame) != null):
		if (hurtboxes[frame].get(shape_name) != null):
			get_node(shape_name).queue_free()
			hurtboxes[frame].erase(shape_name)

func start():
	cFrame = 0
	hide_hurt_boxes()

func next_frame():
	add_frame()
	hide_hurt_boxes(cFrame)

func end():
	for frame in hurtboxes:
		for collider in hurtboxes[frame]:
			collider.hide()

func hide_hurt_boxes(frame: int = 0):
	# Hiding will be done via CollisionShape2D, not the shape resource
	for f in hurtboxes:
		for shape_name in hurtboxes[f]:
			get_node(shape_name).hide()
	
	if hurtboxes.has(frame):
		for shape_name in hurtboxes[frame]:
			get_node(shape_name).show()

func add_frame():
	cFrame += 1
	if hurtboxes.is_empty():
		cFrame = 0
	elif cFrame >= hurtboxes.size():
		cFrame = 0

func sub_frame():
	cFrame -= 1
	if cFrame < 0:
		cFrame = hurtboxes.size() - 1 if not hurtboxes.is_empty() else 0
