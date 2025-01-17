extends Resource

## The array to hold the colliders
var colliders : Array[CollisionShape2D]
## The parent node that this will be adding children to
var parent : Node

## To Initialize 
func _init(_parent: Node = null) -> void:
	colliders = []
	parent = _parent

## Adding a collider to the array
func add(collider: CollisionShape2D) -> int:
	collider.name += "HU%d" % colliders.size()
	if (parent == null):
		push_error("Parent Node not defined")
		return -1
	colliders.append(collider)
	parent.add_child(collider)
	collider.owner = EditorInterface.get_edited_scene_root()
	return 1

## Being able to remove any particular collider in the array [br]
## The default removes the last element in the array
func remove(index:int = -1) -> int :
	print("Delete")
	var collider = colliders.pop_at(index) 
	if is_instance_valid(collider):
		collider.queue_free()
		return 1
	else:
		push_warning("Collider at index %d is not valid" % index) 
		return -1

func reset():
	print(colliders.size())
	for index in colliders.size():
		remove()

func hide():
	for index in colliders.size():
		colliders[index].hide()
		
func show():
	for index in colliders.size():
		colliders[index].show()
