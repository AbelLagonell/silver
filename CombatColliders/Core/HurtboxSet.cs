using Godot;
using Godot.Collections;


[Tool]
[GlobalClass]
[Icon("res://Icons/Hurtbox_Set.svg")]
public partial class HurtboxSet : CombatCollider2D<HurtboxShape2D>
{
    [Signal]
    public delegate void DamageTakenEventHandler(float damage, GodotObject sender);

    [Signal]
    public delegate void HitboxInformationEventHandler(Array<Resource> hitboxInfo);

    private new Color _debugColor = new Color("SkyBlue", 0.41f);

    public void TakeDamage(float dmg, HitboxSet hitboxSet)
    {
        EmitSignalDamageTaken(dmg, hitboxSet);
        GD.Print("Hitbox Shape Entered");
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
        
        AddChild(hurtbox);
        Frames[actingFrame].Add(GetPathTo(hurtbox));

        if (Engine.IsEditorHint())
            hurtbox.Owner = EditorInterface.Singleton.GetEditedSceneRoot();
    }
}