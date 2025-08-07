using Godot;
using Godot.Collections;

[GlobalClass]
public partial class HurtboxSet : CombatCollider2D<HurtboxShape2D>
{
    [Signal]
    public delegate void DamageTakenEventHandler(int damage, GodotObject sender);

    [Signal]
    public delegate void HitboxInformationEventHandler(Array<Resource> hitboxInfo);

    [Export]
    private Color _debugColor = new Color("SkyBlue", 0.41f);


    protected override void ChangeCollision()
    {
        SetCollisionLayerValue(PlayerHurtboxLayer, !IsEnemy);
        SetCollisionLayerValue(EnemyHurtboxLayer, IsEnemy);
    }
}