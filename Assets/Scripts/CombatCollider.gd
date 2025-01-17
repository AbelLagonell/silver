class_name CombatCollider
extends Node

func _extend_inspector_begin(inspector: ExtendableInspector):
	var removeSelfBtn = CommonControls.new(inspector).method_button("_removeSelf")
	removeSelfBtn.text = "Remove This Collider"
	inspector.add_custom_control(removeSelfBtn)
	
	var placeAfterBtn = CommonControls.new(inspector).method_button("_removeSelf")
	removeSelfBtn.text = "Remove This Collider"
	inspector.add_custom_control(removeSelfBtn)
	
	var placeBeforeBtn = CommonControls.new(inspector).method_button("_removeSelf")
	removeSelfBtn.text = "Remove This Collider"
	inspector.add_custom_control(removeSelfBtn)
	
	var addCurrentBtn = CommonControls.new(inspector).method_button("_removeSelf")
	removeSelfBtn.text = "Remove This Collider"
	inspector.add_custom_control(removeSelfBtn)

func _removeSelf():
	print("removing self")
	
func _placeFrameAfter():
	print("adding collider to next frame")

func _placeFrameBefore():
	print("Adding collider to the previous frame")

func _placeCurrentFrame():
	print("Adding collider on the same frame")
