@tool
## A Collection class for [HurtBoxShape] and [HitBoxShape] objects to facilitate
##frame hit/hurtboxes for various action games
## Uses Layer 2 for player and Layer 3 for Enemy
class_name Collider_Frame_Data
extends Area2D

## When a hitbox enters a hurtbox, this will send out how much damage the hitbox does
signal damage_taken(value:int)
## When a hitbox enters a hurtbox, this will send out how much 
##knockback the hitbox does if at all
signal knockback_taken(vector: Vector2)
## When a hitbox enters a hurtbox, this will send out how much
##the screen will shake and if there is any hitstop that the move does
signal ui_effect(hitstop:float, screen_shake: float)

## Holds the combination of {[param String], [param Shape2D]} this is so 
##that the string holds the name of Collider in reference
@export var colliders: Dictionary = {}
## The current frame number can be changed but try to keep it sequential
@export_range(0,10,1,"or_greater") var cFrame: int = 0

enum _ShapeTypes {CIRCLE, RECTANGLE, CAPSULE}
## The default shape that all new colliders will get once they are initialized
@export var shape_type : _ShapeTypes = _ShapeTypes.CAPSULE : 
	set(value):
		match value:
			_ShapeTypes.CIRCLE:
				_shape = CircleShape2D
			_ShapeTypes.RECTANGLE:
				_shape = RectangleShape2D
			_ShapeTypes.CAPSULE:
				_shape = CapsuleShape2D
		shape_type = value
			
			
var _shape = CapsuleShape2D


## Determines if the colliders are going to be in the correct layer index
@export var is_enemy : bool = false:
	set(value):
		is_enemy = value
		_change_collision(is_hitbox, is_enemy)
		return true
			

## Determines if the colliders that are being made 
##are either of [HitBoxShape] or [HurtBoxShape]
## [br] Also determines if its going to be a layer or mask
@export var is_hitbox : bool = false :  
	set(value):
		if value:
			debug_color = Color(Color.RED, 0.2)
			suffix = "HI"
		else:
			debug_color = Color(Color.WHITE, 0.1)
			suffix = "HU"
		is_hitbox = value
		_change_collision(is_hitbox, is_enemy)
		return true

@export_group("Colors and Trees")
## Changes the [Color] of the [member CollisionShape2D.debug_color] of any new colliders
@export var debug_color = Color(Color.SKY_BLUE, 0.41)
## Changes what the Identifier of the collider will be
## \n Example: F2[b]C[/b]3
@export var suffix: String = "C"

@export_group("Debug Features")

##When true it will make all hitboxes visible no matter the stage in the animation
@export var set_visible:= false

## When true it will override the hitbox functionality and make it 
##so that all the colliders under this will always be active 
@export var set_disabled:= false

# A Utility function for layer management
func _change_collision(hitbox: bool, enemy:bool):
	set_collision_layer_value(3, !enemy && hitbox)
	set_collision_layer_value(2, enemy && hitbox)
	set_collision_mask_value(2, !enemy && !hitbox)
	set_collision_mask_value(3, enemy && !hitbox)

func _ready():
	set_collision_layer_value(1, false)
	set_collision_mask_value(1, false)
	
	area_shape_entered.connect(_on_area_shape_entered)
	# Ensure the dictionary is initialized
	if not colliders:
		colliders = {0: {}}
	end()

# Adds a Collision object based on hurt/hitbox with defaults 
#based on what was chosen in the class
func _add_collision_current(frame: int = cFrame) -> void:
	if (frame < 0): frame = 0
	if not colliders.has(frame):
		colliders[frame] = {}
	
	var shape_name = "F%d%s%d" % [frame, suffix, colliders[frame].size()]
	var capsule_shape = _shape.new()
	
	var collision_shape : CollisionShape2D
	if is_hitbox:
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

# Increments the frame count and adds a collision with [method _add_collision_current]
func _add_collision_next():
	cFrame += 1
	_add_collision_current()

# Removes all colliders in all frames
func _reset_frames_all():
	# Loop through all the frames and reset them
	for frame in colliders.keys():
		_reset_frames_current(frame)

# Remvoes all colliders in current frame
func _reset_frames_current(frame:int = cFrame):
	var temp = colliders[frame]
	# Empty the array
	colliders[frame] = {}
	# Free the objects in the tree
	for shape_name in temp:
		get_node(shape_name).queue_free()

# Removes all frame
func _remove_frame_all():
	# Remove all frame data completely
	cFrame = 0
	for frame in colliders.keys():
		_remove_frame_current(frame)

# Removes current frame
func _remove_frame_current(frame:int = cFrame):
	_sub_frame()
	var temp = colliders[frame]
	colliders.erase(frame)
	for shape_name in temp:
		get_node(shape_name).queue_free()

# Removes a single collider in given [param frame], with default being [member cFrame]
func _remove_single_collider(frame: int, shape_name: String):
	if (colliders.get(frame) != null):
		if (colliders[frame].get(shape_name) != null):
			get_node(shape_name).queue_free()
			colliders[frame].erase(shape_name)

# Util Function to hide and disable colliders based on current frame
func _hide_hurt_boxes(frame: int = 0):
	for f in colliders:
		for shape_name in colliders[f]:
			var node: Node = get_node(shape_name)
			node.set_disabled(true and !set_disabled)
			node.set_visible(false or set_visible)
	
	if colliders.has(frame):
		for shape_name in colliders[frame]:
			var node: Node = get_node(shape_name)
			node.set_disabled(false or set_disabled)
			node.set_visible(true or set_visible)

# Goes to the next key-frame in dictionary
func _add_frame():
	if colliders.is_empty():
		cFrame = 0
		return
		
	var keys = colliders.keys()
	var current_index = keys.find(cFrame)
	
	# Get next key with wrapping
	var next_index = (current_index + 1) % keys.size()
	cFrame = keys[next_index]

# Goes to the previous key-frame in dictionary
func _sub_frame():
	if colliders.is_empty():
		cFrame = 0
		return
		
	var keys = colliders.keys()
	var current_index = keys.find(cFrame)
	
	# Get previous key with wrapping
	current_index -= 1
	if current_index < 0:
		current_index = keys.size() - 1
	
	cFrame = keys[current_index]

## Place this on the first frame that you want the colliders to be active
##[br] Usually the first frame for hurtboxes
func start():
	cFrame = 0
	_hide_hurt_boxes()

## Place this on each subsequent frame that you would want to go to the next frame
func next_frame():
	_add_frame()
	_hide_hurt_boxes(cFrame)

## Place this at the end, when you want all the colliders to be disabled.
## Usually for the last frame of the animation with hurtboxes
func end():
	for f in colliders:
		for shape_name in colliders[f]:
			var node: Node = get_node_or_null(shape_name)
			if node and is_instance_valid(node):
				node.set_disabled(true and !set_disabled)
				node.set_visible(false or set_visible)

# Utility for getting when a hurtbox goes into a hitbox
func _on_area_shape_entered(area_rid: RID, area: Area2D, area_shape_index: int, local_shape_index: int):
	var collider = area.get_child(area_shape_index)
	if collider is HitBoxShape:
		damage_taken.emit(collider.damage)
		if collider.has_knockback:
			knockback_taken.emit(collider.get_knockback)
		if collider.has_ui_effect:
			ui_effect.emit(collider.hit_stop, collider.screen_shake_amount)
	
