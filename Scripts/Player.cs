using Godot;
using System;

public partial class Player : CharacterBody2D
{
	
	
	[Export] public  float Speed = 300.0f;
	[Export] public Resource MoveAction = null;

	public override void _Ready()
	{
		this.Set("property", "GUIDEAction");
		GD.Print(this.Get("property"));
	}

	public override void _PhysicsProcess(double delta)
	{
	}
}
