@tool
class_name ColliderInspector 
extends EditorInspectorPlugin

var object: Object
var collider_methods = [
		"_add_collision_current",
		"_add_collision_next",
		"_reset_frames_current",
		"_remove_frame_current",
		"_reset_frames_all",
		"_remove_frame_all" 
	]

func _can_handle(_object: Object) -> bool:
	for method in collider_methods:
		if _object.has_method(method):
			object = _object
			return true
	return false
	
func _parse_begin(_object: Object) -> void:
	for method in collider_methods.slice(0,4):
		if (object.has_method(method)):
			var name: String = ""
			match method:
				"_add_collision_current":
					name = "Add Collider to Current Frame"
				"_add_collision_next":
					name = "Add Collider to Next Frame"
				"_reset_frames_current":
					name = "Delete Colliders in Current Frame"
				"_remove_frame_current":
					name = "Delete Current Frame"
			var btn = Button.new()
			btn.text = name
			btn.pressed.connect(_on_method_pressed.bind(object, method))
			
			add_custom_control(btn)

func _parse_end(_object: Object) -> void:
	for method in collider_methods.slice(4,):
		if (object.has_method(method)):
			var name: String = ""
			match method:
				"_reset_frames_all":
					name = "Delete Colliders in All Frames"
				"_remove_frame_all":
					name = "Delete All Frames"
			var btn = Button.new()
			btn.text = name
			var style = StyleBoxFlat.new()
			style.bg_color = Color(0.5,0,0)
			btn.add_theme_stylebox_override("normal", style)
			btn.pressed.connect(_on_method_pressed.bind(object, method))
			
			add_custom_control(btn)

func _on_method_pressed(object, method):
	object.call(method)
