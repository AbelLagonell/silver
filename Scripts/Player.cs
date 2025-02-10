using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public  float Speed = 300.0f;
	[Export] public Resource MoveAction = null; // TYPE GUIDEAction

	public override void _PhysicsProcess(double delta)
	{
		Position += (Vector2)(MoveAction.Get("value_axis_2D")) * Speed * (float)delta;
	}
}
