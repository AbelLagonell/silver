using Godot;
using Godot.Collections;

[Tool]
[GlobalClass]
[Icon(("res://Icons/Hitbox_Set.svg"))]
public partial class HitboxSet : CombatCollider2D<HitboxShape2D>
{
    [Signal]
    public delegate void DamageGivenEventHandler(HurtboxSet hurtboxSet, HurtboxShape2D hurtboxShape);

    private new Color _debugColor = new Color("Red", 0.3f);

    public override void _Ready()
    {
        base._Ready();
        AreaShapeEntered += OnAreaShapeEntered;
    }

    protected void OnAreaShapeEntered(Rid areaRid, Area2D area, long areaShapeIndex, long localShapeIndex)
    {
        if (area.GetChild((int)areaShapeIndex) is not HurtboxShape2D hurtboxShape) return;
        EmitSignalDamageGiven((HurtboxSet)area, hurtboxShape);
        ((HurtboxSet)area).TakeDamage(GetChild<HitboxShape2D>((int)localShapeIndex).Damage, this);
        GD.Print("Hurtbox Shape Entered");
    }

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

        //Defaults for stuff

        AddChild(hitbox);

        if (Engine.IsEditorHint())
            hitbox.Owner = EditorInterface.Singleton.GetEditedSceneRoot();

        Frames[actingFrame].Add(GetPathTo(hitbox));
    }
}