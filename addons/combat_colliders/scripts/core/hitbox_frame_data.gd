@tool
class_name HitboxFrameData
extends ColliderFrameData

var damage: int = 0

# Knockback
var knockback: bool = false
var knockback_direction: float = 0.0
var knockback_magnitude: float = 0.0

# UI
var ui_effect: bool = false
var hit_stop: float = 0.0
var screen_shake_amount: float = 0.0

func _get(property):
	# Knockback group
	if property == "knockback/has":
		return knockback
	if property == "knockback/direction":
		return knockback_direction
	if property == "knockback/magnitude":
		return knockback_magnitude
	
	# UI group
	if property == "ui_effect/has":
		return ui_effect
	if property == "ui_effect/hit_stop":
		return hit_stop
	if property == "ui_effect/screen_shake":
		return screen_shake_amount

func _set(property, value):
	# Knockback group
	if property == "knockback/has":
		knockback = value
		notify_property_list_changed()
		return true
	if property == "knockback/direction":
		knockback_direction = value
		return true
	if property == "knockback/magnitude":
		knockback_magnitude = value
		return true
	
	# UI group
	if property == "ui_effect/has":
		ui_effect = value
		notify_property_list_changed()
		return true
	if property == "ui_effect/hit_stop":
		if (value < 0): return false
		hit_stop = value
		return true
	if property == "ui_effect/screen_shake":
		if (value < 0): return false
		screen_shake_amount = value
		return true

func _get_property_list():
	var property_list = []
	
	# Knockback group
	property_list.append({
		"hint": PROPERTY_HINT_NONE,
		"usage": PROPERTY_USAGE_DEFAULT,
		"name": "knockback/has",
		"type": TYPE_BOOL
	})
	if knockback == true:
		property_list.append({
			"hint": PROPERTY_HINT_RANGE,
			"usage": "-360,360,1,or_greater,or_less",
			"name": "knockback/direction",
			"type": TYPE_FLOAT
		})
		property_list.append({
			"hint": PROPERTY_HINT_NONE,
			"usage": PROPERTY_USAGE_DEFAULT,
			"name": "knockback/magnitude",
			"type": TYPE_FLOAT
		})
	
	# UI group
	property_list.append({
		"hint": PROPERTY_HINT_NONE,
		"usage": PROPERTY_USAGE_DEFAULT,
		"name": "ui_effect/has",
		"type": TYPE_BOOL
	})
	if ui_effect == true:
		property_list.append({
			"hint": PROPERTY_HINT_NONE,
			"usage": PROPERTY_USAGE_DEFAULT,
			"name": "ui_effect/hit_stop",
			"type": TYPE_FLOAT
		})
		property_list.append({
			"hint": PROPERTY_HINT_NONE,
			"usage": PROPERTY_USAGE_DEFAULT,
			"name": "ui_effect/screen_shake",
			"type": TYPE_FLOAT
		})
	
	return property_list

func _ready() -> void:
	super._ready()
	debug_color = Color(Color.RED, 0.2)
	suffix = "HI"
	
