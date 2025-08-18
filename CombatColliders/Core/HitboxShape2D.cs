using Godot;

[Tool]
[GlobalClass]
public partial class HitboxShape2D : CombatShape2D
{
    private HitboxSet _parent;
    public float Damage = 0;

    public override void _Ready()
    {
        _parent = GetParentOrNull<HitboxSet>();
    }

    protected override void PlaceInFrameBefore()
    {
        _parent.AddCollisionToFrame(Frame - 1);
    }

    protected override void PlaceInCurrentFrame()
    {
        _parent.AddCollisionToFrame(Frame);
    }

    protected override void PlaceInNextFrame()
    {
        _parent.AddCollisionToFrame(Frame + 1);
    }

    protected override void RemoveCurrentCollider()
    {
        _parent.RemoveCollisionFromFrame(Frame, _parent.GetPathTo(this));
    }
}