@tool
class_name CombatCollider
extends Node

@export var frame:int = 0:
	set(_frame):
		frame = _frame

var parent: Node

func _ready():
	parent = get_parent()

func _remove_self():
	parent._remove_single_collider(frame, self.name)
	
func _place_frame_after():
	parent._add_collision_current(frame+1)

func _place_frame_before():
	parent._add_collision_current(frame-1)

func _place_current_frame():
	parent._add_collision_current(frame)
