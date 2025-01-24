@tool
class_name HurtboxFrameData
extends ColliderFrameData

func _ready() -> void:
	super._ready()
	debug_color = Color(Color.WHITE, 0.1)
	suffix = "HU"
	
