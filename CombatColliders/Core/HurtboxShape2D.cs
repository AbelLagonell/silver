using Godot;

[Tool]
[GlobalClass]
public partial class HurtboxShape2D : CollisionShape2D
{
    [Export] public int Frame;

    private void PlaceInFrameBefore() { }

    private void PlaceInCurrentFrame() { }

    private void PlaceInNextFrame() { }

    private void RemoveCurrentCollider() { }
}