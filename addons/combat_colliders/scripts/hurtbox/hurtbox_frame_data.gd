@tool
extends Area2D

## Dictionary = { int : Array[CollisionShape2D] }
@export var hurtboxes : Dictionary
@export var cFrame : int = 0

func _add_collision_current():
	var collision2D := ColliderResource.new()
	var colliders: Array = hurtboxes.get_or_add(cFrame, [])
	#"F%dHU%d" % [cFrame, colliders.size()]
	colliders.append(collision2D)
	#add_child(collision2D)
	#collision2D.owner = EditorInterface.get_edited_scene_root()
	print(hurtboxes.get(cFrame))
	
func _add_collision_next():
	cFrame += 1
	_add_collision_current()
func _reset_frames_all():
	print("Delete Colliders in All Frames")
func _reset_frames_current():
	print("Delete Colliders in Current Frame")
func _remove_frame_all():
	print("Delete All Frames")
func _remove_frame_current():
	print("Delete Current Frame")

func start():
	cFrame = 0
	hide_hurt_boxes()
	
func next_frame():
	_add_frame()
	hide_hurt_boxes(cFrame)

func end():
	for index in hurtboxes.size():
		hurtboxes[index].hide()

func hide_hurt_boxes(frame:int = 0):
	for index in hurtboxes.size():
		hurtboxes[index].hide()
	hurtboxes[frame].show()

func _add_frame():
	cFrame += 1
	if cFrame >= hurtboxes.size():
		cFrame = 0

func _sub_frame():
	cFrame -= 1
	if cFrame < 0:
		cFrame = 0
