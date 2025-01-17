@tool
class_name HurtboxFrameData
extends Area2D

const Collision2DArray = preload("res://Assets/Scripts/CollisionArray.gd")

@export var hurtboxes : Array[Collision2DArray] 
@export var cFrame : int = 0
@export var newScript: Script

func _extend_inspector_begin(inspector: ExtendableInspector):
	var addColShapeBtn = CommonControls.new(inspector).method_button("_add_collision_shape")
	addColShapeBtn.text = "Add Hurtbox to Current Frame"
	inspector.add_custom_control(addColShapeBtn)
	
	var addNextShapeBtn = CommonControls.new(inspector).method_button("_add_next_shape")
	addNextShapeBtn.text = "Add Hurtbox to Next Frame"
	inspector.add_custom_control(addNextShapeBtn)
	
	var resetCurFrameBtn = CommonControls.new(inspector).method_button("_reset_current_frame")
	resetCurFrameBtn.text = "Reset Current Frame"
	inspector.add_custom_control(resetCurFrameBtn)
	
	var removeCurFrameBtn = CommonControls.new(inspector).method_button("_remove_current_frame")
	removeCurFrameBtn.text = "Remove Current Frame"
	inspector.add_custom_control(removeCurFrameBtn)
func _extend_inspector_end(inspector: ExtendableInspector):
	var resetAllFramesBtn = CommonControls.new(inspector).method_button("_reset_all_frames")
	resetAllFramesBtn.text = "Reset All Frames"
	inspector.add_custom_control(resetAllFramesBtn)
	
	var removeAllFramesBtn = CommonControls.new(inspector).method_button("_remove_all_frame")
	removeAllFramesBtn.text = "Remove All Frames"
	inspector.add_custom_control(removeAllFramesBtn)

func _add_collision_shape():
	var shape = CapsuleShape2D.new()
	var collision2D := CollisionShape2D.new()
	collision2D.shape = shape
	collision2D.name = "F%d" % cFrame
	collision2D.debug_color = Color("white", 0.1)
	if (hurtboxes.size() < cFrame+1):
		hurtboxes.append(Collision2DArray.new(self))
	hurtboxes[cFrame].add(collision2D)
func _add_next_shape():
	cFrame += 1
	_add_collision_shape()
func _reset_all_frames():
	for index in hurtboxes.size():
		hurtboxes[index].reset()
func _reset_current_frame():
	hurtboxes[cFrame].reset()
func _remove_all_frame():
	cFrame = 0
	for index in hurtboxes.size():
		hurtboxes[index].reset()
	hurtboxes.clear()
func _remove_current_frame():
	hurtboxes[cFrame].reset()
	if (cFrame == hurtboxes.size()):
		hurtboxes.remove_at(cFrame)
	_sub_frame()

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
