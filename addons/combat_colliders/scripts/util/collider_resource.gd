class_name ColliderResource
extends Resource

var collider:= CollisionShape2D.new()

func _init(_name: String = "Default", _debug_color:Color = Color("white", 0.1)) -> void:
	var shape = CapsuleShape2D.new()
	collider.name = _name
	collider.debug_color = _debug_color
	collider.set_script(load("scripts/util/collider_frame_data.gd"))
	
func _get(property: StringName) -> Variant:
	match property:
		"collider":
			return collider
		"name":
			return collider.name
		_ :
			return null
			
func _set(property: StringName, value: Variant) -> bool:
	match property:
		"name":
			collider.name = value
			return true
		_:
			return false
