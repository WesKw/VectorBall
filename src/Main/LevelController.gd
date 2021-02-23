extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var gravity : Vector3 = Vector3(0, -1, 0)
var maxTilt = deg2rad(2.5);


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	var horiz = Input.get_action_strength("tilt_right") - Input.get_action_strength("tilt_left")
	var vert = Input.get_action_strength("tilt_up") - Input.get_action_strength("tilt_down")
	
	gravity.x = horiz * maxTilt
	gravity.y = -1
	gravity.z = vert * maxTilt
	
	PhysicsServer.area_set_param(get_viewport().find_world().get_space(), PhysicsServer.AREA_PARAM_GRAVITY_VECTOR, gravity);
	pass
