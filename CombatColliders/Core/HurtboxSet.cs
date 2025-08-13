using Godot;
using Godot.Collections;


[Tool]
[GlobalClass]
public partial class HurtboxSet : CombatCollider2D<HurtboxShape2D>
{
    [Signal]
    public delegate void DamageTakenEventHandler(float damage, GodotObject sender);

    [Signal]
    public delegate void HitboxInformationEventHandler(Array<Resource> hitboxInfo);

    private new Color _debugColor = new Color("SkyBlue", 0.41f);

    protected override void OnAreaShapeEntered(Rid areaRid, Area2D area, long areaShapeIndex, long localShapeIndex)
    {
        if (area is not HitboxSet hitboxSet) return;
        var hitboxShape = hitboxSet.GetChild<HitboxShape2D>((int)areaShapeIndex);
        EmitSignalDamageTaken(hitboxShape.Damage, hitboxSet);
        
    }

    protected override void ChangeCollision()
    {
        SetCollisionLayerValue(PlayerHurtboxLayer, !IsEnemy);
        SetCollisionLayerValue(EnemyHurtboxLayer, IsEnemy);
    }
    
    protected override void AddCollisionToFrame(int actingFrame)
    {
        base.AddCollisionToFrame(actingFrame);

        string shapeName = $"Frame {actingFrame} Hurtbox {Frames[actingFrame].Count}";
        Shape2D colliderShape = (Shape2D)System.Activator.CreateInstance(Shape2D.GetType());

        var hurtbox = AddShape(colliderShape, shapeName, _debugColor);
        Frames[actingFrame].Add(hurtbox);
        
        AddChild(hurtbox);

        if (Engine.IsEditorHint())
            hurtbox.Owner = EditorInterface.Singleton.GetEditedSceneRoot();
    }
}