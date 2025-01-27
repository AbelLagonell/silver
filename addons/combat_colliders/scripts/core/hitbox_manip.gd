class_name HitBoxShape
extends HurtBoxShape

var damage: int = 0

# Knockback
var knockback: bool = false
var knockback_toggle: bool = false
var knockback_vector: Vector2 = Vector2(0,0)
var knockback_angle: float = 0.0
var knockback_magnitude: float = 0.0

# UI
var ui_effect: bool = false
var hit_stop: float = 0.0
var screen_shake_amount: float = 0.0

func _get(property):
	# Knockback group
	if property == "knockback/has":
		return knockback
	if property == "knockback/advanced":
		return knockback_toggle
	if property == "knockback/vector":
		return knockback_vector
	if property == "knockback/angle":
		return knockback_angle
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
	if property == "knockback/advanced":
		knockback_toggle = value
		notify_property_list_changed()
		return true
	if property == "knockback/vector":
		knockback_vector = value
		return true
	if property == "knockback/angle":
		knockback_angle = value
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
	if knockback:
		property_list.append({
		"hint": PROPERTY_HINT_NONE,
		"usage": PROPERTY_USAGE_DEFAULT,
		"name": "knockback/advanced",
		"type": TYPE_BOOL
	})
	
	if knockback && knockback_toggle:
		property_list.append({
			"hint": PROPERTY_HINT_NONE,
			"usage": PROPERTY_USAGE_DEFAULT,
			"name": "knockback/vector",
			"type": TYPE_VECTOR2
		})
	
	if knockback && !knockback_toggle:
		property_list.append({
			"hint": PROPERTY_HINT_RANGE,
			"hint_string": "-360,360,degrees",
			"usage": PROPERTY_USAGE_DEFAULT,
			"name": "knockback/angle",
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
	if ui_effect:
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
