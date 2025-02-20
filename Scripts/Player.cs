using System;
using Godot;
using silver.Scripts.Actions;

namespace silver.Scripts;

internal struct AvailableActions {
    const string Idle = "Idle";
    const string Walk = "Walk";

    //const string Jab = "Jab";
    //const string Cross = "Cross";
}

public partial class Player : CharacterBody2D {
    [Export] private ActionManager _actionManager = null;

    public override void _Ready() {
        _actionManager = GetNode<ActionManager>("ActionManager");
    }

    public override void _PhysicsProcess(double delta) {
        Vector2 velocity = Velocity;

        Vector2 direction = Input.GetVector("mv_left", "mv_right", "mv_up", "mv_down");

        
        
        Velocity = velocity;
        MoveAndSlide();
    }
}