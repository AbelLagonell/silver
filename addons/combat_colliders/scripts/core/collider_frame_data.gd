@tool
class_name Collider_Frame_Data
extends Area2D

@export var colliders: Dictionary = {}
@export var cFrame: int = 0

@export var hurtbox_or_hitbox : bool = false :  
	set(value):
		if value:
			debug_color = Color(Color.RED, 0.2)
			suffix = "HI"
			collision_layer = 2
			collision_mask = 0
		else:
			collision_layer = 0
			collision_mask = 2
			debug_color = Color(Color.WHITE, 0.1)
			suffix = "HU"
		hurtbox_or_hitbox = value
		return true

@export_group("Colors and Trees")
@export var debug_color = Color(Color.SKY_BLUE, 0.41)
@export var suffix: String = "C"

func _ready():
	area_shape_entered.connect(_on_area_shape_entered)
	# Ensure the dictionary is initialized
	if not colliders:
		colliders = {0: {}}
	end()

func _add_collision_current(frame: int = cFrame) -> void:
	if (frame < 0): frame = 0
	if not colliders.has(frame):
		colliders[frame] = {}
	
	var shape_name = "F%d%s%d" % [frame, suffix, colliders[frame].size()]
	var capsule_shape = CapsuleShape2D.new()
	
	var collision_shape : CollisionShape2D
	if hurtbox_or_hitbox:
		collision_shape = HitBoxShape.new()
	else: 
		collision_shape = HurtBoxShape.new()
	
	collision_shape.shape = capsule_shape
	collision_shape.name = shape_name
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
			var node: Node = get_node(shape_name)
			node.set_disabled(true)
			node.hide()
	
	if colliders.has(frame):
		for shape_name in colliders[frame]:
			var node: Node = get_node(shape_name)
			node.set_disabled(false)
			node.show()

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
			var node: Node = get_node(shape_name)
			node.set_disabled(true)
			node.hide()
	
func _on_area_shape_entered(area_rid: RID, area: Area2D, area_shape_index: int, local_shape_index: int):
	if hurtbox_or_hitbox: return
	# area_shape_index is the shape index from the other area
	# local_shape_index is the shape index from this area
	print("Area shape entered: ", area.name)
	print("Local shape index: ", local_shape_index)
	
	# Get the CollisionShape2D node by index
	var shapes = get_children().filter(func(node): return node is HitBoxShape)
	if local_shape_index < shapes.size():
		var collision_shape = shapes[local_shape_index]
		print("Shape name: ", collision_shape.name)
	
