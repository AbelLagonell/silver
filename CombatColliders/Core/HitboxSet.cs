using Godot;
using Godot.Collections;

[Tool]
[GlobalClass]
public partial class HitboxSet : CombatCollider2D<HitboxShape2D>
{
    private new Color _debugColor = new Color("Red", 0.3f);

    protected override void ChangeCollision()
    {
        SetCollisionLayerValue(EnemyHitboxLayer, IsEnemy);
        SetCollisionMaskValue(PlayerHurtboxLayer, IsEnemy);

        SetCollisionLayerValue(PlayerHitboxLayer, !IsEnemy);
        SetCollisionMaskValue(EnemyHurtboxLayer, !IsEnemy);
    }
    
    protected override void AddCollisionToFrame(int actingFrame)
    {
        base.AddCollisionToFrame(actingFrame);

        string shapeName = $"Frame {actingFrame} Hitbox {Frames[actingFrame].Count}";
        Shape2D colliderShape = (Shape2D)System.Activator.CreateInstance(Shape2D.GetType());
        
        var hitbox = AddShape(colliderShape, shapeName, _debugColor);
        Frames[actingFrame].Add(hitbox);
        
        //Defaults for stuff
        
        AddChild(hitbox);
        
        if (Engine.IsEditorHint())
            hitbox.Owner = EditorInterface.Singleton.GetEditedSceneRoot();
    }
}