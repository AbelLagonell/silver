using Godot;

namespace silver.Scripts;

public partial class Player : CharacterBody2D {
    public const float Speed = 100.0f;


    public override void _PhysicsProcess(double delta) {
        Vector2 velocity = Velocity;

        Vector2 direction = Input.GetVector("mv_left", "mv_right", "mv_up", "mv_down");

        if (direction != Vector2.Zero) {
            velocity = direction * Speed;
        } else {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
        }


        Velocity = velocity;
        MoveAndSlide();
    }
}