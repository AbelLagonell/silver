using Godot;

public abstract partial class CombatShape2D : CollisionShape2D
{
    //Make this Read only
    [Export]public int Frame;
    
    protected abstract void PlaceInFrameBefore();

    protected abstract void PlaceInCurrentFrame();

    protected abstract void PlaceInNextFrame();

    protected abstract void RemoveCurrentCollider();
}