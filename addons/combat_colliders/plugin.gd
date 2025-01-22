@tool
extends EditorPlugin

const ColliderInspectorPlugin = preload("scripts/util/collider_inspector.gd")
var collider_inspector_plugin

func _enter_tree() -> void:
	# Initialization of the plugin goes here
	collider_inspector_plugin = ColliderInspectorPlugin.new()
	add_inspector_plugin(collider_inspector_plugin)
	add_custom_type("Hurtbox", "Area2D", preload("scripts/hurtbox/hurtbox_frame_data.gd"), preload("res://icon.svg"))


func _exit_tree() -> void:
	# Clean-up of the plugin goes here.
	remove_custom_type("Hurtbox")
	if is_instance_valid(collider_inspector_plugin):
		remove_inspector_plugin(collider_inspector_plugin)
		collider_inspector_plugin = null
